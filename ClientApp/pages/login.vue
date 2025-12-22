<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 relative overflow-hidden font-sans">
    <!-- Background Gradients (Modern & Minimal) -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-2xl" />
        <h1 class="text-4xl font-black tracking-tighter text-white uppercase mb-2">ILMANAR</h1>
        <p class="text-gray-400 font-medium">Bon retour parmi nous</p>
      </div>

      <!-- Login Form -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl p-8 border border-white/10 shadow-2xl animate-fade-in-up">
        <form @submit.prevent="handleLogin" class="space-y-6">
          <!-- Alert -->
          <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-2xl text-sm text-center">
            {{ errorMessage }}
          </div>

          <div v-if="successMessage" class="bg-green-500/10 border border-green-500/20 text-green-400 px-4 py-3 rounded-2xl text-sm text-center">
            {{ successMessage }}
          </div>

          <!-- Email -->
          <div class="space-y-2">
            <label for="email" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
              Email
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 focus:border-white/20 placeholder-gray-600 transition-all outline-none"
              placeholder="votre@email.com"
            />
          </div>

          <!-- Password -->
          <div class="space-y-2">
            <label for="password" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
              Mot de passe
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              required
              class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 focus:border-white/20 placeholder-gray-600 transition-all outline-none"
              placeholder="••••••••"
            />
          </div>

          <!-- Forgot Password Link -->
          <div class="flex items-center justify-between text-xs px-1">
            <div class="flex items-center">
              <input
                id="remember"
                type="checkbox"
                class="h-4 w-4 bg-white/5 border-white/10 rounded text-white focus:ring-offset-[#082540]"
              />
              <label for="remember" class="ml-2 text-gray-500 font-medium">
                Se souvenir
              </label>
            </div>
            <NuxtLink to="/forgot-password" class="text-white hover:text-gray-300 transition-colors font-bold">
              Oublié ?
            </NuxtLink>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="authStore.loading"
            class="w-full bg-white text-[#082540] py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all transform active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed shadow-xl shadow-white/5"
          >
            <span v-if="!authStore.loading">Se connecter</span>
            <span v-else class="flex items-center justify-center">
              <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Chargement...
            </span>
          </button>
        </form>

        <!-- Register Link -->
        <div class="mt-8 text-center text-xs text-gray-500 font-medium">
          Nouveau ici ?
          <NuxtLink to="/register" class="text-white hover:underline font-bold ml-1">
            Créer un compte
          </NuxtLink>
        </div>
      </div>

      <!-- Back to Home -->
      <div class="mt-10 text-center animate-fade-in">
        <NuxtLink to="/" class="text-xs text-gray-500 hover:text-white transition-colors font-bold flex items-center justify-center gap-2">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
          Retour à l'accueil
        </NuxtLink>
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
    router.push(redirect || '/games')
  } else {
    errorMessage.value = result.message || 'Erreur de connexion'
  }
}
</script>

