<template>
  <div class="min-h-screen bg-[#1e1f22] flex items-center justify-center px-4 relative overflow-hidden font-sans">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <div class="absolute top-[10%] left-[-10%] w-[500px] h-[500px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-lg" />
        <h1 class="text-2xl font-black tracking-tighter text-white uppercase mb-2">Nouveau mot de passe</h1>
        <p class="text-[#00B578] font-medium">Sécurisez votre accès à Ilmion</p>
      </div>

      <!-- Card -->
      <div class="bg-[#2b2d31]/80 backdrop-blur-xl rounded-3xl p-8 border border-[#00B578]/20 shadow-[0_0_30px_rgba(0,181,120,0.1)] animate-fade-in-up">
        <div v-if="!passwordReset">
          <p class="text-gray-300 mb-8 text-center font-medium leading-relaxed">
            Choisissez un nouveau mot de passe pour reprendre votre apprentissage.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-xl text-sm font-medium animate-shake text-center">
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
                  class="w-full bg-[#1e1f22] border border-[#00B578]/20 rounded-xl px-4 py-3.5 text-white placeholder-gray-600 focus:outline-none focus:ring-2 focus:ring-[#00B578]/50 focus:border-[#00B578] transition-all group-hover:border-[#00B578]/40 font-medium"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  @click="showPassword = !showPassword"
                  class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-[#00B578] transition-colors"
                >
                  <i :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
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
                  class="w-full bg-[#1e1f22] border border-[#00B578]/20 rounded-xl px-4 py-3.5 text-white placeholder-gray-600 focus:outline-none focus:ring-2 focus:ring-[#00B578]/50 focus:border-[#00B578] transition-all group-hover:border-[#00B578]/40 font-medium"
                  placeholder="••••••••"
                />
                <button
                  type="button"
                  @click="showConfirmPassword = !showConfirmPassword"
                  class="absolute right-4 top-1/2 -translate-y-1/2 text-gray-500 hover:text-[#00B578] transition-colors"
                >
                  <i :class="showConfirmPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
                </button>
              </div>
            </div>

            <!-- Submit Button -->
            <div class="pt-4">
              <button
                type="submit"
                :disabled="authStore.loading || !isFormValid"
                class="w-full bg-[#00B578] text-[#1e1f22] py-4 rounded-xl font-bold text-lg hover:bg-emerald-400 transition-all transform active:scale-95 shadow-lg shadow-[#00B578]/20 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
              >
                <span v-if="!authStore.loading">RÉINITIALISER</span>
                <span v-else class="flex items-center justify-center">
                  <i class="fas fa-circle-notch fa-spin mr-2"></i>
                  TRAITEMENT...
                </span>
              </button>
            </div>
          </form>
          
        </div>

        <!-- Success Message -->
        <div v-else class="text-center py-6">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-[#00B578]/10 rounded-full flex items-center justify-center border border-[#00B578]/30">
              <i class="fas fa-check text-2xl text-[#00B578]"></i>
            </div>
          </div>

          <h3 class="text-2xl font-black text-white mb-4 uppercase tracking-tight">C'est fait !</h3>
          <p class="text-gray-400 mb-10 leading-relaxed font-medium">
            Votre mot de passe a été réinitialisé avec succès. Vous pouvez maintenant vous reconnecter à votre espace.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-[#00B578] text-[#1e1f22] py-4 rounded-xl font-bold text-lg hover:bg-emerald-400 transition-all transform active:scale-95 text-center shadow-lg shadow-[#00B578]/20 tracking-wider"
          >
            SE CONNECTER
          </NuxtLink>
        </div>

        <!-- Footer -->
        <div class="mt-8 pt-6 border-t border-white/5 text-center">
          <NuxtLink to="/login" class="text-xs text-gray-400 hover:text-[#00B578] transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
            <i class="fas fa-arrow-left"></i>
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

useSeoMeta({
  title: 'Réinitialiser le mot de passe | Ilmanar',
  description: 'Choisissez un nouveau mot de passe pour votre compte Ilmanar.'
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
  // Check auth state logic here...
})

const handleSubmit = async () => {
  if (!isFormValid.value) return

  errorMessage.value = ''
  // loading state handled by store

  const result = await authStore.resetPassword(form.value.password, token.value, email.value)

  if (result.success) {
    passwordReset.value = true
  } else {
    errorMessage.value = result.message || 'Erreur lors de la réinitialisation du mot de passe'
  }
}
</script>

<style scoped>
.animate-fade-in { animation: fadeIn 0.8s ease-out forwards; opacity: 0; }
.animate-fade-in-up { animation: fadeInUp 0.8s ease-out forwards; opacity: 0; }
.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
.animate-shake { animation: shake 0.5s cubic-bezier(.36,.07,.19,.97) both; }

@keyframes fadeIn { to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.2; } }
@keyframes shake {
  10%, 90% { transform: translate3d(-1px, 0, 0); }
  20%, 80% { transform: translate3d(2px, 0, 0); }
  30%, 50%, 70% { transform: translate3d(-4px, 0, 0); }
  40%, 60% { transform: translate3d(4px, 0, 0); }
}
</style>
