import { SignalRClient } from '~/services/signalr.client'
import type { Party } from '~/types'

// Singleton instance to persist connection across components
let signalR: SignalRClient | null = null

export const useMultiplayer = () => {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()
    const router = useRouter()

    // State
    const currentCode = useState<string | null>('mp_code', () => null)
    const players = useState<any[]>('mp_players', () => [])
    const gameStarted = useState<boolean>('mp_game_started', () => false)
    const error = useState<string | null>('mp_error', () => null)
    const isHost = useState<boolean>('mp_is_host', () => false)

    // Derived state
    const isConnected = computed(() => !!currentCode.value)

    const initSignalR = async () => {
        if (!signalR) {
            const hubUrl = `${config.public.apiBase}/gameHub`
            signalR = new SignalRClient(hubUrl, authStore.token || undefined)

            // Register global events
            signalR.on('PlayerJoined', (player: any) => {
                console.log('PlayerJoined', player)
                if (!players.value.find(p => p.username === player.username)) {
                    players.value.push(player)
                }
            })

            signalR.on('PlayerLeft', (username: string) => {
                console.log('PlayerLeft', username)
                players.value = players.value.filter(p => p.username !== username)
            })

            signalR.on('GameStarted', (gameId: string) => {
                console.log('GameStarted', gameId)
                gameStarted.value = true
                router.push(`/games/${gameId}?mode=multi`)
            })

            signalR.on('LobbyDisbanded', () => {
                console.log('Lobby disbanded')
                alert("Le groupe a été dissous par l'hôte.")
                currentCode.value = null
                players.value = []
                isHost.value = false
                gameStarted.value = false
            })
        }
        // Ensure connected
        await signalR.start()
    }

    const createLobby = async (usernameParam?: string) => {
        error.value = null
        try {
            await initSignalR()
            if (!signalR) throw new Error("Connection failed")

            const username = usernameParam || authStore.user?.username || 'Hôte'
            const result = await signalR.invoke<{ code: string }>('CreateLobby', username)
            currentCode.value = result.code
            isHost.value = true
            players.value = [{ username, isHost: true }] // Add self
            saveSession(result.code, username, true)
            return result.code
        } catch (e: any) {
            console.error("Error creating lobby", e)
            error.value = "Impossible de créer le groupe."
            return null
        }
    }

    const joinLobby = async (code: string, usernameParam?: string) => {
        error.value = null
        try {
            await initSignalR()
            if (!signalR) throw new Error("Connection failed")

            const username = usernameParam || authStore.user?.username || 'Joueur'
            const result = await signalR.invoke<{ success: boolean, players: any[] }>('JoinLobby', code, username)
            if (result.success) {
                currentCode.value = code
                isHost.value = false
                players.value = result.players
                saveSession(code, username, false)
                return true
            } else {
                error.value = "Code invalide ou groupe plein."
                return false
            }
        } catch (e: any) {
            console.error("Error joining lobby", e)
            error.value = "Impossible de rejoindre le groupe."
            return false
        }
    }

    const leaveLobby = async () => {
        if (signalR) {
            try {
                await signalR.invoke('LeaveLobby')
            } catch (err) {
                console.error("Error leaving lobby", err)
            }
        }
        // Reset state
        currentCode.value = null
        players.value = []
        isHost.value = false
        gameStarted.value = false
        clearSession()
        // Optionally stop connection or keep it open? 
        // For simplicity, we might keep it open IF we expect frequent reconnects, 
        // but stopping it cleans up resources.
        if (signalR) {
            await signalR.stop()
            signalR = null
        }
    }

    const startGame = async (gameId: string) => {
        if (!isHost.value || !signalR || !currentCode.value) return
        try {
            await signalR.invoke('StartGame', currentCode.value, gameId)
        } catch (e: any) {
            console.error("Error starting game", e)
            error.value = "Impossible de lancer la partie."
        }
    }

    const saveSession = (code: string, username: string, host: boolean) => {
        if (process.client) {
            sessionStorage.setItem('ilmanar_lobby', code)
            sessionStorage.setItem('ilmanar_username', username)
            if (host) sessionStorage.setItem('ilmanar_host', 'true')
            else sessionStorage.removeItem('ilmanar_host')
        }
    }

    const clearSession = () => {
        if (process.client) {
            sessionStorage.removeItem('ilmanar_lobby')
            sessionStorage.removeItem('ilmanar_username')
            sessionStorage.removeItem('ilmanar_host')
        }
    }

    const restoreSession = async () => {
        if (!process.client) return

        const code = sessionStorage.getItem('ilmanar_lobby')
        const username = sessionStorage.getItem('ilmanar_username')
        const host = sessionStorage.getItem('ilmanar_host') === 'true'

        if (code && username) {
            console.log("Restoring session...", code, username)
            // Attempt join
            const success = await joinLobby(code, username)
            if (success && host) {
                isHost.value = true // Restore host status claim (verified by server response ideally, but trusted for UI now)
            }
        }
    }

    return {
        isConnected,
        currentCode,
        players,
        gameStarted,
        error,
        isHost,
        createLobby,
        joinLobby,
        leaveLobby,
        startGame,
        restoreSession
    }
}


