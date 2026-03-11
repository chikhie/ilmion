<template>
  <div class="min-h-screen bg-[#1e1f22] text-white flex flex-col relative overflow-hidden font-sans">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <div class="absolute top-[-10%] right-[-10%] w-[500px] h-[500px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] left-[-10%] w-[600px] h-[600px] bg-[#00B578]/5 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Global Header -->
    <AppHeader />

    <main class="flex-grow container mx-auto px-3 pt-4 pb-8 md:px-4 md:pt-6 md:pb-12 relative z-10 max-w-7xl">

      <!-- Feature Header: Theme Selection with Dropdown -->
      <div class="flex flex-col md:flex-row justify-between items-end mb-4 md:mb-8 gap-3 md:gap-4 border-b border-[#00B578]/10 pb-4 md:pb-6 animate-fade-in delay-200 relative z-30">
        
        <div class="hidden md:block">
          <div class="flex items-center gap-3 mb-2">
            <div class="w-10 h-10 rounded-lg bg-[#2b2d31]/80 border border-[#00B578]/30 text-[#00B578] flex items-center justify-center text-lg shadow-sm backdrop-blur-sm">
                <i class="fas fa-atom"></i>
            </div>
            <div>
                 <h1 class="text-2xl md:text-3xl font-sans font-bold text-white tracking-wide">Parcours</h1>
                 <div v-if="progression" class="flex items-center gap-2 text-xs text-[#00B578] animate-fade-in delay-200 mt-1">
                    <i class="fas fa-star text-[0.6rem]"></i>
                    <span class="font-bold tracking-wider">{{ progression.globalPoints }} XP</span>
                    <span class="text-gray-600 px-1">|</span>
                    <span class="font-bold tracking-wider">{{ progression.modulesMasteredCount }} Thèmes Maîtrisés</span>
                 </div>
            </div>
          </div>
          <p class="text-gray-400 text-sm mt-1">Sélectionnez un thème pour explorer les modules scientifiques.</p>
        </div>

        <!-- Theme Filter Dropdown (Custom UI) -->
        <div class="relative w-full md:w-72" ref="dropdownRef">
           <label class="block text-gray-500 text-[0.65rem] font-bold uppercase tracking-widest mb-1.5 ml-1">
               Changer de thème
           </label>
           
           <button 
                @click="isDropdownOpen = !isDropdownOpen"
                class="w-full flex items-center justify-between bg-[#1e1f22]/80 border border-white/5 text-white py-3 px-4 rounded-xl hover:border-[#00B578]/50 focus:border-[#00B578]/50 focus:bg-[#1e1f22] focus:ring-1 focus:ring-[#00B578]/30 transition-all duration-200 group backdrop-blur-sm"
           >
                <span class="font-sans font-bold text-sm">{{ currentTheme?.title || 'Sélectionner un thème' }}</span>
                <i class="fas fa-chevron-down text-gray-400 text-xs transition-transform duration-200" :class="{ 'rotate-180': isDropdownOpen }"></i>
           </button>

           <!-- Dropdown Menu -->
           <transition
                enter-active-class="transition duration-200 ease-out"
                enter-from-class="transform scale-95 opacity-0 -translate-y-2"
                enter-to-class="transform scale-100 opacity-100 translate-y-0"
                leave-active-class="transition duration-150 ease-in"
                leave-from-class="transform scale-100 opacity-100 translate-y-0"
                leave-to-class="transform scale-95 opacity-0 -translate-y-2"
           >
               <div v-if="isDropdownOpen" class="absolute top-full left-0 right-0 mt-2 bg-[#2b2d31]/95 border border-white/10 rounded-xl shadow-xl shadow-black/50 overflow-hidden z-50 backdrop-blur-xl">
                    <div v-if="themes && themes.length > 0">
                        <button 
                            v-for="theme in themes" 
                            :key="theme.id"
                            @click="selectTheme(theme.id)"
                            class="w-full text-left px-4 py-3 hover:bg-[#00B578]/10 transition-colors border-b border-white/5 last:border-0"
                        >
                            <span class="font-sans text-sm" :class="selectedThemeId === theme.id ? 'font-bold text-[#00B578]' : 'font-medium text-gray-300'">
                                {{ theme.title }}
                            </span>
                        </button>
                    </div>
                    <div v-else class="p-4 text-center text-gray-500 text-sm italic">
                        Aucun thème disponible
                    </div>
               </div>
           </transition>
        </div>
      </div>

      <!-- Active Theme Content -->
      <div v-if="currentTheme" class="animate-fade-in-up delay-300 relative z-20">
        
        <!-- Theme Description Box -->
        <div class="mb-6 md:mb-10 p-4 md:p-6 lg:p-8 rounded-xl md:rounded-2xl bg-[#2b2d31]/80 backdrop-blur-md border border-[#00B578]/20 relative overflow-hidden group hover:border-[#00B578]/50 transition-colors shadow-sm">
            <div class="absolute top-0 right-0 p-4 opacity-[0.02] group-hover:opacity-10 transition-opacity duration-700 pointer-events-none scale-150 -translate-y-1/4 translate-x-1/4">
                 <i class="fas fa-microscope text-9xl text-[#00B578]"></i>
            </div>
            <div class="relative z-10 flex flex-row gap-4 md:gap-6 items-start md:items-center">
                 <div class="w-12 h-12 md:w-16 md:h-16 rounded-xl md:rounded-2xl bg-[#1e1f22] border-2 border-[#00B578]/40 flex items-center justify-center text-[#00B578] shadow-inner flex-shrink-0 group-hover:border-[#00B578] transition-colors">
                    <i class="fas fa-laptop-code text-2xl md:text-3xl"></i>
                 </div>
                 <div class="flex-1">
                     <h2 class="text-xl md:text-3xl font-sans font-bold text-white tracking-wide mb-1 md:mb-2">{{ currentTheme.title }}</h2>
                     <p class="text-gray-400 max-w-3xl text-sm md:text-base leading-relaxed line-clamp-2 md:line-clamp-none mb-4">{{ currentTheme.description }}</p>
                     
                     <!-- Theme Progress Bar -->
                     <div v-if="themeProgress" class="max-w-md space-y-2 lg:max-w-xl">
                         <div class="flex items-center justify-between text-xs font-bold uppercase tracking-widest">
                             <span class="text-gray-500">
                                 {{ themeProgress.questionsAnswered }} / {{ themeProgress.totalQuestions }} questions
                             </span>
                             <span class="text-[#00B578]">{{ themeProgress.percentage }}%</span>
                         </div>
                         <div class="h-2 bg-[#1e1f22] rounded-full overflow-hidden border border-[#00B578]/20 relative">
                             <div 
                                 class="absolute top-0 left-0 h-full bg-[#00B578] transition-all duration-700 ease-out rounded-full shadow-[0_0_10px_rgba(0,181,120,0.5)]"
                                 :style="{ width: `${themeProgress.percentage}%` }"
                             ></div>
                         </div>
                     </div>
                 </div>
            </div>
        </div>

        <!-- Quizzes Grid (Flattened) -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 md:gap-6">
             <div v-if="flatParts.length === 0" class="col-span-full text-center py-12 border border-dashed border-white/10 rounded-2xl bg-[#2b2d31]/50 text-gray-500 italic text-base">
                <i class="fas fa-flask mb-3 block text-2xl opacity-50"></i>
                Aucun module disponible pour ce thème pour le moment.
            </div>

            <div v-for="part in flatParts" :key="part.id" 
                 class="group relative flex flex-col justify-between h-full p-4 md:p-6 rounded-2xl bg-[#2b2d31]/80 border border-[#00B578]/10 backdrop-blur-sm transition-all duration-300 hover:-translate-y-1 hover:border-[#00B578]/50 hover:shadow-lg hover:shadow-[#00B578]/10"
            >

                 <!-- Content -->
                <div class="relative z-10 mb-4 md:mb-5">
                    <div class="flex items-start justify-between gap-3 mb-3 md:mb-4">
                         <div class="flex items-center gap-2">
                             <div v-if="part.isMastered" class="w-6 h-6 rounded-full bg-[#00B578]/20 text-[#00B578] flex items-center justify-center text-[0.7rem] border border-[#00B578]/40" title="Maîtrisé">
                                 <i class="fas fa-check"></i>
                             </div>
                             <div v-else-if="part.score && part.score > 0" class="px-2 py-1 rounded bg-[#00B578]/10 border border-[#00B578]/30 text-[#00B578] text-[0.65rem] font-bold tracking-wider">
                                {{ part.score }} pts
                             </div>
                             <div v-else class="flex items-center gap-2">
                                <span class="w-2 h-2 rounded-full bg-[#00B578] shadow-[0_0_8px] shadow-[#00B578] animate-pulse"></span>
                                <span class="text-[#00B578]/80 text-[0.65rem] font-bold uppercase tracking-widest">Nouveau</span>
                             </div>
                         </div>
                         <div class="text-gray-600 group-hover:text-[#00B578]/50 transition-colors">
                             <i class="fas fa-microchip text-lg"></i>
                         </div>
                    </div>

                    <h4 class="font-bold text-white text-lg md:text-xl leading-snug mb-2 group-hover:text-[#00B578] transition-colors min-h-[2.5rem] md:min-h-[3rem]">
                        {{ part.title }}
                    </h4>
                    <p class="text-gray-400 text-sm leading-relaxed line-clamp-2 md:line-clamp-3" v-if="part.description">{{ part.description }}</p>
                    
                    <!-- Progress Bar -->
                    <div class="mt-4 space-y-1.5">
                        <div class="flex items-center justify-between text-[0.65rem] font-bold uppercase tracking-widest text-[#00B578]">
                            <span>Progression</span>
                            <span>{{ part.progressPercentage || 0 }}%</span>
                        </div>
                        <div class="h-1.5 bg-[#1e1f22] rounded-full overflow-hidden border border-white/5 relative">
                            <div 
                                class="absolute top-0 left-0 h-full bg-[#00B578] transition-all duration-500 ease-out shadow-[0_0_5px_rgba(0,181,120,0.5)] rounded-full"
                                :style="{ width: `${part.progressPercentage || 0}%` }"
                            ></div>
                        </div>
                    </div>
                </div>

                <!-- Action Footer -->
                <div class="relative z-10 pt-4 mt-auto border-t border-white/5 flex items-center justify-end">
                    <NuxtLink 
                        :to="`/quizz?themeId=${currentTheme.id}&partId=${part.id}`"
                        class="w-full px-4 py-3 bg-[#00B578] text-[#1e1f22] font-bold text-xs uppercase tracking-widest rounded-xl shadow-lg shadow-[#00B578]/20 hover:bg-[#00C588] hover:shadow-[#00B578]/40 active:scale-95 transition-all duration-300 flex items-center justify-center gap-2"
                    >
                        <span>Démarrer</span>
                        <i class="fas fa-rocket text-[0.7rem] ml-1"></i>
                    </NuxtLink>
                </div>
            </div>
        </div>

      </div>

      <!-- Articles Section -->
      <NuxtLink to="/articles" class="block mt-6 md:mt-10 p-5 md:p-8 rounded-xl md:rounded-2xl bg-[#2b2d31]/80 border border-[#00B578]/20 backdrop-blur-md relative overflow-hidden group hover:border-[#00B578]/50 transition-colors duration-300 animate-fade-in-up delay-300 shadow-sm">
        <div class="flex items-center gap-5">
          <div class="w-14 h-14 rounded-xl bg-[#1e1f22] flex items-center justify-center text-[#00B578] border-2 border-[#00B578]/30 flex-shrink-0 group-hover:border-[#00B578] transition-colors shadow-inner">
            <i class="fas fa-book-journal-whills text-2xl"></i>
          </div>
          <div class="flex-1">
            <h3 class="text-xl font-sans font-bold text-white group-hover:text-[#00B578] transition-colors mb-1">Articles & Logiques</h3>
            <p class="text-gray-400 text-sm md:text-base">Explorez des fondements scientifiques et mathématiques via nos articles détaillés.</p>
          </div>
          <i class="fas fa-arrow-right text-gray-600 text-xl group-hover:text-[#00B578] group-hover:translate-x-2 transition-all duration-300"></i>
        </div>
      </NuxtLink>

      <!-- Loading/Error States -->
      <div v-if="pendingThemes" class="flex justify-center py-20 relative z-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#00B578]"></div>
      </div>

      <div v-else-if="errorThemes" class="text-center py-10 px-6 text-red-400 bg-red-900/20 rounded-2xl border border-red-500/30 backdrop-blur-sm relative z-20">
        <i class="fas fa-bug text-3xl mb-3 text-red-500"></i>
        <p class="font-bold text-sm tracking-wide">Une erreur système est survenue.</p>
        <button @click="() => refresh()" class="mt-5 px-6 py-2 bg-red-500/10 text-red-400 rounded-lg hover:bg-red-500/20 transition-colors text-xs font-bold uppercase tracking-widest border border-red-500/30">Relancer le diagnostic</button>
      </div>

    </main>

    <!-- Global Footer -->
    <AppFooter />
    
    <!-- Mobile Navigation -->
    <!-- <AppBottomNav /> -->

  </div>
</template>

<script setup lang="ts">
useSeoMeta({
  title: 'Parcours | Ilmion',
  description: 'Suivez votre progression et explorez vos thèmes d\'apprentissage sur Ilmion.'
})

import { useAuthStore } from '~/stores/auth'
import { progressionService } from '~/services/progression.service'

definePageMeta({
  middleware: 'auth',
  layout: false // Disable default layout to fully control structure with Global Components
})

const authStore = useAuthStore()
const config = useRuntimeConfig()

// Types matching Backend Entities
interface Part {
    id: string
    title: string
    description?: string
    subjectId?: string // Helper for linking back if needed in flattened view
}

interface Subject {
    id: string
    title: string
    parts: Part[]
    description?: string
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
const { data: themes, pending: pendingThemes, error: errorThemes, refresh } = await useFetch<Theme[]>(`${config.public.apiBase}/Theme`)

const selectedThemeId = ref<string | null>(null)

const currentTheme = computed(() => {
    return themes.value?.find(t => t.id === selectedThemeId.value) || null
})

// Compute flattened parts from the current theme subjects
const flatParts = computed(() => {
    if (!currentTheme.value || !currentTheme.value.subjects) return []
    
    // Find progression for this theme
    const themeStats = progression.value?.modules.find(m => m.moduleId === currentTheme.value?.id)

    return currentTheme.value.subjects.flatMap(subject => {
        return (subject.parts || []).map(part => {
            // Find stats for this part
            const partStats = themeStats?.parts?.find(p => p.partId === part.id)
            
            // Calculate progress percentage
            let progressPercentage = 0
            if (partStats && partStats.totalQuestions > 0) {
                progressPercentage = Math.round((partStats.score / partStats.totalQuestions) * 100)
            }

            return {
                ...part,
                subjectId: subject.id, // Attach subject ID for URL construction
                isMastered: partStats?.isMastered || false,
                score: partStats?.score || 0,
                progressPercentage
            }
        })
    })
})

// Compute theme-level progress
const themeProgress = computed(() => {
    if (!currentTheme.value) return null
    
    const themeStats = progression.value?.modules.find(m => m.moduleId === currentTheme.value?.id)
    if (!themeStats) return null
    
    const percentage = themeStats.totalQuestions > 0 
        ? Math.round((themeStats.questionsAnswered / themeStats.totalQuestions) * 100)
        : 0
    
    return {
        questionsAnswered: themeStats.questionsAnswered,
        totalQuestions: themeStats.totalQuestions,
        percentage
    }
})

// Auto-select first theme
watch(themes, (newThemes) => {
    if (newThemes && newThemes.length > 0 && !selectedThemeId.value) {
        selectedThemeId.value = newThemes[0].id
    }
}, { immediate: true })

// Dropdown Logic
const isDropdownOpen = ref(false)
const dropdownRef = ref<HTMLElement | null>(null)

const selectTheme = (id: string) => {
    selectedThemeId.value = id
    isDropdownOpen.value = false
}

// Close dropdown when clicking outside
onMounted(() => {
    document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside)
})

const handleClickOutside = (event: MouseEvent) => {
    if (dropdownRef.value && !dropdownRef.value.contains(event.target as Node)) {
        isDropdownOpen.value = false
    }
}

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
