<template>
  <div class="min-h-screen bg-gray-900">
    <!-- Header -->
    <header class="bg-gray-800 shadow-sm border-b border-gray-700">
      <div class="container mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-white">Modules disponibles</h1>
      </div>
    </header>

    <div class="container mx-auto px-4 py-8">
      <div v-if="loading" class="text-center py-12">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-500"></div>
        <p class="mt-4 text-gray-400">Chargement des modules...</p>
      </div>

      <div v-else-if="error" class="text-center py-12">
        <p class="text-red-400">❌ Erreur lors du chargement des modules</p>
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <!-- Carte pour le module de démo -->
        <div class="bg-gray-800 rounded-lg shadow-lg hover:shadow-xl transition-all duration-300 overflow-hidden border border-gray-700 hover:border-gray-600 group">
          <div class="h-40 bg-gradient-to-r from-blue-900 to-purple-900 flex items-center justify-center group-hover:from-blue-800 group-hover:to-purple-800 transition-all">
            <span class="text-5xl drop-shadow-lg">⚛️</span>
          </div>
          <div class="p-6">
            <div class="flex justify-between items-start mb-3">
              <h2 class="text-xl font-bold text-white group-hover:text-primary-400 transition-colors">La Structure de la Matière</h2>
              <span class="px-2 py-1 bg-green-900/30 text-green-400 text-xs font-semibold rounded-full border border-green-800">Gratuit</span>
            </div>
            <p class="text-gray-400 mb-4 line-clamp-2">Plongée au cœur de l'infiniment petit : atomes, molécules et particules.</p>
            
            <div class="flex items-center justify-between mt-4 pt-4 border-t border-gray-700">
              <span class="text-sm text-gray-500 flex items-center">
                <span class="mr-1">⏱️</span> 30 min
              </span>
              <NuxtLink 
                to="/modules/demo-structure-matiere" 
                class="px-4 py-2 bg-primary-600 hover:bg-primary-500 text-white rounded transition-colors text-sm font-medium shadow-lg shadow-primary-900/20"
              >
                Commencer
              </NuxtLink>
            </div>
          </div>
        </div>

        <!-- Vrais modules depuis l'API -->
        <div v-for="module in modules" :key="module.id" class="bg-gray-800 rounded-lg shadow-lg hover:shadow-xl transition-all duration-300 overflow-hidden border border-gray-700 hover:border-gray-600 group">
          <div class="h-40 bg-gray-700 flex items-center justify-center group-hover:bg-gray-600 transition-colors">
            <span class="text-5xl drop-shadow-lg">📚</span>
          </div>
          <div class="p-6">
            <div class="flex justify-between items-start mb-3">
              <h2 class="text-xl font-bold text-white group-hover:text-primary-400 transition-colors">{{ module.title }}</h2>
              <span 
                class="px-2 py-1 text-xs font-semibold rounded-full border"
                :class="module.isFree ? 'bg-green-900/30 text-green-400 border-green-800' : 'bg-blue-900/30 text-blue-400 border-blue-800'"
              >
                {{ module.isFree ? 'Gratuit' : `${module.price} €` }}
              </span>
            </div>
            <p class="text-gray-400 mb-4 line-clamp-2">{{ module.description }}</p>
            
            <div class="flex items-center justify-between mt-4 pt-4 border-t border-gray-700">
              <span class="text-sm text-gray-500 flex items-center">
                <span class="mr-1">⏱️</span> {{ module.durationMinutes }} min
              </span>
              <NuxtLink 
                :to="`/modules/${module.id}`" 
                class="px-4 py-2 bg-primary-600 hover:bg-primary-500 text-white rounded transition-colors text-sm font-medium shadow-lg shadow-primary-900/20"
              >
                Voir
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

const api = useApi()
const loading = ref(false) // Mettre à true si on charge depuis l'API
const error = ref(null)
const modules = ref<Module[]>([])

// Optionnel : Charger les vrais modules si l'API est prête
/*
onMounted(async () => {
  loading.value = true
  try {
    // Exemple: récupérer tous les modules d'une matière par défaut ou tous les modules
    // modules.value = await api.getAllModules() 
  } catch (e: any) {
    error.value = e
  } finally {
    loading.value = false
  }
})
*/
</script>
