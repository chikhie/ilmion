<template>
  <div class="min-h-screen bg-[#082540] text-white">
    <!-- Hero Section -->
    <section class="relative overflow-hidden py-20 lg:py-32">
      <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1532094349884-543bc11b234d?q=80&w=2070&auto=format&fit=crop')] bg-cover bg-center opacity-10 mix-blend-overlay"></div>
      <div class="absolute inset-0 bg-gradient-to-b from-transparent to-[#082540]"></div>
      
      <div class="container mx-auto px-4 relative z-10">
        <div class="max-w-4xl mx-auto text-center">
          <h1 class="text-5xl md:text-7xl font-bold mb-6 leading-tight">
            <span class="text-[#C39712]">L'Excellence</span> Éducative<br/> à Portée de Main
          </h1>
          <p class="text-xl md:text-2xl text-gray-300 mb-10 leading-relaxed">
            Plongez dans un univers d'apprentissage interactif. Mathématiques, Physique, Chimie et Biologie n'ont jamais été aussi passionnants.
          </p>
          <div class="flex flex-col sm:flex-row justify-center gap-4">
            <NuxtLink v-if="!authStore.isAuthenticated" to="/register" class="px-8 py-4 bg-[#C39712] text-[#082540] text-lg font-bold rounded-full hover:bg-yellow-500 transition-all transform hover:scale-105 shadow-lg shadow-yellow-900/20">
              Commencer Gratuitement
            </NuxtLink>
            <NuxtLink v-if="authStore.isAuthenticated && !authStore.isPremium" to="/subscribe" class="px-8 py-4 border-2 border-[#C39712] text-[#C39712] text-lg font-bold rounded-full hover:bg-[#C39712] hover:text-[#082540] transition-all">
              Voir l'offre Premium
            </NuxtLink>
            <NuxtLink v-if="authStore.isAuthenticated" to="/dashboard" class="px-8 py-4 bg-[#C39712] text-[#082540] text-lg font-bold rounded-full hover:bg-yellow-500 transition-all transform hover:scale-105 shadow-lg shadow-yellow-900/20">
              Accéder au tableau de bord
            </NuxtLink>
          </div>
          <div class="mt-6">
            <NuxtLink to="/how-it-works" class="text-[#C39712] hover:text-white transition-colors text-sm underline">
              Comment ça fonctionne ? 📚
            </NuxtLink>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="py-20 bg-gray-900">
      <div class="container mx-auto px-4">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-12">
          <div class="bg-gray-800 p-8 rounded-2xl border border-gray-700 hover:border-[#C39712]/50 transition-colors group">
            <div class="w-14 h-14 bg-[#082540] rounded-lg flex items-center justify-center mb-6 text-3xl border border-gray-700 group-hover:border-[#C39712]/50 transition-colors">
              💎
            </div>
            <h3 class="text-xl font-bold text-white mb-4">Prix Accessible</h3>
            <p class="text-gray-400">
              Une éducation de qualité supérieure rendue accessible à tous. Apprenez sans vous ruiner.
            </p>
          </div>
          <div class="bg-gray-800 p-8 rounded-2xl border border-gray-700 hover:border-[#C39712]/50 transition-colors group">
            <div class="w-14 h-14 bg-[#082540] rounded-lg flex items-center justify-center mb-6 text-3xl border border-gray-700 group-hover:border-[#C39712]/50 transition-colors">
              🧠
            </div>
            <h3 class="text-xl font-bold text-white mb-4">Contenu Interactif</h3>
            <p class="text-gray-400">
              Quiz, exercices pratiques et supports visuels pour ancrer durablement les connaissances.
            </p>
          </div>
          <div class="bg-gray-800 p-8 rounded-2xl border border-gray-700 hover:border-[#C39712]/50 transition-colors group">
            <div class="w-14 h-14 bg-[#082540] rounded-lg flex items-center justify-center mb-6 text-3xl border border-gray-700 group-hover:border-[#C39712]/50 transition-colors">
              🎬
            </div>
            <h3 class="text-xl font-bold text-white mb-4">Vidéos Qualitatives</h3>
            <p class="text-gray-400">
              Des leçons vidéo captivantes et pédagogiques, conçues pour rendre l'apprentissage simple et agréable.
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- Subjects Preview -->
    <section class="py-20 bg-[#082540]">
      <div class="container mx-auto px-4">
        <div class="text-center mb-16">
          <h2 class="text-3xl md:text-4xl font-bold text-white mb-4">Nos Matières Phares</h2>
          <div class="w-24 h-1 bg-[#C39712] mx-auto rounded-full"></div>
        </div>

        <div v-if="pending" class="text-center py-12">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-[#C39712]"></div>
        </div>

        <div v-else-if="error" class="text-center py-12">
          <p class="text-red-400">Une erreur est survenue lors du chargement des matières.</p>
        </div>

        <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div v-for="subject in subjects?.slice(0, 4)" :key="subject.id" class="group relative overflow-hidden rounded-xl aspect-[4/3] cursor-pointer hover:shadow-2xl transition-all">
            <div class="absolute inset-0 bg-gradient-to-t from-black/90 to-transparent z-10"></div>
            <!-- Image fixe basée sur le label de la matière -->
            <img 
              :src="getSubjectImage(subject.label)" 
              :alt="subject.label" 
              class="absolute inset-0 w-full h-full object-cover group-hover:scale-110 transition-transform duration-500 opacity-80"
            />
            <div class="absolute bottom-0 left-0 right-0 p-6 z-20">
              <h3 class="text-xl font-bold text-white group-hover:text-[#C39712] transition-colors">{{ subject.label }}</h3>
              <NuxtLink :to="`/subjects/${subject.id}`" class="text-sm text-gray-300 mt-2 inline-block opacity-0 group-hover:opacity-100 transform translate-y-2 group-hover:translate-y-0 transition-all">
                Découvrir →
              </NuxtLink>
            </div>
          </div>
        </div>
        
        <div class="text-center mt-12">
          <p class="text-gray-400 text-sm">
            Cliquez sur une matière pour explorer ses modules et chapitres
          </p>
        </div>
      </div>
    </section>

    <!-- Pricing Section (Integrated) -->
    <!-- <section class="py-20 bg-gray-800">
      ... Section retirée ...
    </section> -->
    
    <!-- CTA Final -->
    <section v-if="!authStore.isPremium" class="py-20 bg-gradient-to-r from-[#C39712] to-yellow-600">
      <div class="container mx-auto px-4 text-center">
        <h2 class="text-3xl md:text-4xl font-bold text-[#082540] mb-6">Prêt à commencer votre voyage ?</h2>
        <p class="text-[#082540]/80 text-xl mb-10 max-w-2xl mx-auto">Rejoignez des milliers d'étudiants et maîtrisez les sciences dès aujourd'hui.</p>
        
        <div class="flex flex-col sm:flex-row justify-center gap-4">
          <NuxtLink v-if="!authStore.isAuthenticated" to="/register" class="px-10 py-4 bg-[#082540] text-white text-lg font-bold rounded-full hover:bg-gray-900 transition-all shadow-xl">
            Créer un compte gratuit
          </NuxtLink>
          <NuxtLink to="/subscribe" class="px-10 py-4 border-2 border-[#082540] text-[#082540] text-lg font-bold rounded-full hover:bg-[#082540] hover:text-white transition-all shadow-xl">
            Offre Premium à 10€/an
          </NuxtLink>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { Subject } from '~/types'

useHead({
  title: 'Ilmanar - L\'Excellence Éducative'
})

const api = useApi()
const authStore = useAuthStore()

// Rediriger automatiquement vers le dashboard si l'utilisateur est connecté
if (authStore.isAuthenticated) {
  navigateTo('/dashboard')
}

const { data: subjects, pending, error } = await useAsyncData<Subject[]>(
  'subjects',
  async () => {
    try {
      return await api.getSubjects()
    } catch (e) {
      console.error('Erreur lors du chargement des matières:', e)
      // Retourner des données par défaut en cas d'erreur
      return []
    }
  }
)

const getSubjectImage = (label: string) => {
  const images: Record<string, string> = {
    'Mathématiques': 'https://images.unsplash.com/photo-1509228468518-180dd4864904?q=80&w=800&auto=format&fit=crop',
    'Physique': 'https://images.unsplash.com/photo-1635070041078-e363dbe005cb?q=80&w=800&auto=format&fit=crop',
    'Chimie': 'https://images.unsplash.com/photo-1532094349884-543bc11b234d?q=80&w=800&auto=format&fit=crop',
    'Biologie': 'https://images.unsplash.com/photo-1576086213369-97a306d36557?q=80&w=800&auto=format&fit=crop',
    'Informatique': 'https://images.unsplash.com/photo-1555066931-4365d14bab8c?q=80&w=800&auto=format&fit=crop'
  }
  
  // Recherche partielle insensible à la casse
  const normalizedLabel = label.toLowerCase()
  if (normalizedLabel.includes('math')) return images['Mathématiques']
  if (normalizedLabel.includes('physi')) return images['Physique']
  if (normalizedLabel.includes('chimie')) return images['Chimie']
  if (normalizedLabel.includes('bio')) return images['Biologie']
  if (normalizedLabel.includes('info') || normalizedLabel.includes('code')) return images['Informatique']
  
  // Image par défaut (Science générique)
  return 'https://images.unsplash.com/photo-1507413245164-6160d8298b31?q=80&w=800&auto=format&fit=crop'
}
</script>

