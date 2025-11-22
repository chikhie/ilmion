<template>
  <div class="bg-white rounded-lg shadow-md p-6 mb-4">
    <div class="flex items-center justify-between mb-4">
      <h3 class="text-2xl font-bold text-gray-800">{{ section.title }}</h3>
      <span
        class="px-3 py-1 rounded-full text-sm font-medium"
        :class="typeClass"
      >
        {{ section.typeLabel }}
      </span>
    </div>

    <div class="content">
      <!-- Contenu texte/markdown -->
      <div v-if="section.type === 0" class="prose max-w-none" v-html="renderedContent"></div>

      <!-- Contenu interactif protégé -->
      <div v-else-if="section.type === 1 && section.hasProtectedComponent" class="interactive-protected">
        <ProtectedComponent :section-id="section.id" />
      </div>

      <!-- Contenu interactif simple (fallback) -->
      <div v-else-if="section.type === 1" class="interactive-content">
        <div v-html="section.content"></div>
      </div>

      <!-- Quiz avec composant protégé -->
      <div v-else-if="section.type === 2 && section.hasProtectedComponent" class="quiz-content">
        <div class="bg-blue-50 p-4 rounded-lg">
          <p class="text-sm text-blue-800 mb-2 font-semibold flex items-center">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
            </svg>
            Quiz Interactif
          </p>
          <ProtectedComponent :section-id="section.id" />
        </div>
      </div>

      <!-- Quiz simple -->
      <div v-else-if="section.type === 2" class="quiz-content">
        <div class="bg-blue-50 p-4 rounded-lg">
          <p class="text-sm text-blue-800 mb-2">📝 Quiz</p>
          <div v-html="renderedContent"></div>
        </div>
      </div>

      <!-- Exercice avec composant protégé -->
      <div v-else-if="section.type === 3 && section.hasProtectedComponent" class="exercise-content">
        <div class="bg-green-50 p-4 rounded-lg">
          <p class="text-sm text-green-800 mb-2 font-semibold flex items-center">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z"></path>
            </svg>
            Exercice Pratique
          </p>
          <ProtectedComponent :section-id="section.id" />
        </div>
      </div>

      <!-- Exercice simple -->
      <div v-else-if="section.type === 3" class="exercise-content">
        <div class="bg-green-50 p-4 rounded-lg">
          <p class="text-sm text-green-800 mb-2">✏️ Exercice</p>
          <div v-html="renderedContent"></div>
        </div>
      </div>

      <!-- Résumé -->
      <div v-else-if="section.type === 4" class="summary-content">
        <div class="bg-yellow-50 p-4 rounded-lg border-l-4 border-yellow-500">
          <p class="text-sm text-yellow-800 mb-2 font-bold">📌 Résumé</p>
          <div v-html="renderedContent"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Section } from '~/types'

const props = defineProps<{
  section: Section
}>()

const renderedContent = computed(() => {
  // Simple conversion markdown ou HTML
  return props.section.content
})

const typeClass = computed(() => {
  switch (props.section.type) {
    case 0: return 'bg-gray-100 text-gray-700'
    case 1: return 'bg-purple-100 text-purple-700'
    case 2: return 'bg-blue-100 text-blue-700'
    case 3: return 'bg-green-100 text-green-700'
    case 4: return 'bg-yellow-100 text-yellow-700'
    default: return 'bg-gray-100 text-gray-700'
  }
})
</script>

<style scoped>
.prose {
  @apply text-gray-700 leading-relaxed;
}

.prose h1 {
  @apply text-3xl font-bold mb-4 text-gray-900;
}

.prose h2 {
  @apply text-2xl font-bold mb-3 text-gray-900;
}

.prose h3 {
  @apply text-xl font-bold mb-2 text-gray-900;
}

.prose p {
  @apply mb-4;
}

.prose ul {
  @apply list-disc list-inside mb-4;
}

.prose ol {
  @apply list-decimal list-inside mb-4;
}
</style>

