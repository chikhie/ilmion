<template>
    <header class="bg-brand-dark/95 backdrop-blur-xl border-b border-brand-gold/10 sticky top-0 z-40 font-sans-body transition-all duration-300">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-12 md:h-20">
        <!-- Logo -->
        <NuxtLink to="/" class="flex items-center space-x-2 md:space-x-3 group">
          <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-6 w-6 md:h-10 md:w-10 transition-transform duration-300 group-hover:scale-110 drop-shadow-[0_0_15px_rgba(195,151,18,0.3)]" />
          <span class="text-lg md:text-2xl font-serif-title font-bold tracking-tight text-brand-parchment group-hover:text-brand-gold transition-colors uppercase">ILMANAR</span>
        </NuxtLink>

        <!-- Navigation -->
        <nav class="hidden lg:flex items-center space-x-8">
          
          <!-- Authenticated -->
          <div class="flex items-center space-x-8">
            <NuxtLink to="/games" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Jeux
            </NuxtLink>
            <NuxtLink to="/magazine" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Magazine
            </NuxtLink>
            <NuxtLink to="/" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Accueil
            </NuxtLink>
            <NuxtLink to="/videos" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Vidéos
            </NuxtLink>
            
            <div class="pl-4 border-l border-white/10">
                 <NuxtLink to="/profile" class="text-sm font-bold text-brand-gold hover:text-white transition-colors uppercase tracking-widest">
                  Profil
                </NuxtLink>
            </div>
          </div>

          <!-- Guest -->
          <!--
          <div v-else class="flex items-center space-x-6">
            <NuxtLink
              to="/login"
              class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest"
            >
              Connexion
            </NuxtLink>
            <NuxtLink
              to="/register"
              class="px-6 py-2.5 bg-white text-[#082540] rounded-full text-sm font-bold hover:bg-gray-100 transition-all shadow-lg active:scale-95"
            >
              S'inscrire
            </NuxtLink>
          </div>
          -->
        </nav>

        <!-- Mobile menu button (Hidden as we switched to Bottom Nav) -->
        <button
          class="hidden" 
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
        enter-from-class="opacity-0 -translate-y-4"
        enter-to-class="opacity-100 translate-y-0"
        leave-active-class="transition ease-in duration-150"
        leave-from-class="opacity-100 translate-y-0"
        leave-to-class="opacity-0 -translate-y-4"
      >
        <div v-if="mobileMenuOpen" class="md:hidden py-6 border-t border-white/5 bg-[#082540]/95 backdrop-blur-2xl px-2">
          <nav class="flex flex-col space-y-2">

            <template v-if="isAuthenticated">
              <NuxtLink
                to="/multiplayer"
                class="px-4 py-3 text-gray-400 hover:text-white hover:bg-white/5 rounded-2xl transition-all font-bold text-sm uppercase tracking-widest"
                @click="mobileMenuOpen = false"
              >
                Multijoueur
              </NuxtLink>
              <NuxtLink
                to="/profile"
                class="px-4 py-3 text-gray-400 hover:text-white hover:bg-white/5 rounded-2xl transition-all font-bold text-sm uppercase tracking-widest"
                @click="mobileMenuOpen = false"
              >
                Mon profil
              </NuxtLink>
              <button
                @click="handleLogout"
                class="px-4 py-3 text-red-400/80 hover:text-red-400 hover:bg-red-500/10 rounded-2xl transition-all text-left font-bold text-sm uppercase tracking-widest"
              >
                Déconnexion
              </button>
            </template>

            <!--
            <template v-else>
              <NuxtLink
                to="/login"
                class="px-4 py-3 text-gray-400 hover:text-white hover:bg-white/5 rounded-2xl transition-all font-bold text-sm uppercase tracking-widest"
                @click="mobileMenuOpen = false"
              >
                Connexion
              </NuxtLink>
              <NuxtLink
                to="/register"
                class="mx-2 px-4 py-4 bg-white text-[#082540] hover:bg-gray-100 rounded-2xl transition-all text-center font-bold text-base shadow-xl"
                @click="mobileMenuOpen = false"
              >
                S'inscrire
              </NuxtLink>
            </template>
            -->
          </nav>
        </div>
      </transition>
    </div>
  </header>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
const isAuthenticated = computed(() => authStore.isLoggedIn)
const mobileMenuOpen = ref(false)

const handleLogout = async () => {
  await authStore.logout()
  mobileMenuOpen.value = false
  navigateTo('/login')
}
</script>

