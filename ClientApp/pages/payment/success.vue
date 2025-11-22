<template>
  <div class="min-h-screen bg-gradient-to-br from-green-50 to-blue-50 flex items-center justify-center px-4">
    <div class="max-w-md w-full bg-white rounded-lg shadow-xl p-8 text-center">
      <!-- Success Icon -->
      <div class="mb-6">
        <div class="mx-auto w-20 h-20 bg-green-100 rounded-full flex items-center justify-center">
          <svg class="w-12 h-12 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
          </svg>
        </div>
      </div>

      <!-- Title -->
      <h1 class="text-2xl font-bold text-gray-900 mb-2">Paiement réussi !</h1>
      <p class="text-gray-600 mb-8">
        Merci pour votre achat. Vous pouvez maintenant accéder à votre module.
      </p>

      <!-- Loading Status -->
      <div v-if="loading" class="mb-6">
        <div class="flex justify-center items-center">
          <svg class="animate-spin h-8 w-8 text-primary-600" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
        </div>
        <p class="text-sm text-gray-500 mt-2">Vérification du paiement...</p>
      </div>

      <!-- Payment Details -->
      <div v-else-if="paymentStatus" class="bg-gray-50 rounded-lg p-4 mb-6 text-left">
        <div class="space-y-2 text-sm">
          <div class="flex justify-between">
            <span class="text-gray-600">Montant:</span>
            <span class="font-medium">{{ formatPrice(paymentStatus.amount) }}</span>
          </div>
          <div class="flex justify-between">
            <span class="text-gray-600">Date:</span>
            <span class="font-medium">{{ formatDate(paymentStatus.purchaseDate) }}</span>
          </div>
        </div>
      </div>

      <!-- Actions -->
      <div class="space-y-3">
        <button
          v-if="paymentStatus?.moduleId"
          @click="goToModule"
          class="w-full bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
        >
          Accéder au module
        </button>
        
        <NuxtLink
          to="/my-purchases"
          class="block w-full bg-gray-200 text-gray-800 px-6 py-3 rounded-lg font-medium hover:bg-gray-300 transition-colors"
        >
          Voir mes achats
        </NuxtLink>

        <NuxtLink
          to="/"
          class="block text-gray-600 hover:text-gray-800 text-sm"
        >
          Retour à l'accueil
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: false
})

useHead({
  title: 'Paiement réussi'
})

const route = useRoute()
const router = useRouter()
const api = useApi()

const sessionId = route.query.session_id as string
const loading = ref(true)
const paymentStatus = ref<any>(null)

onMounted(async () => {
  if (!sessionId) {
    router.push('/')
    return
  }

  try {
    paymentStatus.value = await api.getPaymentStatus(sessionId)
  } catch (error) {
    console.error('Error loading payment status:', error)
  } finally {
    loading.value = false
  }
})

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency: 'EUR'
  }).format(price)
}

const formatDate = (dateString: string) => {
  const date = new Date(dateString)
  return new Intl.DateTimeFormat('fr-FR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date)
}

const goToModule = () => {
  if (paymentStatus.value?.moduleId) {
    router.push(`/modules/${paymentStatus.value.moduleId}`)
  }
}
</script>

