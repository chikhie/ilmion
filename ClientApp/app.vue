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
    return titleChunk ? `${titleChunk} - Ilmanar` : 'Ilmanar - Plateforme Éducative'
  },
  link: [
    { rel: 'icon', type: 'image/svg+xml', href: '/Ilmanar.svg' },
    { rel: 'preload', as: 'image', href: '/patterns/arabesque.png' }
  ]
})
</script>

