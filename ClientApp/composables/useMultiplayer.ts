import { HubConnectionBuilder, HttpTransportType, HubConnection, HubConnectionState } from '@microsoft/signalr'

export const useMultiplayer = () => {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()

    // State
    const connection = ref<HubConnection | null>(null)
    const isConnected = ref(false)
    const currentCode = ref<string | null>(null)
    const players = ref<any[]>([])
    const gameStarted = ref(false)
    const error = ref<string | null>(null)
    const isHost = ref(false)

    // Sync Events
    const showReveal = ref(false)
    const moveToNext = ref(false)

    // Initialize Connection
    const connect = async () => {
        if (connection.value?.state === HubConnectionState.Connected) return

        const hubUrl = `${config.public.apiBase.replace('/api', '')}/gameHub`

        connection.value = new HubConnectionBuilder()
            .withUrl(hubUrl, {
                accessTokenFactory: () => authStore.token || '',
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build()

        // Event Listeners
        connection.value.on('PlayerJoined', (updatedPlayers: any[]) => {
            players.value = updatedPlayers
        })

        connection.value.on("PlayerLeft", (updatedPlayers: any[]) => {
            players.value = updatedPlayers
        })

        connection.value.on("LobbyClosed", (reason: string) => {
            console.warn("Lobby Closed:", reason)
            error.value = reason
            gameStarted.value = false
            players.value = []
            currentCode.value = null
            // Optional: Redirect or just show error
            alert(reason)
            disconnect()
        })

        connection.value.on("GameStarted", () => {
            gameStarted.value = true
        })

        connection.value.on('ScoreUpdated', (updatedPlayers: any[]) => {
            players.value = updatedPlayers
        })

        // Sync Listeners
        connection.value.on('RevealAnswer', () => {
            showReveal.value = true
        })

        connection.value.on('NextQuestion', () => {
            moveToNext.value = true
            // Auto reset flags shortly after to allow consumption
            setTimeout(() => { moveToNext.value = false; showReveal.value = false }, 500)
        })

        connection.value.on('Error', (msg: string) => {
            error.value = msg
        })

        try {
            await connection.value.start()
            isConnected.value = true
            console.log('SignalR Connected')
        } catch (err: any) {
            console.error('SignalR Connection Error: ', err)
            error.value = "Impossible de se connecter au serveur de jeu."
        }
    }

    const disconnect = async () => {
        if (connection.value) {
            await connection.value.stop()
            isConnected.value = false
            currentCode.value = null
            players.value = []
            isHost.value = false
            gameStarted.value = false
            showReveal.value = false
            moveToNext.value = false
        }
    }

    // Actions
    const createLobby = async (gameId: string) => {
        if (!connection.value) await connect()
        try {
            const code = await connection.value!.invoke<string>('CreateLobby', authStore.user?.username || 'Guest', gameId)
            currentCode.value = code
            isHost.value = true
            // Add self to players list initially (although server sends update, good for instant feedback)
            players.value = [{ connectionId: connection.value!.connectionId, username: authStore.user?.username || 'Guest', score: 0 }]
            return code
        } catch (err) {
            console.error(err)
            throw err
        }
    }

    const joinLobby = async (code: string) => {
        if (!connection.value) await connect()
        try {
            await connection.value!.invoke('JoinLobby', code, authStore.user?.username || 'Guest')
            currentCode.value = code
            isHost.value = false
        } catch (err) {
            console.error(err)
            throw err
        }
    }

    const startGame = async () => {
        if (!connection.value || !currentCode.value) return
        await connection.value.invoke('StartGame', currentCode.value)
    }

    const updateScore = async (score: number) => {
        if (!connection.value || !currentCode.value) return
        await connection.value.invoke('UpdateScore', currentCode.value, score)
    }

    // Sync Actions
    const submitAnswer = async () => {
        if (!connection.value || !currentCode.value) return
        await connection.value.invoke('SubmitAnswer', currentCode.value)
    }

    const requestNextQuestion = async () => {
        if (!connection.value || !currentCode.value) return
        await connection.value.invoke('NextQuestion', currentCode.value)
    }

    return {
        connect,
        disconnect,
        createLobby,
        joinLobby,
        startGame,
        updateScore,
        submitAnswer,
        requestNextQuestion,
        isConnected,
        currentCode,
        players,
        gameStarted,
        error,
        isHost,
        showReveal,
        moveToNext
    }
}
