<template>
  <div class="min-h-screen bg-[#082540] text-white flex flex-col font-sans">
    <AppHeader v-if="route.path !== '/'" />
    <main class="flex-grow">
      <NuxtPage />
    </main>
    <AppFooter />
  </div>
</template>

<script setup lang="ts">
  const route = useRoute()
  useHead({
  titleTemplate: (titleChunk) => {
    return titleChunk ? `${titleChunk} - Ilmanar` : 'Ilmanar - Plateforme Éducative'
  },
  link: [
    { rel: 'icon', type: 'image/svg+xml', href: '/Ilmanar.svg' }
  ]
})
</script>

