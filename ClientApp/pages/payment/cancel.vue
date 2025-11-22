<template>
  <div class="min-h-screen bg-gradient-to-br from-red-50 to-orange-50 flex items-center justify-center px-4">
    <div class="max-w-md w-full bg-white rounded-lg shadow-xl p-8 text-center">
      <!-- Cancel Icon -->
      <div class="mb-6">
        <div class="mx-auto w-20 h-20 bg-red-100 rounded-full flex items-center justify-center">
          <svg class="w-12 h-12 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
          </svg>
        </div>
      </div>

      <!-- Title -->
      <h1 class="text-2xl font-bold text-gray-900 mb-2">Paiement annulé</h1>
      <p class="text-gray-600 mb-8">
        Votre paiement a été annulé. Aucun montant n'a été débité.
      </p>

      <!-- Actions -->
      <div class="space-y-3">
        <button
          @click="$router.back()"
          class="w-full bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
        >
          Réessayer
        </button>

        <NuxtLink
          to="/"
          class="block w-full bg-gray-200 text-gray-800 px-6 py-3 rounded-lg font-medium hover:bg-gray-300 transition-colors"
        >
          Retour à l'accueil
        </NuxtLink>
      </div>

      <!-- Help -->
      <div class="mt-8 pt-6 border-t border-gray-200">
        <p class="text-sm text-gray-600">
          Besoin d'aide ? 
          <a href="mailto:support@ilmanar.com" class="text-primary-600 hover:text-primary-700 font-medium">
            Contactez-nous
          </a>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: false
})

useHead({
  title: 'Paiement annulé'
})
</script>

