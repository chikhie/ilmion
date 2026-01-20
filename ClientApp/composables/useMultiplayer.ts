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
        if (signalR && currentCode.value) {
            try {
                await signalR.invoke('LeaveLobby', currentCode.value)
            } catch (err) {
                console.error("Error leaving lobby", err)
            }
        }
        // Reset state
        currentCode.value = null
        players.value = []
        isHost.value = false
        gameStarted.value = false
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
        startGame
    }
}


