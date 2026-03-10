<template>
  <div class="h-full bg-[#1e1f22] flex flex-col relative overflow-hidden font-sans text-[#f2f3f5]">
    
    <!-- Background Elements -->
    <div class="absolute inset-0 z-0 pointer-events-none bg-[#1e1f22]">
        <!-- Mathematical Tech Grid Pattern -->
        <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#0D99FF 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
        
        <!-- Glow Effects (Brilliant Cyan & Emerald) -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-[#0D99FF]/15 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/15 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Header Actions -->
    <div class="absolute top-6 right-6 z-20 flex items-center gap-4">
        <!-- Login Button -->
        <NuxtLink to="/login" class="px-5 py-2.5 rounded-xl bg-[#2b2d31] border border-[#1e1f22] text-white font-bold text-sm hover:bg-[#313338] shadow-sm transition-all duration-200">
            {{ $t('common.login') }}
        </NuxtLink>

        <!-- Language Dropdown -->
        <div class="relative">
            <button 
                @click="isLangMenuOpen = !isLangMenuOpen"
                class="px-3 py-2.5 rounded-xl bg-[#2b2d31] hover:bg-[#313338] text-[#dbdee1] transition-colors duration-200 flex items-center gap-2"
                aria-label="Changer la langue"
            >
                <span class="text-xl">{{ locale === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
                <span class="text-sm font-medium uppercase tracking-wide">{{ locale }}</span>
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-[#00B578] transition-transform duration-200" :class="{ 'rotate-180': isLangMenuOpen }" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                </svg>
            </button>

            <!-- Dropdown Menu -->
            <div 
                v-if="isLangMenuOpen"
                class="absolute top-full right-0 mt-2 w-full min-w-[100px] bg-[#2b2d31] border border-[#1e1f22] rounded-xl shadow-xl overflow-hidden animate-fade-in-up"
            >
                <button 
                    v-for="loc in locales" 
                    :key="loc.code"
                    @click="setLocale(loc.code); isLangMenuOpen = false"
                    class="w-full px-4 py-3 flex items-center gap-3 text-sm hover:bg-[#1e1f22] transition-colors"
                    :class="locale === loc.code ? 'text-[#00B578] font-bold' : 'text-[#dbdee1]'"
                >
                    <span class="text-lg">{{ loc.code === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
                    <span class="uppercase tracking-wide">{{ loc.code }}</span>
                </button>
            </div>
        </div>
    </div>

    <!-- Main Content -->
    <div class="relative z-10 flex-grow flex flex-col items-center justify-center container mx-auto px-4 py-16 text-center">
        
        <!-- Logo / Icon Placeholder -->
        <div class="mb-8 p-5 rounded-3xl bg-[#2b2d31] shadow-xl border-b-[6px] border-[#1e1f22] animate-fade-in-down flex items-center justify-center transform hover:-translate-y-1 transition-transform" style="width: 112px; height: 112px;">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-16 h-16 text-[#00B578]" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true" stroke-width="1.5">
                <path stroke-linecap="round" stroke-linejoin="round" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" />
            </svg>
        </div>

        <!-- Title -->
        <h1 class="text-5xl md:text-7xl font-sans font-extrabold text-[#f2f3f5] mb-4 tracking-tight animate-fade-in max-w-4xl leading-tight">
            {{ $t('home.title') }} <span class="text-[#00B578] relative inline-block">
                {{ $t('home.titleHighlight') }}
                <svg class="absolute -bottom-2 left-0 w-full h-3 text-[#00B578]/30" viewBox="0 0 100 10" preserveAspectRatio="none">
                    <path d="M0 5 Q 50 10 100 5" stroke="currentColor" stroke-width="4" stroke-linecap="round" fill="none"/>
                </svg>
            </span>
        </h1>
        
        <h2 class="text-xl md:text-2xl text-[#dbdee1] font-medium mb-8 animate-fade-in delay-200">
            {{ $t('home.subtitle') }}
        </h2>

        <!-- Description -->
        <p class="text-lg md:text-xl text-[#b5bac1] max-w-2xl mx-auto mb-12 leading-relaxed animate-fade-in delay-300">{{ $t('home.description') }}</p>

        <!-- CTA Buttons -->
        <div class="flex flex-col sm:flex-row gap-5 w-full max-w-xl mx-auto animate-fade-in-up delay-300 px-4">
            <NuxtLink to="/quizz" class="group flex-1 relative px-8 py-5 bg-[#00B578] text-white rounded-2xl font-extrabold text-xl shadow-[0_8px_0_#00895a] hover:translate-y-1 hover:shadow-[0_4px_0_#00895a] active:translate-y-2 active:shadow-[0_0px_0_#00895a] transition-all flex items-center justify-center border-2 border-transparent">
                <span class="relative z-10 flex items-center justify-center gap-3">
                    {{ $t('home.startQuiz') }}
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 transition-transform duration-300 group-hover:translate-x-1" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
                    </svg>
                </span>
            </NuxtLink>
            
            <NuxtLink to="/register" class="group flex-1 relative px-8 py-5 bg-transparent border-2 border-[#313338] text-white rounded-2xl font-extrabold text-xl shadow-[0_8px_0_#313338] hover:bg-[#2b2d31] hover:translate-y-1 hover:shadow-[0_4px_0_#313338] hover:border-[#42454a] hover:text-[#00B578] active:translate-y-2 active:shadow-[0_0px_0_#313338] transition-all flex items-center justify-center">
                <span class="relative z-10 flex items-center justify-center gap-3">
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-[#00B578] group-hover:scale-110 transition-transform" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M8 9a3 3 0 100-6 3 3 0 000 6zM8 11a6 6 0 016 6H2a6 6 0 016-6zM16 7a1 1 0 10-2 0v1h-1a1 1 0 100 2h1v1a1 1 0 102 0v-1h1a1 1 0 100-2h-1V7z" />
                    </svg>
                    {{ $t('home.register') }}
                </span>
            </NuxtLink>
        </div>

        <!-- Articles Link -->
        <NuxtLink to="/articles" class="group mt-12 w-full max-w-lg flex items-center gap-5 px-6 py-5 rounded-2xl bg-[#2b2d31] border border-[#1e1f22] hover:bg-[#313338] hover:-translate-y-1 transition-all duration-300 animate-fade-in-up delay-500 shadow-xl">
            <div class="w-14 h-14 rounded-2xl bg-[#00B578]/15 flex items-center justify-center flex-shrink-0 group-hover:bg-[#00B578]/30 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-7 h-7 text-[#00B578]" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"/>
                </svg>
            </div>
            <div class="flex-1 text-left">
                <span class="block text-white font-extrabold text-lg">Leçons Visuelles</span>
                <span class="block text-[#b5bac1] text-sm mt-1">Explorez les mathématiques, la physique, et l'informatique interactivement.</span>
            </div>
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 text-[#00B578] group-hover:translate-x-1 transition-all flex-shrink-0" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
        </NuxtLink>
    </div>
    
    <!-- Footer Decorative Line -->
    <div class="w-full h-2 bg-gradient-to-r from-[#1e1f22] via-[#00B578] to-[#1e1f22] mt-auto relative z-10"></div>
  </div>
</template>

<script setup lang="ts">
useSeoMeta({
  title: 'Accueil | Mathématiques et Sciences',
  description: 'Rejoignez une communauté d\'apprenants. Explorez les mathématiques, la physique et l\'informatique à travers des leçons visuelles et interactives.'
})

const { locale, locales, setLocale } = useI18n()
const isLangMenuOpen = ref(false)
const authStore = useAuthStore()
const router = useRouter()

definePageMeta({
  layout: 'default'
})

onMounted(() => {
    if (authStore.isLoggedIn) {
        navigateTo('/dashboard')
    }
})

// Also watch for state changes (e.g. hydration)
watch(() => authStore.isLoggedIn, (loggedIn) => {
    if (loggedIn) {
        navigateTo('/dashboard')
    }
}, { immediate: true })
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.8s ease-out forwards;
  opacity: 0;
}

.animate-fade-in-down {
    animation: fadeInDown 0.8s ease-out forwards;
    opacity: 0;
}

.animate-fade-in-up {
    animation: fadeInUp 0.8s ease-out forwards;
    opacity: 0;
}

.delay-200 { animation-delay: 0.2s; }
.delay-300 { animation-delay: 0.3s; }
.delay-500 { animation-delay: 0.5s; }
.delay-1000 { animation-delay: 1s; }

.animate-pulse-slow {
    animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes fadeIn {
  to { opacity: 1; }
}

@keyframes fadeInDown {
    from { opacity: 0; transform: translateY(-20px); }
    to { opacity: 1; transform: translateY(0); }
}

@keyframes fadeInUp {
    from { opacity: 0; transform: translateY(20px); }
    to { opacity: 1; transform: translateY(0); }
}

@keyframes pulse {
  0%, 100% { opacity: 0.1; }
  50% { opacity: 0.2; }
}
</style>
