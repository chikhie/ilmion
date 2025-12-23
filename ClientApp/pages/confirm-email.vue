<template>
  <div class="min-h-screen bg-[#082540] flex items-center justify-center px-4 relative overflow-hidden font-sans">
    <!-- Background Gradients -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-2xl" />
        <h1 class="text-2xl font-black tracking-tighter text-white uppercase mb-2">Confirmation Email</h1>
      </div>

      <!-- Card -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl p-8 border border-white/10 shadow-2xl animate-fade-in-up">
        <!-- Loading -->
        <div v-if="confirming" class="py-8 text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-4 border-white/10 border-t-white mb-6"></div>
          <h2 class="text-xl font-bold text-white mb-2 uppercase tracking-tight">Vérification en cours...</h2>
          <p class="text-gray-400 font-medium">Nous validons votre accès à Ilmanar</p>
        </div>

        <!-- Success -->
        <div v-else-if="confirmed" class="py-4 text-center">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-white/10 rounded-full flex items-center justify-center border border-white/10">
              <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
              </svg>
            </div>
          </div>

          <h2 class="text-2xl font-black text-white mb-4 uppercase tracking-tight">Compte activé !</h2>
          <p class="text-gray-400 mb-10 leading-relaxed font-medium">
            Votre adresse email a été confirmée avec succès. Vous faites maintenant partie de l'aventure Ilmanar.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-white text-[#082540] py-4 rounded-2xl font-bold text-lg hover:bg-gray-100 transition-all transform active:scale-95 shadow-xl shadow-white/5"
          >
            Se connecter
          </NuxtLink>
        </div>

        <!-- Error -->
        <div v-else class="py-4 text-center">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-red-500/10 rounded-full flex items-center justify-center border border-red-500/20">
              <svg class="w-8 h-8 text-red-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
              </svg>
            </div>
          </div>

          <h2 class="text-2xl font-black text-white mb-4 uppercase tracking-tight">Échec de validation</h2>
          <p class="text-red-400 mb-6 font-bold bg-red-500/10 py-3 px-4 rounded-xl border border-red-500/20 text-sm">
            {{ errorMessage }}
          </p>
          <p class="text-gray-500 mb-8 text-xs font-medium uppercase tracking-widest">
            Le lien de confirmation est peut-être expiré ou invalide.
          </p>

          <div class="space-y-4">
            <button
              @click="retryConfirmation"
              class="w-full bg-white/10 text-white border border-white/10 py-4 rounded-2xl font-bold text-lg hover:bg-white/20 transition-all transform active:scale-95"
            >
              Réessayer
            </button>

            <NuxtLink
              to="/login"
              class="block text-xs text-gray-400 hover:text-white transition-colors font-bold uppercase tracking-widest"
            >
              Retour à la connexion
            </NuxtLink>
          </div>
        </div>

        <!-- Footer -->
        <div class="mt-10 pt-8 border-t border-white/5 text-center">
          <NuxtLink to="/" class="text-xs text-gray-500 hover:text-white transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
            Accueil
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
  title: 'Confirmation d\'email'
})

const route = useRoute()
const config = useRuntimeConfig()

const confirming = ref(true)
const confirmed = ref(false)
const errorMessage = ref('')

const userId = route.query.userId as string
const token = route.query.token as string

const confirmEmail = async () => {
  if (!userId || !token) {
    confirming.value = false
    errorMessage.value = 'Paramètres manquants dans le lien de confirmation'
    return
  }

  try {
    const response = await $fetch(`${config.public.apiBase}/auth/confirm-email?userId=${userId}&token=${token}`, {
      method: 'GET'
    })

    confirmed.value = true
  } catch (error: any) {
    confirmed.value = false
    errorMessage.value = error.data?.message || error.message || 'Erreur lors de la confirmation de l\'email'
    console.error('Erreur confirmation:', error)
  } finally {
    confirming.value = false
  }
}

const retryConfirmation = () => {
  confirming.value = true
  errorMessage.value = ''
  confirmEmail()
}

// Confirmation automatique au chargement
onMounted(() => {
  confirmEmail()
})
</script>

