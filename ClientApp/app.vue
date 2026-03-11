<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup lang="ts">
  import { useAuthStore } from '~/stores/auth'
  
  const authStore = useAuthStore()
  const route = useRoute()

  onMounted(() => {
    authStore.initAuth()
  })
  
  useHead({
  titleTemplate: (titleChunk) => {
    return titleChunk ? `${titleChunk} - Ilmion` : 'Ilmion - Plateforme Éducative'
  },  
  link: [
    { rel: 'icon', type: 'image/svg+xml', href: '/Ilmion.svg' },
    { rel: 'preload', as: 'image', href: '/patterns/arabesque.png', fetchpriority: 'high' }
  ]
})
</script>

