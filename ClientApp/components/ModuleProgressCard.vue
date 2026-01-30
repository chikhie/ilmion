<template>
  <div class="group relative overflow-hidden rounded-2xl bg-brand-wood/10 border border-brand-gold/10 p-6 backdrop-blur-md transition-all duration-300 hover:scale-[1.02] hover:border-brand-gold/30 hover:bg-brand-wood/20 hover:shadow-[0_0_20px_rgba(195,151,18,0.1)]">
    
    <!-- Decorative Icon Background -->
    <div class="absolute -right-4 -top-4 opacity-5 transition-opacity group-hover:opacity-10 pointer-events-none">
      <i class="fas fa-book text-9xl text-brand-gold"></i>
    </div>

    <div class="relative z-10 flex flex-col h-full">
      
      <!-- Card Header -->
      <div class="flex justify-between items-start mb-6">
        <div>
          <h3 class="font-serif-title text-2xl font-bold text-brand-parchment mb-2 tracking-wide group-hover:text-brand-gold transition-colors">{{ module.label }}</h3>
          <p class="text-sm font-sans-body text-brand-parchment/60 font-medium tracking-wide">
            {{ module.questionsAnswered }} questions répondues
          </p>
        </div>
        <div 
          class="px-4 py-1.5 rounded-full text-xs font-bold uppercase tracking-widest border shadow-sm backdrop-blur-sm"
          :class="masteryColorClass"
        >
          {{ module.currentMasteryLabel }}
        </div>
      </div>

      <div class="mt-auto space-y-6">
        <!-- XP Progress -->
        <div>
          <div class="flex justify-between text-xs font-bold uppercase tracking-widest mb-3">
            <span class="text-brand-parchment/50">Progression</span>
            <span class="text-brand-gold">{{ module.totalPoints }} XP</span>
          </div>
          
          <div class="h-2 w-full bg-brand-dark/50 rounded-full overflow-hidden border border-brand-gold/10">
            <div 
              class="h-full bg-gradient-to-r from-brand-wood to-brand-gold rounded-full relative"
              :style="{ width: `${Math.min((module.totalPoints / 200) * 100, 100)}%` }"
            >
                <div class="absolute inset-0 bg-white/20 animate-pulse"></div>
            </div>
          </div>
        </div>

        <!-- Action Button -->
        <button 
          @click="$emit('play', module.moduleId)"
          class="w-full py-4 rounded-xl font-bold text-sm uppercase tracking-widest transition-all duration-300 transform active:scale-95 flex items-center justify-center gap-3 overflow-hidden group/btn relative"
          :class="module.isMastered 
            ? 'bg-brand-wood/20 text-brand-gold border border-brand-gold/30 hover:bg-brand-gold hover:text-brand-dark' 
            : 'bg-brand-gold text-brand-dark shadow-lg hover:bg-white'"
        >
          <span class="relative z-10">{{ module.isMastered ? 'Réviser' : 'Continuer' }}</span>
          <i class="fas fa-arrow-right relative z-10 transition-transform duration-300 group-hover/btn:translate-x-1"></i>
          
          <!-- Hover shine effect -->
          <div v-if="!module.isMastered" class="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent -translate-x-full group-hover/btn:animate-shine"></div>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { ModuleProgressionDto } from '~/types'

const props = defineProps<{
  module: ModuleProgressionDto
}>()

defineEmits<{
  (e: 'play', moduleId: string): void
}>()

const masteryColorClass = computed(() => {
  // Using brand colors for badges
  switch (props.module.currentMasteryLabel) {
    case 'Maître': return 'bg-brand-gold/20 text-brand-gold border-brand-gold/40'
    case 'Avancé': return 'bg-purple-500/10 text-purple-300 border-purple-500/30' // Keep purple for distinction but desaturated
    case 'Intermédiaire': return 'bg-blue-500/10 text-blue-300 border-blue-500/30' // Keep blue for distinction
    default: return 'bg-brand-wood/30 text-brand-parchment/50 border-brand-parchment/10'
  }
})
</script>

<style scoped>
/* Shine animation for button */
@keyframes shine {
  100% {
    transform: translateX(100%);
  }
}
.group-hover\/btn\:animate-shine:hover {
  animation: shine 0.5s;
}
</style>
