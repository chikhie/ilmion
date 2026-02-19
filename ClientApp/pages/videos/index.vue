<template>
  <div class="min-h-screen bg-brand-dark pt-20 pb-12 font-sans-body text-brand-parchment">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      
      <!-- Header Section -->
      <div class="text-center mb-16 relative">
          <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-32 h-32 bg-brand-gold/5 rounded-full blur-3xl -z-10"></div>
          <h1 class="text-4xl md:text-5xl font-serif-title font-bold text-brand-gold mb-6 tracking-wide drop-shadow-lg">
            Bibliothèque Vidéo
          </h1>
          <p class="text-lg md:text-xl text-brand-parchment/80 max-w-2xl mx-auto leading-relaxed">
            Explorez notre collection de contenus éducatifs et enrichissez vos connaissances.
          </p>
      </div>

      <!-- Video Grid -->
      <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        <div v-for="n in 6" :key="n" class="bg-white/5 rounded-2xl aspect-video animate-pulse border border-white/10"></div>
      </div>

      <div v-else-if="videos.length === 0" class="text-center py-20 bg-white/5 rounded-3xl border border-white/10 backdrop-blur-sm">
        <div class="inline-flex items-center justify-center w-16 h-16 rounded-full bg-brand-gold/10 text-brand-gold mb-4">
          <i class="fas fa-film text-2xl"></i>
        </div>
        <h3 class="text-xl font-bold text-white mb-2">Aucune vidéo disponible</h3>
        <p class="text-brand-parchment/60">Revenez bientôt pour découvrir du nouveau contenu.</p>
      </div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        <NuxtLink 
          v-for="video in videos" 
          :key="video" 
          :to="`/videos/${video}`"
          class="group relative bg-[#082540] rounded-2xl overflow-hidden border border-brand-gold/20 hover:border-brand-gold/50 transition-all duration-300 hover:shadow-[0_0_30px_rgba(195,151,18,0.15)] hover:-translate-y-1 block"
        >
          <!-- Thumbnail Container -->
          <div class="aspect-video bg-black/50 relative overflow-hidden">
             <!-- Placeholder Thumbnail (Using specific style for now) -->
             <div class="absolute inset-0 bg-gradient-to-br from-brand-gold/20 to-brand-dark/80 flex items-center justify-center">
                 <i class="fas fa-play-circle text-5xl text-white/80 group-hover:text-brand-gold group-hover:scale-110 transition-all duration-300 drop-shadow-lg"></i>
             </div>
             
             <!-- Overlay -->
             <div class="absolute inset-0 bg-black/20 group-hover:bg-transparent transition-colors duration-300"></div>
          </div>

          <!-- Content -->
          <div class="p-6">
            <h3 class="text-xl font-bold text-brand-parchment group-hover:text-brand-gold transition-colors mb-2 line-clamp-1">
              {{ formatVideoTitle(video) }}
            </h3>
            <div class="flex items-center text-sm text-brand-parchment/60 gap-4">
               <span class="flex items-center gap-1">
                 <i class="fas fa-clock text-xs"></i>
                 <span>--:--</span>
               </span>
               <span class="px-2 py-0.5 rounded-full bg-brand-gold/10 text-brand-gold text-xs font-bold uppercase tracking-wider border border-brand-gold/20">
                 Vidéo
               </span>
            </div>
          </div>
        </NuxtLink>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
const config = useRuntimeConfig()
const videos = ref<string[]>([])
const loading = ref(true)

// Helper to format directory name to title (e.g. "my-video" -> "My Video")
const formatVideoTitle = (id: string) => {
  return id
    .replace(/-/g, ' ')
    .replace(/_/g, ' ')
    .replace(/\b\w/g, c => c.toUpperCase())
}

onMounted(async () => {
    try {
        // Fetch videos from API
        // Assuming API base URL is configured in Nuxt config or proxy
        // Direct fetch for now, matching established patterns if useApi composable exists (it likely does based on context)
        // But for safety, using $fetch with fully qualified URL or relative if proxied.
        // Given existing code context, let's try to find useApi or similar, but for now standard fetch.
        
        // Use relative path for proxy or direct absolute if needed.
        // Assuming /api proxy is set up or base URL.
        const response = await $fetch<string[]>(`${config.public.apiBase}/api/videos`)
        videos.value = response
    } catch (e) {
        console.error('Failed to fetch videos', e)
    } finally {
        loading.value = false
    }
})

useHead({
  title: 'Vidéos',
  meta: [
    { name: 'description', content: 'Explorez notre bibliothèque de vidéos éducatives sur Ilmanar.' }
  ]
})
</script>
