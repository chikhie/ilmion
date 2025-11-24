<template>
  <footer class="bg-[#082540] border-t border-gray-700 mt-auto">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="border-t border-gray-700 mt-8 pt-6 text-center">
        <p class="text-gray-400 text-sm">
          &copy; {{ new Date().getFullYear() }} Ilmanar - Tous droits réservés
          📧 <a href="mailto:contact@ilmanar.com" class="text-gray-300 hover:text-[#C39712] text-sm transition-colors">contact@ilmanar.com</a>

        </p>

      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>

