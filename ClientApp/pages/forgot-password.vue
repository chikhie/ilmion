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
        <h1 class="text-2xl font-black tracking-tighter text-white uppercase mb-2">MOT DE PASSE OUBLIÉ</h1>
        <p class="text-gray-400 font-medium">Récupération de compte</p>
      </div>

      <!-- Form -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl p-8 border border-white/10 shadow-2xl animate-fade-in-up">
        <div v-if="!emailSent">
          <p class="text-gray-400 text-sm mb-8 leading-relaxed text-center font-medium">
            Entrez votre adresse email pour recevoir un lien de réinitialisation sécurisé.
          </p>

          <form @submit.prevent="handleSubmit" class="space-y-6">
            <!-- Alert -->
            <div v-if="errorMessage" class="bg-red-500/10 border border-red-500/20 text-red-400 px-4 py-3 rounded-2xl text-sm text-center">
              {{ errorMessage }}
            </div>

            <!-- Email -->
            <div class="space-y-2">
              <label for="email" class="block text-xs font-bold uppercase tracking-widest text-gray-500 ml-1">
                Email
              </label>
              <input
                id="email"
                v-model="email"
                type="email"
                required
                class="w-full px-5 py-4 bg-white/5 border border-white/10 text-white rounded-2xl focus:ring-2 focus:ring-white/20 transition-all outline-none placeholder-gray-600"
                placeholder="votre@email.com"
              />
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="authStore.loading"
              class="w-full bg-white text-[#082540] py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all transform active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed shadow-xl shadow-white/5"
            >
              <span v-if="!authStore.loading">Envoyer le lien</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin h-5 w-5 mr-2" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Envoi...
              </span>
            </button>
          </form>

          <!-- Back to Login -->
          <div class="mt-8 text-center">
            <NuxtLink to="/login" class="text-xs text-gray-500 hover:text-white transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
              Retour
            </NuxtLink>
          </div>
        </div>

        <!-- Success Message -->
        <div v-else class="text-center py-4">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-white/10 rounded-full flex items-center justify-center border border-white/10">
              <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"></path>
              </svg>
            </div>
          </div>

          <h3 class="text-xl font-bold text-white mb-3">Email envoyé !</h3>
          <p class="text-gray-400 text-sm mb-10 leading-relaxed font-medium">
            Si un compte existe, vous recevrez un lien de réinitialisation sous peu.
          </p>

          <NuxtLink
            to="/login"
            class="inline-block bg-white text-[#082540] px-8 py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all shadow-xl shadow-white/5"
          >
            Retour à la connexion
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
  title: 'Mot de passe oublié'
})

const email = ref('')
const loading = ref(false)
const errorMessage = ref('')
const emailSent = ref(false)

const authStore = useAuthStore()

const handleSubmit = async () => {
  errorMessage.value = ''
  // loading.value = true // Handled by store

  const result = await authStore.forgotPassword(email.value)
  
  if (result.success) {
    emailSent.value = true
  } else {
    errorMessage.value = result.message || 'Erreur lors de l\'envoi de l\'email'
  }
}
</script>

