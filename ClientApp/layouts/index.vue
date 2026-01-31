<template>
  <div class="flex-grow flex flex-col items-center justify-start relative overflow-hidden py-6">
    <!-- Background Layers -->
    <div class="absolute inset-0 z-0 pointer-events-none">
       <div class="absolute inset-0 pattern-geometric opacity-[0.03]"></div>
       <div class="absolute inset-0 bg-texture-wood opacity-50 mix-blend-overlay"></div>
       <div class="absolute bottom-0 right-0 w-[500px] h-[500px] bg-brand-gold/5 blur-[100px] rounded-full"></div>
    </div>

    <div class="relative z-10 w-full max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pt-4 pb-8 md:pt-6 md:pb-12">
      
      <!-- Lobby Manager -->
      <PartyManager />

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-brand-gold"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 bg-brand-dark/40 backdrop-blur-lg border border-red-500/20 rounded-2xl">
        <p class="text-red-400 font-sans-body text-base">Une erreur est survenue lors du chargement des jeux.</p>
      </div>
      
      <!-- Desktop/Tablet Grid View -->
      <div v-if="games && games.length > 0" class="hidden md:grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 lg:gap-6">
        <div v-for="game in games" :key="game.id" 
             class="group bg-texture-parchment border border-brand-gold/30 overflow-hidden rounded-xl hover:shadow-[0_20px_50px_rgba(8,_37,_64,_0.5)] transition-all duration-300 hover:-translate-y-1 flex flex-col relative">
            
             <!-- Corner Decor -->
             <div class="absolute top-0 right-0 w-16 h-16 bg-brand-gold/5 rounded-bl-[80px] pointer-events-none transition-transform duration-500 group-hover:scale-110"></div>

          <div class="h-32 relative overflow-hidden border-b border-brand-gold/10">
            <img v-if="game.thumbnailPath" :src="game.thumbnailPath" alt="Game thumbnail" class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-700 filter sepia-[.2] group-hover:sepia-0" />
          </div>

          <div class="p-4 flex-1 flex flex-col relative z-10">
            <h3 class="text-lg font-serif-title font-bold text-brand-dark mb-2 group-hover:text-brand-wood transition-colors line-clamp-1">
              {{ game.title }}
            </h3>
            
            <div class="mt-auto pt-3">
              <button @click="handlePlay()" 
                :disabled="isConnected && !isHost"
                :class="[
                  isConnected && !isHost ? 'opacity-50 cursor-not-allowed bg-gray-500' : 'bg-brand-wood hover:bg-brand-dark cursor-pointer active:scale-95',
                  isConnected && isHost ? 'border-brand-gold border-2' : 'border-brand-gold/20'
                ]"
                class="w-full text-center text-brand-parchment rounded-lg py-1.5 px-2 text-sm font-bold font-serif-title transition-all duration-300 shadow-md border">
                {{ isConnected ? (isHost ? 'Lancer la partie' : 'En attente de l\'hôte') : 'Jouer' }}
              </button>
            </div>
          </div>
        </div>
      </div>


      <!-- Mobile List View (Compact) -->
      <div v-if="games && games.length > 0" class="md:hidden flex flex-col gap-2 px-0 sm:px-2">
          <div v-for="game in games" :key="game.id" 
             @click="(!isConnected || isHost) && handlePlay()"
             :class="isConnected && !isHost ? 'opacity-70 grayscale-[0.5]' : 'active:scale-[0.98] cursor-pointer'"
             class="bg-white p-2 rounded-xl shadow-sm border border-gray-100 flex items-center gap-3 transition-transform">
              
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
              </div>
              
              <!-- Play Icon -->
              <div class="text-brand-gold pr-1" v-if="!isConnected || isHost">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 opacity-80" viewBox="0 0 20 20" fill="currentColor">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" clip-rule="evenodd" />
                  </svg>
              </div>
               <div class="text-gray-400 pr-1 text-[10px] font-bold uppercase" v-else>
                  En attente
              </div>
          </div>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import type { Game } from '~/types'
import PartyManager from '~/components/game/PartyManager.vue'
useHead({
  title: 'Jeux & Activités'
})

// Static Games List (as requested to not be dynamic)
const games = ref<Game[]>([
    {
        id: "quizz",
        title: "Quiz Culture Générale",
        description: "Testez vos connaissances sur la culture islamique et l'histoire de l'Andalousie.",
        thumbnailPath: "https://pixabay.com/fr/images/download/x-2492009_1920.jpg",
    }
])
const pending = ref(false)
const error = ref(null)

const { isConnected, isHost, startGame, restoreSession } = useMultiplayer()
const router = useRouter()

onMounted(() => {
    restoreSession()
})

const handlePlay = async () => {
    if (isConnected.value) {
        router.push(`/games/quizz?isSolo=false`)
    } else {
        router.push(`/games/quizz?isSolo=true`)
    }
}

</script>
