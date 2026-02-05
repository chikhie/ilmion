<template>
  <div class="h-full bg-brand-dark flex flex-col relative overflow-hidden font-sans-body">
    
    <!-- Background Elements -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <!-- Abstract Pattern -->
        <div class="absolute inset-0 opacity-[0.03]" style="background-image: url('/patterns/arabesque.png')"></div>
        
        <!-- Glow Effects -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-brand-gold/10 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-brand-wood/10 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Header Actions -->
    <div class="absolute top-6 right-6 z-20 flex items-center gap-4">
        <!-- Login Button -->
        <NuxtLink to="/login" class="px-5 py-2.5 rounded-xl border border-brand-gold/30 bg-brand-wood/10 backdrop-blur-sm text-brand-parchment font-bold text-sm uppercase tracking-wide hover:bg-brand-gold hover:text-brand-dark transition-all duration-300">
            {{ $t('common.login') }}
        </NuxtLink>

        <!-- Language Dropdown -->
        <div class="relative">
            <button 
                @click="isLangMenuOpen = !isLangMenuOpen"
                class="px-3 py-2.5 rounded-xl border border-brand-gold/30 bg-brand-wood/10 backdrop-blur-sm text-brand-parchment hover:border-brand-gold/60 transition-all duration-200 flex items-center gap-2"
                aria-label="Changer la langue"
            >
                <span class="text-xl">{{ locale === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
                <span class="text-sm font-medium uppercase tracking-wide">{{ locale }}</span>
                <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-brand-gold transition-transform duration-200" :class="{ 'rotate-180': isLangMenuOpen }" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                </svg>
            </button>

            <!-- Dropdown Menu -->
            <div 
                v-if="isLangMenuOpen"
                class="absolute top-full right-0 mt-2 w-full min-w-[100px] bg-[#082540] border border-brand-gold/20 rounded-xl shadow-xl overflow-hidden animate-fade-in-up"
            >
                <button 
                    v-for="loc in locales" 
                    :key="loc.code"
                    @click="setLocale(loc.code); isLangMenuOpen = false"
                    class="w-full px-4 py-3 flex items-center gap-3 text-sm hover:bg-brand-gold/10 transition-colors"
                    :class="locale === loc.code ? 'text-brand-gold font-bold' : 'text-brand-parchment/70'"
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
        <div class="mb-8 p-6 rounded-full border border-brand-gold/20 bg-brand-wood/10 backdrop-blur-sm shadow-[0_0_30px_rgba(195,151,18,0.1)] animate-fade-in-down" style="width: 112px; height: 112px;">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-16 h-16 text-brand-gold" fill="none" viewBox="0 0 24 24" stroke="currentColor" width="64" height="64" aria-hidden="true">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
        </div>

        <!-- Title -->
        <h1 class="text-5xl md:text-7xl font-serif-title font-bold text-brand-parchment mb-4 drop-shadow-lg tracking-wide animate-fade-in max-w-4xl leading-tight">
            {{ $t('home.title') }} <span class="text-brand-gold relative inline-block">
                {{ $t('home.titleHighlight') }}
                <svg class="absolute -bottom-2 left-0 w-full h-3 text-brand-gold/30" viewBox="0 0 100 10" preserveAspectRatio="none">
                    <path d="M0 5 Q 50 10 100 5" stroke="currentColor" stroke-width="2" fill="none"/>
                </svg>
            </span>
        </h1>
        
        <h2 class="text-xl md:text-2xl text-brand-gold font-serif-title mb-8 opacity-90 animate-fade-in delay-200">
            {{ $t('home.subtitle') }}
        </h2>

        <!-- Description -->
        <p class="text-lg md:text-xl text-brand-parchment/80 max-w-2xl mx-auto mb-12 leading-relaxed animate-fade-in delay-300">{{ $t('home.description') }}</p>

        <!-- CTA Buttons -->
        <div class="flex flex-col sm:flex-row gap-6 w-full max-w-lg animate-fade-in-up delay-300">
            <NuxtLink to="/quizz" class="group flex-1 relative px-8 py-5 bg-brand-gold text-brand-dark rounded-xl font-bold text-xl uppercase tracking-wider shadow-lg hover:shadow-[0_0_20px_rgba(195,151,18,0.4)] transition-all duration-300 overflow-hidden flex items-center justify-center">
                <span class="relative z-10 flex items-center justify-center gap-3 whitespace-nowrap">
                    {{ $t('home.startQuiz') }}
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 transition-transform duration-300 group-hover:translate-x-1" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
                    </svg>
                </span>
                <div class="absolute inset-0 bg-white/20 translate-y-full group-hover:translate-y-0 transition-transform duration-300 ease-out"></div>
            </NuxtLink>
            
            <NuxtLink to="/register" class="group flex-1 relative px-8 py-5 border-2 border-brand-gold text-brand-gold rounded-xl font-bold text-xl uppercase tracking-wider hover:bg-brand-gold/10 transition-all duration-300 overflow-hidden flex items-center justify-center">
                <span class="relative z-10 flex items-center justify-center gap-3 whitespace-nowrap">
                    {{ $t('home.register') }}
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 transition-transform duration-300 group-hover:scale-110" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M8 9a3 3 0 100-6 3 3 0 000 6zM8 11a6 6 0 016 6H2a6 6 0 016-6zM16 7a1 1 0 10-2 0v1h-1a1 1 0 100 2h1v1a1 1 0 102 0v-1h1a1 1 0 100-2h-1V7z" />
                    </svg>
                </span>
            </NuxtLink>
        </div>
    </div>
    
    <!-- Footer Decorative Line -->
    <div class="w-full h-2 bg-gradient-to-r from-brand-dark via-brand-gold/30 to-brand-dark mt-auto relative z-10"></div>
  </div>
</template>

<script setup lang="ts">
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
