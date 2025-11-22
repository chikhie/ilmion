<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <!-- Logo/Header -->
      <div class="text-center mb-8">
        <h1 class="text-4xl font-bold text-primary-600 mb-2">📚 Ilmanar</h1>
        <p class="text-gray-600">Nouveau mot de passe</p>
      </div>

      <!-- Form -->
      <div class="bg-white rounded-lg shadow-xl p-8">
        <div v-if="!passwordReset">
          <p class="text-gray-600 mb-6">
            Choisissez un nouveau mot de passe sécurisé pour votre compte.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm">
              {{ errorMessage }}
            </div>

            <!-- New Password -->
            <div>
              <label for="password" class="block text-sm font-medium text-gray-700 mb-2">
                Nouveau mot de passe
              </label>
              <input
                id="password"
                v-model="form.password"
                type="password"
                required
                minlength="6"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="••••••••"
              />
              <p class="mt-1 text-xs text-gray-500">Minimum 6 caractères</p>
            </div>

            <!-- Confirm Password -->
            <div>
              <label for="confirmPassword" class="block text-sm font-medium text-gray-700 mb-2">
                Confirmer le mot de passe
              </label>
              <input
                id="confirmPassword"
                v-model="form.confirmPassword"
                type="password"
                required
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="••••••••"
              />
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="loading || !isFormValid"
              class="w-full bg-primary-600 text-white py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span v-if="!loading">Réinitialiser le mot de passe</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Réinitialisation...
              </span>
            </button>
          </form>
        </div>

        <!-- Success Message -->
        <div v-else class="text-center">
          <div class="mb-6">
            <div class="mx-auto w-16 h-16 bg-green-100 rounded-full flex items-center justify-center">
              <svg class="w-10 h-10 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
              </svg>
            </div>
          </div>

          <h3 class="text-xl font-semibold text-gray-900 mb-2">Mot de passe réinitialisé !</h3>
          <p class="text-gray-600 mb-6">
            Votre mot de passe a été changé avec succès. Vous pouvez maintenant vous connecter.
          </p>

          <NuxtLink
            to="/login"
            class="inline-block bg-primary-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors"
          >
            Se connecter
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
  title: 'Réinitialiser le mot de passe'
})

const route = useRoute()
const token = route.query.token as string
const email = route.query.email as string

const form = ref({
  password: '',
  confirmPassword: ''
})

const loading = ref(false)
const errorMessage = ref('')
const passwordReset = ref(false)

const isFormValid = computed(() => {
  return (
    form.value.password.length >= 6 &&
    form.value.password === form.value.confirmPassword
  )
})

onMounted(() => {
  if (!token || !email) {
    errorMessage.value = 'Lien de réinitialisation invalide'
  }
})

const handleSubmit = async () => {
  if (!isFormValid.value) return

  errorMessage.value = ''
  loading.value = true

  try {
    // TODO: Implémenter l'API de réinitialisation de mot de passe
    // await api.resetPassword({
    //   email: email,
    //   token: token,
    //   newPassword: form.value.password
    // })
    
    // Simuler la réinitialisation
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    passwordReset.value = true
  } catch (error: any) {
    errorMessage.value = error.data?.message || 'Erreur lors de la réinitialisation du mot de passe'
  } finally {
    loading.value = false
  }
}
</script>

