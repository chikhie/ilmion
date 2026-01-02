<template>
  <div class="min-h-screen bg-brand-dark overflow-hidden relative">
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 opacity-20 pointer-events-none">
        <div class="absolute top-0 left-0 w-full h-full bg-[url('https://www.transparenttextures.com/patterns/arabesque.png')]"></div>
    </div>
    
    <!-- Hero Section -->
    <div class="relative z-10 w-full h-full flex flex-col justify-between items-center text-center py-6 px-4 safe-area-inset-bottom">
      
        <!-- Main Actions Grid -->
        <div class="grid grid-cols-1 gap-3 w-full max-w-md flex-grow justify-center content-center pb-20 md:pb-0 md:grid-cols-3 md:gap-6 md:max-w-5xl md:flex-grow-0">
            
            <!-- Jeux -->
            <NuxtLink to="/games" class="group relative rounded-2xl overflow-hidden shadow-xl border border-brand-gold/20 flex-1 min-h-[100px] md:h-64 md:min-h-auto">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1557683316-973673baf926?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-l from-brand-dark/80 via-brand-dark/40 to-transparent md:bg-gradient-to-t"></div>
                <div class="absolute inset-0 p-4 flex items-center md:items-end md:p-6">
                    <div class="text-left">
                        <h3 class="text-xl md:text-2xl font-serif-title font-bold text-brand-parchment group-hover:text-brand-gold transition-colors">Jeux</h3>
                        <p class="text-xs md:text-sm text-brand-wood opacity-90">Explorez & Apprenez</p>
                    </div>
                </div>
            </NuxtLink>

            <!-- Magazine -->
            <NuxtLink to="/magazine" class="group relative rounded-2xl overflow-hidden shadow-xl border border-brand-gold/20 flex-1 min-h-[100px] md:h-64 md:min-h-auto">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1457369804613-52c61a468e7d?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-l from-brand-dark/80 via-brand-dark/40 to-transparent md:bg-gradient-to-t"></div>
                <div class="absolute inset-0 p-4 flex items-center md:items-end md:p-6">
                    <div class="text-left">
                        <h3 class="text-xl md:text-2xl font-serif-title font-bold text-brand-parchment group-hover:text-brand-gold transition-colors">Magazine</h3>
                        <p class="text-xs md:text-sm text-brand-wood opacity-90">Lisez & Découvrez</p>
                    </div>
                </div>
            </NuxtLink>

            <!-- Vidéos -->
            <NuxtLink to="/videos" class="group relative rounded-2xl overflow-hidden shadow-xl border border-brand-gold/20 flex-1 min-h-[100px] md:h-64 md:min-h-auto">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1536440136628-849c177e76a1?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-l from-brand-dark/80 via-brand-dark/40 to-transparent md:bg-gradient-to-t"></div>
                <div class="absolute inset-0 p-4 flex items-center md:items-end md:p-6">
                    <div class="text-left">
                        <h3 class="text-xl md:text-2xl font-serif-title font-bold text-brand-parchment group-hover:text-brand-gold transition-colors">Vidéos</h3>
                        <p class="text-xs md:text-sm text-brand-wood opacity-90">Regardez & Comprenez</p>
                    </div>
                </div>
            </NuxtLink>

        </div>
    </div>

    <!-- PWA Install Button -->
    <client-only>
      <div v-if="!isInstalled" class="fixed bottom-24 right-4 md:bottom-8 md:right-8 z-50 animate-bounce-slow">
           <button @click="handleInstallClick" class="flex items-center gap-3 bg-brand-gold text-brand-dark px-6 py-4 rounded-full font-bold shadow-2xl hover:bg-white hover:scale-105 transition-all">
               <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                 <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
               </svg>
               <span>Installer l'application</span>
           </button>
      </div>

      <!-- Installation Help Modal -->
      <div v-if="showInstallHelp" class="fixed inset-0 z-[60] flex items-center justify-center bg-black/50 p-4" @click="showInstallHelp = false">
        <div class="bg-brand-parchment p-6 rounded-2xl max-w-sm w-full shadow-2xl relative" @click.stop>
          <button @click="showInstallHelp = false" class="absolute top-2 right-2 text-brand-wood hover:text-brand-dark">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
          
          <h3 class="text-xl font-serif-title font-bold text-brand-dark mb-4">Installer Ilmanar</h3>
          
          <div v-if="isIOS" class="space-y-3 text-brand-wood">
            <p>Pour installer sur iOS :</p>
            <ol class="list-decimal list-inside space-y-2 text-sm">
              <li>Appuyez sur le bouton <strong>Partager</strong> <svg class="inline h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z" /></svg></li>
              <li>Faites défiler et choisissez <strong>"Sur l'écran d'accueil"</strong></li>
            </ol>
          </div>
          
          <div v-else class="space-y-3 text-brand-wood">
            <p>Si l'installation rapide ne fonctionne pas :</p>
            <p class="text-sm">Cherchez l'icône d'installation <svg class="inline h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" /></svg> dans la barre d'adresse de votre navigateur.</p>
          </div>
        </div>
      </div>
    </client-only>
  </div>
</template>

<script setup lang="ts">
  const installPrompt = ref<any>(null)
  const isInstalled = ref(false)
  const showInstallHelp = ref(false)
  const isIOS = ref(false)

  onMounted(() => {
    // Check if already installed
    if (window.matchMedia('(display-mode: standalone)').matches) {
      isInstalled.value = true
    }

    // Check device type
    const userAgent = window.navigator.userAgent.toLowerCase()
    isIOS.value = /iphone|ipad|ipod/.test(userAgent)

    // Listen for install prompt
    window.addEventListener('beforeinstallprompt', (e) => {
      e.preventDefault()
      installPrompt.value = e
    })
  })

  const handleInstallClick = async () => {
      if (installPrompt.value) {
        // Native install
        installPrompt.value.prompt()
        const { outcome } = await installPrompt.value.userChoice
        if (outcome === 'accepted') {
            installPrompt.value = null
        }
      } else {
        // Show help manual
        showInstallHelp.value = true
      }
  }

  definePageMeta({
    layout: 'default' 
  })
</script>

<style scoped>
.animate-bounce-slow {
    animation: bounce 3s infinite;
}
</style>
