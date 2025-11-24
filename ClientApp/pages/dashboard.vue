<template>
  <div class="min-h-screen bg-gray-900">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Header Section -->
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-white">
          Bonjour, <span class="text-[#C39712]">{{ authStore.user?.username }}</span> 👋
        </h1>
        <p class="text-gray-400 mt-2">Heureux de vous revoir. Prêt à apprendre ?</p>
      </div>

      <!-- Stats Grid -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-12">
        <div class="bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-gray-400 font-medium">Modules Suivis</h3>
            <span class="p-2 bg-blue-900/30 text-blue-400 rounded-lg">📚</span>
          </div>
          <p class="text-3xl font-bold text-white">{{ myModules?.length || 0 }}</p>
          <p class="text-sm text-gray-500 mt-2">Modules dans votre bibliothèque</p>
        </div>

        <div class="bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-gray-400 font-medium">Progression</h3>
            <span class="p-2 bg-green-900/30 text-green-400 rounded-lg">📈</span>
          </div>
          <p class="text-3xl font-bold text-white">0%</p>
          <p class="text-sm text-gray-500 mt-2">Moyenne globale</p>
        </div>

        <div class="bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-gray-400 font-medium">Temps d'apprentissage</h3>
            <span class="p-2 bg-purple-900/30 text-purple-400 rounded-lg">⏱️</span>
          </div>
          <p class="text-3xl font-bold text-white">0h</p>
          <p class="text-sm text-gray-500 mt-2">Cette semaine</p>
        </div>
      </div>

      <!-- Content Grid -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Recent Modules (Left Column - Wider) -->
        <div class="lg:col-span-2 space-y-6">
          <div class="flex items-center justify-between">
            <h2 class="text-xl font-bold text-white">Reprendre l'apprentissage</h2>
            <NuxtLink to="/my-purchases" class="text-[#C39712] hover:text-yellow-500 text-sm font-medium">
              Voir tout →
            </NuxtLink>
          </div>

          <div v-if="pending" class="flex justify-center py-12">
            <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-[#C39712]"></div>
          </div>

          <div v-else-if="myModules && myModules.length > 0" class="grid gap-4">
            <div v-for="module in myModules.slice(0, 3)" :key="module.id" 
                 class="bg-gray-800 rounded-lg p-4 border border-gray-700 hover:border-[#C39712]/50 transition-all flex items-center gap-4 group cursor-pointer"
                 @click="navigateTo(`/modules/${module.id}`)">
              <div class="h-16 w-16 rounded-lg bg-gray-700 flex-shrink-0 overflow-hidden">
                <img :src="getModuleImage(module.label)" class="h-full w-full object-cover" alt="" />
              </div>
              <div class="flex-grow">
                <h3 class="text-white font-medium group-hover:text-[#C39712] transition-colors">{{ module.label }}</h3>
                <p class="text-sm text-gray-400 line-clamp-1">{{ module.chapterCount }} chapitres • {{ module.subjectName }}</p>
                <div class="mt-2 w-full bg-gray-700 rounded-full h-1.5">
                  <div class="bg-[#C39712] h-1.5 rounded-full" style="width: 0%"></div>
                </div>
              </div>
              <button class="p-2 text-gray-400 group-hover:text-white transition-colors">
                ▶️
              </button>
            </div>
          </div>

          <div v-else class="bg-gray-800 rounded-lg p-8 text-center border border-gray-700">
            <p class="text-gray-400 mb-4">Vous n'avez pas encore commencé de module.</p>
            <NuxtLink to="/modules" class="inline-block px-6 py-2 bg-[#C39712] text-[#082540] rounded-full font-medium hover:bg-yellow-500 transition-colors">
              Explorer le catalogue
            </NuxtLink>
          </div>
        </div>

        <!-- Right Column (Suggestions/Profile) -->
        <div class="space-y-6">
          <div class="bg-gray-800 rounded-xl p-6 border border-gray-700">
            <h3 class="text-lg font-bold text-white mb-4">Profil Rapide</h3>
            <div class="flex items-center gap-4 mb-6">
              <div class="h-12 w-12 rounded-full bg-[#082540] text-[#C39712] flex items-center justify-center text-xl font-bold border border-[#C39712]">
                {{ authStore.user?.username?.charAt(0).toUpperCase() }}
              </div>
              <div>
                <p class="text-white font-medium">{{ authStore.user?.firstName }} {{ authStore.user?.lastName }}</p>
                <p class="text-sm text-gray-400">{{ authStore.user?.email }}</p>
              </div>
            </div>
            <NuxtLink to="/profile" class="block w-full py-2 text-center border border-gray-600 text-gray-300 rounded-lg hover:bg-gray-700 transition-colors text-sm">
              Gérer mon profil
            </NuxtLink>
          </div>

          <div class="bg-gradient-to-br from-[#082540] to-gray-900 rounded-xl p-6 border border-[#C39712]/30">
            <h3 class="text-lg font-bold text-[#C39712] mb-2">Accès Premium</h3>
            <p class="text-gray-300 text-sm mb-4">Débloquez tous les chapitres pour seulement 10€/an.</p>
            <NuxtLink to="/subscribe" class="block w-full py-2 bg-[#C39712] text-[#082540] text-center rounded-lg font-bold hover:bg-yellow-500 transition-colors text-sm">
              S'abonner (10€/an)
            </NuxtLink>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

definePageMeta({
  middleware: 'auth'
})

useHead({
  title: 'Tableau de bord'
})

const authStore = useAuthStore()
const api = useApi()

// Pour l'instant, on affiche les modules de la première matière comme exemple
// Dans une vraie implémentation, on chargerait les modules récemment consultés
const { data: myModules, pending } = await useAsyncData<Module[]>('dashboard-modules', async () => {
  try {
    // Charger les modules de la première matière (Math) comme exemple
    return await api.getModulesBySubject(1)
  } catch {
    return []
  }
})

const getModuleImage = (title: string) => {
  // Logique simple pour l'image basée sur le titre (similaire à index.vue)
  const t = title.toLowerCase()
  if (t.includes('math')) return 'https://images.unsplash.com/photo-1509228468518-180dd4864904?auto=format&fit=crop&w=150&q=80'
  if (t.includes('physi')) return 'https://images.unsplash.com/photo-1635070041078-e363dbe005cb?auto=format&fit=crop&w=150&q=80'
  if (t.includes('chimie') || t.includes('matière')) return 'https://images.unsplash.com/photo-1532094349884-543bc11b234d?auto=format&fit=crop&w=150&q=80'
  if (t.includes('bio')) return 'https://images.unsplash.com/photo-1576086213369-97a306d36557?auto=format&fit=crop&w=150&q=80'
  return 'https://images.unsplash.com/photo-1507413245164-6160d8298b31?auto=format&fit=crop&w=150&q=80'
}
</script>
