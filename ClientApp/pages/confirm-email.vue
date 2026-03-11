<template>
  <div class="min-h-screen bg-[#1e1f22] flex items-center justify-center px-4 py-8 sm:py-0 relative overflow-hidden font-sans">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <!-- Glow Effects -->
        <div class="absolute top-[-10%] left-[-10%] w-[500px] h-[500px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/5 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <div class="max-w-md w-full relative z-10">
      <!-- Logo/Header -->
      <div class="text-center mb-10 animate-fade-in">
        <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-20 w-20 mx-auto mb-6 drop-shadow-lg" />
        <h1 class="text-2xl sm:text-3xl font-black text-white uppercase mb-2 tracking-tighter">Confirmation Email</h1>
        <p class="text-[#00B578] font-medium">Activation de votre compte</p>
      </div>

      <!-- Card -->
      <div class="bg-[#2b2d31]/80 backdrop-blur-xl rounded-3xl p-8 border border-[#00B578]/20 shadow-[0_0_30px_rgba(0,181,120,0.1)] animate-fade-in-up">
        
        <!-- Loading -->
        <div v-if="confirming" class="py-8 text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-4 border-[#00B578]/20 border-t-[#00B578] mb-6 shadow-lg shadow-[#00B578]/20"></div>
          <h2 class="text-xl font-black text-white mb-2 uppercase tracking-tight">Vérification en cours...</h2>
          <p class="text-gray-400 font-medium">Nous validons votre compte sur le réseau</p>
        </div>

        <!-- Success -->
        <div v-else-if="confirmed" class="py-4 text-center">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-[#00B578]/10 rounded-full flex items-center justify-center border border-[#00B578]/30 shadow-lg shadow-[#00B578]/20">
              <i class="fas fa-check text-2xl text-[#00B578]"></i>
            </div>
          </div>

          <h2 class="text-2xl font-black text-white mb-4 uppercase tracking-tighter">Compte activé !</h2>
          <p class="text-gray-400 mb-10 leading-relaxed font-medium">
            Votre adresse email a été confirmée avec succès.
          </p>

          <NuxtLink
            to="/login"
            class="block w-full bg-[#00B578] text-[#1e1f22] py-4 rounded-xl font-bold text-lg hover:bg-emerald-400 transition-all transform active:scale-95 text-center shadow-lg shadow-[#00B578]/20 tracking-wider"
          >
            SE CONNECTER
          </NuxtLink>
        </div>

        <!-- Error -->
        <div v-else class="py-4 text-center">
          <div class="mb-8">
            <div class="mx-auto w-16 h-16 bg-red-500/10 rounded-full flex items-center justify-center border border-red-500/20 shadow-lg shadow-red-500/10">
              <i class="fas fa-exclamation-triangle text-2xl text-red-400"></i>
            </div>
          </div>

          <h2 class="text-2xl font-black text-white mb-4 tracking-tighter uppercase">Échec de validation</h2>
          <p class="text-red-400 mb-6 font-bold bg-red-500/10 py-3 px-4 rounded-xl border border-red-500/20 text-sm">
            {{ errorMessage }}
          </p>
          <p class="text-gray-500 mb-8 text-xs font-bold uppercase tracking-widest">
            Le lien de confirmation est peut-être expiré ou invalide.
          </p>

          <div class="space-y-4">
            <button
              @click="retryConfirmation"
              class="w-full bg-[#1e1f22] text-[#00B578] border border-[#00B578]/30 py-4 rounded-xl font-bold text-lg hover:bg-[#00B578]/10 hover:border-[#00B578]/50 transition-all transform active:scale-95"
            >
              RÉESSAYER
            </button>

            <NuxtLink
              to="/login"
              class="block text-xs text-gray-500 hover:text-[#00B578] transition-colors font-bold uppercase tracking-widest"
            >
              Retour à la connexion
            </NuxtLink>
          </div>
        </div>

        <!-- Footer -->
        <div class="mt-8 pt-6 border-t border-white/5 text-center">
          <NuxtLink to="/" class="text-xs text-gray-400 hover:text-[#00B578] transition-colors font-bold uppercase tracking-widest flex items-center justify-center gap-2">
            <i class="fas fa-home"></i>
            Accueil réseau
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
.animate-fade-in { animation: fadeIn 0.8s ease-out forwards; opacity: 0; }
.animate-fade-in-up { animation: fadeInUp 0.8s ease-out forwards; opacity: 0; }
.delay-1000 { animation-delay: 1s; }
.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }

@keyframes fadeIn { to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.2; } }
</style>
