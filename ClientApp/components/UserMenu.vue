<template>
  <div class="relative" ref="menuRef">
    <!-- User Button -->
    <button
      @click="toggleMenu"
      class="flex items-center space-x-2 p-2 rounded-lg hover:bg-gray-700 transition-colors"
    >
      <img 
        v-if="user?.profilePicture"
        :src="user.profilePicture"
        alt="Photo de profil"
        class="w-10 h-10 rounded-full object-cover border-2 border-primary-500"
      />
      <div v-else class="w-10 h-10 rounded-full bg-primary-600 flex items-center justify-center border-2 border-primary-700">
        <span class="text-sm text-white font-bold">{{ getInitials() }}</span>
      </div>

      <span class="hidden md:block text-sm font-medium text-gray-200">{{ user?.username }}</span>
      
      <svg class="hidden md:block w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
      </svg>
    </button>

    <!-- Dropdown Menu -->
    <transition
      enter-active-class="transition ease-out duration-100"
      enter-from-class="transform opacity-0 scale-95"
      enter-to-class="transform opacity-100 scale-100"
      leave-active-class="transition ease-in duration-75"
      leave-from-class="transform opacity-100 scale-100"
      leave-to-class="transform opacity-0 scale-95"
    >
      <div
        v-if="isOpen"
        class="absolute right-0 mt-2 w-56 bg-gray-800 rounded-lg shadow-lg border border-gray-700 py-1 z-50"
      >
        <!-- User Info -->
        <div class="px-4 py-3 border-b border-gray-700">
          <p class="text-sm font-medium text-gray-200">{{ user?.username }}</p>
          <p class="text-xs text-gray-400 truncate">{{ user?.email }}</p>
        </div>

        <!-- Menu Items -->
        <NuxtLink
          to="/profile"
          class="flex items-center px-4 py-2 text-sm text-gray-300 hover:bg-gray-700 transition-colors"
          @click="closeMenu"
        >
          <svg class="w-4 h-4 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
          </svg>
          Mon profil
        </NuxtLink>


        <div class="border-t border-gray-700 my-1"></div>

        <button
          @click="handleLogout"
          class="w-full flex items-center px-4 py-2 text-sm text-red-400 hover:bg-red-900/30 transition-colors"
        >
          <svg class="w-4 h-4 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path>
          </svg>
          Déconnexion
        </button>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
const user = computed(() => authStore.user)

const isOpen = ref(false)
const menuRef = ref<HTMLElement | null>(null)

const toggleMenu = () => {
  isOpen.value = !isOpen.value
}

const closeMenu = () => {
  isOpen.value = false
}

const getInitials = () => {
  if (!user.value) return '?'
  return user.value.username?.[0]?.toUpperCase() || '?'
}

const getDisplayName = () => {
  if (!user.value) return 'Utilisateur'
  
  // Si prénom et nom existent, les afficher
  if (user.value.firstName && user.value.lastName) {
    return `${user.value.firstName} ${user.value.lastName}`
  }
  
  // Sinon juste le prénom
  if (user.value.firstName) {
    return user.value.firstName
  }
  
  // Sinon le nom d'utilisateur
  return user.value.username || 'Utilisateur'
}

const handleLogout = async () => {
  await authStore.logout()
  closeMenu()
  navigateTo('/login')
}

// Fermer le menu si on clique à l'extérieur
onMounted(() => {
  const handleClickOutside = (event: MouseEvent) => {
    if (menuRef.value && !menuRef.value.contains(event.target as Node)) {
      closeMenu()
    }
  }

  document.addEventListener('click', handleClickOutside)

  onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside)
  })
})
</script>

