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

            signalR.on('GameStarted', (gameId: string, options: any, questions: any[]) => {
                console.log('GameStarted', gameId, options)
                gameStarted.value = true
                mpSettings.value = options
                mpQuestions.value = questions || []
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
            signalR.on('ShowReveal', (updatedPlayers: any[]) => {
                console.log('ShowReveal received', updatedPlayers)
                if (updatedPlayers) {
                    players.value = updatedPlayers
                }
                showReveal.value = true
                moveToNext.value = false
            })

            signalR.on('NextQuestion', (index: number) => {
                console.log('NextQuestion received', index)
                showReveal.value = false
                moveToNext.value = true // Trigger to reset UI
                // We could sync index here if needed: currentQuestionIndex.value = index
                // But let the component handle the trigger
                setTimeout(() => { moveToNext.value = false }, 100) // Reset trigger
            })
        }
        // Ensure connected
        await signalR.start()
    }

    const createLobby = async (gameId?: string, usernameParam?: string) => {
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
        // Stop connection
        if (signalR) {
            await signalR.stop()
            signalR = null
        }
    }

    const startGame = async (gameId: string, options?: any) => {
        if (!isHost.value || !signalR || !currentCode.value) return
        try {
            await signalR.invoke('StartGame', currentCode.value, gameId, options)
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
                isHost.value = true // Restore host status claim
            }
        }
    }

    // Game Sync State
    const mpQuestions = useState<any[]>('mp_questions', () => [])
    const showReveal = useState<boolean>('mp_show_reveal', () => false)
    const moveToNext = useState<boolean>('mp_move_next', () => false)
    const mpSettings = useState<any | null>('mp_settings', () => null)

    const updateScore = async (score: number) => {
        // Implement real sync later if needed
    }

    const submitAnswer = async (answer: string) => {
        if (!signalR || !currentCode.value) return
        try {
            await signalR.invoke('SubmitAnswer', currentCode.value, answer)
        } catch (e) {
            console.error("Error submitting answer", e)
        }
    }

    const requestNextQuestion = async () => {
        if (!isHost.value || !signalR || !currentCode.value) return
        try {
            await signalR.invoke('RequestNextQuestion', currentCode.value)
        } catch (e) {
            console.error("Error requesting next question", e)
        }
    }

    const disconnect = async () => {
        await leaveLobby()
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
        restoreSession,
        // Game Actions
        updateScore,
        submitAnswer,
        requestNextQuestion,
        disconnect,
        // Game State
        mpQuestions,
        showReveal,
        moveToNext,
        mpSettings
    }
}


