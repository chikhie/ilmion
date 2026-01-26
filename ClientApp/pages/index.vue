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

    <!-- Language Switcher -->
    <div class="absolute top-4 right-4 z-20 flex gap-2">
        <button 
            v-for="loc in locales" 
            :key="loc.code"
            @click="setLocale(loc.code)"
            :class="[
                'px-3 py-2 rounded-lg border transition-all duration-200 flex items-center gap-2 text-sm',
                locale === loc.code 
                    ? 'bg-brand-gold text-brand-dark border-brand-gold' 
                    : 'bg-brand-wood/30 text-brand-parchment/70 border-brand-gold/30 hover:border-brand-gold/60'
            ]"
        >
            <span>{{ loc.code === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
            <span class="hidden sm:inline">{{ loc.name }}</span>
        </button>
    </div>

    <!-- Main Content -->
    <div class="relative z-10 flex-grow flex flex-col items-center justify-center container mx-auto px-4 py-16 text-center">
        
        <!-- Logo / Icon Placeholder -->
        <div class="mb-8 p-6 rounded-full border border-brand-gold/20 bg-brand-wood/10 backdrop-blur-sm shadow-[0_0_30px_rgba(195,151,18,0.1)] animate-fade-in-down">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-16 h-16 text-brand-gold" fill="none" viewBox="0 0 24 24" stroke="currentColor">
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
        <div class="flex flex-col sm:flex-row gap-6 w-full max-w-md animate-fade-in-up delay-300">
            <NuxtLink to="/games/quizz" class="group flex-1 relative px-8 py-5 bg-brand-gold text-brand-dark rounded-xl font-bold text-xl uppercase tracking-wider shadow-lg hover:shadow-[0_0_20px_rgba(195,151,18,0.4)] transition-all duration-300 overflow-hidden">
                <span class="relative z-10 flex items-center justify-center gap-3">
                    {{ $t('home.startQuiz') }}
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 transition-transform duration-300 group-hover:translate-x-1" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
                    </svg>
                </span>
                <div class="absolute inset-0 bg-white/20 translate-y-full group-hover:translate-y-0 transition-transform duration-300 ease-out"></div>
            </NuxtLink>
        </div>
    </div>
    
    <!-- Footer Decorative Line -->
    <div class="w-full h-2 bg-gradient-to-r from-brand-dark via-brand-gold/30 to-brand-dark mt-auto relative z-10"></div>
  </div>
</template>

<script setup lang="ts">
const { locale, locales, setLocale } = useI18n()

definePageMeta({
  layout: 'default'
})
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
