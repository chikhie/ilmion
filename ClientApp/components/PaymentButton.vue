<template>
  <div>
    <!-- Module gratuit -->
    <button
      v-if="module.isFree"
      class="w-full bg-green-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-green-700 transition-colors"
      @click="$emit('access')"
    >
      📖 Accéder gratuitement
    </button>

    <!-- Module déjà acheté -->
    <button
      v-else-if="module.hasAccess"
      class="w-full bg-blue-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-blue-700 transition-colors"
      @click="$emit('access')"
    >
      ▶️ Continuer le module
    </button>

    <!-- Module à acheter -->
    <button
      v-else
      :disabled="loading"
      class="w-full bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
      @click="handlePurchase"
    >
      <span v-if="!loading">
        💳 Acheter pour {{ formatPrice(module.price) }}
      </span>
      <span v-else class="flex items-center">
        <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
        Traitement...
      </span>
    </button>

    <!-- Message d'erreur -->
    <div v-if="errorMessage" class="mt-3 bg-red-50 border border-red-200 text-red-700 px-4 py-2 rounded-lg text-sm">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Module } from '~/types'

const props = defineProps<{
  module: Module
}>()

defineEmits<{
  access: []
}>()

const api = useApi()
const authStore = useAuthStore()

const loading = ref(false)
const errorMessage = ref('')

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency: 'EUR'
  }).format(price)
}

const handlePurchase = async () => {
  // Vérifier si l'utilisateur est connecté
  if (!authStore.isAuthenticated) {
    errorMessage.value = 'Vous devez être connecté pour effectuer un achat'
    setTimeout(() => {
      navigateTo('/login?redirect=' + encodeURIComponent(window.location.pathname))
    }, 1500)
    return
  }

  loading.value = true
  errorMessage.value = ''

  try {
    // Créer une session de paiement Stripe
    const session = await api.createCheckoutSession(props.module.id)
    
    // Rediriger vers Stripe Checkout
    if (session.sessionUrl) {
      window.location.href = session.sessionUrl
    }
  } catch (error: any) {
    console.error('Payment error:', error)
    errorMessage.value = error.data?.message || 'Erreur lors de la création de la session de paiement'
  } finally {
    loading.value = false
  }
}
</script>

