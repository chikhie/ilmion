<template>
  <div class="min-h-screen bg-[#1e1f22] flex items-center justify-center px-4 py-8 sm:py-12 relative overflow-hidden font-sans text-[#f2f3f5]">
    <!-- Background Elements -->
    <div class="absolute inset-0 z-0 pointer-events-none bg-[#1e1f22]">
        <!-- Mathematical Tech Grid Pattern -->
        <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#00B578 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
        
        <!-- Glow Effects -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-[#00B578]/15 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/15 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <div class="max-w-4xl w-full relative z-10 transition-all duration-300">
      <!-- Logo/Header -->
      <div class="text-center mb-8 animate-fade-in">
        <div class="mb-6 inline-flex items-center justify-center p-5 rounded-3xl bg-[#2b2d31] shadow-xl border-b-[4px] border-[#1e1f22] hover:-translate-y-1 transition-transform">
          <!-- Science Icon Logo -->
          <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 text-[#00B578]" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
            <path stroke-linecap="round" stroke-linejoin="round" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" />
          </svg>
        </div>
        <h1 class="text-2xl sm:text-3xl md:text-4xl font-extrabold text-[#f2f3f5] tracking-tight mb-2 drop-shadow-lg uppercase">Rejoindre Ilmanar</h1>
      </div>

      <!-- Register Form -->
      <div class="bg-[#2b2d31] backdrop-blur-xl rounded-[2rem] p-8 md:p-10 border border-[#1e1f22] shadow-[0_8px_30px_rgba(0,0,0,0.4)] animate-fade-in-up max-w-[500px] w-full mx-auto relative">
        <form @submit.prevent="handleRegister" class="space-y-6 sm:space-y-8">
          <!-- Alert -->
          <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-2xl text-sm text-center">
            {{ errorMessage }}
          </div>

          <div class="space-y-4 sm:space-y-6">
            <!-- Username -->
            <div class="space-y-2">
              <label for="username" class="block text-xs font-bold uppercase tracking-widest text-[#b5bac1] ml-1">
                Nom d'utilisateur
              </label>
              <input
                id="username"
                v-model="form.username"
                type="text"
                required
                minlength="3"
                class="w-full px-5 py-4 bg-[#1e1f22] border-2 border-transparent text-[#f2f3f5] rounded-2xl focus:border-[#00B578] focus:bg-[#2b2d31] placeholder-[#4f545c] transition-all outline-none"
                placeholder="votre_pseudo"
              />
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
              <div class="relative">
                <input
                  id="password"
                  v-model="form.password"
                  :type="showPassword ? 'text' : 'password'"
                  required
                  minlength="6"
                  class="w-full px-5 py-4 bg-[#1e1f22] border-2 border-transparent text-[#f2f3f5] rounded-2xl focus:border-[#00B578] focus:bg-[#2b2d31] placeholder-[#4f545c] transition-all outline-none pr-12"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  @click="showPassword = !showPassword"
                  class="absolute right-4 top-1/2 transform -translate-y-1/2 text-[#949ba4] hover:text-[#f2f3f5] transition-colors focus:outline-none"
                  title="Afficher/Masquer le mot de passe"
                  aria-label="Afficher ou masquer le mot de passe"
                >
                  <svg v-if="!showPassword" class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                  </svg>
                  <svg v-else class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.542-7a10.05 10.05 0 011.574-2.59M5.275 3.725l14.85 14.85M9.88 9.88l4.24 4.24M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                </button>
              </div>
            </div>
          </div>


          <!-- Submit Button -->
          <div class="relative group mt-8">
            <button
              type="submit"
              :disabled="authStore.loading || !isFormValid"
              class="w-full bg-[#00B578] text-white py-4 rounded-2xl font-extrabold text-lg shadow-[0_6px_0_#00895a] hover:-translate-y-1 hover:shadow-[0_8px_0_#00895a] active:translate-y-1 active:shadow-[0_0px_0_#00895a] disabled:opacity-50 disabled:translate-y-0 disabled:shadow-[0_6px_0_#00895a] transition-all cursor-pointer uppercase tracking-wider"
            >
              <span v-if="!authStore.loading">Créer mon compte</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin h-6 w-6 mr-3" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Calcul en cours...
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

        <!-- Login Link -->
        <div class="mt-8 text-center text-sm text-[#949ba4] font-medium">
          Déjà membre ?
          <NuxtLink to="/login" class="text-[#00B578] hover:text-[#00895a] hover:underline font-bold ml-1 transition-colors">
            Connectez-vous ici
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
  <AppFooter />
</template>

<script setup lang="ts">
definePageMeta({
  middleware: 'guest',
  layout: false
})

useSeoMeta({
  title: 'Inscription | Ilmanar',
  description: 'Créez un compte pour commencer à apprendre.'
})

const authStore = useAuthStore()
const router = useRouter()

const showPassword = ref(false)
const form = ref({
  username: '',
  full_name: '',
  email: '',
  password: ''
})

const errorMessage = ref('')

const isFormValid = computed(() => {
  return (
    form.value.username.length >= 3 &&
    form.value.email.includes('@') &&
    form.value.password.length >= 6 &&
    /[A-Z]/.test(form.value.password) &&
    /\d/.test(form.value.password) &&
    /[!@#$%^&*(),.?":{}|<>-]/.test(form.value.password)
  )
})

const validationMessage = computed(() => {
  if (authStore.loading) return ''
  
  if (form.value.username.length < 3) {
    return 'Le nom d\'utilisateur doit contenir au moins 3 caractères'
  }
  if (!form.value.email.includes('@')) {
    return 'Veuillez entrer une adresse email valide'
  }
  if (form.value.password.length < 6) {
    return 'Le mot de passe doit contenir au moins 6 caractères'
  }
  if (!/[A-Z]/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins une majuscule'
  }
  if (!/\d/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins un chiffre'
  }
  if (!/[!@#$%^&*(),.?":{}|<>-]/.test(form.value.password)) {
    return 'Le mot de passe doit contenir au moins un caractère spécial'
  }
  
  return ''
})

const handleRegister = async () => {
  errorMessage.value = ''

  const result = await authStore.register({
    username: form.value.username,
    full_name: form.value.username,
    email: form.value.email,
    password: form.value.password,
    firstName: '', // Optional/empty for now
    lastName: ''   // Optional/empty for now
  })

  if (result.success) {
    // Rediriger vers la page de connexion avec un message de succès
    router.push('/login?registered=true')
  } else {
    errorMessage.value = result.message || 'Erreur lors de l\'inscription'
  }
}
</script>

