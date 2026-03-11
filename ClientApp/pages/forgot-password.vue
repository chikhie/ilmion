<template>
  <div class="min-h-screen bg-[#1e1f22] flex items-center justify-center px-4 relative overflow-hidden font-sans">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <div class="absolute -top-[10%] -left-[10%] w-[500px] h-[500px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmion.svg" alt="Ilmion Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-lg" />
        <h1 class="text-2xl font-black tracking-tighter text-white uppercase mb-2">MOT DE PASSE OUBLIÉ</h1>
        <p class="text-[#00B578] font-medium">Récupération de compte</p>
      </div>

      <!-- Form -->
      <div class="bg-[#2b2d31]/80 backdrop-blur-xl rounded-3xl p-8 border border-[#00B578]/20 shadow-[0_0_30px_rgba(0,181,120,0.1)] animate-fade-in-up">
        <div v-if="!emailSent">
          <p class="text-gray-400 text-sm mb-8 leading-relaxed text-center font-medium">
            Entrez votre adresse email pour recevoir un lien de réinitialisation sécurisé.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-xl text-sm font-medium animate-shake text-center">
              {{ errorMessage }}
            </div>

            <!-- Email -->
            <div class="space-y-2">
              <label for="email" class="block text-xs font-bold uppercase tracking-widest text-gray-400 ml-1">
                Email
              </label>
              <input
                id="email"
                v-model="email"
                type="email"
                required
                class="w-full px-4 py-3.5 bg-[#1e1f22] border border-[#00B578]/20 text-white rounded-xl focus:ring-2 focus:ring-[#00B578]/50 focus:border-[#00B578] transition-all outline-none placeholder-gray-600 font-medium"
                placeholder="votre@email.com"
              />
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="authStore.loading"
              class="w-full bg-[#00B578] text-[#1e1f22] py-4 rounded-xl font-bold text-lg hover:bg-emerald-400 transition-all transform active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg shadow-[#00B578]/20 tracking-wider"
            >
              <span v-if="!authStore.loading">Envoyer le lien</span>
              <span v-else class="flex items-center justify-center">
                <i class="fas fa-circle-notch fa-spin mr-2"></i>
                Envoi...
              </span>
            </button>
          </form>

          <!-- Back to Login -->
          <div class="mt-8 text-center">
            <NuxtLink to="/login" class="text-xs text-gray-400 hover:text-[#00B578] transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
              <i class="fas fa-arrow-left"></i>
              Retour connexion
            </NuxtLink>
          </div>
        </div>

        <!-- Success Message -->
        <div v-else class="text-center py-4">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-[#00B578]/10 rounded-full flex items-center justify-center border border-[#00B578]/30">
              <i class="fas fa-check text-2xl text-[#00B578]"></i>
            </div>
          </div>

          <h3 class="text-2xl font-black tracking-tight text-white mb-3">Email envoyé !</h3>
          <p class="text-gray-400 text-sm mb-10 leading-relaxed font-medium">
            Si un compte existe, vous recevrez un lien de réinitialisation sous peu.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-[#00B578] text-[#1e1f22] py-4 rounded-xl font-bold text-lg hover:bg-emerald-400 text-center transition-all transform active:scale-95 shadow-lg shadow-[#00B578]/20 tracking-wider"
          >
            Retour connexion
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

useSeoMeta({
  title: 'Mot de passe oublié | Ilmion',
  description: 'Récupérez l\'accès à votre compte Ilmion en réinitialisant votre mot de passe.'
})

const email = ref('')
const loading = ref(false)
const errorMessage = ref('')
const emailSent = ref(false)

const authStore = useAuthStore()

const handleSubmit = async () => {
  errorMessage.value = ''

  const result = await authStore.forgotPassword(email.value)
  
  if (result.success) {
    emailSent.value = true
  } else {
    errorMessage.value = result.message || 'Erreur lors de l\'envoi de l\'email'
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
