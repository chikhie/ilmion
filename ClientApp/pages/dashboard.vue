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

    <main class="flex-grow container mx-auto px-3 pt-4 pb-8 md:px-4 md:pt-6 md:pb-12 relative z-10 max-w-7xl">

      <!-- Feature Header: Theme Selection with Dropdown -->
      <div class="flex flex-col md:flex-row justify-between items-end mb-4 md:mb-8 gap-3 md:gap-4 border-b border-brand-gold/10 pb-4 md:pb-6 animate-fade-in delay-200 relative z-30">
        
        <div class="hidden md:block">
          <div class="flex items-center gap-3 mb-2">
            <div class="w-10 h-10 rounded-full bg-brand-gold text-brand-dark flex items-center justify-center text-lg shadow-lg shadow-brand-gold/20">
                <i class="fas fa-book-reader"></i>
            </div>
            <h1 class="text-2xl md:text-3xl font-serif-title font-bold text-brand-parchment">Programme</h1>
          </div>
          <p class="text-brand-parchment/60 text-sm">Sélectionnez un thème pour explorer les modules.</p>
        </div>

        <!-- Theme Filter Dropdown (Custom UI) -->
        <div class="relative w-full md:w-72" ref="dropdownRef">
           <label class="block text-brand-gold/60 text-[0.65rem] font-bold uppercase tracking-widest mb-1.5 ml-1">
               Changer de thème
           </label>
           
           <button 
                @click="isDropdownOpen = !isDropdownOpen"
                class="w-full flex items-center justify-between bg-brand-wood/20 border border-brand-gold/20 text-brand-parchment py-3 px-4 rounded-xl hover:bg-brand-wood/30 hover:border-brand-gold/40 transition-all duration-300 group shadow-lg backdrop-blur-md"
           >
                <div class="flex items-center gap-3">
                    <div class="w-6 h-6 rounded-full bg-brand-gold/10 flex items-center justify-center text-brand-gold text-xs border border-brand-gold/10 group-hover:bg-brand-gold group-hover:text-brand-dark transition-colors">
                        <i class="fas fa-layer-group"></i>
                    </div>
                    <span class="font-serif-title font-bold text-lg tracking-wide">{{ currentTheme?.title || 'Sélectionner' }}</span>
                </div>
                <i class="fas fa-chevron-down text-brand-gold/50 text-xs transition-transform duration-300" :class="{ 'rotate-180': isDropdownOpen }"></i>
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
               <div v-if="isDropdownOpen" class="absolute top-full left-0 right-0 mt-2 bg-brand-dark/95 border border-brand-gold/20 rounded-xl shadow-2xl overflow-hidden z-50 backdrop-blur-xl">
                    <div v-if="themes && themes.length > 0">
                        <button 
                            v-for="theme in themes" 
                            :key="theme.id"
                            @click="selectTheme(theme.id)"
                            class="w-full text-left px-4 py-3 flex items-center justify-between hover:bg-brand-wood/30 transition-colors group/item border-b border-brand-white/5 last:border-0"
                            :class="{ 'bg-brand-gold/5': selectedThemeId === theme.id }"
                        >
                            <span class="font-serif-title text-brand-parchment group-hover/item:text-brand-gold transition-colors" :class="{ 'font-bold text-brand-gold': selectedThemeId === theme.id }">
                                {{ theme.title }}
                            </span>
                            <i v-if="selectedThemeId === theme.id" class="fas fa-check text-brand-gold text-xs"></i>
                        </button>
                    </div>
                    <div v-else class="p-4 text-center text-brand-parchment/40 text-sm italic">
                        Aucun thème disponible
                    </div>
               </div>
           </transition>
        </div>
      </div>

      <!-- Active Theme Content -->
      <div v-if="currentTheme" class="animate-fade-in-up delay-300 relative z-20">
        
        <!-- Theme Description Box -->
        <div class="mb-6 md:mb-10 p-4 md:p-6 lg:p-8 rounded-xl md:rounded-2xl bg-gradient-to-r from-brand-wood/10 to-transparent border border-brand-gold/5 backdrop-blur-sm relative overflow-hidden group">
            <div class="absolute top-0 right-0 p-4 opacity-5 group-hover:opacity-10 transition-opacity duration-700 pointer-events-none">
                 <i class="fas fa-mosque text-9xl text-brand-gold"></i>
            </div>
            <div class="relative z-10 flex flex-row gap-4 md:gap-6 items-start md:items-center">
                 <div class="w-12 h-12 md:w-16 md:h-16 rounded-xl md:rounded-2xl bg-brand-gold/10 flex items-center justify-center text-brand-gold border border-brand-gold/20 shadow-inner flex-shrink-0">
                    <i class="fas fa-quran text-2xl md:text-3xl"></i>
                 </div>
                 <div>
                     <h2 class="text-xl md:text-3xl font-serif-title font-bold text-brand-gold mb-1 md:mb-2">{{ currentTheme.title }}</h2>
                     <p class="text-brand-parchment/70 max-w-3xl text-sm md:text-base leading-relaxed font-light line-clamp-2 md:line-clamp-none">{{ currentTheme.description }}</p>
                 </div>
            </div>
        </div>

        <!-- Quizzes Grid (Flattened) -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-3 md:gap-5">
             <div v-if="flatParts.length === 0" class="col-span-full text-center py-12 border border-dashed border-brand-white/10 rounded-xl bg-brand-wood/5 text-brand-parchment/40 italic text-base">
                <i class="fas fa-hourglass-start mb-3 block text-2xl opacity-50"></i>
                Aucun quiz disponible pour ce thème pour le moment.
            </div>

            <div v-for="part in flatParts" :key="part.id" 
                 class="group relative flex flex-col justify-between h-full p-4 md:p-5 rounded-xl bg-brand-wood/10 border border-brand-gold/5 backdrop-blur-sm transition-all duration-300 hover:-translate-y-1 hover:bg-brand-wood/20 hover:border-brand-gold/30 hover:shadow-xl hover:shadow-brand-gold/5"
            >

                 <!-- Content -->
                <div class="relative z-10 mb-4">
                    <div class="flex items-start justify-between gap-3 mb-3">
                         <div class="flex items-center gap-2">
                            <span class="w-2 h-2 rounded-full bg-brand-gold shadow-[0_0_8px] shadow-brand-gold"></span>
                            <span class="text-brand-gold/80 text-[0.65rem] font-bold uppercase tracking-widest">Quiz</span>
                         </div>
                         <div class="text-brand-parchment/20 group-hover:text-brand-gold/40 transition-colors">
                             <i class="fas fa-tasks"></i>
                         </div>
                    </div>

                    <h4 class="font-bold text-brand-parchment text-lg leading-snug mb-2 group-hover:text-white transition-colors min-h-[3.5rem]">
                        {{ part.title }}
                    </h4>
                    <p class="text-brand-parchment/50 text-xs leading-relaxed line-clamp-3" v-if="part.description">{{ part.description }}</p>
                </div>

                <!-- Action Footer -->
                <div class="relative z-10 pt-4 mt-auto border-t border-brand-white/5 flex items-center justify-end">
                    <NuxtLink 
                        :to="`/quizz?themeId=${currentTheme.id}&partId=${part.id}`"
                        class="w-full px-4 py-3 bg-brand-gold text-brand-dark font-bold text-xs uppercase tracking-widest rounded-lg shadow-lg shadow-brand-gold/10 hover:bg-white hover:text-brand-dark hover:shadow-brand-gold/30 transition-all duration-300 flex items-center justify-center gap-2"
                    >
                        <span>Commencer</span>
                        <i class="fas fa-play text-[0.6rem]"></i>
                    </NuxtLink>
                </div>
            </div>
        </div>

      </div>

      <!-- Loading/Error States -->
      <div v-if="pendingThemes" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-brand-gold"></div>
      </div>

      <div v-else-if="errorThemes" class="text-center py-10 px-6 text-red-300 bg-red-900/10 rounded-xl border border-red-500/20 backdrop-blur-sm">
        <i class="fas fa-exclamation-circle text-2xl mb-2"></i>
        <p>Une erreur est survenue lors du chargement des contenus.</p>
        <button @click="() => refresh()" class="mt-4 text-sm underline text-brand-gold hover:text-white">Réessayer</button>
      </div>

    </main>

    <!-- Global Footer -->
    <AppFooter />
    
    <!-- Mobile Navigation -->
    <!-- <AppBottomNav /> -->

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
    
    return currentTheme.value.subjects.flatMap(subject => {
        return (subject.parts || []).map(part => ({
            ...part,
            subjectId: subject.id // Attach subject ID for URL construction
        }))
    })
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
