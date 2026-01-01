import { RealtimeChannel } from '@supabase/supabase-js'
import { ApiClient } from '~/services/api.client'
import { GameService } from '~/services/game.service'

export const useMultiplayer = () => {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()
    const supabase = useSupabaseClient()

    // Services
    const apiClient = new ApiClient(config.public.apiBase)
    const gameService = new GameService(apiClient)

    // State
    const currentCode = ref<string | null>(null)
    const currentPartyId = ref<string | null>(null)
    const players = ref<any[]>([])
    const myUsername = ref<string | null>(null) // Local tracking for guests/auth
    const gameStarted = ref(false)
    const error = ref<string | null>(null)
    const isHost = ref(false)
    const partyStatus = ref('waiting')

    // Sync Events (Derived from DB state)
    const showReveal = ref(false)
    const moveToNext = ref(false) // Triggered when Question Index changes
    const currentQuestionIndex = ref(0) // Synced from DB
    const mpQuestions = ref<any[]>([])

    // Computed
    const isConnected = computed(() => !!currentPartyId.value)

    // Subscribe to DB Changes
    let partyChannel: RealtimeChannel | null = null

    const subscribeToParty = async (partyId: string) => {
        if (partyChannel) await supabase.removeChannel(partyChannel)

        // Initial Fetch
        await fetchPartyState(partyId)
        await fetchPlayers(partyId)

        partyChannel = supabase.channel(`party_db:${partyId}`)
            .on(
                'postgres_changes',
                { event: 'UPDATE', schema: 'public', table: 'parties', filter: `id=eq.${partyId}` },
                (payload) => {
                    const newParty = payload.new as any
                    if (newParty.status === 'playing' && !gameStarted.value) {
                        startGameClientSide(newParty)
                    }
                    if (newParty.current_question_index !== currentQuestionIndex.value) {
                        // Question changed
                        handleNextQuestion(newParty.current_question_index)
                    }
                    partyStatus.value = newParty.status
                }
            )
            .on(
                'postgres_changes',
                { event: '*', schema: 'public', table: 'party_players', filter: `party_id=eq.${partyId}` },
                () => {
                    // Simplest strategy: Refetch players on any change (join, score up, answer)
                    // Optimizations possible later
                    fetchPlayers(partyId)
                }
            )
            .subscribe()
    }

    const fetchPartyState = async (partyId: string) => {
        const { data, error: err } = await supabase
            .from('parties')
            .select('*')
            .eq('id', partyId)
            .single()

        if (err) {
            console.error('Error fetching party', err)
            return
        }

        partyStatus.value = data.status
        currentQuestionIndex.value = data.current_question_index
        if (data.status === 'playing') {
            // If joining late or refresh
            if (!gameStarted.value) startGameClientSide(data)
        }
    }

    const fetchPlayers = async (partyId: string) => {
        const { data, error: err } = await supabase
            .from('party_players')
            .select('*')
            .order('score', { ascending: false })

        if (err) {
            console.error('Error fetching players', err)
            return
        }

        players.value = data.map(p => ({
            ...p,
            // Map DB fields to frontend expected fields if diff
            connectionId: p.username // Use username as ID for simple frontend logic
        }))

        // Check for Reveal condition locally
        // If everyone answered, we might want to show reveal. 
        // Or wait for 'playing' logic.
        // For now, let's derive 'showReveal' from players.every(p => p.has_answered)
        if (gameStarted.value && data.length > 0) {
            const allAnswered = data.every((p: any) => p.has_answered)
            if (allAnswered && !showReveal.value && !moveToNext.value) {
                showReveal.value = true
            }
        }
    }

    const startGameClientSide = async (party: any) => {
        // We need to load questions.
        // Assuming we know gameId from party or somewhere.
        // Use GameService to fetch game content
        try {
            const game = await gameService.getById(party.game_id)
            if (game.content) {
                const content = (typeof game.content === 'string') ? JSON.parse(game.content) : game.content
                if (content.questions) mpQuestions.value = content.questions
            }
            gameStarted.value = true
        } catch (e) {
            console.error(e)
        }
    }

    const handleNextQuestion = (newIndex: number) => {
        currentQuestionIndex.value = newIndex
        moveToNext.value = true
        showReveal.value = false // Reset reveal
        // Auto reset flag locally for UI
        setTimeout(() => { moveToNext.value = false }, 500)
    }

    const disconnect = async () => {
        // If Host, delete the party to prevent stale lobbies
        if (isHost.value && currentPartyId.value) {
            try {
                // Delete players first to ensure no FK violations (if cascade missing)
                await supabase.from('party_players').delete().eq('party_id', currentPartyId.value)
                // Then delete party
                await supabase.from('parties').delete().eq('id', currentPartyId.value)
            } catch (e) {
                console.error("Error deleting party:", e)
            }
        }

        if (partyChannel) await supabase.removeChannel(partyChannel)
        partyChannel = null
        currentCode.value = null
        currentPartyId.value = null
        players.value = []
        isHost.value = false
        gameStarted.value = false
        showReveal.value = false
        moveToNext.value = false
    }

    const subscribeToParty = async (partyId: string) => {
        if (partyChannel) await supabase.removeChannel(partyChannel)

        // Initial Fetch
        await fetchPartyState(partyId)
        await fetchPlayers(partyId)

        partyChannel = supabase.channel(`party_db:${partyId}`)
            .on(
                'postgres_changes',
                { event: 'UPDATE', schema: 'public', table: 'parties', filter: `id=eq.${partyId}` },
                (payload) => {
                    const newParty = payload.new as any
                    if (newParty.status === 'playing' && !gameStarted.value) {
                        startGameClientSide(newParty)
                    }
                    if (newParty.current_question_index !== currentQuestionIndex.value) {
                        // Question changed
                        handleNextQuestion(newParty.current_question_index)
                    }
                    partyStatus.value = newParty.status
                }
            )
            .on(
                'postgres_changes',
                { event: 'DELETE', schema: 'public', table: 'parties', filter: `id=eq.${partyId}` },
                () => {
                    // Party deleted by host
                    if (!isHost.value) {
                        error.value = "Le chef de caravane a dissous la partie."
                        setTimeout(() => {
                            disconnect() // Guest cleanup
                        }, 3000)
                    }
                }
            )
            .on(
                'postgres_changes',
                { event: '*', schema: 'public', table: 'party_players', filter: `party_id=eq.${partyId}` },
                () => {
                    fetchPlayers(partyId)
                }
            )
            .subscribe()
    }

    // Actions
    const createLobby = async (gameId: string, usernameParam?: string) => {
        const username = usernameParam || authStore.user?.username || 'Guest_' + Math.floor(Math.random() * 1000)
        const code = Math.random().toString(36).substring(2, 8).toUpperCase()

        try {
            // 1. Create Party in DB
            const { data: party, error: err } = await supabase
                .from('parties')
                .insert({
                    code,
                    game_id: gameId,
                    host_username: username,
                    status: 'waiting'
                })
                .select()
                .single()

            if (err) throw err

            // 2. Add Host as Player
            const { error: pErr } = await supabase
                .from('party_players')
                .insert({
                    party_id: party.id,
                    username,
                    is_ready: true
                })

            if (pErr) throw pErr

            currentCode.value = code
            currentPartyId.value = party.id
            isHost.value = true
            myUsername.value = username

            await subscribeToParty(party.id)
            return code
        } catch (err: any) {
            console.error(err)
            error.value = "Erreur lors de la création de la partie."
            throw err
        }
    }

    const joinLobby = async (code: string, usernameParam?: string) => {
        const username = usernameParam || authStore.user?.username || 'Guest_' + Math.floor(Math.random() * 1000)

        try {
            // 1. Find Party
            const { data: party, error: err } = await supabase
                .from('parties')
                .select('*')
                .eq('code', code)
                .single()

            if (err || !party) throw new Error("Partie introuvable")

            // 2. Add Player (if not exists)
            // upsert to avoid error if rejoining? or just ignore conflict
            const { error: pErr } = await supabase
                .from('party_players')
                .upsert({
                    party_id: party.id,
                    username,
                    is_ready: true
                }, { onConflict: 'party_id, username' })

            if (pErr) throw pErr

            currentCode.value = code
            currentPartyId.value = party.id
            isHost.value = (party.host_username === username)
            myUsername.value = username

            await subscribeToParty(party.id)
        } catch (err: any) {
            console.error(err)
            error.value = "Impossible de rejoindre la partie."
            throw err
        }
    }

    const startGame = async () => {
        if (!currentPartyId.value || !isHost.value) return

        await supabase
            .from('parties')
            .update({ status: 'playing' })
            .eq('id', currentPartyId.value)
    }

    const updateScore = async (score: number) => {
        const username = myUsername.value || authStore.user?.username
        if (!username || !currentPartyId.value) return

        await supabase
            .from('party_players')
            .update({ score: score, has_answered: true })
            .eq('party_id', currentPartyId.value)
            .eq('username', username)
    }

    const submitAnswer = async () => {
        // Redundant if updateScore handles it, but maybe useful for 0-point answers
        // We'll trust updateScore called by QuizGame
    }

    // QuizGame calls this when checking host status, but trigger handles reset logic
    const requestNextQuestion = async () => {
        if (!currentPartyId.value || !isHost.value) return

        // Increment index in DB
        // The Trigger 'reset_round_on_next_question' will handle resetting 'has_answered'
        await supabase.rpc('increment_question_index', { party_id_param: currentPartyId.value })
        // Alternatively if RPC not created, fetch, increment, update. RPC is safer for concurrency.
        // Wait, I didn't create RPC in Step 204. I created a trigger.
        // I should just update current_question_index = current_question_index + 1

        // Optimistic update using current value
        await supabase
            .from('parties')
            .update({ current_question_index: currentQuestionIndex.value + 1 })
            .eq('id', currentPartyId.value)
    }

    // Also we need `mpSubmitAnswer` to just set has_answered=true without score if score didn't change?
    // In QuizGame it calls updateScore(score.value) then submitAnswer().
    // We can merge logic.

    return {
        connect: () => { },
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
        moveToNext,
        mpQuestions
    }
}
