<template>
  <div class="relative" ref="menuRef">
    <!-- Subjects Button -->
    <button
      @click="toggleMenu"
      class="flex items-center space-x-1 text-gray-700 hover:text-primary-600 font-medium transition-colors"
    >
      <span>Matières</span>
      <svg 
        class="w-4 h-4 transition-transform duration-200" 
        :class="{ 'rotate-180': isOpen }"
        fill="none" 
        stroke="currentColor" 
        viewBox="0 0 24 24"
      >
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
        class="absolute left-0 mt-2 w-64 bg-white rounded-lg shadow-lg border border-gray-200 py-2 z-50"
      >
        <!-- Loading -->
        <div v-if="loading" class="px-4 py-6 flex justify-center">
          <svg class="animate-spin h-5 w-5 text-primary-600" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
        </div>

        <!-- Subjects List -->
        <div v-else-if="subjects.length > 0">
          <NuxtLink
            v-for="subject in subjects"
            :key="subject.id"
            :to="`/subjects/${subject.id}`"
            class="flex items-center px-4 py-3 hover:bg-gray-50 transition-colors group"
            @click="closeMenu"
          >
            <div class="text-sm font-medium text-gray-900 group-hover:text-primary-600">
              {{ subject.label }}
            </div>
          </NuxtLink>
        </div>

        <!-- Empty State -->
        <div v-else class="px-4 py-6 text-center text-sm text-gray-500">
          Aucune matière disponible
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import type { Subject } from '~/types'

const api = useApi()
const isOpen = ref(false)
const menuRef = ref<HTMLElement | null>(null)
const loading = ref(false)
const subjects = ref<Subject[]>([])

const toggleMenu = async () => {
  isOpen.value = !isOpen.value
  
  // Charger les matières si le menu est ouvert et qu'elles ne sont pas déjà chargées
  if (isOpen.value && subjects.value.length === 0) {
    await loadSubjects()
  }
}

const closeMenu = () => {
  isOpen.value = false
}

const loadSubjects = async () => {
  loading.value = true
  try {
    subjects.value = await api.getSubjects()
  } catch (error) {
    console.error('Error loading subjects:', error)
  } finally {
    loading.value = false
  }
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

