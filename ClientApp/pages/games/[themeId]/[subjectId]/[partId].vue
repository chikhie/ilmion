<template>
  <div class="min-h-screen bg-gray-50 py-12">
    <div class="max-w-4xl mx-auto px-4">
      <NuxtLink :to="`/games/${themeId}`" class="inline-flex items-center text-gray-500 hover:text-emerald-600 mb-6 transition-colors">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
        </svg>
        Retour au sujet
      </NuxtLink>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-emerald-600"></div>
      </div>
      
      <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded relative text-center">
        <strong class="font-bold">Erreur !</strong>
        <span class="block sm:inline"> Impossible de charger le quiz.</span>
      </div>
      
      <div v-else class="animate-fade-in">
         <ClientOnly>
             <GameQuizGame 
                :questions="questions" 
                :title="'Quiz'"
                :key="refreshKey"
                @close="goBack"
                @retry="restartGame"
             />
         </ClientOnly>
      </div>
    </div>
  </div>
</template>

<script setup>
const route = useRoute()
const router = useRouter()
const themeId = route.params.themeId
const subjectId = route.params.subjectId
const partId = route.params.partId
const refreshKey = ref(0)

const { data: questions, pending, error } = await useFetch(`/api/theme/${themeId}/subjects/${subjectId}/parts/${partId}/quiz`)

function goBack() {
  router.push(`/games/${themeId}`)
}

function restartGame() {
    refreshKey.value++
}
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.5s ease-out;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
