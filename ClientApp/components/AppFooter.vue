<template>
  <footer class="bg-texture-wood border-t border-brand-gold/20 py-6 text-center font-sans-body relative z-10 shadow-lg">
    <div class="max-w-7xl mx-auto px-4">
      <p class="text-brand-parchment/60 text-sm font-medium">
        &copy; {{ new Date().getFullYear() }} Ilmanar &middot; 
        <a href="mailto:contact@ilmanar.com" class="hover:text-brand-gold transition-colors">Tous droits reservés</a>
      </p>
    </div>
  </footer>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>

