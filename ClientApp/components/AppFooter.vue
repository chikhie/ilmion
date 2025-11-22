<template>
  <footer class="bg-white border-t mt-auto">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
        <!-- À propos -->
        <div>
          <h3 class="text-lg font-semibold text-gray-900 mb-4">📚 Ilmanar</h3>
          <p class="text-gray-600 text-sm">
            Plateforme éducative pour apprendre les sciences de manière interactive et engageante.
          </p>
        </div>

        <!-- Liens -->
        <div>
          <h3 class="text-lg font-semibold text-gray-900 mb-4">Liens rapides</h3>
          <ul class="space-y-2">
            <li>
              <NuxtLink to="/" class="text-gray-600 hover:text-primary-600 text-sm transition-colors">
                Catalogue
              </NuxtLink>
            </li>
            <li v-if="authStore.isAuthenticated">
              <NuxtLink to="/my-purchases" class="text-gray-600 hover:text-primary-600 text-sm transition-colors">
                Mes achats
              </NuxtLink>
            </li>
            <li v-if="authStore.isAuthenticated">
              <NuxtLink to="/profile" class="text-gray-600 hover:text-primary-600 text-sm transition-colors">
                Mon profil
              </NuxtLink>
            </li>
          </ul>
        </div>

        <!-- Contact -->
        <div>
          <h3 class="text-lg font-semibold text-gray-900 mb-4">Contact</h3>
          <ul class="space-y-2">
            <li class="text-gray-600 text-sm">
              📧 contact@ilmanar.com
            </li>
            <li>
              <a href="https://localhost:5000/swagger" target="_blank" class="text-gray-600 hover:text-primary-600 text-sm transition-colors">
                📚 Documentation API
              </a>
            </li>
          </ul>
        </div>
      </div>

      <div class="border-t border-gray-200 mt-8 pt-6 text-center">
        <p class="text-gray-600 text-sm">
          &copy; {{ new Date().getFullYear() }} Ilmanar - Tous droits réservés
        </p>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>

