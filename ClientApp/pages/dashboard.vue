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
          <p class="text-3xl font-bold text-white">{{ stats?.followedModules.length || 0 }}</p>
          <p class="text-sm text-gray-500 mt-2">Modules dans votre bibliothèque</p>
        </div>

        <div class="bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-gray-400 font-medium">Progression</h3>
            <span class="p-2 bg-green-900/30 text-green-400 rounded-lg">📈</span>
          </div>
          <p class="text-3xl font-bold text-white">{{ stats?.globalProgress || 0 }}%</p>
          <p class="text-sm text-gray-500 mt-2">Moyenne globale</p>
        </div>

        <div class="bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-gray-400 font-medium">Temps d'apprentissage</h3>
            <span class="p-2 bg-purple-900/30 text-purple-400 rounded-lg">⏱️</span>
          </div>
          <p class="text-3xl font-bold text-white">{{ stats?.formattedLearningTime || '0h' }}</p>
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

          <div v-else-if="stats?.followedModules && stats.followedModules.length > 0" class="grid gap-4">
            <div v-for="module in stats.followedModules.slice(0, 3)" :key="module.id" 
                 class="bg-gray-800 rounded-lg p-4 border border-gray-700 hover:border-[#C39712]/50 transition-all flex items-center gap-4 group cursor-pointer"
                 @click="navigateTo(`/modules/${module.id}`)">
              <div class="h-16 w-16 rounded-lg bg-gray-700 flex-shrink-0 overflow-hidden">
                <img :src="getModuleImage(module.label)" class="h-full w-full object-cover" alt="" />
              </div>
              <div class="flex-grow">
                <h3 class="text-white font-medium group-hover:text-[#C39712] transition-colors">{{ module.label }}</h3>
                <p class="text-sm text-gray-400 line-clamp-1">{{ module.chapterCount }} chapitres • {{ module.subjectName }}</p>
                <div class="mt-2 w-full bg-gray-700 rounded-full h-1.5">
                  <div class="bg-[#C39712] h-1.5 rounded-full" :style="{ width: module.progress + '%' }"></div>
                </div>
              </div>
              <button class="p-2 text-gray-400 group-hover:text-white transition-colors">
                ▶️
              </button>
            </div>
          </div>

          <div v-else class="space-y-6">
            <div class="bg-gray-800 rounded-lg p-6 border border-gray-700 text-center mb-6">
              <p class="text-gray-300 mb-2">Vous n'avez pas encore commencé de module.</p>
              <p class="text-sm text-gray-500">Explorez nos matières ci-dessous pour commencer votre apprentissage.</p>
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div v-for="subject in subjects" :key="subject.id" class="group relative overflow-hidden rounded-xl aspect-[16/9] hover:shadow-xl transition-all cursor-pointer border border-gray-700 hover:border-[#C39712]/50" @click="navigateTo(`/subjects/${subject.id}`)">
                <div class="absolute inset-0 bg-gradient-to-t from-black/90 to-transparent z-10"></div>
                <img 
                  :src="getSubjectImage(subject.label)" 
                  :alt="subject.label" 
                  class="absolute inset-0 w-full h-full object-cover group-hover:scale-110 transition-transform duration-500 opacity-60 group-hover:opacity-80"
                />
                <div class="absolute bottom-0 left-0 right-0 p-4 z-20">
                  <h3 class="text-lg font-bold text-white group-hover:text-[#C39712] transition-colors">{{ subject.label }}</h3>
                  <span class="text-xs text-gray-400 group-hover:text-gray-300 transition-colors">Voir les modules →</span>
                </div>
              </div>
            </div>
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

          <!-- <div v-if="!authStore.isPremium" class="bg-gradient-to-br from-[#082540] to-gray-900 rounded-xl p-6 border border-[#C39712]/30">
            <h3 class="text-lg font-bold text-[#C39712] mb-2">Accès Premium</h3>
            <p class="text-gray-300 text-sm mb-4">Débloquez tous les chapitres pour seulement 10€/an.</p>
            <NuxtLink to="/subscribe" class="block w-full py-2 bg-[#C39712] text-[#082540] text-center rounded-lg font-bold hover:bg-yellow-500 transition-colors text-sm">
              S'abonner (10€/an)
            </NuxtLink>
          </div>

          <div v-else class="bg-gradient-to-br from-[#C39712] to-yellow-600 rounded-xl p-6 border border-yellow-500/30">
            <div class="flex items-center mb-3">
              <svg class="w-6 h-6 text-[#082540] mr-2" fill="currentColor" viewBox="0 0 20 20">
                <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
              </svg>
              <h3 class="text-lg font-bold text-[#082540]">Membre Premium</h3>
            </div>
            <p class="text-[#082540]/80 text-sm mb-4">
              ✨ Vous avez accès à tous les contenus de la plateforme !
            </p>
            <NuxtLink to="/profile" class="block w-full py-2 bg-[#082540] text-[#C39712] text-center rounded-lg font-bold hover:bg-gray-900 transition-colors text-sm">
              Gérer mon abonnement
            </NuxtLink>
          </div> -->
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { DashboardStats, Subject } from '~/types'

definePageMeta({
  middleware: 'auth'
})

useHead({
  title: 'Tableau de bord'
})

const authStore = useAuthStore()
const api = useApi()

const { data: stats, pending } = await useAsyncData<DashboardStats>('dashboard-stats', async () => {
  try {
    return await api.getDashboardStats()
  } catch {
    return null
  }
})

// Charger les matières si aucun module n'est suivi
const { data: subjects } = await useAsyncData<Subject[]>('dashboard-subjects', async () => {
  // On charge les matières uniquement si nécessaire (si stats est déjà chargé et vide), 
  // mais comme useAsyncData est parallèle, on peut juste les charger ou conditionner.
  // Pour simplifier l'UX et éviter des clignotements, on peut charger les matières en parallèle ou après.
  // Ici on les charge toujours pour simplifier, le coût est faible.
  try {
    return await api.getSubjects()
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

const getSubjectImage = (label: string) => {
  const images: Record<string, string> = {
    'Mathématiques': 'https://images.unsplash.com/photo-1509228468518-180dd4864904?q=80&w=800&auto=format&fit=crop',
    'Physique': 'https://images.unsplash.com/photo-1635070041078-e363dbe005cb?q=80&w=800&auto=format&fit=crop',
    'Chimie': 'https://images.unsplash.com/photo-1532094349884-543bc11b234d?q=80&w=800&auto=format&fit=crop',
    'Biologie': 'https://images.unsplash.com/photo-1576086213369-97a306d36557?q=80&w=800&auto=format&fit=crop',
    'Informatique': 'https://images.unsplash.com/photo-1555066931-4365d14bab8c?q=80&w=800&auto=format&fit=crop'
  }
  
  const normalizedLabel = label.toLowerCase()
  if (normalizedLabel.includes('math')) return images['Mathématiques']
  if (normalizedLabel.includes('physi')) return images['Physique']
  if (normalizedLabel.includes('chimie')) return images['Chimie']
  if (normalizedLabel.includes('bio')) return images['Biologie']
  if (normalizedLabel.includes('info') || normalizedLabel.includes('code')) return images['Informatique']
  
  return 'https://images.unsplash.com/photo-1507413245164-6160d8298b31?q=80&w=800&auto=format&fit=crop'
}
</script>
