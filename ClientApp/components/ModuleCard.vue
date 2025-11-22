<template>
  <div
    class="block p-5 bg-white rounded-lg shadow hover:shadow-lg transition-all duration-300 border-l-4"
    :class="borderColor"
  >
    <div class="flex justify-between items-start mb-3">
      <h4 class="text-lg font-semibold text-gray-800">{{ module.title }}</h4>
      <div class="flex items-center space-x-2">
        <!-- Badge Prix -->
        <span v-if="!module.isFree" class="text-xs bg-green-100 text-green-700 px-2 py-1 rounded-full font-medium">
          {{ formatPrice(module.price) }}
        </span>
        <span v-else class="text-xs bg-blue-100 text-blue-700 px-2 py-1 rounded-full font-medium">
          Gratuit
        </span>
        <!-- Badge Durée -->
        <span class="text-xs bg-primary-100 text-primary-700 px-2 py-1 rounded-full">
          {{ module.durationMinutes }} min
        </span>
      </div>
    </div>
    
    <p class="text-sm text-gray-600 mb-3">{{ module.description }}</p>
    
    <div class="flex items-center justify-between text-xs text-gray-500 mb-4">
      <span>{{ module.sectionCount }} sections</span>
      <span v-if="module.videoId" class="flex items-center">
        🎥 Vidéo incluse
      </span>
    </div>

    <!-- Statut d'accès -->
    <div class="flex items-center justify-between">
      <span v-if="module.hasAccess" class="text-xs text-green-600 font-medium flex items-center">
        ✓ Accès accordé
      </span>
      <span v-else-if="!module.isFree" class="text-xs text-gray-500">
        🔒 Achat requis
      </span>
      <span v-else class="text-xs text-blue-600 font-medium">
        📖 Accès libre
      </span>

      <NuxtLink
        :to="`/modules/${module.id}`"
        class="text-sm text-primary-600 hover:text-primary-700 font-medium"
      >
        Voir le module →
      </NuxtLink>
    </div>
  </div>
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
  return colors[props.module.displayOrder % colors.length]
})

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency: 'EUR'
  }).format(price)
}
</script>

