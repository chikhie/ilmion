<template>
  <div class="min-h-screen bg-brand-dark pt-20 pb-12 font-sans-body text-brand-parchment flex flex-col items-center">
    <div class="w-full max-w-6xl px-4 sm:px-6 lg:px-8">
      
      <!-- Back Button -->
      <NuxtLink to="/videos" class="inline-flex items-center gap-2 text-brand-gold hover:text-white mb-8 transition-colors group">
        <i class="fas fa-arrow-left group-hover:-translate-x-1 transition-transform"></i>
        <span>Retour à la bibliothèque</span>
      </NuxtLink>

      <!-- Video Player Container -->
      <div class="bg-black rounded-3xl overflow-hidden shadow-[0_0_50px_rgba(0,0,0,0.5)] border border-brand-gold/20 aspect-video relative group">
         <video 
           ref="videoPlayer" 
           class="w-full h-full object-contain" 
           controls 
           preload="auto"
         >
           Votre navigateur ne supporte pas la lecture de vidéos.
         </video>
      </div>

      <!-- Video Details -->
      <div class="mt-8">
        <h1 class="text-3xl md:text-4xl font-serif-title font-bold text-brand-gold mb-4">{{ videoTitle }}</h1>
        <p class="text-brand-parchment/80 leading-relaxed text-lg">
          Description de la vidéo (à implémenter si disponible). Profitez de ce contenu éducatif exclusif.
        </p>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import Hls from 'hls.js'

const route = useRoute()
const config = useRuntimeConfig()
const videoId = route.params.id as string
const videoPlayer = ref<HTMLVideoElement | null>(null)

const videoTitle = computed(() => {
  return videoId
    .replace(/-/g, ' ')
    .replace(/_/g, ' ')
    .replace(/\b\w/g, c => c.toUpperCase())
})

const videoSrc = computed(() => `${config.public.apiBase}/api/videos/${videoId}/master.m3u8`)

onMounted(() => {
  if (videoPlayer.value) {
    if (Hls.isSupported()) {
      const hls = new Hls()
      hls.loadSource(videoSrc.value)
      hls.attachMedia(videoPlayer.value)
    } else if (videoPlayer.value.canPlayType('application/vnd.apple.mpegurl')) {
      // Native HLS support (Safari)
      videoPlayer.value.src = videoSrc.value
    }
  }
})

useHead({
  title: videoTitle.value,
  meta: [
    { name: 'description', content: `Regardez la vidéo ${videoTitle.value} sur Ilmanar.` }
  ],
  script: [
      { src: 'https://cdn.jsdelivr.net/npm/hls.js@latest', tagPosition: 'bodyClose' } // Load hls.js from CDN if not in package.json
  ]
})
</script>
