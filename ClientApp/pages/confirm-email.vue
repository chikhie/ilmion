<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center px-4">
    <div class="max-w-md w-full bg-white rounded-lg shadow-xl p-8 text-center">
      <!-- Loading -->
      <div v-if="confirming" class="py-8">
        <div class="inline-block animate-spin rounded-full h-16 w-16 border-b-4 border-primary-600 mb-4"></div>
        <h2 class="text-xl font-semibold text-gray-900 mb-2">Confirmation en cours...</h2>
        <p class="text-gray-600">Veuillez patienter</p>
      </div>

      <!-- Success -->
      <div v-else-if="confirmed" class="py-8">
        <div class="mb-6">
          <div class="mx-auto w-20 h-20 bg-green-100 rounded-full flex items-center justify-center">
            <svg class="w-12 h-12 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
            </svg>
          </div>
        </div>

        <h1 class="text-3xl font-bold text-gray-900 mb-4">✅ Email confirmé !</h1>
        <p class="text-gray-600 mb-8">
          Votre adresse email a été confirmée avec succès. Vous pouvez maintenant vous connecter.
        </p>

        <NuxtLink
          to="/login"
          class="inline-block bg-primary-600 text-white px-8 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
        >
          Se connecter
        </NuxtLink>
      </div>

      <!-- Error -->
      <div v-else class="py-8">
        <div class="mb-6">
          <div class="mx-auto w-20 h-20 bg-red-100 rounded-full flex items-center justify-center">
            <svg class="w-12 h-12 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
          </div>
        </div>

        <h1 class="text-3xl font-bold text-gray-900 mb-4">❌ Erreur de confirmation</h1>
        <p class="text-red-600 mb-2">{{ errorMessage }}</p>
        <p class="text-gray-600 mb-8 text-sm">
          Le lien de confirmation est peut-être expiré ou invalide.
        </p>

        <div class="space-y-3">
          <button
            @click="retryConfirmation"
            class="w-full bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
          >
            Réessayer
          </button>

          <NuxtLink
            to="/login"
            class="block text-gray-600 hover:text-gray-800"
          >
            Retour à la connexion
          </NuxtLink>
        </div>
      </div>

      <!-- Footer -->
      <div class="mt-8 pt-6 border-t border-gray-200">
        <NuxtLink to="/" class="text-sm text-gray-500 hover:text-gray-700">
          ← Retour à l'accueil
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
  title: 'Confirmation d\'email'
})

const route = useRoute()
const config = useRuntimeConfig()

const confirming = ref(true)
const confirmed = ref(false)
const errorMessage = ref('')

const userId = route.query.userId as string
const token = route.query.token as string

const confirmEmail = async () => {
  if (!userId || !token) {
    confirming.value = false
    errorMessage.value = 'Paramètres manquants dans le lien de confirmation'
    return
  }

  try {
    const response = await $fetch(`${config.public.apiBase}/auth/confirm-email?userId=${userId}&token=${token}`, {
      method: 'GET'
    })

    confirmed.value = true
  } catch (error: any) {
    confirmed.value = false
    errorMessage.value = error.data?.message || error.message || 'Erreur lors de la confirmation de l\'email'
    console.error('Erreur confirmation:', error)
  } finally {
    confirming.value = false
  }
}

const retryConfirmation = () => {
  confirming.value = true
  errorMessage.value = ''
  confirmEmail()
}

// Confirmation automatique au chargement
onMounted(() => {
  confirmEmail()
})
</script>

