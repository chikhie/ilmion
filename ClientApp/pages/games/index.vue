<template>
  <div class="flex-grow flex flex-col items-center justify-center relative overflow-hidden py-12 min-h-screen">
    <!-- Background Layers -->
    <div class="absolute inset-0 z-0 pointer-events-none">
       <div class="absolute inset-0 pattern-geometric opacity-[0.03]"></div>
       <div class="absolute inset-0 bg-texture-wood opacity-50 mix-blend-overlay"></div>
       <div class="absolute bottom-0 right-0 w-[500px] h-[500px] bg-brand-gold/5 blur-[100px] rounded-full"></div>
    </div>

    <div class="relative z-10 w-full max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      
      <div class="text-center mb-16">
        <h1 class="text-4xl md:text-5xl font-serif-title font-bold text-brand-parchment mb-4 drop-shadow-md">Jeux & Activités</h1>
        <div class="h-1 w-24 bg-brand-gold mx-auto rounded-full opacity-60"></div>
      </div>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-brand-gold"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 bg-brand-dark/40 backdrop-blur-lg border border-red-500/20 rounded-2xl">
        <p class="text-red-400 font-sans-body text-lg">Une erreur est survenue lors du chargement des jeux.</p>
      </div>
      
      <!-- Desktop/Tablet Grid View -->
      <div v-if="filteredGames && filteredGames.length > 0" class="hidden md:grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        <div v-for="game in filteredGames" :key="game.id" 
             class="group bg-texture-parchment border border-brand-gold/30 overflow-hidden rounded-xl hover:shadow-[0_20px_50px_rgba(8,_37,_64,_0.5)] transition-all duration-500 hover:-translate-y-2 flex flex-col relative">
            
             <!-- Existing Desktop Card Content -->
             <div class="absolute top-0 right-0 w-24 h-24 bg-brand-gold/5 rounded-bl-[100px] pointer-events-none transition-transform duration-500 group-hover:scale-110"></div>

          <div class="h-48 relative overflow-hidden border-b border-brand-gold/10">
            <img v-if="game.thumbnailPath" :src="game.thumbnailPath" alt="Game thumbnail" class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700 filter sepia-[.2] group-hover:sepia-0" />
            <div v-else class="w-full h-full flex items-center justify-center bg-brand-wood text-brand-gold">
               <svg v-if="game.type === 3" xmlns="http://www.w3.org/2000/svg" class="h-20 w-20 opacity-50" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
               </svg>
               <span v-else class="text-6xl font-serif-title font-bold opacity-30">{{ getGameTypeName(game.type).charAt(0) }}</span>
            </div>
            
            <div v-if="game.isPremium" class="absolute top-4 left-4 bg-brand-gold text-brand-dark text-[10px] font-bold px-3 py-1 rounded shadow-lg uppercase tracking-wider">
              PREMIUM
            </div>
          </div>

          <div class="p-8 flex-1 flex flex-col relative z-10">
            <div class="flex items-center justify-between mb-4">
                <span class="text-xs font-bold text-brand-wood bg-brand-wood/10 px-3 py-1 rounded-full uppercase tracking-wider font-sans-body">
                  {{ getGameTypeName(game.type) }}
                </span>
            </div>
            <h3 class="text-2xl font-serif-title font-bold text-brand-dark mb-4 group-hover:text-brand-wood transition-colors line-clamp-2">
              {{ game.title }}
            </h3>
            
            <div class="mt-auto pt-6 flex gap-3">
              <NuxtLink :to="`/games/${game.id}?mode=single`" 
                class="group/btn relative flex-1 text-center bg-brand-wood text-brand-parchment rounded py-3 px-2 text-base font-bold font-serif-title hover:bg-brand-dark transition-all duration-300 shadow-md border border-brand-gold/20">
                Solo
              </NuxtLink>
              <NuxtLink :to="`/games/${game.id}?mode=multi`" 
                class="group/btn relative flex-1 text-center bg-[#0B3152] text-brand-parchment rounded py-3 px-2 text-base font-bold font-serif-title hover:bg-brand-dark transition-all duration-300 shadow-md border border-brand-gold/20">
                Multi
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>

      <!-- Mobile List View (Plato Style) -->
      <div v-if="filteredGames && filteredGames.length > 0" class="md:hidden flex flex-col gap-3 px-1">
          <NuxtLink v-for="game in filteredGames" :key="game.id" 
             :to="`/games/${game.id}`"
             class="bg-white p-3 rounded-2xl shadow-sm border border-gray-100 flex items-center gap-4 active:scale-[0.98] transition-transform">
              
              <!-- Thumbnail (Rounded Square) -->
              <div class="w-16 h-16 flex-shrink-0 rounded-2xl overflow-hidden shadow-inner relative">
                  <img v-if="game.thumbnailPath" :src="game.thumbnailPath" class="w-full h-full object-cover" />
                  <div v-else class="w-full h-full bg-brand-wood/10 flex items-center justify-center text-brand-wood">
                      <span class="font-serif-title font-bold text-lg">{{ getGameTypeName(game.type).charAt(0) }}</span>
                  </div>
              </div>
              
              <!-- Content -->
              <div class="flex-1 min-w-0">
                  <h3 class="text-base font-bold text-gray-900 leading-tight mb-0.5 truncate">{{ game.title }}</h3>
                  <div class="flex items-center gap-2 text-xs text-brand-wood/60">
                      <span class="font-medium uppercase tracking-wider text-[10px]">{{ getGameTypeName(game.type) }}</span>
                      <span v-if="game.difficulty" class="w-1 h-1 bg-gray-300 rounded-full"></span>
                      <span v-if="game.difficulty" class="capitalize">{{ game.difficulty }}</span>
                  </div>
              </div>
              
              <!-- Chevron -->
              <div class="text-gray-300 pr-1">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                      <path fill-rule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clip-rule="evenodd" />
                  </svg>
              </div>
          </NuxtLink>
      </div>
      
      <div v-if="!pending && (!filteredGames || filteredGames.length === 0)" class="flex flex-col items-center justify-center min-h-[40vh] text-center bg-brand-dark/20 backdrop-blur-md border border-brand-gold/10 rounded-3xl p-12">
          <p class="text-brand-parchment/60 text-2xl font-serif-title italic">Aucun jeu disponible pour le moment.</p>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import type { Game } from '~/types'

definePageMeta({
  // middleware: 'auth' - Removed to allow public access
})

useHead({
  title: 'Jeux & Activités'
})

const api = useApi()
const authStore = useAuthStore()
// api.game uses the refactored GameService instance
const { data: games, pending, error } = await useAsyncData<Game[]>('games', async () => {
  try {
    return await api.game.getAll()
  } catch {
    return []
  }
})

const getGameTypeName = (type: number) => {
  switch (type) {
    case 0: return 'Placement sur Carte'
    case 1: return 'Quiz'
    default: return 'Jeu'
  }
}

const filteredGames = computed(() => {
  if (!games.value || !Array.isArray(games.value)) {
      console.warn('games.value is not an array:', games.value)
      return []
  }
  // Return all games
  return games.value
})
</script>
