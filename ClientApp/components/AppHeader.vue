<template>
  <header class="bg-[#082540] shadow-sm border-b border-gray-700 sticky top-0 z-40">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <!-- Logo -->
        <NuxtLink to="/" class="flex items-center space-x-2">
          <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-8 w-8" />
          <span class="text-xl font-bold text-[#C39712]">ILMANAR</span>
        </NuxtLink>

        <!-- Navigation -->
        <nav class="hidden md:flex items-center space-x-6 ml-auto">
          <NuxtLink v-if="isAuthenticated" to="/dashboard">
            <span class="text-gray-300 hover:text-[#C39712] font-medium transition-colors">
              Tableau de bord
            </span>
          </NuxtLink>
          <div v-if="isAuthenticated">
             <SubjectsMenu />
          </div>
          
          <!-- Subscription Badge -->
          <!-- <SubscriptionBadge /> -->
          
          <!-- Authenticated -->
          <div v-if="isAuthenticated">
            <UserMenu />
          </div>

          <!-- Guest -->
          <div v-else class="flex items-center space-x-3">
            <NuxtLink
              to="/login"
              class="text-white hover:text-[#C39712] font-medium transition-colors"
            >
              Connexion
            </NuxtLink>
            <NuxtLink
              to="/register"
              class="px-4 py-2 bg-[#C39712] text-[#082540] rounded-lg font-medium hover:bg-yellow-500 transition-colors"
            >
              S'inscrire
            </NuxtLink>
          </div>
        </nav>

        <!-- Mobile menu button -->
        <button
          @click="mobileMenuOpen = !mobileMenuOpen"
          class="md:hidden p-2 rounded-lg hover:bg-gray-700 text-white"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path
              v-if="!mobileMenuOpen"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M4 6h16M4 12h16M4 18h16"
            ></path>
            <path
              v-else
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M6 18L18 6M6 6l12 12"
            ></path>
          </svg>
        </button>
      </div>

      <!-- Mobile Menu -->
      <transition
        enter-active-class="transition ease-out duration-200"
        enter-from-class="opacity-0 -translate-y-1"
        enter-to-class="opacity-100 translate-y-0"
        leave-active-class="transition ease-in duration-150"
        leave-from-class="opacity-100 translate-y-0"
        leave-to-class="opacity-0 -translate-y-1"
      >
        <div v-if="mobileMenuOpen" class="md:hidden py-4 border-t border-gray-700">
          <nav class="flex flex-col space-y-3">
            <!-- Mobile Subjects List -->
            <div v-if="isAuthenticated" class="px-3 py-2">
              <div class="text-xs font-semibold text-gray-400 uppercase mb-2">Matières</div>
              <div v-if="loadingSubjects" class="py-2 text-sm text-gray-400">Chargement...</div>
              <div v-else class="space-y-1">
                <NuxtLink
                  v-for="subject in mobileSubjects"
                  :key="subject.id"
                  :to="`/subjects/${subject.id}`"
                  class="flex items-center px-2 py-1.5 text-sm text-gray-300 hover:bg-gray-700 rounded transition-colors"
                  @click="mobileMenuOpen = false"
                >
                  {{ subject.label }}
                </NuxtLink>
              </div>
            </div>

            <template v-if="isAuthenticated">
              <NuxtLink
                to="/profile"
                class="px-3 py-2 text-gray-300 hover:bg-gray-700 rounded-lg transition-colors"
                @click="mobileMenuOpen = false"
              >
                Mon profil
              </NuxtLink>
              <button
                @click="handleLogout"
                class="px-3 py-2 text-red-400 hover:bg-red-900/30 rounded-lg transition-colors text-left"
              >
                Déconnexion
              </button>
            </template>

            <template v-else>
              <NuxtLink
                to="/login"
                class="px-3 py-2 text-gray-300 hover:bg-gray-700 rounded-lg transition-colors"
                @click="mobileMenuOpen = false"
              >
                Connexion
              </NuxtLink>
              <NuxtLink
                to="/register"
                class="px-3 py-2 bg-[#C39712] text-[#082540] hover:bg-yellow-500 rounded-lg transition-colors text-center"
                @click="mobileMenuOpen = false"
              >
                S'inscrire
              </NuxtLink>
            </template>
          </nav>
        </div>
      </transition>
    </div>
  </header>
</template>

<script setup lang="ts">
import type { Subject } from '~/types'

const authStore = useAuthStore()
const api = useApi()
const isAuthenticated = computed(() => authStore.isAuthenticated)
const mobileMenuOpen = ref(false)
const loadingSubjects = ref(false)
const mobileSubjects = ref<Subject[]>([])

// Charger les matières pour le menu mobile
watch(mobileMenuOpen, async (isOpen) => {
  if (isOpen && mobileSubjects.value.length === 0) {
    loadingSubjects.value = true
    try {
      mobileSubjects.value = await api.getSubjects()
    } catch (error) {
      console.error('Error loading subjects:', error)
    } finally {
      loadingSubjects.value = false
    }
  }
})

const handleLogout = async () => {
  await authStore.logout()
  mobileMenuOpen.value = false
  navigateTo('/login')
}
</script>

