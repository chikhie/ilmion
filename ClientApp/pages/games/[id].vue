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

          <!-- Mode Selection Screen -->
          <div v-if="gameMode === 'selection'">
              
              <!-- Desktop Selection Cards -->
              <div class="hidden md:flex flex-row gap-8 justify-center items-stretch max-w-5xl mx-auto">
                  
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

              <!-- Mobile Detail View (Crunchyroll Style) -->
              <div class="md:hidden flex flex-col relative -mx-4 -mt-12">
                  <!-- Hero Image -->
                  <div class="h-[45vh] w-full relative">
                      <img v-if="game.thumbnailPath" :src="game.thumbnailPath" class="w-full h-full object-cover" />
                      <div v-else class="w-full h-full bg-brand-wood flex items-center justify-center">
                          <span class="text-6xl font-serif-title opacity-20">{{ getGameTypeName(game.type).charAt(0) }}</span>
                      </div>
                      <div class="absolute inset-0 bg-gradient-to-t from-brand-dark via-brand-dark/50 to-transparent"></div>
                      
                      <!-- Floating Back Button -->
                      <button @click="router.back()" class="absolute top-4 left-4 p-2 bg-black/30 backdrop-blur rounded-full text-white">
                         <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"/></svg>
                      </button>
                  </div>

                  <!-- Content Body -->
                  <div class="relative -mt-12 px-5 pb-24">
                      <!-- Tags -->
                      <div class="flex gap-2 mb-3">
                          <span class="px-2 py-1 bg-brand-gold text-brand-dark text-[10px] font-bold uppercase tracking-wider rounded">{{ getGameTypeName(game.type) }}</span>
                          <span class="px-2 py-1 bg-white/10 text-brand-parchment text-[10px] font-bold uppercase tracking-wider rounded border border-white/10">{{ game.difficulty || 'Normal' }}</span>
                      </div>

                      <!-- Title -->
                      <h1 class="text-3xl font-serif-title font-bold text-white mb-4 leading-tight drop-shadow-lg">{{ game.title }}</h1>
                      
                      <!-- Description -->
                      <p class="text-white/80 font-sans-body text-sm leading-relaxed mb-8">
                          {{ game.description }}
                      </p>

                      <!-- Action Buttons (Sticky-ish feel) -->
                      <div class="grid grid-cols-2 gap-3">
                          <button @click="selectMode('single')" 
                              class="py-3.5 bg-brand-gold text-brand-dark rounded-xl font-bold uppercase tracking-widest text-sm shadow-lg active:scale-95 transition-transform flex items-center justify-center gap-2">
                              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.752 11.168l-3.197-2.132A1 1 0 0010 9.87v4.263a1 1 0 001.555.832l3.197-2.132a1 1 0 000-1.664z"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
                              <span>Solo</span>
                          </button>
                          
                          <button @click="selectMode('multi')" 
                              class="py-3.5 bg-white/10 backdrop-blur border border-white/20 text-white rounded-xl font-bold uppercase tracking-widest text-sm shadow-lg active:scale-95 transition-transform flex items-center justify-center gap-2">
                              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"/></svg>
                              <span>Multi</span>
                          </button>
                      </div>
                  </div>
              </div>
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
                     :game-content="game.content" 
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

<script setup lang="ts">
const route = useRoute()
const router = useRouter()
const config = useRuntimeConfig()
const gameId = route.params.id as string
const refreshKey = ref(0)
const modeParam = route.query.mode as string
const gameMode = ref(modeParam === 'single' || modeParam === 'multi' ? modeParam : 'selection')

const api = useApi()

// Use useAsyncData to compatible with SSR and client-side navigation
const { data: game, pending, error } = await useAsyncData(`game-${gameId}`, () => api.game.getById(gameId))

function selectMode(mode: string) {
    gameMode.value = mode
}

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
