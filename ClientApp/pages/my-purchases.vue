<template>
  <div class="min-h-screen bg-gray-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-900">Mes achats</h1>
        <p class="mt-2 text-gray-600">Retrouvez tous les modules que vous avez achetés</p>
      </div>

      <!-- Loading -->
      <div v-if="loading" class="flex justify-center items-center py-12">
        <svg class="animate-spin h-12 w-12 text-primary-600" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>

      <!-- Empty state -->
      <div v-else-if="purchases.length === 0" class="bg-white rounded-lg shadow p-12 text-center">
        <div class="text-6xl mb-4">🛒</div>
        <h3 class="text-xl font-semibold text-gray-900 mb-2">Aucun achat pour le moment</h3>
        <p class="text-gray-600 mb-6">Explorez nos modules pour commencer votre apprentissage</p>
        <NuxtLink
          to="/"
          class="inline-block bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
        >
          Découvrir les modules
        </NuxtLink>
      </div>

      <!-- Purchase list -->
      <div v-else class="grid gap-6">
        <div
          v-for="purchase in purchases"
          :key="purchase.id"
          class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow"
        >
          <div class="flex items-start justify-between">
            <div class="flex-1">
              <div class="flex items-center mb-2">
                <span class="text-sm text-gray-500 mr-3">{{ purchase.subjectName }}</span>
                <span
                  :class="{
                    'bg-green-100 text-green-800': purchase.status === 'Completed',
                    'bg-yellow-100 text-yellow-800': purchase.status === 'Pending',
                    'bg-red-100 text-red-800': purchase.status === 'Failed',
                  }"
                  class="px-2 py-1 text-xs font-medium rounded"
                >
                  {{ getStatusLabel(purchase.status) }}
                </span>
              </div>
              
              <h3 class="text-xl font-semibold text-gray-900 mb-2">
                {{ purchase.moduleTitle }}
              </h3>
              
              <div class="flex items-center text-sm text-gray-600 space-x-4">
                <span>📅 {{ formatDate(purchase.purchaseDate) }}</span>
                <span>💰 {{ formatPrice(purchase.amountPaid, purchase.currency) }}</span>
              </div>
            </div>

            <div class="ml-4">
              <NuxtLink
                :to="`/modules/${purchase.moduleId}`"
                class="inline-block bg-primary-600 text-white px-4 py-2 rounded-lg text-sm font-medium hover:bg-primary-700 transition-colors"
              >
                Accéder au module
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { ModulePurchase } from '~/types'

definePageMeta({
  middleware: 'auth'
})

useHead({
  title: 'Mes achats'
})

const api = useApi()

const purchases = ref<ModulePurchase[]>([])
const loading = ref(true)

onMounted(async () => {
  try {
    purchases.value = await api.getMyPurchases()
  } catch (error) {
    console.error('Error loading purchases:', error)
  } finally {
    loading.value = false
  }
})

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

const formatPrice = (price: number, currency: string) => {
  return new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency: currency || 'EUR'
  }).format(price)
}

const getStatusLabel = (status: string) => {
  const labels: Record<string, string> = {
    'Completed': 'Payé',
    'Pending': 'En attente',
    'Failed': 'Échoué',
    'Refunded': 'Remboursé'
  }
  return labels[status] || status
}
</script>

