<template>
  <div class="min-h-screen bg-[#1e1f22] flex items-center justify-center px-4 py-8 sm:py-0 relative overflow-hidden font-sans text-[#f2f3f5]">
    <!-- Background Elements -->
    <div class="absolute inset-0 z-0 pointer-events-none bg-[#1e1f22]">
        <!-- Mathematical Tech Grid Pattern -->
        <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#00B578 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
        
        <!-- Glow Effects -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-[#00B578]/15 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/15 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <div class="max-w-md w-full relative z-10 transition-all duration-300">
      <!-- Logo/Header -->
      <div class="text-center mb-8 animate-fade-in">
        <div class="mb-6 inline-flex items-center justify-center p-5 rounded-3xl bg-[#2b2d31] shadow-xl border-b-[4px] border-[#1e1f22] hover:-translate-y-1 transition-transform">
          <!-- Science Icon Logo -->
          <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 text-[#00B578]" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
            <path stroke-linecap="round" stroke-linejoin="round" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" />
          </svg>
        </div>
        <h1 class="text-3xl sm:text-4xl font-extrabold text-[#f2f3f5] tracking-tight mb-2 drop-shadow-lg">ILMANAR</h1>
      </div>

      <!-- Login Form -->
      <div class="bg-[#2b2d31] backdrop-blur-xl rounded-[2rem] p-8 border border-[#1e1f22] shadow-[0_8px_30px_rgba(0,0,0,0.4)] animate-fade-in-up relative">
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
            <label for="email" class="block text-xs font-bold uppercase tracking-widest text-[#b5bac1] ml-1">
              Email
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              class="w-full px-5 py-4 bg-[#1e1f22] border-2 border-transparent text-[#f2f3f5] rounded-2xl focus:border-[#00B578] focus:bg-[#2b2d31] placeholder-[#4f545c] transition-all outline-none"
              placeholder="votre@email.com"
            />
          </div>

          <!-- Password -->
          <div class="space-y-2">
            <label for="password" class="block text-xs font-bold uppercase tracking-widest text-[#b5bac1] ml-1">
              Mot de passe
            </label>
            <div class="relative group">
              <input
                id="password"
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                required
                class="w-full px-5 py-4 bg-[#1e1f22] border-2 border-transparent text-[#f2f3f5] rounded-2xl focus:border-[#00B578] focus:bg-[#2b2d31] placeholder-[#4f545c] transition-all outline-none"
                placeholder="••••••••"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-4 top-1/2 -translate-y-1/2 text-[#949ba4] hover:text-[#f2f3f5] transition-colors"
                title="Afficher/Masquer le mot de passe"
                aria-label="Afficher ou masquer le mot de passe"
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
          <div class="flex items-center justify-end text-xs px-1">
            <NuxtLink to="/forgot-password" class="text-[#00B578] hover:text-[#00895a] transition-colors font-bold uppercase tracking-wide">
              Mot de passe oublié ?
            </NuxtLink>
          </div>

          <!-- Submit Button -->
          <div class="relative group mt-8">
            <button
              type="submit"
              :disabled="authStore.loading || !isFormValid"
              class="w-full bg-[#00B578] text-white py-4 rounded-2xl font-extrabold text-lg shadow-[0_6px_0_#00895a] hover:-translate-y-1 hover:shadow-[0_8px_0_#00895a] active:translate-y-1 active:shadow-[0_0px_0_#00895a] disabled:opacity-50 disabled:translate-y-0 disabled:shadow-[0_6px_0_#00895a] transition-all cursor-pointer uppercase tracking-wider"
            >
              <span v-if="!authStore.loading">Se connecter</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin h-5 w-5 mr-3" viewBox="0 0 24 24">
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
        <div class="mt-8 text-center text-sm text-[#949ba4] font-medium">
          Nouveau ici ?
          <NuxtLink to="/register" class="text-[#00B578] hover:text-[#00895a] hover:underline font-bold ml-1 transition-colors">
            Créer un compte
          </NuxtLink>
        </div>
      </div>

      <!-- Back to Home -->
      <div class="mt-8 text-center animate-fade-in">
        <NuxtLink to="/" class="text-xs text-[#949ba4] hover:text-[#f2f3f5] transition-colors font-bold flex items-center justify-center gap-2 uppercase tracking-widest">
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

useSeoMeta({
  title: 'Connexion | Ilmanar',
  description: 'Connectez-vous pour continuer votre apprentissage.'
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
  const { email, password } = form.value
  return (
    email.includes('@') &&
    password.length >= 0
  )
})

const validationMessage = computed(() => {
  if (authStore.loading) return ''
  
  if (!form.value.email.includes('@')) {
    return 'Veuillez entrer une adresse email valide'
  }
  if (form.value.password.length < 0) {
    return 'Veuillez entrer un mot de passe'
  }
  return ''
})

const isMounted = ref(false)

// Afficher le message de succès si redirection depuis register
onMounted(() => {
  isMounted.value = true
  if (route.query.registered === 'true') {
    successMessage.value = 'Inscription réussie ! Veuillez vérifier votre email pour confirmer votre compte.'
  } else if (route.query.verify === 'true') {
    successMessage.value = 'Votre compte a bien été vérifié ! Vous pouvez maintenant vous connecter.'
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
    // Check for specific error messages or default to generic
    if (result.status === 401) {
        errorMessage.value = 'Email ou mot de passe incorrect.'
    } else {
        errorMessage.value = result.message || 'Erreur de connexion'
    }
  }
}
</script>

