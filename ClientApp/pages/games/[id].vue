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
      <NuxtLink to="/games" class="inline-flex items-center text-xs font-bold uppercase tracking-widest text-brand-parchment/60 hover:text-brand-gold mb-10 transition-all group">
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
          <div class="mb-12 text-center">
              <h1 class="text-4xl md:text-5xl font-serif-title font-bold tracking-tight text-brand-parchment mb-4 drop-shadow-md">{{ game.title }}</h1>
              <div class="h-1 w-24 bg-brand-gold mx-auto rounded-full opacity-60 mb-6"></div>
              <p v-if="game.description" class="text-brand-parchment/80 font-sans-body text-lg max-w-2xl mx-auto leading-relaxed">{{ game.description }}</p>
          </div>

          <!-- Mode Selection Screen -->
          <div v-if="gameMode === 'selection'" class="flex flex-col md:flex-row gap-8 justify-center items-stretch max-w-5xl mx-auto">
              
              <!-- Card Solo -->
              <button @click="selectMode('single')" 
                  class="flex-1 bg-texture-parchment rounded-3xl p-8 border-2 border-brand-gold/20 hover:border-brand-gold/60 transition-all hover:scale-105 group text-left relative overflow-hidden shadow-2xl min-h-[300px] flex flex-col">
                  <div class="absolute top-0 right-0 p-6 opacity-10 group-hover:opacity-20 transition-opacity">
                      <svg class="w-32 h-32 text-brand-dark" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 3c1.66 0 3 1.34 3 3s-1.34 3-3 3-3-1.34-3-3 1.34-3 3-3zm0 14.2c-2.5 0-4.71-1.28-6-3.22.03-1.99 4-3.08 6-3.08 1.99 0 5.97 1.09 6 3.08-1.29 1.94-3.5 3.22-6 3.22z"/></svg>
                  </div>
                  <div class="relative z-10">
                      <h3 class="text-2xl font-serif-title font-bold text-brand-dark mb-4 group-hover:text-brand-gold transition-colors">Voyageur Solitaire</h3>
                      <p class="text-brand-wood font-sans-body mb-8 leading-relaxed">
                          Arpentez les chemins du savoir à votre rythme. Testez vos connaissances et découvrez les secrets de l'histoire.
                      </p>
                      <div class="mt-auto inline-flex items-center text-brand-dark font-bold uppercase tracking-widest text-sm border-b-2 border-brand-gold pb-1 group-hover:pl-2 transition-all">
                          Commencer en Solo
                          <svg class="w-4 h-4 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H3"/></svg>
                      </div>
                  </div>
              </button>

              <!-- Card Multi -->
              <button @click="selectMode('multi')" 
                  class="flex-1 bg-[#0B3152] rounded-3xl p-8 border-2 border-brand-gold/30 hover:border-brand-gold transition-all hover:scale-105 group text-left relative overflow-hidden shadow-2xl min-h-[300px] flex flex-col">
                  <!-- Decorative pattern -->
                   <div class="absolute inset-0 pattern-geometric opacity-10"></div>
                   <div class="absolute top-0 right-0 p-6 opacity-10 group-hover:opacity-20 transition-opacity text-brand-parchment">
                      <svg class="w-32 h-32" fill="currentColor" viewBox="0 0 24 24"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>
                  </div>
                  <div class="relative z-10">
                      <h3 class="text-2xl font-serif-title font-bold text-brand-parchment mb-4 group-hover:text-brand-gold transition-colors">En Caravane</h3>
                      <p class="text-brand-parchment/70 font-sans-body mb-8 leading-relaxed">
                          Défiez vos amis ou rejoignez d'autres érudits. La compétition stimule l'apprentissage !
                          <span class="block text-xs mt-2 text-brand-gold opacity-80">(Multijoueur en temps réel)</span>
                      </p>
                      <div class="mt-auto inline-flex items-center text-brand-parchment font-bold uppercase tracking-widest text-sm border-b-2 border-brand-gold/50 pb-1 group-hover:pl-2 transition-all">
                          Créer ou Rejoindre
                          <svg class="w-4 h-4 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 8l4 4m0 0l-4 4m4-4H3"/></svg>
                      </div>
                  </div>
              </button>

          </div>

          <!-- Game View (Solo or Multi) -->
          <div v-else class="animate-fade-in-up">
              <!-- Back to selection header -->
              <button @click="gameMode = 'selection'" class="mb-6 flex items-center text-brand-parchment/40 hover:text-brand-gold text-xs font-bold uppercase tracking-widest transition-colors mb-4">
                  <svg class="w-3 h-3 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"/></svg>
                  Changer de mode
              </button>

              <ClientOnly>
                  <!-- Type 3 is Quiz -->
                  <GameQuizGame 
                     v-if="game.type === 3" 
                     :game-content="game.contentJson" 
                     :title="game.title"
                     :key="refreshKey"
                     :is-multiplayer="gameMode === 'multi'"
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

<script setup>
const route = useRoute()
const router = useRouter()
const config = useRuntimeConfig()
const gameId = route.params.id
const refreshKey = ref(0)
const gameMode = ref('selection') // 'selection' | 'single' | 'multi'

const { data: game, pending, error } = await useFetch(`${config.public.apiBase}/game/${gameId}`)

function selectMode(mode) {
    gameMode.value = mode
}

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
