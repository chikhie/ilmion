<template>
  <div class="min-h-screen bg-brand-dark text-white flex flex-col font-sans-body">
    <AppHeader v-if="showHeader" />
    <main class="flex-grow">
      <NuxtPage />
    </main>
    <AppFooter />
  </div>
</template>

<script setup lang="ts">
  import { useAuthStore } from '~/stores/auth'
  
  const authStore = useAuthStore()
  const route = useRoute()
  
  const showHeader = computed(() => {
    return authStore.isAuthenticated
  })

  onMounted(() => {
    authStore.initAuth()
  })
  
  useHead({
  titleTemplate: (titleChunk) => {
    return titleChunk ? `${titleChunk} - Ilmanar` : 'Ilmanar - Plateforme Éducative'
  },
  link: [
    { rel: 'icon', type: 'image/svg+xml', href: '/Ilmanar.svg' }
  ]
})
</script>

