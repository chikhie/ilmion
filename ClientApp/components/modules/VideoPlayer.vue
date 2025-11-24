<template>
  <div class="aspect-video bg-gray-800 rounded-lg overflow-hidden border border-gray-700 relative">
    <!-- YouTube Player -->
    <iframe
      v-if="isYouTube && youtubeId"
      :src="`https://www.youtube.com/embed/${youtubeId}`"
      class="w-full h-full"
      frameborder="0"
      allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
      allowfullscreen
    ></iframe>
    
    <!-- YouTube Error -->
    <div v-else-if="isYouTube && !youtubeId" class="absolute inset-0 flex items-center justify-center bg-gray-900">
      <div class="text-center p-6">
        <p class="text-red-400 mb-2">❌ Erreur YouTube</p>
        <p class="text-sm text-gray-400">Impossible d'extraire l'ID de la vidéo</p>
        <p class="text-xs text-gray-500 mt-2 break-all">URL: {{ src }}</p>
      </div>
    </div>

    <!-- HLS Player -->
    <template v-else>
      <video
        ref="videoElement"
        class="w-full h-full"
        controls
        :poster="poster"
        playsinline
      >
        Votre navigateur ne supporte pas la lecture de vidéos.
      </video>
      
      <!-- Loading State -->
      <div v-if="isLoading" class="absolute inset-0 flex items-center justify-center bg-gray-900/50">
        <div class="text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-500"></div>
          <p class="mt-4 text-gray-300">Chargement de la vidéo...</p>
        </div>
      </div>

      <!-- Error State -->
      <div v-if="error" class="absolute inset-0 flex items-center justify-center bg-gray-900/90">
        <div class="text-center p-6">
          <p class="text-red-400 mb-2">❌ Erreur de chargement</p>
          <p class="text-sm text-gray-400">{{ error }}</p>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import Hls from 'hls.js'

interface Props {
  src: string
  poster?: string
}

const props = defineProps<Props>()

const videoElement = ref<HTMLVideoElement | null>(null)
const isLoading = ref(true)
const error = ref<string | null>(null)
let hls: Hls | null = null

// Détecter si c'est une URL YouTube
const isYouTube = computed(() => {
  return props.src.includes('youtube.com') || props.src.includes('youtu.be')
})

// Extraire l'ID YouTube
const youtubeId = computed(() => {
  if (!isYouTube.value) return null
  
  try {
    // Essayer d'abord avec une URL complète
    const url = new URL(props.src.startsWith('http') ? props.src : `https://${props.src}`)
    
    // Format: https://www.youtube.com/watch?v=VIDEO_ID
    if (url.hostname.includes('youtube.com')) {
      return url.searchParams.get('v')
    }
    
    // Format: https://youtu.be/VIDEO_ID
    if (url.hostname.includes('youtu.be')) {
      // Retirer le slash initial et tout après un éventuel ?
      return url.pathname.slice(1).split('?')[0]
    }
  } catch (e) {
    console.warn('Erreur lors du parsing de l\'URL YouTube:', e)
  }
  
  // Fallback: extraction par regex
  const patterns = [
    /(?:youtube\.com\/watch\?v=|youtu\.be\/)([a-zA-Z0-9_-]{11})/,
    /youtube\.com\/embed\/([a-zA-Z0-9_-]{11})/,
    /youtube\.com\/v\/([a-zA-Z0-9_-]{11})/
  ]
  
  for (const pattern of patterns) {
    const match = props.src.match(pattern)
    if (match && match[1]) {
      return match[1]
    }
  }
  
  console.error('Impossible d\'extraire l\'ID YouTube de:', props.src)
  return null
})

onMounted(() => {
  // Si c'est YouTube, pas besoin d'initialiser HLS
  if (isYouTube.value) {
    isLoading.value = false
    return
  }

  if (!videoElement.value) {
    error.value = "Élément vidéo non disponible"
    isLoading.value = false
    return
  }

  // Vérifier si le navigateur supporte HLS nativement (Safari)
  if (videoElement.value.canPlayType('application/vnd.apple.mpegurl')) {
    videoElement.value.src = props.src
    
    videoElement.value.addEventListener('loadeddata', () => {
      isLoading.value = false
    })
    
    videoElement.value.addEventListener('error', () => {
      error.value = "Erreur lors du chargement de la vidéo"
      isLoading.value = false
    })
  }
  // Sinon, utiliser hls.js pour les autres navigateurs
  else if (Hls.isSupported()) {
    hls = new Hls({
      enableWorker: true,
      lowLatencyMode: false,
    })

    hls.loadSource(props.src)
    hls.attachMedia(videoElement.value)

    hls.on(Hls.Events.MANIFEST_PARSED, () => {
      isLoading.value = false
    })

    hls.on(Hls.Events.ERROR, (event, data) => {
      if (data.fatal) {
        switch (data.type) {
          case Hls.ErrorTypes.NETWORK_ERROR:
            error.value = "Erreur réseau lors du chargement de la vidéo"
            hls?.startLoad()
            break
          case Hls.ErrorTypes.MEDIA_ERROR:
            error.value = "Erreur de lecture média"
            hls?.recoverMediaError()
            break
          default:
            error.value = "Erreur fatale lors de la lecture"
            hls?.destroy()
            break
        }
        isLoading.value = false
      }
    })
  } else {
    error.value = "Votre navigateur ne supporte pas la lecture HLS"
    isLoading.value = false
  }
})

onBeforeUnmount(() => {
  if (hls) {
    hls.destroy()
    hls = null
  }
})
</script>
