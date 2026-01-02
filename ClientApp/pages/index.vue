<template>
  <div class="min-h-screen bg-brand-dark overflow-hidden relative">
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 opacity-20 pointer-events-none">
        <div class="absolute top-0 left-0 w-full h-full bg-[url('https://www.transparenttextures.com/patterns/arabesque.png')]"></div>
    </div>
    
    <!-- Hero Section -->
    <div class="relative z-10 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-[calc(100vh-80px)] flex flex-col justify-center items-center text-center">
        
        <p class="text-xl sm:text-2xl text-brand-wood font-libre italic mb-10 max-w-2xl leading-relaxed">
          "La lumière du savoir éclaire le chemin de la sagesse"
        </p>

        <!-- Main Actions Grid -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 w-full max-w-5xl">
            
            <!-- Jeux -->
            <NuxtLink to="/games" class="group relative h-48 md:h-64 rounded-3xl overflow-hidden shadow-2xl transition-all duration-300 hover:scale-[1.02] border border-brand-gold/20">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1557683316-973673baf926?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-t from-brand-dark via-brand-dark/50 to-transparent"></div>
                <div class="absolute bottom-0 left-0 p-6">
                    <h3 class="text-2xl font-serif-title font-bold text-brand-parchment mb-1 group-hover:text-brand-gold transition-colors">Jeux</h3>
                    <p class="text-sm text-brand-wood opacity-80">Explorez & Apprenez</p>
                </div>
            </NuxtLink>

            <!-- Magazine -->
            <NuxtLink to="/magazine" class="group relative h-48 md:h-64 rounded-3xl overflow-hidden shadow-2xl transition-all duration-300 hover:scale-[1.02] border border-brand-gold/20">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1457369804613-52c61a468e7d?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-t from-brand-dark via-brand-dark/50 to-transparent"></div>
                <div class="absolute bottom-0 left-0 p-6">
                    <h3 class="text-2xl font-serif-title font-bold text-brand-parchment mb-1 group-hover:text-brand-gold transition-colors">Magazine</h3>
                    <p class="text-sm text-brand-wood opacity-80">Lisez & Découvrez</p>
                </div>
            </NuxtLink>

            <!-- Vidéos -->
            <NuxtLink to="/videos" class="group relative h-48 md:h-64 rounded-3xl overflow-hidden shadow-2xl transition-all duration-300 hover:scale-[1.02] border border-brand-gold/20">
                <div class="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1536440136628-849c177e76a1?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80')] bg-cover bg-center transition-transform duration-700 group-hover:scale-110 opacity-60 group-hover:opacity-40"></div>
                <div class="absolute inset-0 bg-gradient-to-t from-brand-dark via-brand-dark/50 to-transparent"></div>
                <div class="absolute bottom-0 left-0 p-6">
                    <h3 class="text-2xl font-serif-title font-bold text-brand-parchment mb-1 group-hover:text-brand-gold transition-colors">Vidéos</h3>
                    <p class="text-sm text-brand-wood opacity-80">Regardez & Comprenez</p>
                </div>
            </NuxtLink>

        </div>
    </div>

    <!-- PWA Debug/Install Section -->
    <div class="fixed bottom-4 right-4 z-50 flex flex-col gap-2 items-end">
        <!-- Debug Info (Temporary) -->
        <div class="bg-black/80 text-white p-4 rounded-lg text-xs max-w-xs mb-2">
            <p>Debug PWA:</p>
            <p>Prompt fired: {{ installPrompt ? 'Yes' : 'No' }}</p>
            <p>SW Active: {{ isServiceWorkerActive ? 'Yes' : 'No' }}</p>
            <p>Manifest Link: {{ hasManifest ? 'Found' : 'Missing' }}</p>
        </div>

        <button v-if="installPrompt" @click="installPWA" class="animate-bounce-slow flex items-center gap-3 bg-brand-gold text-brand-dark px-6 py-4 rounded-full font-bold shadow-2xl hover:bg-white hover:scale-105 transition-all">
             <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
             </svg>
             <span>Installer l'application</span>
         </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  const installPrompt = ref<any>(null)
  const isServiceWorkerActive = ref(false)
  const hasManifest = ref(false)

  onMounted(async () => {
    // Check Manifest
    hasManifest.value = !!document.querySelector('link[rel="manifest"]')

    // Check SW
    if ('serviceWorker' in navigator) {
        const registration = await navigator.serviceWorker.getRegistration()
        isServiceWorkerActive.value = !!registration?.active
        
        navigator.serviceWorker.ready.then(() => {
            isServiceWorkerActive.value = true
            console.log('PWA: Service Worker Ready')
        })
    }

    window.addEventListener('beforeinstallprompt', (e) => {
      console.log('PWA: beforeinstallprompt fired', e);
      e.preventDefault()
      installPrompt.value = e
    })
    console.log('PWA: Listener added');
  })

  const installPWA = async () => {
      if (!installPrompt.value) return
      installPrompt.value.prompt()
      const { outcome } = await installPrompt.value.userChoice
      if (outcome === 'accepted') {
          installPrompt.value = null
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
