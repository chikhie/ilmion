<template>
  <div class="min-h-screen bg-brand-dark text-white flex flex-col font-sans-body">
    <AppHeader />
    <main class="flex-grow pb-24 lg:pb-0">
      <NuxtPage />
    </main>
    <AppFooter class="hidden lg:block" /> <!-- Optionally hide footer on mobile if it clutters, or keep it. User didn't specify, but often bottom nav replaces footer nav functionality on mobile. Let's keep it but maybe add padding. -->
    <AppBottomNav />
  </div>
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
    { rel: 'icon', type: 'image/svg+xml', href: '/Ilmanar.svg' }
  ]
})
</script>

