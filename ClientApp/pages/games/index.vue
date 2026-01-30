<template>
  <div class="min-h-screen bg-gray-50 py-12">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="text-center mb-12">
        <h1 class="text-4xl font-extrabold text-gray-900 sm:text-5xl sm:tracking-tight lg:text-6xl">
          Thèmes & Quiz
        </h1>
        <p class="mt-5 max-w-xl mx-auto text-xl text-gray-500">
          Explorez nos thèmes et testez vos connaissances.
        </p>
      </div>

      <div v-if="pending" class="flex justify-center py-20">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-emerald-600"></div>
      </div>

      <div v-else-if="error" class="text-center py-20 text-red-600">
        Une erreur est survenue lors du chargement des thèmes.
      </div>
      
      <div v-else class="space-y-12">
        <div v-for="theme in themes" :key="theme.id" class="bg-white rounded-2xl shadow-xl overflow-hidden">
             <div class="bg-emerald-600 px-6 py-4">
                 <h2 class="text-2xl font-bold text-white">{{ theme.title }}</h2>
                 <p class="text-emerald-100">{{ theme.description }}</p>
             </div>
             
             <div class="p-6">
                 <p class="text-gray-600 mb-4">Ce thème contient {{ theme.subjects?.length || 0 }} sujets.</p>
                 <NuxtLink :to="`/games/${theme.id}`" class="inline-block bg-gray-900 text-white rounded-lg py-2 px-6 hover:bg-emerald-600 transition-colors">
                    Explorer ce thème
                 </NuxtLink>
             </div>
        </div>
      </div>
      
      <div v-if="!pending && (!themes || themes.length === 0)" class="text-center py-20 bg-white rounded-xl shadow-sm border border-dashed border-gray-300">
          <p class="text-gray-500 text-lg">Aucun thème disponible pour le moment.</p>
      </div>

    </div>
  </div>
</template>

<script setup>
const { data: themes, pending, error } = await useFetch('/api/theme')
</script>
