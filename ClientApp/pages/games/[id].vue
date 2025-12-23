<template>
  <div class="min-h-screen bg-[#082540] py-12 relative overflow-hidden font-sans">
    <!-- Background Gradients (Modern & Minimal) -->
    <div class="fixed inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-4xl mx-auto px-4 relative z-10">
      <!-- Back Link -->
      <NuxtLink to="/games" class="inline-flex items-center text-xs font-black uppercase tracking-widest text-gray-500 hover:text-white mb-10 transition-all group">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-2 transition-transform group-hover:-translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        Retour aux jeux
      </NuxtLink>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-white opacity-20"></div>
      </div>
      
      <div v-else-if="error" class="bg-red-500/10 border border-red-500/20 text-red-400 px-6 py-4 rounded-2xl text-center">
        <strong class="font-bold">Erreur !</strong>
        <span class="block sm:inline ml-2">Impossible de charger le jeu.</span>
      </div>
      
      <div v-else-if="game" class="animate-fade-in">
         <div class="mb-12 text-center">
             <h1 class="text-4xl font-black tracking-tighter uppercase text-white mb-3">{{ game.title }}</h1>
             <p v-if="game.description" class="text-gray-400 font-medium text-lg max-w-2xl mx-auto">{{ game.description }}</p>
         </div>

         <ClientOnly>
             <!-- Type 3 is Quiz -->
             <GameQuizGame 
                v-if="game.type === 3" 
                :game-content="game.contentJson" 
                :title="game.title"
                :key="refreshKey"
                @close="goBack"
                @retry="restartGame"
             />
             <div v-else class="text-center p-12 bg-white/5 backdrop-blur-xl rounded-3xl border border-white/10 shadow-2xl">
                 <p class="text-gray-500 font-bold uppercase tracking-widest text-sm">Ce type de jeu ({{ getGameTypeName(game.type) }}) n'est pas encore disponible.</p>
             </div>
         </ClientOnly>
      </div>
    </div>
  </div>
</template>

<script setup>
const route = useRoute()
const router = useRouter()
const config = useRuntimeConfig()
const gameId = route.params.id
const refreshKey = ref(0) // Used to force component re-render

const { data: game, pending, error } = await useFetch(`${config.public.apiBase}/game/${gameId}`)

function goBack() {
  router.push('/games')
}

function restartGame() {
    refreshKey.value++
}

function getGameTypeName(type) {
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
