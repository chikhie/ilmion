<template>
  <div class="min-h-screen bg-gray-50 py-12">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <NuxtLink to="/games" class="inline-flex items-center text-gray-500 hover:text-emerald-600 mb-6 transition-colors">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
        </svg>
        Retour aux thèmes
      </NuxtLink>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-emerald-600"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 text-red-600">
        Impossible de charger le thème.
      </div>

      <div v-else>
         <div class="bg-white rounded-2xl shadow-xl overflow-hidden mb-8">
             <div class="bg-gray-900 px-8 py-6 text-white">
                 <h1 class="text-3xl font-bold">{{ theme.title }}</h1>
                 <p class="text-gray-300 mt-2">{{ theme.description }}</p>
             </div>
         </div>

         <div class="space-y-6">
            <div v-for="subject in theme.subjects" :key="subject.id" class="bg-white rounded-xl shadow-md overflow-hidden border border-gray-100">
                <div class="px-6 py-4 bg-gray-50 border-b border-gray-100 flex justify-between items-center">
                    <h2 class="text-xl font-semibold text-gray-800">{{ subject.title }}</h2>
                    <span class="text-sm text-gray-500 bg-white px-3 py-1 rounded-full border border-gray-200">
                        {{ subject.parts?.length || 0 }} partie(s)
                    </span>
                </div>
                
                <div class="p-6">
                    <div v-if="!subject.parts || subject.parts.length === 0" class="text-gray-400 italic text-sm">
                        Aucune partie disponible pour ce sujet.
                    </div>
                    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        <div v-for="part in subject.parts" :key="part.id" class="border rounded-lg p-4 hover:shadow-md transition-shadow">
                            <h3 class="font-bold text-gray-800 mb-1">{{ part.title }}</h3>
                            <p class="text-xs text-gray-600 mb-3 line-clamp-2">{{ part.description }}</p>
                             <div class="flex items-center justify-between mt-2">
                                 <span class="text-xs font-medium text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded">Quiz</span>
                                 <NuxtLink :to="`/games/${theme.id}/${subject.id}/${part.id}`" class="text-sm bg-emerald-600 text-white px-4 py-1.5 rounded hover:bg-emerald-700 transition-colors">
                                    Jouer
                                 </NuxtLink>
                             </div>
                        </div>
                    </div>
                </div>
            </div>
         </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const route = useRoute()
const themeId = route.params.themeId

const { data: theme, pending, error } = await useFetch(`/api/theme/${themeId}`)
</script>
