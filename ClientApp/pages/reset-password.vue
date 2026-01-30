<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 relative overflow-hidden font-sans">
    <!-- Background Gradients -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-2xl" />
        <h1 class="text-2xl font-black tracking-tighter text-white uppercase mb-2">Nouveau mot de passe</h1>
        <p class="text-gray-400 font-medium">Sécurisez votre accès à Ilmanar</p>
      </div>

      <!-- Card -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl p-8 border border-white/10 shadow-2xl animate-fade-in-up">
        <div v-if="!passwordReset">
          <p class="text-gray-300 mb-8 text-center font-medium leading-relaxed">
            Choisissez un nouveau mot de passe pour reprendre votre apprentissage.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-2xl text-sm font-medium animate-shake">
              {{ errorMessage }}
            </div>

            <!-- New Password -->
            <div class="space-y-2">
              <label for="password" class="block text-xs font-bold text-gray-400 uppercase tracking-widest ml-1">
                Nouveau mot de passe
              </label>
              <div class="relative group">
                <input
                  id="password"
                  v-model="form.password"
                  :type="showPassword ? 'text' : 'password'"
                  required
                  minlength="6"
                  class="w-full bg-white/5 border border-white/10 rounded-2xl px-5 py-4 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white/20 focus:border-transparent transition-all group-hover:bg-white/10 font-medium"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  @click="showPassword = !showPassword"
                  class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-white transition-colors"
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
              <p class="mt-1 text-[10px] text-gray-500 font-bold uppercase tracking-widest ml-1">Minimum 6 caractères</p>
            </div>

            <!-- Confirm Password -->
            <div class="space-y-2">
              <label for="confirmPassword" class="block text-xs font-bold text-gray-400 uppercase tracking-widest ml-1">
                Confirmer le mot de passe
              </label>
              <div class="relative group">
                <input
                  id="confirmPassword"
                  v-model="form.confirmPassword"
                  :type="showConfirmPassword ? 'text' : 'password'"
                  required
                  class="w-full bg-white/5 border border-white/10 rounded-2xl px-5 py-4 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white/20 focus:border-transparent transition-all group-hover:bg-white/10 font-medium"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  @click="showConfirmPassword = !showConfirmPassword"
                  class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-white transition-colors"
                >
                  <svg v-if="!showConfirmPassword" class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                  </svg>
                  <svg v-else class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.542-7 1.274-4.057 5.064-7 9.542-7 1.225 0 2.39.22 3.46.612M15 12a3 3 0 11-6 0 3 3 0 016 0zm-9 9l12-12" />
                  </svg>
                </button>
              </div>
            </div>

            <!-- Submit Button -->
            <div class="pt-4">
              <button
                type="submit"
                :disabled="authStore.loading || !isFormValid"
                class="w-full bg-white text-[#082540] py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all transform active:scale-95 shadow-xl shadow-white/5 disabled:opacity-30 disabled:cursor-not-allowed disabled:transform-none"
              >
                <span v-if="!authStore.loading">RÉINITIALISER</span>
                <span v-else class="flex items-center justify-center">
                  <svg class="animate-spin h-5 w-5 mr-3" viewBox="0 0 24 24">
                    <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                    <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  TRAITEMENT...
                </span>
              </button>
            </div>
          </form>
          
          <!-- Debug Info (Temporary) -->
          <div class="mt-4 p-4 bg-black/50 rounded-xl text-xs font-mono text-gray-300">
            <p>Pass: {{ form.password?.length }} chars</p>
            <p>Confirm: {{ form.confirmPassword?.length }} chars</p>
            <p>Match: {{ form.password === form.confirmPassword }}</p>
            <p>Valid: {{ isFormValid }}</p>
            <p>Loading: {{ authStore.loading }}</p>
          </div>
        </div>

        <!-- Success Message -->
        <div v-else class="text-center py-6">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-white/10 rounded-full flex items-center justify-center border border-white/10">
              <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
              </svg>
            </div>
          </div>

          <h3 class="text-2xl font-black text-white mb-4 uppercase tracking-tight">C'est fait !</h3>
          <p class="text-gray-400 mb-10 leading-relaxed font-medium">
            Votre mot de passe a été réinitialisé avec succès. Vous pouvez maintenant vous reconnecter à votre espace.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-white text-[#082540] py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all transform active:scale-95"
          >
            SE CONNECTER
          </NuxtLink>
        </div>

        <!-- Footer -->
        <div class="mt-10 pt-8 border-t border-white/5 text-center">
          <NuxtLink to="/login" class="text-xs text-gray-500 hover:text-white transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
            Retour connexion
          </NuxtLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: false
})

useHead({
  title: 'Réinitialiser le mot de passe'
})

const authStore = useAuthStore()
const route = useRoute()
const router = useRouter()

// Extract token and email from URL query params (sent via reset password email link)
const token = computed(() => route.query.token as string || '')
const email = computed(() => route.query.email as string || '')

const form = ref({
  password: '',
  confirmPassword: ''
})

const errorMessage = ref('')
const passwordReset = ref(false)
const showPassword = ref(false)
const showConfirmPassword = ref(false)

const isFormValid = computed(() => {
  return (
    form.value.password.length >= 6 &&
    form.value.password === form.value.confirmPassword
  )
})

onMounted(() => {
  // Supabase handles the session via the hash fragment from the email link.
  // If the user is not authenticated, it means the link is invalid or expired.
  // We can check user.value or wait for auth state change if needed.
  // For now, let's rely on the middleware or the fact that updateUser requires a session.
  
  // Optional: Redirect if no session?
  // if (!user.value) {
  //   errorMessage.value = 'Session invalide ou expirée. Veuillez redemander un lien.'
  // }
})

const handleSubmit = async () => {
  if (!isFormValid.value) return

  errorMessage.value = ''
  // loading state handled by store

  const result = await authStore.resetPassword(form.value.password, token.value, email.value)

  if (result.success) {
    passwordReset.value = true
    // Optional: Redirect after a few seconds?
  } else {
    errorMessage.value = result.message || 'Erreur lors de la réinitialisation du mot de passe'
  }
}
</script>

