<template>
  <div class="flex-grow flex flex-col items-center justify-center relative overflow-hidden py-12 min-h-screen">
    <!-- Background Layers -->
    <div class="absolute inset-0 z-0 pointer-events-none">
       <div class="absolute inset-0 pattern-geometric opacity-[0.03]"></div>
       <div class="absolute inset-0 bg-texture-wood opacity-50 mix-blend-overlay"></div>
       <div class="absolute bottom-0 right-0 w-[500px] h-[500px] bg-brand-gold/5 blur-[100px] rounded-full"></div>
    </div>

    <div class="relative z-10 w-full max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 md:py-12">
      
      <div class="text-center mb-8 md:mb-12">
        <h1 class="text-3xl md:text-4xl lg:text-5xl font-serif-title font-bold text-brand-parchment mb-2 drop-shadow-md">Jeux & Activités</h1>
        <div class="h-1 w-16 md:w-24 bg-brand-gold mx-auto rounded-full opacity-60"></div>
      </div>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-brand-gold"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 bg-brand-dark/40 backdrop-blur-lg border border-red-500/20 rounded-2xl">
        <p class="text-red-400 font-sans-body text-base">Une erreur est survenue lors du chargement des jeux.</p>
      </div>
      
      <!-- Desktop/Tablet Grid View -->
      <div v-if="filteredGames && filteredGames.length > 0" class="hidden md:grid grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 lg:gap-6">
        <div v-for="game in filteredGames" :key="game.id" 
             class="group bg-texture-parchment border border-brand-gold/30 overflow-hidden rounded-xl hover:shadow-[0_20px_50px_rgba(8,_37,_64,_0.5)] transition-all duration-300 hover:-translate-y-1 flex flex-col relative">
            
             <!-- Corner Decor -->
             <div class="absolute top-0 right-0 w-16 h-16 bg-brand-gold/5 rounded-bl-[80px] pointer-events-none transition-transform duration-500 group-hover:scale-110"></div>

          <div class="h-40 relative overflow-hidden border-b border-brand-gold/10">
            <img v-if="game.thumbnailPath" :src="game.thumbnailPath" alt="Game thumbnail" class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-700 filter sepia-[.2] group-hover:sepia-0" />
            <div v-else class="w-full h-full flex items-center justify-center bg-brand-wood text-brand-gold">
               <svg v-if="game.type === 3" xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 opacity-50" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
               </svg>
              </div>
            
            <div v-if="game.isPremium" class="absolute top-3 left-3 bg-brand-gold text-brand-dark text-[10px] font-bold px-2 py-0.5 rounded shadow-lg uppercase tracking-wider">
              PREMIUM
            </div>
          </div>

          <div class="p-5 flex-1 flex flex-col relative z-10">
            <h3 class="text-xl font-serif-title font-bold text-brand-dark mb-2 group-hover:text-brand-wood transition-colors line-clamp-1">
              {{ game.title }}
            </h3>
            
            <div class="mt-auto pt-4">
              <button @click="handlePlay(game.id)" 
                class="w-full text-center bg-brand-wood text-brand-parchment rounded-lg py-2 px-2 text-sm font-bold font-serif-title hover:bg-brand-dark transition-all duration-300 shadow-md border border-brand-gold/20 cursor-pointer active:scale-95">
                Jouer
              </button>
            </div>
          </div>
        </div>
      </div>


      <!-- Mobile List View (Compact) -->
      <div v-if="filteredGames && filteredGames.length > 0" class="md:hidden flex flex-col gap-2 px-0 sm:px-2">
          <div v-for="game in filteredGames" :key="game.id" 
             @click="handlePlay(game.id)"
             class="bg-white p-2 rounded-xl shadow-sm border border-gray-100 flex items-center gap-3 active:scale-[0.98] transition-transform cursor-pointer">
              
              <!-- Thumbnail -->
              <div class="w-14 h-14 flex-shrink-0 rounded-lg overflow-hidden shadow-inner relative">
                  <img v-if="game.thumbnailPath" :src="game.thumbnailPath" class="w-full h-full object-cover" />
                  <div v-else class="w-full h-full bg-brand-wood/10 flex items-center justify-center text-brand-wood">
                     <svg class="w-6 h-6 opacity-40" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.752 11.168l-3.197-2.132A1 1 0 0010 9.87v4.263a1 1 0 001.555.832l3.197-2.132a1 1 0 000-1.664z"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
                  </div>
              </div>
              
              <!-- Content -->
              <div class="flex-1 min-w-0">
                  <h3 class="text-sm font-bold text-gray-900 leading-tight mb-0.5 truncate">{{ game.title }}</h3>
                  <div class="flex items-center gap-1.5 text-[10px] text-brand-wood/70">
                      <span v-if="game.difficulty" class="w-1 h-1 bg-brand-gold rounded-full"></span>
                      <span v-if="game.difficulty" class="capitalize">{{ game.difficulty }}</span>
                  </div>
              </div>
              
              <!-- Play Icon -->
              <div class="text-brand-gold pr-1">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 opacity-80" viewBox="0 0 20 20" fill="currentColor">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" clip-rule="evenodd" />
                  </svg>
              </div>
          </div>
      </div>
      
      <div v-if="!pending && (!filteredGames || filteredGames.length === 0)" class="flex flex-col items-center justify-center min-h-[40vh] text-center bg-brand-dark/20 backdrop-blur-md border border-brand-gold/10 rounded-3xl p-8">
          <p class="text-brand-parchment/60 text-xl font-serif-title italic">Aucun jeu disponible pour le moment.</p>
      </div>

    </div>
    
    <PartyManager />
  </div>
</template>

<script setup lang="ts">
import type { Game } from '~/types'
import PartyManager from '~/components/game/PartyManager.vue'

definePageMeta({
  // middleware: 'auth' - Removed to allow public access
})

useHead({
  title: 'Jeux & Activités'
})

import { GameService } from '~/services/game.service'
import { ApiClient } from '~/services/api.client'
const api = new ApiClient()
const gameService = new GameService(api)
const { data: games, pending, error } = await useAsyncData<Game[]>('games', async () => {
  try {
    return await gameService.getAll()
  } catch {
    return []
  }
})

const { isConnected, isHost, startGame } = useMultiplayer()
const router = useRouter()

const handlePlay = async (gameId: string) => {
    if (isConnected.value) {
        if (isHost.value) {
            await startGame(gameId)
        } else {
             // If not host but in lobby, maybe show message "Wait for host"? 
             // Or allow creating separate solo game? 
             // Requirement says "joueur ne doit pas être automatiquement dans un lobby".
             // If they ARE in a lobby, expected behavior is "Multiplayer".
             alert("En attente que l'hôte lance la partie...")
        }
    } else {
        router.push(`/games/${gameId}`)
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
