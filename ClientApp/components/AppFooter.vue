<template>
  <footer class="bg-[#082540] border-t border-white/5 py-6 text-center font-sans relative z-10">
    <div class="max-w-7xl mx-auto px-4">
      <p class="text-gray-500 text-sm">
        &copy; {{ new Date().getFullYear() }} Ilmanar &middot; 
        <a href="mailto:contact@ilmanar.com" class="hover:text-white transition-colors">contact@ilmanar.com</a>
      </p>
    </div>
  </footer>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>

