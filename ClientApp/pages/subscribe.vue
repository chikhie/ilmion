<template>
  <div class="min-h-screen bg-gray-900">
    <!-- Header -->
    <header class="bg-gray-800 shadow-sm border-b border-gray-700">
      <div class="container mx-auto px-4 py-4">
        <NuxtLink to="/" class="inline-flex items-center text-gray-300 hover:text-white transition-colors">
          ← Retour à l'accueil
        </NuxtLink>
      </div>
    </header>

    <div class="container mx-auto px-4 py-12">
      <!-- Hero Section -->
      <div class="text-center mb-12">
        <h1 class="text-4xl md:text-5xl font-bold text-white mb-4">
          Accédez à tous les contenus
        </h1>
        <p class="text-xl text-gray-400 max-w-2xl mx-auto">
          Un abonnement annuel à prix mini pour un accès illimité à tous les chapitres, vidéos et exercices
        </p>
      </div>

      <!-- Subscription active message -->
      <div v-if="isAuthenticated && hasSubscription && subscription" class="max-w-3xl mx-auto mb-12">
        <div class="bg-green-900/30 border border-green-600 rounded-lg p-6">
          <div class="flex items-start">
            <span class="text-3xl mr-4">✓</span>
            <div class="flex-1">
              <h3 class="text-xl font-bold text-green-400 mb-2">Vous êtes abonné !</h3>
              <p class="text-gray-300 mb-2">
                Abonnement <strong>Premium</strong> actif
              </p>
              <p class="text-sm text-gray-400">
                Expire le {{ new Date(subscription.endDate).toLocaleDateString('fr-FR') }}
                ({{ subscription.daysRemaining }} jours restants)
              </p>
              <button 
                @click="cancelSub" 
                :disabled="cancelling"
                class="mt-4 px-4 py-2 bg-red-600 hover:bg-red-700 disabled:bg-gray-600 text-white rounded text-sm transition-colors"
              >
                {{ cancelling ? 'Annulation...' : 'Annuler l\'abonnement' }}
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Free vs Premium Comparison -->
      <div class="max-w-5xl mx-auto grid md:grid-cols-2 gap-8 mb-12">
        <!-- Free Plan -->
        <div class="bg-gray-800 rounded-lg border border-gray-700 p-8">
          <div class="text-center mb-6">
            <h3 class="text-2xl font-bold text-white mb-2">Gratuit</h3>
            <div class="text-5xl font-bold text-gray-400 mb-2">0€</div>
            <p class="text-gray-400">à vie</p>
          </div>
          
          <ul class="space-y-3 mb-8">
            <li class="flex items-center text-gray-300">
              <span class="text-green-400 mr-2">✓</span>
              Premier chapitre de chaque module
            </li>
            <li class="flex items-center text-gray-400">
              <span class="text-gray-600 mr-2">✗</span>
              Chapitres avancés verrouillés
            </li>
            <li class="flex items-center text-gray-400">
              <span class="text-gray-600 mr-2">✗</span>
              Accès limité aux contenus
            </li>

          </ul>

        </div>

        <!-- Premium Plan -->
        <div class="bg-gradient-to-br from-primary-600/20 to-primary-800/20 rounded-lg border-2 border-primary-500 p-8 relative">
          <div class="absolute -top-4 left-1/2 transform -translate-x-1/2">
            <span class="bg-primary-500 text-white px-4 py-1 rounded-full text-sm font-medium shadow-lg">
              ⭐ Recommandé
            </span>
          </div>
          
          <div class="text-center mb-6">
            <h3 class="text-2xl font-bold text-white mb-2">Premium</h3>
            <div class="text-5xl font-bold text-primary-400 mb-2">10€</div>
            <p class="text-gray-300">par an</p>
          </div>
          
          <ul class="space-y-3 mb-8">
            <li class="flex items-center text-gray-300">
              <span class="text-green-400 mr-2 text-lg">✓</span>
              Accès illimité à TOUS les contenus
            </li>
            <li class="flex items-center text-gray-300">
              <span class="text-green-400 mr-2 text-lg">✓</span>
              Tous les chapitres débloqués
            </li>
            <li class="flex items-center text-gray-300">
              <span class="text-green-400 mr-2 text-lg">✓</span>
              Vidéos HD + Exercices + Quiz
            </li>
            <li class="flex items-center text-gray-300">
              <span class="text-green-400 mr-2 text-lg">✓</span>
              Nouveaux contenus inclus
            </li>
          </ul>

          <button
            v-if="!isAuthenticated || !hasSubscription"
            @click="subscribe"
            :disabled="loading"
            class="w-full py-4 bg-primary-500 hover:bg-primary-600 disabled:bg-gray-600 text-white rounded-lg font-bold text-lg transition-all transform hover:scale-105 shadow-lg"
          >
            {{ loading ? 'Chargement...' : isAuthenticated ? 'S\'abonner maintenant' : 'Créer un compte et s\'abonner' }}
          </button>
        </div>
      </div>

      <!-- Features -->
      <div class="max-w-4xl mx-auto">
        <h2 class="text-2xl font-bold text-white text-center mb-8">Ce qui est inclus</h2>
        <div class="grid md:grid-cols-3 gap-6">
          <div class="bg-gray-800 rounded-lg p-6 border border-gray-700">
            <div class="text-4xl mb-3">📚</div>
            <h3 class="text-lg font-bold text-white mb-2">Tous les cours</h3>
            <p class="text-gray-400 text-sm">
              Accès complet à tous les modules et chapitres de la plateforme
            </p>
          </div>
          <div class="bg-gray-800 rounded-lg p-6 border border-gray-700">
            <div class="text-4xl mb-3">🎥</div>
            <h3 class="text-lg font-bold text-white mb-2">Vidéos HD</h3>
            <p class="text-gray-400 text-sm">
              Toutes les vidéos explicatives en haute qualité
            </p>
          </div>
          <div class="bg-gray-800 rounded-lg p-6 border border-gray-700">
            <div class="text-4xl mb-3">✍️</div>
            <h3 class="text-lg font-bold text-white mb-2">Exercices</h3>
            <p class="text-gray-400 text-sm">
              Quiz et exercices pour valider vos connaissances
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Subscription } from '~/types'

const api = useApi()
const authStore = useAuthStore()
const router = useRouter()
const loading = ref(false)
const cancelling = ref(false)

const isAuthenticated = computed(() => authStore.isLoggedIn)

// Vérifier si l'utilisateur a déjà un abonnement (seulement s'il est connecté)
const { data: subData } = await useAsyncData(
  'my-subscription',
  async () => {
    if (!isAuthenticated.value) {
      return { hasSubscription: false, subscription: null }
    }
    try {
      return await api.getMySubscription()
    } catch {
      return { hasSubscription: false, subscription: null }
    }
  },
  {
    server: false // Client-side only
  }
)

const hasSubscription = computed(() => subData.value?.hasSubscription || false)
const subscription = computed(() => subData.value?.subscription as Subscription | undefined)

const subscribe = async () => {
  // Si l'utilisateur n'est pas connecté, le rediriger vers la page d'inscription
  if (!isAuthenticated.value) {
    router.push('/register?redirect=/subscribe')
    return
  }

  loading.value = true
  
  try {
    // Récupérer les informations de l'utilisateur
    const user = authStore.user
    
    if (!user?.id) {
      throw new Error('Utilisateur non trouvé')
    }

    // URL du Payment Link Stripe (mode test)
    const stripePaymentLink = 'https://buy.stripe.com/test_7sY00kfmJeT9atS2qb0Fi00'
    
    // Construire l'URL avec les metadata
    const params = new URLSearchParams({
      'client_reference_id': user.id,  // ID de l'utilisateur
      'prefilled_email': user.email || ''  // Email pré-rempli
    })
    
    // Rediriger vers le Payment Link Stripe avec les paramètres
    window.location.href = `${stripePaymentLink}?${params.toString()}`
  } catch (error: any) {
    console.error('Erreur lors de la redirection vers Stripe:', error)
    alert('Erreur lors de la préparation du paiement. Veuillez réessayer.')
    loading.value = false
  }
}

const cancelSub = async () => {
  if (!confirm('Êtes-vous sûr de vouloir annuler votre abonnement ?')) {
    return
  }
  
  cancelling.value = true
  try {
    await api.cancelSubscription()
    alert('Votre abonnement a été annulé avec succès.')
    // Recharger les données
    await refreshNuxtData('my-subscription')
  } catch (error) {
    console.error('Erreur lors de l\'annulation:', error)
    alert('Erreur lors de l\'annulation. Veuillez réessayer.')
  } finally {
    cancelling.value = false
  }
}

useHead({
  title: 'S\'abonner - Premium 10€/an'
})
</script>
