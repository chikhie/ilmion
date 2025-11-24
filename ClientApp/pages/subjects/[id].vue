<template>
  <div class="min-h-screen bg-gray-900">
    <!-- Header -->
    <header class="bg-gray-800 shadow-sm border-b border-gray-700">
      <div class="container mx-auto px-4 py-4">
        <NuxtLink to="/" class="inline-flex items-center text-gray-300 hover:text-[#C39712] transition-colors">
          ← Retour à l'accueil
        </NuxtLink>
      </div>
    </header>

    <!-- Subject Header -->
    <section class="bg-gradient-to-r from-[#082540] to-[#0a3a5f] text-white relative overflow-hidden">
      <div class="absolute inset-0 opacity-10">
        <div class="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHZpZXdCb3g9IjAgMCA2MCA2MCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBmaWxsPSJub25lIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiPjxnIGZpbGw9IiNmZmYiIGZpbGwtb3BhY2l0eT0iMC4xIj48cGF0aCBkPSJNMzYgMzRjMC0yLjIxLTEuNzktNC00LTRzLTQgMS43OS00IDQgMS43OSA0IDQgNCA0LTEuNzkgNC00em0wLTEwYzAtMi4yMS0xLjc5LTQtNC00cy00IDEuNzktNCA0IDEuNzkgNCA0IDQgNC0xLjc5IDQtNHptLTEwIDBjMC0yLjIxLTEuNzktNC00LTRzLTQgMS43OS00IDQgMS43OSA0IDQgNCA0LTEuNzkgNC00eiIvPjwvZz48L2c+PC9zdmc+')] bg-repeat"></div>
      </div>
      
      <div class="container mx-auto px-4 py-16 relative z-10">
        <div v-if="pending" class="text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-[#C39712]"></div>
        </div>
        <div v-else-if="subject">
          <!-- Breadcrumb -->
          <Breadcrumb 
            :items="[
              { label: 'Accueil', path: '/' },
              { label: subject.label, path: `/subjects/${subject.id}` }
            ]"
            class="mb-8"
          />
          
          <div class="flex items-center justify-between">
            <div class="flex items-center">
              <div class="text-7xl mr-6 drop-shadow-lg">{{ subject.label === 'Mathématiques' ? '🔢' : subject.label === 'Physique' ? '⚡' : subject.label === 'Chimie' ? '🧪' : subject.label === 'Informatique' ? '💻' : '🧬' }}</div>
              <div>
                <h1 class="text-5xl font-bold mb-3 text-white">{{ subject.label }}</h1>
                <p class="text-xl text-gray-300">{{ modules?.length || 0 }} modules disponibles</p>
              </div>
            </div>
            <div class="hidden md:block">
              <div class="bg-[#C39712]/20 border border-[#C39712] rounded-lg px-6 py-4 text-center">
                <p class="text-3xl font-bold text-[#C39712]">{{ modules?.length || 0 }}</p>
                <p class="text-sm text-gray-300 mt-1">Modules</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Modules List -->
    <section class="container mx-auto px-4 py-16">
      <div v-if="pending" class="text-center py-12">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-[#C39712]"></div>
        <p class="text-gray-400 mt-4">Chargement des modules...</p>
      </div>

      <div v-else-if="error" class="text-center py-12">
        <div class="bg-red-900/20 border border-red-600 rounded-lg p-8 max-w-md mx-auto">
          <p class="text-red-400 text-lg mb-4">❌ Erreur lors du chargement des modules</p>
          <button
            @click="refresh()"
            class="px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-medium"
          >
            Réessayer
          </button>
        </div>
      </div>

      <div v-else-if="modules && modules.length > 0">
        <div class="mb-8">
          <h2 class="text-2xl font-bold text-white mb-2">Modules disponibles</h2>
          <div class="w-20 h-1 bg-[#C39712] rounded-full"></div>
        </div>
        
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <NuxtLink
            v-for="module in modules"
            :key="module.id"
            :to="`/modules/${module.id}`"
            class="group bg-gray-800 rounded-xl overflow-hidden border border-gray-700 hover:border-[#C39712] transition-all duration-300 hover:shadow-xl hover:shadow-[#C39712]/10 hover:scale-105"
          >
            <div class="p-6">
              <div class="flex items-start justify-between mb-4">
                <h3 class="text-xl font-bold text-white group-hover:text-[#C39712] transition-colors flex-1">
                  {{ module.label }}
                </h3>
                <span class="ml-3 px-3 py-1 bg-gray-700 group-hover:bg-[#C39712]/20 text-gray-300 group-hover:text-[#C39712] rounded-full text-sm font-medium transition-colors border border-gray-600 group-hover:border-[#C39712]">
                  {{ module.chapterCount }} chapitres
                </span>
              </div>
              
              <p class="text-gray-400 text-sm mb-4 line-clamp-2">
                Explorez {{ module.chapterCount }} chapitres passionnants dans ce module.
              </p>
              
              <div class="flex items-center justify-between">
                <span class="text-sm text-gray-500">{{ module.subjectName }}</span>
                <span class="text-[#C39712] group-hover:text-white transition-colors font-medium text-sm flex items-center">
                  Explorer
                  <svg class="w-4 h-4 ml-1 transform group-hover:translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"></path>
                  </svg>
                </span>
              </div>
            </div>
          </NuxtLink>
        </div>
      </div>

      <div v-else class="text-center py-16">
        <div class="bg-gray-800 border border-gray-700 rounded-lg p-12 max-w-md mx-auto">
          <div class="text-6xl mb-4">📚</div>
          <p class="text-gray-400 text-lg">Aucun module disponible pour cette matière</p>
          <NuxtLink to="/" class="inline-block mt-6 px-6 py-3 bg-[#C39712] text-[#082540] rounded-lg font-bold hover:bg-yellow-500 transition-colors">
            Retour à l'accueil
          </NuxtLink>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { Module, Subject } from '~/types'

const route = useRoute()
const api = useApi()

const subjectId = parseInt(route.params.id as string)

// Charger le sujet
const { data: subject } = await useAsyncData<Subject>(
  `subject-${subjectId}`,
  () => api.getSubject(subjectId)
)

// Charger les modules du sujet
const { data: modules, pending, error, refresh } = await useAsyncData<Module[]>(
  `subject-modules-${subjectId}`,
  () => api.getModulesBySubject(subjectId)
)

useHead({
  title: subject.value?.label || 'Matière'
})
</script>

