<template>
  <div class="glass-card p-5 rounded-xl mb-4 group hover:bg-slate-800/40 transition-all duration-300 border-l-4 border-l-primary-500">
    <div class="flex flex-col md:flex-row justify-between items-center gap-4">
      
      <!-- Content Section -->
      <div class="flex-1 text-left w-full md:w-auto">
        <div class="flex items-center gap-3 mb-1">
          <span class="text-xs font-bold text-gray-500 uppercase tracking-wider">QUIZ</span>
          <div v-if="module.isMastered" class="px-2 py-0.5 rounded text-[10px] bg-green-900/30 text-green-400 border border-green-800">
            MAÎTRISÉ
          </div>
        </div>
        <h3 class="text-lg font-bold text-white mb-1">{{ module.label }}</h3>
        <p class="text-sm text-gray-400 line-clamp-1">
          {{ getModuleDescription(module.moduleId) }}
        </p>
      </div>

      <!-- Action Section -->
      <div class="flex items-center gap-6 w-full md:w-auto justify-between md:justify-end">
        <!-- Progress Info (Optional, keep it subtle) -->
        <div v-if="module.totalPoints > 0" class="text-right hidden sm:block">
          <div class="text-xs text-gray-400">Score</div>
          <div class="text-sm font-mono text-primary-400">{{ module.totalPoints }} XP</div>
        </div>

        <button 
          @click="$emit('play', module.moduleId)"
          class="px-6 py-2 rounded-lg font-bold text-sm bg-yellow-500 hover:bg-yellow-400 text-slate-900 shadow-lg shadow-yellow-900/20 transition-all transform active:scale-95 uppercase tracking-wide"
        >
          JOUER
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

// Helper to provide descriptions since backend might not send them yet
const getModuleDescription = (id: string) => {
  if (id === 'tawheed') return "Les fondements de la foi et l'unicité d'Allah."
  if (id === 'culture') return "Culture générale et histoire islamique."
  return "Module de quiz éducatif."
}
</script>

<style scoped>
.glass-card {
  background: rgba(30, 41, 59, 0.3);
  backdrop-filter: blur(4px);
  border: 1px solid rgba(255, 255, 255, 0.05); /* Slight border for separation */
}
</style>
