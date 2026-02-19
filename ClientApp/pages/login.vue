<template>
  <div class="min-h-screen bg-brand-dark flex items-center justify-center px-4 py-8 sm:py-0 relative overflow-hidden font-sans-body">
    <!-- Background Elements -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <!-- Abstract Pattern -->
        <div class="absolute inset-0 opacity-[0.03]" style="background-image: url('/patterns/arabesque.png')"></div>
        
        <!-- Glow Effects -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-brand-gold/10 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-brand-wood/10 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <div class="max-w-md w-full relative z-10 transition-all duration-300">
      <!-- Logo/Header -->
      <div class="text-center mb-8 animate-fade-in">
        <div class="mb-6 inline-block p-4 rounded-full border border-brand-gold/20 bg-brand-wood/10 backdrop-blur-sm shadow-[0_0_30px_rgba(195,151,18,0.1)]">
             <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-16 w-16 drop-shadow-2xl" />
        </div>
        <h1 class="text-3xl sm:text-4xl font-serif-title font-bold text-brand-parchment mb-2 drop-shadow-lg tracking-wide">ILMANAR</h1>
      </div>

      <!-- Login Form -->
      <div class="bg-brand-wood/10 backdrop-blur-xl rounded-3xl p-8 border border-brand-gold/20 shadow-2xl animate-fade-in-up">
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
            <label for="email" class="block text-xs font-bold uppercase tracking-widest text-brand-parchment/90 ml-1">
              Email
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              class="w-full px-5 py-4 bg-brand-wood/20 border border-brand-gold/30 text-brand-parchment rounded-2xl focus:ring-2 focus:ring-brand-gold/50 focus:border-brand-gold/50 placeholder-brand-parchment/30 transition-all outline-none"
              placeholder="votre@email.com"
            />
          </div>

          <!-- Password -->
          <div class="space-y-2">
            <label for="password" class="block text-xs font-bold uppercase tracking-widest text-brand-parchment/90 ml-1">
              Mot de passe
            </label>
            <div class="relative group">
              <input
                id="password"
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                required
                class="w-full px-5 py-4 bg-brand-wood/20 border border-brand-gold/30 text-brand-parchment rounded-2xl focus:ring-2 focus:ring-brand-gold/50 focus:border-brand-gold/50 placeholder-brand-parchment/30 transition-all outline-none"
                placeholder="••••••••"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-4 top-1/2 -translate-y-1/2 text-brand-gold/60 hover:text-brand-gold transition-colors"
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
            <NuxtLink to="/forgot-password" class="text-brand-gold hover:text-brand-gold/80 transition-colors font-bold uppercase tracking-wide">
              Mot de passe oublié ?
            </NuxtLink>
          </div>

          <!-- Submit Button -->
          <div class="relative group pt-2">
            <button
              type="submit"
              :disabled="!isMounted || authStore.loading || !isFormValid"
              class="w-full bg-brand-gold text-brand-dark py-4 rounded-2xl font-bold text-lg hover:shadow-[0_0_20px_rgba(195,151,18,0.4)] transition-all transform active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed uppercase tracking-wider"
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
        <div class="mt-8 text-center text-sm text-brand-parchment/60 font-medium">
          Nouveau ici ?
          <NuxtLink to="/register" class="text-brand-gold hover:text-brand-gold/80 hover:underline font-bold ml-1">
            Créer un compte
          </NuxtLink>
        </div>
      </div>

      <!-- Back to Home -->
      <div class="mt-8 text-center animate-fade-in">
        <NuxtLink to="/" class="text-xs text-brand-parchment/50 hover:text-brand-parchment transition-colors font-bold flex items-center justify-center gap-2 uppercase tracking-widest">
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
  const { email, password } = form.value
  return (
    email.includes('@') &&
    password.length >= 6 &&
    /[A-Z]/.test(password) &&
    /[0-9]/.test(password) &&
    /[!@#-$%^&*(),.?":{}|<>]/.test(password)
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
  if (!/[A-Z]/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins une majuscule'
  }
  if (!/[0-9]/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins un chiffre'
  }
  if (!/[!@#$%^&*(),.?":{}|<>-]/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins un caractère spécial'
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

