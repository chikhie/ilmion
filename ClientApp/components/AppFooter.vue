<template>
  <footer class="bg-[#1e1f22] border-t border-[#2b2d31] py-6 text-center font-sans relative z-10 shadow-lg">
    <div class="max-w-7xl mx-auto px-4">
      <p class="text-[#949ba4] text-sm font-medium">
        &copy; {{ new Date().getFullYear() }} Ilmion &middot; 
        <a href="mailto:contact@ilmion.com" class="hover:text-[#0D99FF] transition-colors">Tous droits reservés</a>
      </p>
    </div>
  </footer>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>

