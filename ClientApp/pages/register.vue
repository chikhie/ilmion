<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 py-12 relative overflow-hidden font-sans">
    <!-- Background Gradients (Modern & Minimal) -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-4xl w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-16 w-16 mx-auto mb-6 drop-shadow-2xl" />
        <h1 class="text-4xl font-black tracking-tighter text-white uppercase mb-2">REJOINDRE ILMANAR</h1>
        <p class="text-gray-400 font-medium text-lg">Créez votre accès au savoir</p>
      </div>

      <!-- Register Form -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl p-8 md:p-12 border border-white/10 shadow-2xl animate-fade-in-up">
        <form @submit.prevent="handleRegister" class="space-y-8">
          <!-- Alert -->
          <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-2xl text-sm text-center">
            {{ errorMessage }}
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-x-12 gap-y-8">
            <!-- Colonne Gauche -->
            <div class="space-y-6">
              <!-- Prénom -->
              <div class="space-y-2">
                <label for="firstName" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                  Prénom <span class="normal-case opacity-50 font-normal ml-1">(optionnel)</span>
                </label>
                <input
                  id="firstName"
                  v-model="form.firstName"
                  type="text"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none"
                />
              </div>

              <!-- Nom -->
              <div class="space-y-2">
                <label for="lastName" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                  Nom <span class="normal-case opacity-50 font-normal ml-1">(optionnel)</span>
                </label>
                <input
                  id="lastName"
                  v-model="form.lastName"
                  type="text"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none"
                />
              </div>

              <!-- Username -->
              <div class="space-y-2">
                <label for="username" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                  Nom d'utilisateur
                </label>
                <input
                  id="username"
                  v-model="form.username"
                  type="text"
                  required
                  minlength="3"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none placeholder-gray-600"
                  placeholder="votre_pseudo"
                />
              </div>

              <!-- Date de naissance -->
              <div class="space-y-2">
                <label for="dateOfBirth" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                  Date de naissance <span class="normal-case opacity-50 font-normal ml-1">(optionnel)</span>
                </label>
                <input
                  id="dateOfBirth"
                  v-model="form.dateOfBirth"
                  type="date"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none [color-scheme:dark]"
                />
              </div>
            </div>

            <!-- Colonne Droite -->
            <div class="space-y-6">
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
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none placeholder-gray-600"
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
                  minlength="6"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none placeholder-gray-600"
                  placeholder="••••••••"
                />
              </div>

              <!-- Confirm Password -->
              <div class="space-y-2">
                <label for="confirmPassword" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                  Confirmation
                </label>
                <input
                  id="confirmPassword"
                  v-model="form.confirmPassword"
                  type="password"
                  required
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none placeholder-gray-600"
                  placeholder="••••••••"
                />
              </div>
            </div>
          </div>

          <!-- Terms -->
          <div class="flex items-start pt-6 border-t border-white/5">
            <input
              id="terms"
              v-model="form.acceptTerms"
              type="checkbox"
              required
              class="h-4 w-4 mt-1 bg-white/5 border-white/10 rounded text-white focus:ring-offset-[#082540]"
            />
            <label for="terms" class="ml-3 text-xs text-gray-500 leading-relaxed font-medium">
              En m'inscrivant, j'accepte les <a href="#" class="text-white hover:underline">conditions d'utilisation</a>
              et la <a href="#" class="text-white hover:underline">politique de confidentialité</a>.
            </label>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="authStore.loading || !isFormValid"
            class="w-full bg-white text-[#082540] py-5 rounded-2xl font-black text-xl hover:bg-gray-100 transition-all transform active:scale-[0.98] disabled:opacity-50 disabled:cursor-not-allowed shadow-2xl shadow-white/5"
          >
            <span v-if="!authStore.loading">Créer mon compte</span>
            <span v-else class="flex items-center justify-center">
              <svg class="animate-spin h-6 w-6 mr-2" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Calcul en cours...
            </span>
          </button>
        </form>

        <!-- Login Link -->
        <div class="mt-10 text-center text-xs text-gray-500 font-medium">
          Déjà membre ?
          <NuxtLink to="/login" class="text-white hover:underline font-bold ml-1">
            Connectez-vous ici
          </NuxtLink>
        </div>
      </div>

      <!-- Back to Home -->
      <div class="mt-10 text-center animate-fade-in">
        <NuxtLink to="/" class="text-xs text-gray-500 hover:text-white transition-colors font-bold flex items-center justify-center gap-2 uppercase tracking-widest">
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

