<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm">
      <div class="container mx-auto px-4 py-4">
        <button @click="$router.back()" class="inline-flex items-center text-primary-600 hover:text-primary-700">
          ← Retour
        </button>
      </div>
    </header>

    <div v-if="modulePending" class="container mx-auto px-4 py-12 text-center">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      <p class="mt-4 text-gray-600">Chargement du module...</p>
    </div>

    <div v-else-if="moduleError" class="container mx-auto px-4 py-12 text-center">
      <p class="text-red-600">❌ Erreur lors du chargement du module</p>
    </div>

    <div v-else class="container mx-auto px-4 py-8">
      <!-- Module Header -->
      <div class="bg-white rounded-lg shadow-md p-8 mb-8">
        <div class="flex justify-between items-start mb-4">
          <div class="flex-1">
            <h1 class="text-3xl font-bold text-gray-800 mb-2">{{ moduleData?.title }}</h1>
            <p class="text-gray-600">{{ moduleData?.description }}</p>
          </div>
          <div class="ml-4">
            <span class="inline-block px-4 py-2 bg-primary-100 text-primary-700 rounded-lg font-medium">
              {{ moduleData?.durationMinutes }} min
            </span>
          </div>
        </div>
        
        <div class="flex items-center text-sm text-gray-500 space-x-4">
          <span>📚 {{ moduleData?.subjectName }}</span>
          <span>📝 {{ sections?.length || 0 }} sections</span>
          <span v-if="moduleData?.videoId">🎥 Vidéo incluse</span>
        </div>
      </div>

      <!-- Sections -->
      <div v-if="sectionsPending" class="text-center py-12">
        <p class="text-gray-600">Chargement des sections...</p>
      </div>

      <div v-else-if="sectionsError" class="text-center py-12">
        <p class="text-red-600">❌ Erreur lors du chargement des sections</p>
      </div>

      <div v-else-if="sections && sections.length > 0">
        <SectionViewer
          v-for="section in sections"
          :key="section.id"
          :section="section"
        />
      </div>

      <div v-else class="text-center py-12 bg-white rounded-lg shadow">
        <p class="text-gray-600">Aucune section disponible pour ce module</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Module, Section } from '~/types'

const route = useRoute()
const api = useApi()

const moduleId = route.params.id as string

// Charger le module
const { data: moduleData, pending: modulePending, error: moduleError } = await useAsyncData<Module>(
  `module-${moduleId}`,
  () => api.getModule(moduleId)
)

// Charger les sections
const { data: sections, pending: sectionsPending, error: sectionsError } = await useAsyncData<Section[]>(
  `module-sections-${moduleId}`,
  () => api.getSectionsByModule(moduleId)
)

useHead({
  title: moduleData.value?.title || 'Module'
})
</script>

