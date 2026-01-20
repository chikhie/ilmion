<template>
  <div class="min-h-screen bg-brand-dark py-12 relative overflow-hidden font-sans-body">
    <!-- Background Layers -->
    <div class="fixed inset-0 z-0 pointer-events-none">
       <div class="absolute inset-0 pattern-geometric opacity-[0.03]"></div>
       <div class="absolute inset-0 bg-texture-wood opacity-50 mix-blend-overlay"></div>
       <div class="absolute top-0 right-0 w-[500px] h-[500px] bg-brand-gold/5 blur-[100px] rounded-full"></div>
    </div>

    <div class="max-w-4xl mx-auto px-4 relative z-10">
      <!-- Back Link -->
      <NuxtLink to="/games" class="hidden md:inline-flex items-center text-xs font-bold uppercase tracking-widest text-brand-parchment/60 hover:text-brand-gold mb-10 transition-all group">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-2 transition-transform group-hover:-translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        Retour aux jeux
      </NuxtLink>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-brand-gold opacity-50"></div>
      </div>
      
      <div v-else-if="error" class="bg-red-500/10 border border-red-500/20 text-red-400 px-6 py-4 rounded-2xl text-center font-sans-body">
        <strong class="font-bold">Erreur !</strong>
        <span class="block sm:inline ml-2">Impossible de charger le jeu.</span>
      </div>
      
       <div v-else-if="game" class="animate-fade-in">
          <div class="hidden md:block mb-12 text-center">
              <h1 class="text-4xl md:text-5xl font-serif-title font-bold tracking-tight text-brand-parchment mb-4 drop-shadow-md">{{ game.title }}</h1>
              <div class="h-1 w-24 bg-brand-gold mx-auto rounded-full opacity-60 mb-6"></div>
              <p v-if="game.description" class="text-brand-parchment/80 font-sans-body text-lg max-w-2xl mx-auto leading-relaxed">{{ game.description }}</p>
          </div>

          <div class="animate-fade-in-up">
              <ClientOnly>
                  <!-- Type 3 is Quiz -->
                  <GameQuizGame 
                     v-if="game.type === 3" 
                     :game-content="game.content" 
                     :title="game.title"
                     :key="refreshKey"
                     :is-multiplayer="isMultiplayer"
                     @close="goBack"
                     @retry="restartGame"
                  />
                  <div v-else class="text-center p-12 bg-texture-parchment rounded-3xl border border-brand-gold/30 shadow-2xl relative overflow-hidden">
                      <div class="absolute inset-0 bg-brand-gold/5 pointer-events-none"></div>
                      <p class="text-brand-dark font-serif-title font-bold uppercase tracking-widest text-sm relative z-10">Ce type de jeu ({{ getGameTypeName(game.type) }}) n'est pas encore disponible.</p>
                  </div>
              </ClientOnly>
          </div>
       </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const route = useRoute()
const router = useRouter()
const config = useRuntimeConfig()
const gameId = route.params.id as string
const refreshKey = ref(0)
const isMultiplayer = computed(() => {
    // If explicitly set in query
    if (route.query.mode === 'multi') return true
    
    // Or if connected to a lobby (implicit, though query param is preferred for deep linking/refresh)
    const { isConnected } = useMultiplayer()
    return isConnected.value
})

const api = useApi()

// Use useAsyncData to compatible with SSR and client-side navigation
const { data: game, pending, error } = await useAsyncData(`game-${gameId}`, () => api.game.getById(gameId))

/* function selectMode(mode: string) {
    gameMode.value = mode
} */

function goBack() {
  router.push('/games')
}

function restartGame() {
    refreshKey.value++
}

function getGameTypeName(type: number) {
    const types = ['Placement sur Carte', 'Traduction', 'Généalogie', 'Quiz']
    return types[type] || 'Inconnu'
}
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.5s ease-out;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
