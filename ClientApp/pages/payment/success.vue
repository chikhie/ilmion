<template>
  <div class="min-h-screen bg-gray-900 flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <div class="bg-gray-800 rounded-lg border border-green-600 p-8 text-center">
        <div class="w-20 h-20 bg-green-600 rounded-full flex items-center justify-center mx-auto mb-6">
          <svg class="w-12 h-12 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
          </svg>
        </div>
        
        <h1 class="text-3xl font-bold text-white mb-4">Paiement réussi !</h1>
        
        <div v-if="loading" class="my-6">
          <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-500"></div>
          <p class="text-gray-400 mt-4">Vérification de votre abonnement...</p>
        </div>
        
        <div v-else-if="subscription" class="my-6">
          <p class="text-gray-300 mb-4">
            Votre abonnement <strong class="text-primary-400">Premium</strong> est maintenant actif !
          </p>
          <div class="bg-gray-900 rounded-lg p-4 mb-6">
            <p class="text-sm text-gray-400">Valable jusqu'au</p>
            <p class="text-lg font-bold text-white">{{ formatDate(subscription.endDate) }}</p>
          </div>
          <p class="text-sm text-gray-400">
            Vous avez maintenant accès à tous les contenus de la plateforme 🎉
          </p>
        </div>
        
        <div v-else class="my-6">
          <p class="text-gray-300 mb-4">
            Votre paiement a été effectué avec succès !
          </p>
          <p class="text-sm text-gray-400">
            Votre abonnement sera activé dans quelques instants.
          </p>
        </div>
        
        <div class="space-y-3 mt-8">
          <NuxtLink 
            to="/dashboard" 
            class="block w-full px-6 py-3 bg-primary-600 hover:bg-primary-700 text-white rounded-lg font-medium transition-colors"
          >
            Accéder au tableau de bord
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
