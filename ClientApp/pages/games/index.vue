<template>
  <div class="flex-grow flex flex-col items-center justify-center relative overflow-hidden py-12 min-h-screen">
    <!-- Background Gradients (Modern & Minimal) - Matches Landing Page -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="relative z-10 w-full max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      
      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#C39712]"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 bg-[#0B3152]/40 backdrop-blur-lg border border-red-500/20 rounded-2xl">
        <p class="text-red-400  text-lg">Une erreur est survenue lors du chargement des jeux.</p>
      </div>
      
      <div v-else-if="filteredGames && filteredGames.length > 0" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        <div v-for="game in filteredGames" :key="game.id" 
             class="group bg-white/5 border border-white/10 backdrop-blur-md overflow-hidden rounded-3xl hover:bg-white/10 transition-all duration-500 hover:-translate-y-2 flex flex-col">
          <div class="h-56 relative overflow-hidden">
            <img v-if="game.thumbnailPath" :src="game.thumbnailPath" alt="Game thumbnail" class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700" />
            <div v-else class="w-full h-full flex items-center justify-center bg-gradient-to-br from-[#082540] to-[#0B3152] text-[#C39712]">
               <!-- Icon based on type -->
               <svg v-if="game.type === 3" xmlns="http://www.w3.org/2000/svg" class="h-20 w-20 opacity-50" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
               </svg>
               <span v-else class="text-5xl font-bold opacity-30 ">{{ getGameTypeName(game.type).charAt(0) }}</span>
            </div>
            <div v-if="game.isPremium" class="absolute top-4 right-4 bg-gradient-to-r from-[#C39712] to-[#AA8A2E] text-[#082540] text-[10px] font-bold px-3 py-1 rounded-full shadow-lg">
              PREMIUM
            </div>
            <div class="absolute inset-0 bg-gradient-to-t from-[#082540] to-transparent opacity-60"></div>
          </div>

          <div class="p-8 flex-1 flex flex-col">
            <div class="flex items-center justify-between mb-4">
                <span class="text-xs font-bold text-[#C39712] bg-[#C39712]/10 px-3 py-1 rounded-full uppercase tracking-wider ">
                  {{ getGameTypeName(game.type) }}
                </span>
            </div>
            <h3 class="text-2xl font-bold text-white mb-4  group-hover:text-[#C39712] transition-colors line-clamp-2">
              {{ game.title }}
            </h3>
            
            <div class="mt-auto pt-6">
              <NuxtLink :to="`/games/${game.id}`" 
                class="group/btn relative block w-full text-center bg-white text-[#082540] rounded-full py-3 px-4 text-lg font-bold hover:bg-gray-100 transition-all duration-300 shadow-lg">
                <span class="flex items-center justify-center gap-2">
                  Commencer à jouer
                  <svg class="w-5 h-5 transform group-hover/btn:translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7l5 5m0 0l-5 5m5-5H6"></path></svg>
                </span>
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>
      
      <div v-if="!pending && (!filteredGames || filteredGames.length === 0)" class="flex flex-col items-center justify-center min-h-[40vh] text-center bg-white/5 backdrop-blur-md border border-dashed border-white/10 rounded-3xl p-12">
          <p class="text-gray-400 text-2xl ">Aucun jeu disponible pour le moment.</p>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import type { Game } from '~/types'

definePageMeta({
  middleware: 'auth'
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
