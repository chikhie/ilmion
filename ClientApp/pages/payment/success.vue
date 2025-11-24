<template>
  <div class="min-h-screen bg-gray-900 flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <div class="bg-gray-800 rounded-lg border border-[#C39712] shadow-xl p-8 text-center">
        <div class="w-20 h-20 bg-gradient-to-br from-green-500 to-green-600 rounded-full flex items-center justify-center mx-auto mb-6 shadow-lg">
          <svg class="w-12 h-12 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7"></path>
          </svg>
        </div>
        
        <h1 class="text-4xl font-bold text-white mb-2">Paiement réussi !</h1>
        <p class="text-[#C39712] text-lg mb-6">🎉 Bienvenue dans la communauté Premium</p>
        
        <div v-if="loading" class="my-8">
          <div class="inline-block animate-spin rounded-full h-10 w-10 border-b-2 border-[#C39712]"></div>
          <p class="text-gray-400 mt-4">Activation de votre abonnement en cours...</p>
        </div>
        
        <div v-else-if="subscription" class="my-8">
          <p class="text-gray-300 text-lg mb-6">
            Votre abonnement <strong class="text-[#C39712]">Premium Annuel</strong> est maintenant actif !
          </p>
          <div class="bg-gradient-to-br from-[#082540] to-[#0a3a5f] rounded-lg p-6 mb-6 border border-[#C39712]/30">
            <p class="text-sm text-gray-400 mb-2">Valable jusqu'au</p>
            <p class="text-2xl font-bold text-[#C39712]">{{ formatDate(subscription.endDate) }}</p>
            <p class="text-xs text-gray-500 mt-2">{{ subscription.daysRemaining }} jours restants</p>
          </div>
          <div class="bg-green-900/20 border border-green-600 rounded-lg p-4">
            <p class="text-green-400 font-medium">
              ✨ Vous avez maintenant accès à tous les contenus de la plateforme !
            </p>
          </div>
        </div>
        
        <div v-else class="my-8">
          <p class="text-gray-300 text-lg mb-4">
            Votre paiement a été effectué avec succès !
          </p>
          <div class="bg-yellow-900/20 border border-yellow-600 rounded-lg p-4">
            <p class="text-yellow-400 text-sm">
              ⏳ Votre abonnement sera activé dans quelques instants...
            </p>
          </div>
        </div>
        
        <div class="space-y-3 mt-8">
          <NuxtLink 
            to="/dashboard" 
            class="block w-full px-6 py-3 bg-[#C39712] hover:bg-yellow-500 text-[#082540] rounded-lg font-bold text-lg transition-all transform hover:scale-105 shadow-lg"
          >
            🎓 Accéder au tableau de bord
          </NuxtLink>
          <NuxtLink 
            to="/" 
            class="block w-full px-6 py-3 bg-gray-700 hover:bg-gray-600 text-white rounded-lg font-medium transition-colors"
          >
            Retour à l'accueil
          </NuxtLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Subscription } from '~/types'

definePageMeta({
  middleware: 'auth'
})

const route = useRoute()
const api = useApi()

const loading = ref(true)
const subscription = ref<Subscription | null>(null)

onMounted(async () => {
  // Attendre un peu pour que le webhook Stripe soit traité
  await new Promise(resolve => setTimeout(resolve, 2000))
  
  try {
    const result = await api.getMySubscription()
    if (result.hasSubscription) {
      subscription.value = result.subscription
      
      // ✨ Mettre à jour l'état d'abonnement dans le store
      await authStore.checkAccess()
      
      // ✨ Invalider le cache Nuxt pour recharger les données
      await clearNuxtData()
    }
  } catch (error) {
    console.error('Erreur lors de la récupération de l\'abonnement:', error)
  } finally {
    loading.value = false
  }
})

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('fr-FR', {
    day: 'numeric',
    month: 'long',
    year: 'numeric'
  })
}

useHead({
  title: 'Paiement réussi'
})
</script>
