<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <!-- Logo/Header -->
      <div class="text-center mb-8">
        <h1 class="text-4xl font-bold text-primary-600 mb-2">📚 Ilmanar</h1>
        <p class="text-gray-600">Réinitialisation du mot de passe</p>
      </div>

      <!-- Form -->
      <div class="bg-white rounded-lg shadow-xl p-8">
        <div v-if="!emailSent">
          <p class="text-gray-600 mb-6">
            Entrez votre adresse email et nous vous enverrons un lien pour réinitialiser votre mot de passe.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm">
              {{ errorMessage }}
            </div>

            <!-- Email -->
            <div>
              <label for="email" class="block text-sm font-medium text-gray-700 mb-2">
                Email
              </label>
              <input
                id="email"
                v-model="email"
                type="email"
                required
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="votre@email.com"
              />
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="loading"
              class="w-full bg-primary-600 text-white py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span v-if="!loading">Envoyer le lien</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Envoi...
              </span>
            </button>
          </form>

          <!-- Back to Login -->
          <div class="mt-6 text-center">
            <NuxtLink to="/login" class="text-primary-600 hover:text-primary-700 font-medium text-sm">
              ← Retour à la connexion
            </NuxtLink>
          </div>
        </div>

        <!-- Success Message -->
        <div v-else class="text-center">
          <div class="mb-6">
            <div class="mx-auto w-16 h-16 bg-green-100 rounded-full flex items-center justify-center">
              <svg class="w-10 h-10 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"></path>
              </svg>
            </div>
          </div>

          <h3 class="text-xl font-semibold text-gray-900 mb-2">Email envoyé !</h3>
          <p class="text-gray-600 mb-6">
            Si un compte existe avec cette adresse email, vous recevrez un lien de réinitialisation dans quelques minutes.
          </p>

          <NuxtLink
            to="/login"
            class="inline-block bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
          >
            Retour à la connexion
          </NuxtLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  middleware: 'guest',
  layout: false
})

useHead({
  title: 'Mot de passe oublié'
})

const email = ref('')
const loading = ref(false)
const errorMessage = ref('')
const emailSent = ref(false)

const handleSubmit = async () => {
  errorMessage.value = ''
  loading.value = true

  try {
    // TODO: Implémenter l'API de récupération de mot de passe
    // await api.forgotPassword(email.value)
    
    // Simuler l'envoi d'email
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    emailSent.value = true
  } catch (error: any) {
    errorMessage.value = error.data?.message || 'Erreur lors de l\'envoi de l\'email'
  } finally {
    loading.value = false
  }
}
</script>

