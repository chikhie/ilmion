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

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-8 animate-fade-in">
        <div class="mb-6 inline-block p-4 rounded-full border border-brand-gold/20 bg-brand-wood/10 backdrop-blur-sm shadow-[0_0_30px_rgba(195,151,18,0.1)]">
             <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-16 w-16 drop-shadow-2xl" />
        </div>
        <h1 class="text-3xl sm:text-4xl font-serif-title font-bold text-brand-parchment mb-2 drop-shadow-lg tracking-wide">Confirmation Email</h1>
      </div>

      <!-- Card -->
      <div class="bg-brand-wood/10 backdrop-blur-xl rounded-3xl p-8 border border-brand-gold/20 shadow-2xl animate-fade-in-up">
        <!-- Loading -->
        <div v-if="confirming" class="py-8 text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-4 border-brand-gold/20 border-t-brand-gold mb-6"></div>
          <h2 class="text-xl font-bold text-brand-parchment mb-2 uppercase tracking-tight">Vérification en cours...</h2>
          <p class="text-brand-parchment/60 font-medium">Nous validons votre compte</p>
        </div>

        <!-- Success -->
        <div v-else-if="confirmed" class="py-4 text-center">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-brand-gold/20 rounded-full flex items-center justify-center border border-brand-gold/30">
              <svg class="w-8 h-8 text-brand-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
              </svg>
            </div>
          </div>

          <h2 class="text-2xl font-serif-title font-bold text-brand-parchment mb-4 tracking-wide">Compte activé !</h2>
          <p class="text-brand-parchment/70 mb-10 leading-relaxed">
            Votre adresse email a été confirmée avec succès.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-brand-gold text-brand-dark py-4 rounded-2xl font-bold text-lg hover:shadow-[0_0_20px_rgba(195,151,18,0.4)] transition-all transform active:scale-95 uppercase tracking-wider"
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

          <h2 class="text-2xl font-serif-title font-bold text-brand-parchment mb-4 tracking-wide">Échec de validation</h2>
          <p class="text-red-400 mb-6 font-medium bg-red-500/10 py-3 px-4 rounded-xl border border-red-500/20 text-sm">
            {{ errorMessage }}
          </p>
          <p class="text-brand-parchment/50 mb-8 text-xs font-medium uppercase tracking-widest">
            Le lien de confirmation est peut-être expiré ou invalide.
          </p>

          <div class="space-y-4">
            <button
              @click="retryConfirmation"
              class="w-full bg-brand-wood/20 text-brand-parchment border border-brand-gold/30 py-4 rounded-2xl font-bold text-lg hover:bg-brand-wood/30 hover:border-brand-gold/50 transition-all transform active:scale-95"
            >
              Réessayer
            </button>

            <NuxtLink
              to="/login"
              class="block text-xs text-brand-parchment/50 hover:text-brand-gold transition-colors font-bold uppercase tracking-widest"
            >
              Retour à la connexion
            </NuxtLink>
          </div>
        </div>

        <!-- Footer -->
        <div class="mt-10 pt-8 border-t border-brand-gold/10 text-center">
          <NuxtLink to="/" class="text-xs text-brand-parchment/50 hover:text-brand-parchment transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
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

useSeoMeta({
  title: 'Confirmation d\'email | Ilmanar',
  description: 'Confirmez votre adresse email pour activer votre compte Ilmanar.'
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

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.8s ease-out forwards;
  opacity: 0;
}

.animate-fade-in-up {
    animation: fadeInUp 0.8s ease-out forwards;
    opacity: 0;
}

.delay-1000 { animation-delay: 1s; }

.animate-pulse-slow {
    animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes fadeIn {
  to { opacity: 1; }
}

@keyframes fadeInUp {
    from { opacity: 0; transform: translateY(20px); }
    to { opacity: 1; transform: translateY(0); }
}

@keyframes pulse {
  0%, 100% { opacity: 0.1; }
  50% { opacity: 0.2; }
}
</style>
