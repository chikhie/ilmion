<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm">
      <div class="container mx-auto px-4 py-4">
        <NuxtLink to="/" class="inline-flex items-center text-primary-600 hover:text-primary-700">
          ← Retour aux matières
        </NuxtLink>
      </div>
    </header>

    <!-- Subject Header -->
    <section class="bg-gradient-to-r from-primary-600 to-primary-800 text-white">
      <div class="container mx-auto px-4 py-12">
        <div v-if="pending" class="text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-white"></div>
        </div>
        <div v-else-if="modules && modules.length > 0" class="flex items-center">
          <div class="text-6xl mr-6">{{ modules[0].subjectName === 'Mathématiques' ? '🔢' : modules[0].subjectName === 'Physique' ? '⚡' : modules[0].subjectName === 'Chimie' ? '🧪' : '🧬' }}</div>
          <div>
            <h1 class="text-4xl font-bold mb-2">{{ modules[0].subjectName }}</h1>
            <p class="text-xl opacity-90">{{ modules.length }} modules disponibles</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Modules List -->
    <section class="container mx-auto px-4 py-12">
      <div v-if="pending" class="text-center py-12">
        <p class="text-gray-600">Chargement des modules...</p>
      </div>

      <div v-else-if="error" class="text-center py-12">
        <p class="text-red-600">❌ Erreur lors du chargement des modules</p>
        <button
          @click="refresh()"
          class="mt-4 px-6 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
        >
          Réessayer
        </button>
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <ModuleCard
          v-for="module in modules"
          :key="module.id"
          :module="module"
        />
      </div>

      <div v-if="!pending && modules?.length === 0" class="text-center py-12">
        <p class="text-gray-600">Aucun module disponible pour cette matière</p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

const route = useRoute()
const api = useApi()

const subjectId = route.params.id as string

const { data: modules, pending, error, refresh } = await useAsyncData<Module[]>(
  `subject-modules-${subjectId}`,
  () => api.getModulesBySubject(subjectId)
)

useHead({
  title: modules.value?.[0]?.subjectName || 'Matière'
})
</script>

