<template>
  <div class="min-h-screen bg-brand-dark flex flex-col relative overflow-hidden font-sans-body">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.03]" style="background-image: url('/patterns/arabesque.png')"></div>
        <div class="absolute top-[-10%] right-[-10%] w-[500px] h-[500px] bg-brand-gold/5 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] left-[-10%] w-[600px] h-[600px] bg-brand-wood/10 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Global Header -->
    <AppHeader />

    <main class="flex-grow container mx-auto px-4 pt-6 pb-12 relative z-10 max-w-6xl">
      
      <!-- Welcome Section -->
      <div class="mb-8 text-center md:text-left flex items-baseline gap-4">
        <h1 class="text-2xl md:text-3xl font-serif-title font-bold tracking-wide text-brand-parchment animate-fade-in">
          <span class="opacity-70">Salam,</span> 
          <span class="text-brand-gold drop-shadow-md"> {{ authStore.user?.username }}</span>
        </h1>
        <p class="text-brand-parchment/60 text-sm md:text-base font-light hidden md:block">Continue ton apprentissage.</p>
      </div>

      <!-- Stats Overview (Compact) -->
      <div class="grid grid-cols-2 lg:grid-cols-3 gap-4 mb-8 animate-fade-in-up">
        
        <!-- Modules Mastered -->
        <div class="bg-brand-wood/10 border border-brand-gold/10 p-4 rounded-xl backdrop-blur-md relative overflow-hidden group hover:border-brand-gold/30 transition-colors duration-300 flex items-center gap-4">
            <div class="p-3 bg-brand-gold/5 rounded-full group-hover:bg-brand-gold/10 transition-colors">
                <i class="fas fa-certificate text-2xl text-brand-gold"></i>
            </div>
            <div>
                 <div class="text-brand-parchment/60 font-bold uppercase text-[0.65rem] tracking-widest">Maîtrisés</div>
                 <div class="text-2xl font-serif-title font-bold text-brand-parchment leading-none">{{ progression?.modulesMasteredCount || 0 }}</div>
            </div>
        </div>

        <!-- Total XP -->
        <div class="bg-brand-wood/10 border border-brand-gold/10 p-4 rounded-xl backdrop-blur-md relative overflow-hidden group hover:border-brand-gold/30 transition-colors duration-300 flex items-center gap-4">
             <div class="p-3 bg-brand-gold/5 rounded-full group-hover:bg-brand-gold/10 transition-colors">
                <i class="fas fa-fire text-2xl text-brand-gold"></i>
            </div>
            <div>
              <div class="text-brand-parchment/60 font-bold uppercase text-[0.65rem] tracking-widest">Total XP</div>
              <div class="text-2xl font-serif-title font-bold text-brand-gold drop-shadow-[0_0_5px_rgba(195,151,18,0.3)] leading-none">{{ progression?.globalPoints || 0 }}</div>
            </div>
        </div>

        <!-- Rank -->
        <div class="bg-brand-wood/10 border border-brand-gold/10 p-4 rounded-xl backdrop-blur-md relative overflow-hidden group hover:border-brand-gold/30 transition-colors duration-300 col-span-2 lg:col-span-1 flex items-center gap-4">
            <div class="p-3 bg-brand-gold/5 rounded-full group-hover:bg-brand-gold/10 transition-colors">
                <i class="fas fa-chess-knight text-2xl text-brand-gold"></i>
            </div>
            <div>
               <div class="text-brand-parchment/60 font-bold uppercase text-[0.65rem] tracking-widest">Niveau</div>
               <div class="text-2xl font-serif-title font-bold text-brand-parchment leading-none">Apprenti</div> 
           </div>
        </div>
      </div>

      <!-- Themes Grid -->
      <h2 class="text-xl md:text-2xl font-serif-title font-bold mb-6 flex items-center gap-3 text-brand-parchment animate-fade-in delay-200">
        <span class="w-8 h-8 rounded-full bg-brand-gold text-brand-dark flex items-center justify-center text-sm shadow-lg">
            <i class="fas fa-book-open"></i>
        </span>
        Programme
      </h2>
      
      <div v-if="pendingThemes" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-brand-gold"></div>
      </div>

      <div v-else-if="errorThemes" class="text-center py-10 px-6 text-red-300 bg-red-900/10 rounded-xl border border-red-500/20 backdrop-blur-sm">
        <i class="fas fa-exclamation-circle text-2xl mb-2"></i>
        <p>Une erreur est survenue lors du chargement des contenus.</p>
      </div>

      <div v-else class="grid grid-cols-1 gap-4 animate-fade-in-up delay-300">
        <!-- Theme Card (Compact) -->
        <div v-for="theme in themes" :key="theme.id" 
             class="bg-brand-wood/5 border border-brand-gold/10 rounded-xl overflow-hidden backdrop-blur-sm transition-all duration-300 hover:shadow-lg hover:shadow-brand-gold/5"
             :class="{ 'ring-1 ring-brand-gold/30 bg-brand-wood/10': expandedThemes.includes(theme.id) }">
            
            <!-- Theme Header (Clickable & Compact) -->
            <div class="p-4 flex flex-col md:flex-row gap-4 md:items-center justify-between cursor-pointer group" @click="toggleTheme(theme.id)">
                <div class="flex-1">
                     <div class="flex items-center gap-3 mb-1">
                        <span class="px-2 py-0.5 rounded text-[0.6rem] bg-brand-gold/10 text-brand-gold font-bold uppercase tracking-wider">Thème</span>
                        <h3 class="text-lg font-bold text-brand-parchment font-serif-title tracking-wide group-hover:text-brand-gold transition-colors">{{ theme.title }}</h3>
                     </div>
                     <p class="text-brand-parchment/60 text-sm leading-snug max-w-4xl truncate">{{ theme.description }}</p>
                </div>
                 <div class="flex items-center gap-3 self-end md:self-center">
                    <span class="text-brand-parchment/40 text-[0.65rem] font-medium uppercase tracking-widest group-hover:text-brand-gold/70 transition-colors">
                        {{ expandedThemes.includes(theme.id) ? 'Fermer' : 'Ouvrir' }}
                    </span>
                    <div class="h-8 w-8 rounded-full bg-brand-dark border border-brand-gold/20 flex items-center justify-center text-brand-gold transition-all duration-300 group-hover:border-brand-gold" :class="{ 'rotate-180 bg-brand-gold text-brand-dark': expandedThemes.includes(theme.id) }">
                        <i class="fas fa-chevron-down text-xs"></i>
                    </div>
                </div>
            </div>

            <!-- Expanded Content (Subjects & Parts) -->
            <transition
                enter-active-class="transition-[max-height] duration-500 ease-in-out overflow-hidden"
                enter-from-class="max-h-0 opacity-0"
                enter-to-class="max-h-[2000px] opacity-100"
                leave-active-class="transition-[max-height] duration-300 ease-in-out overflow-hidden"
                leave-from-class="max-h-[2000px] opacity-100"
                leave-to-class="max-h-0 opacity-0"
            >
                <div v-show="expandedThemes.includes(theme.id)" class="border-t border-brand-gold/5 bg-black/20">
                    <div class="p-4 space-y-6">
                        <div v-if="!theme.subjects || theme.subjects.length === 0" class="text-center py-4 text-brand-parchment/40 italic text-sm">
                            Aucun sujet disponible.
                        </div>

                        <div v-for="subject in theme.subjects" :key="subject.id" class="relative pl-4 md:pl-0">
                            <!-- Subject Connect line (Mobile) -->
                            <div class="absolute left-0 top-0 bottom-0 w-px bg-brand-gold/20 md:hidden"></div>

                            <!-- Subject Header (Compact) -->
                            <div class="flex items-center gap-3 mb-3">
                                <div class="w-6 h-6 rounded-md bg-brand-gold/10 flex items-center justify-center text-brand-gold shadow-sm ring-1 ring-brand-gold/20">
                                    <i class="fas fa-scroll text-xs"></i>
                                </div>
                                <h4 class="text-base font-bold text-brand-parchment font-serif-title">{{ subject.title }}</h4>
                                <div class="h-px flex-grow bg-brand-gold/10"></div>
                            </div>
                            
                            <!-- Parts Grid (Compact) -->
                            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-3">
                                 <div v-if="!subject.parts || subject.parts.length === 0" class="col-span-full py-2 px-4 rounded-lg bg-brand-wood/5 text-brand-parchment/40 text-xs italic border border-brand-white/5">
                                    Bientôt disponible
                                </div>

                                <div v-for="part in subject.parts" :key="part.id" 
                                     class="flex flex-col justify-between p-3 rounded-lg bg-brand-dark/40 border border-brand-gold/5 hover:border-brand-gold/30 hover:bg-brand-wood/10 transition-all duration-300 group/card relative overflow-hidden">
                                     
                                     <!-- Hover Glow -->
                                     <div class="absolute inset-0 bg-gradient-to-br from-brand-gold/0 to-brand-gold/5 opacity-0 group-hover/card:opacity-100 transition-opacity duration-500"></div>

                                    <div class="relative z-10 mb-2">
                                        <div class="flex justify-between items-start gap-2">
                                            <h5 class="text-brand-parchment font-bold text-sm leading-tight mb-1 line-clamp-1" :title="part.title">{{ part.title }}</h5>
                                        </div>
                                        <p class="text-brand-parchment/50 text-[0.65rem] line-clamp-2 leading-snug" v-if="part.description">{{ part.description }}</p>
                                    </div>
                                    
                                    <div class="relative z-10 pt-2 border-t border-white/5 flex items-center justify-between mt-auto">
                                        <span class="text-brand-gold/60 text-[0.6rem] font-medium uppercase tracking-wider">Quiz</span>
                                        <NuxtLink 
                                            :to="`/games/${theme.id}/${subject.id}/${part.id}`"
                                            class="flex items-center gap-1.5 px-3 py-1.5 bg-brand-gold text-brand-dark text-[0.65rem] font-bold uppercase tracking-wider roundedHover:bg-white hover:text-brand-dark hover:scale-105 hover:shadow-md hover:shadow-brand-gold/20 transition-all duration-300 transform active:scale-95"
                                        >
                                            Jouer
                                            <i class="fas fa-play text-[0.5rem]"></i>
                                        </NuxtLink>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </transition>
        </div>
      </div>
    </main>

    <!-- Global Footer -->
    <AppFooter />
    
    <!-- Mobile Navigation -->
    <AppBottomNav />

  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'
import { progressionService } from '~/services/progression.service'

definePageMeta({
  middleware: 'auth',
  layout: false // Disable default layout to fully control structure with Global Components
})

const authStore = useAuthStore()
const router = useRouter()
const config = useRuntimeConfig()

// Types matching Backend Entities
interface Part {
    id: string
    title: string
    description?: string
}

interface Subject {
    id: string
    title: string
    parts: Part[]
    description?: string // Added description
}

interface Theme {
    id: string
    title: string
    description: string
    subjects: Subject[]
}

// Fetch Progression Stats
const { data: progression } = await useAsyncData('user-progression', async () => {
    if (authStore.token) {
        return await progressionService.getProgression(authStore.token)
    }
    return null
})

// Fetch Themes
const { data: themes, pending: pendingThemes, error: errorThemes } = await useFetch<Theme[]>(`${config.public.apiBase}/Theme`)

// Accordion Logic
const expandedThemes = ref<string[]>([])

const toggleTheme = (id: string) => {
    if (expandedThemes.value.includes(id)) {
        expandedThemes.value = expandedThemes.value.filter(t => t !== id)
    } else {
        expandedThemes.value.push(id)
    }
}

// Auto-expand first theme if available
watch(themes, (newThemes) => {
    if (newThemes && newThemes.length > 0) {
        // Only expand the first one by default
        if (expandedThemes.value.length === 0) {
             expandedThemes.value = [newThemes[0].id]
        }
    }
}, { immediate: true })

</script>

<style scoped>
.animate-pulse-slow {
  animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

.animate-fade-in {
    animation: fadeIn 0.8s ease-out forwards;
    opacity: 0;
}

.animate-fade-in-up {
    animation: fadeInUp 0.8s ease-out forwards;
    opacity: 0;
}

.delay-200 { animation-delay: 0.2s; }
.delay-300 { animation-delay: 0.3s; }
.delay-1000 { animation-delay: 1s; }

@keyframes pulse {
  0%, 100% { opacity: 0.2; }
  50% { opacity: 0.1; }
}

@keyframes fadeIn {
    to { opacity: 1; }
}

@keyframes fadeInUp {
    from { opacity: 0; transform: translateY(20px); }
    to { opacity: 1; transform: translateY(0); }
}
</style>
