<template>
  <NuxtLayout>
    <div class="h-full min-h-screen flex-grow bg-[#1e1f22] flex flex-col relative overflow-hidden font-sans text-[#f2f3f5]">
      
      <!-- Background Elements -->
      <div class="absolute inset-0 z-0 pointer-events-none bg-[#1e1f22]">
          <!-- Mathematical Tech Grid Pattern -->
          <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#0D99FF 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
          
          <!-- Glow Effects (Brilliant Cyan & Emerald) -->
          <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-[#0D99FF]/15 rounded-full blur-[100px] animate-pulse-slow"></div>
          <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/15 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
      </div>

      <!-- Main Content -->
      <div class="relative z-10 flex-grow flex flex-col items-center justify-center container mx-auto px-4 py-16 text-center">
          
          <!-- Warning Icon -->
          <div class="mb-8 p-5 rounded-3xl bg-[#2b2d31] shadow-xl border-b-[6px] border-[#1e1f22] animate-fade-in-down flex items-center justify-center transform hover:-translate-y-1 transition-transform" style="width: 112px; height: 112px;">
              <svg class="w-16 h-16 text-[#00B578]" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path>
              </svg>
          </div>

          <!-- Title -->
          <h1 class="text-7xl md:text-9xl font-sans font-extrabold text-[#f2f3f5] mb-4 tracking-tight animate-fade-in max-w-4xl leading-tight">
              {{ is404 ? '404' : error?.statusCode }} 
          </h1>
          
          <!-- Subtitle -->
          <h2 class="text-xl md:text-3xl text-[#dbdee1] font-medium mb-8 animate-fade-in delay-200">
              {{ is404 ? 'Page Introuvable' : 'Une erreur est survenue' }}
          </h2>

          <!-- Description -->
          <p class="text-lg md:text-xl text-[#b5bac1] max-w-2xl mx-auto mb-12 leading-relaxed animate-fade-in delay-300">
            {{ is404 ? "Il semble que vous vous soyez perdu. La page que vous recherchez n'existe pas ou a été déplacée." : (error?.message || "Nous rencontrons des difficultés techniques. Veuillez réessayer plus tard.") }}
          </p>

          <!-- Action Button -->
          <button @click="handleError" class="group mt-4 flex items-center justify-center gap-4 px-8 py-4 rounded-2xl bg-[#00B578] text-[#1e1f22] font-extrabold hover:bg-[#00d48a] hover:-translate-y-1 transition-all duration-300 animate-fade-in-up delay-500 shadow-xl">
              Retourner à l'accueil
              <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 group-hover:translate-x-1 transition-all" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
              </svg>
          </button>
      </div>
      
      <!-- Footer Decorative Line -->
      <div class="w-full h-2 bg-gradient-to-r from-[#1e1f22] via-[#00B578] to-[#1e1f22] mt-auto relative z-10"></div>
    </div>
  </NuxtLayout>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { NuxtError } from '#app'

const props = defineProps({
  error: Object as () => NuxtError
})

const is404 = computed(() => {
  return props.error?.statusCode === 404 || props.error?.statusCode === '404'
})

const handleError = () => clearError({ redirect: '/' })
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
