<template>
  <NuxtLink
    :to="`/modules/${module.id}`"
    class="block p-6 bg-white rounded-lg shadow hover:shadow-lg transition-all duration-300 border-l-4"
    :class="borderColor"
  >
    <div class="flex justify-between items-start mb-3">
      <h4 class="text-xl font-semibold text-gray-800">{{ module.label }}</h4>
      <span class="text-xs bg-primary-100 text-primary-700 px-3 py-1 rounded-full font-medium">
        {{ module.chapterCount }} chapitres
      </span>
    </div>
    
    <div class="flex items-center justify-between mt-4">
      <span class="text-sm text-gray-500">
        {{ module.subjectName }}
      </span>
      <span class="text-sm text-primary-600 hover:text-primary-700 font-medium">
        Voir les chapitres →
      </span>
    </div>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

const props = defineProps<{
  module: Module
}>()

const borderColor = computed(() => {
  const colors = [
    'border-blue-500',
    'border-green-500',
    'border-purple-500',
    'border-orange-500',
    'border-pink-500'
  ]
  // Utiliser l'ID pour générer une couleur cohérente
  const hash = props.module.id.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0)
  return colors[hash % colors.length]
})
</script>

