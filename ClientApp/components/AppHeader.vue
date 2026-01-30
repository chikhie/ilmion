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


            <NuxtLink to="/dashboard" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Dashboard
            </NuxtLink>
            <NuxtLink to="/themes" class="text-sm font-bold text-gray-400 hover:text-white transition-colors uppercase tracking-widest" active-class="!text-brand-gold">
              Thèmes
            </NuxtLink>
            
            <!-- User Dropdown -->
            <div class="relative ml-4 border-l border-white/10 pl-6" ref="dropdownRef">
                <button 
                    @click="userDropdownOpen = !userDropdownOpen"
                    class="flex items-center gap-3 text-brand-gold hover:text-white transition-colors group focus:outline-none"
                >
                    <div class="w-8 h-8 rounded-full bg-brand-gold/10 border border-brand-gold/30 flex items-center justify-center text-xs overflow-hidden">
                        <img v-if="authStore.user?.profilePicture" :src="authStore.user.profilePicture" class="w-full h-full object-cover" />
                        <span v-else>{{ authStore.user?.username?.charAt(0).toUpperCase() }}</span>
                    </div>
                    <span class="text-sm font-bold uppercase tracking-widest">{{ authStore.user?.username || 'Profil' }}</span>
                    <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 transition-transform duration-200" :class="{ 'rotate-180': userDropdownOpen }" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                    </svg>
                </button>

                <!-- Dropdown Menu -->
                <transition
                    enter-active-class="transition ease-out duration-200"
                    enter-from-class="opacity-0 translate-y-2"
                    enter-to-class="opacity-100 translate-y-0"
                    leave-active-class="transition ease-in duration-150"
                    leave-from-class="opacity-100 translate-y-0"
                    leave-to-class="opacity-0 translate-y-2"
                >
                    <div v-if="userDropdownOpen" class="absolute right-0 mt-2 w-48 bg-[#082540] border border-brand-gold/20 rounded-xl shadow-xl overflow-hidden z-50">
                        <NuxtLink 
                            to="/profile" 
                            class="block px-4 py-3 text-sm text-brand-parchment hover:bg-brand-gold/10 hover:text-brand-gold transition-colors"
                            @click="userDropdownOpen = false"
                        >
                            <div class="flex items-center gap-2">
                                <i class="fas fa-user-circle"></i>
                                Mon Profil
                            </div>
                        </NuxtLink>
                        <button 
                            @click="handleLogout"
                            class="w-full text-left px-4 py-3 text-sm text-red-400 hover:bg-red-500/10 hover:text-red-300 transition-colors border-t border-white/5"
                        >
                             <div class="flex items-center gap-2">
                                <i class="fas fa-sign-out-alt"></i>
                                Déconnexion
                            </div>
                        </button>
                    </div>
                </transition>
            </div>
          </div>
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
                to="/dashboard"
                class="px-4 py-3 text-gray-400 hover:text-white hover:bg-white/5 rounded-2xl transition-all font-bold text-sm uppercase tracking-widest"
                @click="mobileMenuOpen = false"
              >
                Dashboard
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
const userDropdownOpen = ref(false)
const dropdownRef = ref<HTMLElement | null>(null)

// Close dropdown when clicking outside
onMounted(() => {
    document.addEventListener('click', (e) => {
        if (dropdownRef.value && !dropdownRef.value.contains(e.target as Node)) {
            userDropdownOpen.value = false
        }
    })
})

const handleLogout = async () => {
  await authStore.logout()
  mobileMenuOpen.value = false
  userDropdownOpen.value = false // Fix: Close desktop dropdown
  navigateTo('/')
}
</script>

