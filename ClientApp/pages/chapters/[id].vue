<template>
  <div class="min-h-screen bg-gray-900">
    <!-- Header -->
    <header class="bg-gray-800 shadow-sm border-b border-gray-700">
      <div class="container mx-auto px-4 py-4">
        <button @click="$router.back()" class="inline-flex items-center text-gray-300 hover:text-white transition-colors">
          ← Retour
        </button>
      </div>
    </header>

    <div v-if="chapterPending" class="container mx-auto px-4 py-12 text-center">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-500"></div>
      <p class="mt-4 text-gray-400">Chargement du chapitre...</p>
    </div>

    <div v-else-if="chapterError" class="container mx-auto px-4 py-12 text-center">
      <p class="text-red-400">❌ Erreur lors du chargement du chapitre</p>
    </div>

    <div v-else class="container mx-auto px-4 py-8">
      <!-- Chapter Header -->
      <div class="bg-gray-800 rounded-lg shadow-lg p-8 mb-8 border border-gray-700">
        <!-- Breadcrumb -->
        <Breadcrumb 
          v-if="chapterData"
          :items="[
            { label: 'Accueil', path: '/' },
            { label: chapterData.subjectName, path: `/subjects/${chapterData.subjectId}` },
            { label: chapterData.moduleName, path: `/modules/${chapterData.moduleId}` },
            { label: chapterData.title, path: `/chapters/${chapterData.id}` }
          ]"
          class="mb-6"
        />
        
        <div class="flex justify-between items-start mb-4">
          <div class="flex-1">
            <h1 class="text-3xl font-bold text-white mb-2">{{ chapterData?.title }}</h1>
            <p class="text-gray-400">{{ chapterData?.description }}</p>
          </div>
          <div class="ml-4">
            <span class="inline-block px-4 py-2 bg-gray-700 text-primary-400 rounded-lg font-medium border border-gray-600">
              {{ chapterData?.durationMinutes }} min
            </span>
          </div>
        </div>
        
        <div class="flex items-center text-sm text-gray-400 space-x-4">
          <span class="flex items-center"><span class="mr-2">🎓</span> {{ chapterData?.subjectName }}</span>
          <span class="flex items-center"><span class="mr-2">📚</span> {{ chapterData?.moduleName }}</span>
          <span class="flex items-center"><span class="mr-2">📝</span> {{ sections?.length || 0 }} sections</span>
          <span v-if="chapterData?.videoUrl" class="flex items-center"><span class="mr-2">🎥</span> Vidéo incluse</span>
        </div>
      </div>

      <!-- No Access Message -->
      <div v-if="!hasAccess" class="bg-yellow-900/30 border border-yellow-600 rounded-lg p-6 mb-8">
        <div class="flex items-start">
          <span class="text-3xl mr-4">🔒</span>
          <div class="flex-1">
            <h3 class="text-xl font-bold text-yellow-400 mb-2">Abonnement requis</h3>
            <p class="text-gray-300 mb-4">
              Ce contenu est réservé aux abonnés. Souscrivez à un abonnement pour accéder à tous les chapitres et contenus de la plateforme.
            </p>
            <NuxtLink 
              to="/subscribe" 
              class="inline-block px-6 py-3 bg-primary-600 hover:bg-primary-700 text-white rounded-lg font-medium transition-colors"
            >
              S'abonner maintenant
            </NuxtLink>
          </div>
        </div>
      </div>

      <!-- Content (only if has access) -->
      <div v-else>
        <div v-if="sectionsPending" class="text-center py-12">
          <p class="text-gray-400">Chargement du contenu...</p>
        </div>

        <div v-else-if="sectionsError" class="text-center py-12">
          <p class="text-red-400">❌ Erreur lors du chargement du contenu</p>
        </div>

        <div v-else-if="sections && sections.length > 0" class="space-y-8">
          <!-- Vidéo Section (supporte YouTube et HLS) -->
          <div v-if="chapterData?.videoId" class="mb-8">
            <!-- Exemple YouTube : https://www.youtube.com/watch?v=dQw4w9WgXcQ -->
            <!-- Exemple HLS : https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8 -->
            <VideoPlayer :src="`https://www.youtube.com/watch?v=dQw4w9WgXcQ`" />
          </div>

          <!-- Sections -->
          <div v-for="section in sections" :key="section.id" class="space-y-6">
            <!-- Text Section -->
            <div v-if="section.sectionTypeId === 1" class="mb-6">
              <h2 v-if="section.title" class="text-2xl font-bold text-white mb-4">{{ section.title }}</h2>
              <div class="text-gray-300 leading-relaxed text-lg whitespace-pre-wrap">{{ section.content }}</div>
            </div>

            <!-- Image Section -->
            <div v-else-if="section.sectionTypeId === 2" class="flex flex-col items-center mb-6">
              <img :src="section.content" alt="Illustration" class="rounded-lg max-w-full h-auto shadow-md mb-2 border border-gray-700" />
              <p v-if="section.title" class="text-sm text-gray-400 italic">{{ section.title }}</p>
            </div>

            <!-- Video Section -->
            <div v-else-if="section.sectionTypeId === 3" class="mb-8">
              <h2 v-if="section.title" class="text-2xl font-bold text-white mb-4">{{ section.title }}</h2>
              <VideoPlayer :src="section.content" />
            </div>

            <!-- Quiz Section -->
            <div v-else-if="section.sectionTypeId === 4">
              <QuizSection :title="section.title" :content="section.content" />
            </div>

            <!-- Code Section -->
            <div v-else-if="section.sectionTypeId === 5" class="mb-6">
              <h2 v-if="section.title" class="text-2xl font-bold text-white mb-4">{{ section.title }}</h2>
              <pre class="bg-gray-950 text-gray-300 p-4 rounded-lg overflow-x-auto border border-gray-700"><code>{{ section.content }}</code></pre>
            </div>

            <!-- Exercise Section -->
            <div v-else-if="section.sectionTypeId === 6" class="bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-700 mb-6">
              <h2 class="text-2xl font-bold text-white mb-4">{{ section.title }}</h2>
              <div class="text-gray-300 leading-relaxed whitespace-pre-wrap">{{ section.content }}</div>
            </div>
          </div>
        </div>

        <div v-else class="text-center py-12 bg-gray-800 rounded-lg shadow border border-gray-700">
          <p class="text-gray-400">Aucun contenu disponible pour ce chapitre</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Chapter, Section } from '~/types'
import VideoPlayer from '~/components/modules/VideoPlayer.vue'
import QuizSection from '~/components/QuizSection.vue'
import { useAuthStore } from '~/stores/auth'

const route = useRoute()
const api = useApi()
const authStore = useAuthStore()

const chapterId = route.params.id as string

// Charger le chapitre (désactiver le cache serveur pour toujours avoir les données fraîches)
const { data: chapterData, pending: chapterPending, error: chapterError, refresh: refreshChapter } = await useAsyncData<Chapter>(
  `chapter-${chapterId}`,
  () => api.getChapter(chapterId),
  {
    server: false, // Toujours charger côté client
    watch: [() => authStore.hasActiveSubscription] // Recharger quand l'abonnement change
  }
)

// Calculer l'accès : utilisateur premium OU chapitre gratuit OU hasAccess du backend
const hasAccess = computed(() => {
  return authStore.isPremium || chapterData.value?.isFree || chapterData.value?.hasAccess
})

// Charger les sections du chapitre
const { data: sections, pending: sectionsPending, error: sectionsError } = await useAsyncData<Section[]>(
  `chapter-sections-${chapterId}`,
  () => api.getSectionsByChapter(chapterId),
  {
    // Ne charger les sections que si l'utilisateur a accès
    lazy: true,
    server: false
  }
)

useHead({
  title: chapterData.value?.title || 'Chapitre'
})
</script>
