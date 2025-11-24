<template>
  <div class="min-h-screen bg-gray-900 flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <div class="bg-gray-800 rounded-lg border border-gray-700 p-8 text-center">
        <div class="w-20 h-20 bg-gray-700 rounded-full flex items-center justify-center mx-auto mb-6">
          <svg class="w-12 h-12 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
          </svg>
        </div>
        
        <h1 class="text-3xl font-bold text-white mb-4">Paiement annulé</h1>
        
        <p class="text-gray-300 mb-6">
          Votre paiement a été annulé. Aucun montant n'a été débité.
        </p>
        
        <p class="text-sm text-gray-400 mb-8">
          Vous pouvez réessayer à tout moment pour profiter de l'accès Premium à tous les contenus.
        </p>
        
        <div class="space-y-3">
          <NuxtLink 
            to="/subscribe" 
            class="block w-full px-6 py-3 bg-primary-600 hover:bg-primary-700 text-white rounded-lg font-medium transition-colors"
          >
            Réessayer
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
useHead({
  title: 'Paiement annulé'
})
</script>
