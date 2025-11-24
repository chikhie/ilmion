<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <!-- Logo/Header -->
      <div class="text-center mb-8">
        <h1 class="text-4xl font-bold text-[#C39712] mb-2">Ilmanar</h1>
        <p class="text-gray-400">Connectez-vous à votre compte</p>
      </div>

      <!-- Login Form -->
      <div class="bg-gray-800 rounded-lg shadow-xl p-8 border border-gray-700">
        <form @submit.prevent="handleLogin" class="space-y-6">
          <!-- Alert -->
          <div v-if="errorMessage" class="bg-red-900/30 border border-red-800 text-red-300 px-4 py-3 rounded-lg">
            {{ errorMessage }}
          </div>

          <div v-if="successMessage" class="bg-green-900/30 border border-green-800 text-green-300 px-4 py-3 rounded-lg">
            {{ successMessage }}
          </div>

          <!-- Email -->
          <div>
            <label for="email" class="block text-sm font-medium text-gray-300 mb-2">
              Email
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
              placeholder="votre@email.com"
            />
          </div>

          <!-- Password -->
          <div>
            <label for="password" class="block text-sm font-medium text-gray-300 mb-2">
              Mot de passe
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              required
              class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
              placeholder="••••••••"
            />
          </div>

          <!-- Forgot Password Link -->
          <div class="flex items-center justify-between text-sm">
            <div class="flex items-center">
              <input
                id="remember"
                type="checkbox"
                class="h-4 w-4 text-[#C39712] focus:ring-[#C39712] border-gray-600 rounded bg-gray-700"
              />
              <label for="remember" class="ml-2 text-gray-400">
                Se souvenir de moi
              </label>
            </div>
            <NuxtLink to="/forgot-password" class="text-[#C39712] hover:text-yellow-500 transition-colors">
              Mot de passe oublié ?
            </NuxtLink>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="authStore.loading"
            class="w-full bg-[#C39712] text-[#082540] py-3 rounded-lg font-medium hover:bg-yellow-500 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span v-if="!authStore.loading">Se connecter</span>
            <span v-else class="flex items-center justify-center">
              <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Connexion...
            </span>
          </button>
        </form>

        <!-- Register Link -->
        <div class="mt-6 text-center text-sm text-gray-400">
          Vous n'avez pas de compte ?
          <NuxtLink to="/register" class="text-[#C39712] hover:text-yellow-500 font-medium transition-colors">
            Inscrivez-vous
          </NuxtLink>
        </div>

        <!-- Back to Home -->
        <div class="mt-4 text-center">
          <NuxtLink to="/" class="text-sm text-gray-500 hover:text-gray-300 transition-colors">
            ← Retour à l'accueil
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
  title: 'Connexion'
})

const authStore = useAuthStore()
const route = useRoute()
const router = useRouter()

const form = ref({
  email: '',
  password: ''
})

const errorMessage = ref('')
const successMessage = ref('')

// Afficher le message de succès si redirection depuis register
onMounted(() => {
  if (route.query.registered === 'true') {
    successMessage.value = 'Inscription réussie ! Veuillez vérifier votre email pour confirmer votre compte.'
  }
})

const handleLogin = async () => {
  errorMessage.value = ''
  successMessage.value = ''

  const result = await authStore.login(form.value)

  if (result.success) {
    // Rediriger vers la page demandée ou le dashboard
    const redirect = route.query.redirect as string
    router.push(redirect || '/dashboard')
  } else {
    errorMessage.value = result.message || 'Erreur de connexion'
  }
}
</script>

