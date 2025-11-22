<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
    <!-- Hero Section -->
    <section class="container mx-auto px-4 py-12">
      <div class="text-center mb-12">
        <h2 class="text-4xl md:text-5xl font-bold text-gray-800 mb-4">
          Apprenez les Sciences
        </h2>
        <p class="text-xl text-gray-600 max-w-2xl mx-auto">
          Explorez nos cours interactifs en mathématiques, physique, chimie et biologie
        </p>
      </div>
    </section>

    <!-- Subjects Grid -->
    <section class="container mx-auto px-4 pb-16">
      <div v-if="pending" class="text-center py-12">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Chargement des matières...</p>
      </div>

      <div v-else-if="error" class="text-center py-12">
        <p class="text-red-600">❌ Erreur lors du chargement des matières</p>
        <button
          @click="refresh()"
          class="mt-4 px-6 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors"
        >
          Réessayer
        </button>
      </div>

      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        <SubjectCard
          v-for="subject in subjects"
          :key="subject.id"
          :subject="subject"
        />
      </div>

      <div v-if="!pending && subjects?.length === 0" class="text-center py-12">
        <p class="text-gray-600 mb-4">Aucune matière disponible pour le moment</p>
        <p class="text-sm text-gray-500">Les cours seront bientôt disponibles !</p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { Subject } from '~/types'

useHead({
  title: 'Accueil'
})

const api = useApi()

const { data: subjects, pending, error, refresh } = await useAsyncData<Subject[]>(
  'subjects',
  () => api.getSubjects()
)
</script>

