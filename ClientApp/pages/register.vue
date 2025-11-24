<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 py-12">
    <div class="max-w-4xl w-full">
      <!-- Logo/Header -->
      <div class="text-center mb-8">
        <h1 class="text-4xl font-bold text-[#C39712] mb-2">Ilmanar</h1>
        <p class="text-gray-400">Créez votre compte</p>
      </div>

      <!-- Register Form -->
      <div class="bg-gray-800 rounded-lg shadow-xl p-8 border border-gray-700">
        <form @submit.prevent="handleRegister" class="space-y-6">
          <!-- Alert -->
          <div v-if="errorMessage" class="bg-red-900/30 border border-red-800 text-red-300 px-4 py-3 rounded-lg text-sm">
            {{ errorMessage }}
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <!-- Colonne Gauche -->
            <div class="space-y-6">
              <!-- Prénom -->
              <div>
                <label for="firstName" class="block text-sm font-medium text-gray-300 mb-2">
                  Prénom <span class="text-xs text-gray-500">(optionnel)</span>
                </label>
                <input
                  id="firstName"
                  v-model="form.firstName"
                  type="text"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
                />
              </div>

              <!-- Nom -->
              <div>
                <label for="lastName" class="block text-sm font-medium text-gray-300 mb-2">
                  Nom <span class="text-xs text-gray-500">(optionnel)</span>
                </label>
                <input
                  id="lastName"
                  v-model="form.lastName"
                  type="text"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
                />
              </div>

              <!-- Username -->
              <div>
                <label for="username" class="block text-sm font-medium text-gray-300 mb-2">
                  Nom d'utilisateur
                </label>
                <input
                  id="username"
                  v-model="form.username"
                  type="text"
                  required
                  minlength="3"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
                />
              </div>

              <!-- Date de naissance -->
              <div>
                <label for="dateOfBirth" class="block text-sm font-medium text-gray-300 mb-2">
                  Date de naissance <span class="text-xs text-gray-500">(optionnel)</span>
                </label>
                <input
                  id="dateOfBirth"
                  v-model="form.dateOfBirth"
                  type="date"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400 [color-scheme:dark]"
                />
              </div>
            </div>

            <!-- Colonne Droite -->
            <div class="space-y-6">
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
                  minlength="6"
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
                />
              </div>

              <!-- Confirm Password -->
              <div>
                <label for="confirmPassword" class="block text-sm font-medium text-gray-300 mb-2">
                  Confirmer le mot de passe
                </label>
                <input
                  id="confirmPassword"
                  v-model="form.confirmPassword"
                  type="password"
                  required
                  class="w-full px-4 py-3 bg-gray-700 border border-gray-600 text-white rounded-lg focus:ring-2 focus:ring-[#C39712] focus:border-transparent placeholder-gray-400"
                />
              </div>
            </div>
          </div>

          <!-- Terms (Full Width) -->
          <div class="flex items-start pt-4 border-t border-gray-700">
            <input
              id="terms"
              v-model="form.acceptTerms"
              type="checkbox"
              required
              class="h-4 w-4 mt-1 text-[#C39712] focus:ring-[#C39712] border-gray-600 rounded bg-gray-700"
            />
            <label for="terms" class="ml-2 text-sm text-gray-400">
              J'accepte les <a href="#" class="text-[#C39712] hover:text-yellow-500 transition-colors">conditions d'utilisation</a>
              et la <a href="#" class="text-[#C39712] hover:text-yellow-500 transition-colors">politique de confidentialité</a>
            </label>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="authStore.loading || !isFormValid"
            class="w-full bg-[#C39712] text-[#082540] py-3 rounded-lg font-medium hover:bg-yellow-500 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span v-if="!authStore.loading">S'inscrire</span>
            <span v-else class="flex items-center justify-center">
              <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Inscription...
            </span>
          </button>
        </form>

        <!-- Login Link -->
        <div class="mt-6 text-center text-sm text-gray-400">
          Vous avez déjà un compte ?
          <NuxtLink to="/login" class="text-[#C39712] hover:text-yellow-500 font-medium transition-colors">
            Connectez-vous
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
  title: 'Inscription'
})

const authStore = useAuthStore()
const router = useRouter()

const form = ref({
  firstName: '',
  lastName: '',
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
  dateOfBirth: '',
  acceptTerms: false
})

const errorMessage = ref('')

const isFormValid = computed(() => {
  return (
    form.value.username.length >= 3 &&
    form.value.email.includes('@') &&
    form.value.password.length >= 6 &&
    form.value.password === form.value.confirmPassword &&
    form.value.acceptTerms
  )
})

const handleRegister = async () => {
  errorMessage.value = ''

  // Vérifier que les mots de passe correspondent
  if (form.value.password !== form.value.confirmPassword) {
    errorMessage.value = 'Les mots de passe ne correspondent pas'
    return
  }

  const result = await authStore.register({
    username: form.value.username,
    email: form.value.email,
    password: form.value.password,
    firstName: form.value.firstName || undefined,
    lastName: form.value.lastName || undefined,
    dateOfBirth: form.value.dateOfBirth || undefined
  })

  if (result.success) {
    // Rediriger vers la page de connexion avec un message de succès
    router.push('/login?registered=true')
  } else {
    errorMessage.value = result.message || 'Erreur lors de l\'inscription'
  }
}
</script>

