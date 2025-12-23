<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 py-8 sm:py-0 relative overflow-hidden font-sans">
    <!-- Background Gradients (Modern & Minimal) -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-6 sm:mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-14 w-14 sm:h-20 sm:w-20 mx-auto mb-4 sm:mb-6 drop-shadow-2xl" />
        <h1 class="text-3xl sm:text-4xl font-black tracking-tighter text-white uppercase mb-2">ILMANAR</h1>
      </div>

      <!-- Login Form -->
      <div class="bg-white/5 backdrop-blur-xl rounded-2xl sm:rounded-3xl p-6 sm:p-8 border border-white/10 shadow-2xl animate-fade-in-up">
        <form @submit.prevent="handleLogin" class="space-y-5 sm:space-y-6">
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
            <div class="relative group">
              <input
                id="password"
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                required
                class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 focus:border-white/20 placeholder-gray-600 transition-all outline-none"
                placeholder="••••••••"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-white transition-colors"
                title="Afficher/Masquer le mot de passe"
              >
                <svg v-if="!showPassword" class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
                <svg v-else class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.542-7 1.274-4.057 5.064-7 9.542-7 1.225 0 2.39.22 3.46.612M15 12a3 3 0 11-6 0 3 3 0 016 0zm-9 9l12-12" />
                </svg>
              </button>
            </div>
          </div>

          <!-- Forgot Password Link -->
          <div class="flex items-center justify-between text-xs px-1">
            <div class="flex items-center">
              <!-- <input
                id="remember"
                type="checkbox"
                class="h-4 w-4 bg-white/5 border-white/10 rounded text-white focus:ring-offset-[#082540]"
              />
              <label for="remember" class="ml-2 text-gray-500 font-medium">
                Se souvenir
              </label> -->
            </div>
            <NuxtLink to="/forgot-password" class="text-white hover:text-gray-300 transition-colors font-bold">
              Mot de passe oublié ?
            </NuxtLink>
          </div>

          <!-- Submit Button -->
          <div class="relative group">
            <button
              type="submit"
              :disabled="authStore.loading || !isFormValid"
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
            <!-- Validation Tooltip -->
            <div
              v-if="!isFormValid && validationMessage"
              class="absolute bottom-full left-1/2 transform -translate-x-1/2 mb-2 opacity-0 group-hover:opacity-100 transition-opacity pointer-events-none"
            >
              <div class="bg-red-500/90 text-white text-xs px-4 py-2 rounded-lg whitespace-nowrap shadow-lg">
                {{ validationMessage }}
                <div class="absolute top-full left-1/2 transform -translate-x-1/2 -mt-1">
                  <div class="border-4 border-transparent border-t-red-500/90"></div>
                </div>
              </div>
            </div>
          </div>
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
const showPassword = ref(false)

const isFormValid = computed(() => {
  return (
    form.value.email.includes('@') &&
    form.value.password.length >= 6
  )
})

const validationMessage = computed(() => {
  if (authStore.loading) return ''
  
  if (!form.value.email.includes('@')) {
    return 'Veuillez entrer une adresse email valide'
  }
  if (form.value.password.length < 6) {
    return 'Le mot de passe doit contenir au moins 6 caractères'
  }
  
  return ''
})

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

