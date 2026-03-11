<template>
  <NuxtLink
    :to="`/modules/${module.id}`"
    class="block p-6 bg-[#2b2d31] rounded-lg shadow-md hover:shadow-xl transition-all duration-300 border border-[#00B578]/20 group relative overflow-hidden"
  >
    <!-- Decoration d'angle -->
    <div class="absolute top-0 right-0 w-16 h-16 bg-gradient-to-bl from-brand-gold/10 to-transparent pointer-events-none"></div>

    <div class="flex justify-between items-start mb-3 relative z-10">
      <h4 class="text-xl font-sans font-bold text-[#1e1f22] group-hover:text-[#2b2d31] transition-colors">{{ module.label }}</h4>
      <span class="text-xs bg-[#1e1f22]/10 text-[#1e1f22] px-3 py-1 rounded-full font-bold uppercase tracking-wider">
        {{ module.chapterCount }} chapitres
      </span>
    </div>
    
    <div class="flex items-center justify-between mt-4 relative z-10">
      <span class="text-sm text-gray-600 font-sans">
        {{ module.subjectName }}
      </span>
      <span class="text-sm text-[#2b2d31] font-bold hover:text-[#00B578] transition-colors flex items-center gap-1">
        Voir les chapitres <span class="group-hover:translate-x-1 transition-transform">→</span>
      </span>
    </div>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

const props = defineProps<{
  module: Module
}>()

</script>

