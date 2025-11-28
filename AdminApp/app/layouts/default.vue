<template>
  <div class="flex min-h-screen">
    <!-- Sidebar -->
    <div class="w-64 bg-white dark:bg-gray-800 border-r border-gray-200 dark:border-gray-700 p-4 flex flex-col">
      <div class="mb-8 flex items-center gap-2">
        <UIcon name="i-heroicons-book-open" class="text-primary-500 text-2xl" />
        <h1 class="text-xl font-bold text-gray-900 dark:text-white">KitabAdmin</h1>
      </div>
      
      <UVerticalNavigation :links="links" class="flex-1" />
      
      <div class="border-t border-gray-200 dark:border-gray-700 pt-4 mt-4">
        <UButton 
          block 
          color="gray" 
          variant="ghost" 
          icon="i-heroicons-arrow-right-on-rectangle" 
          label="Déconnexion" 
          @click="logout" 
        />
      </div>
    </div>

    <!-- Main Content -->
    <div class="flex-1 p-8">
      <slot />
    </div>
  </div>
</template>

<script setup lang="ts">
const auth = useAuth()

const links = [
  {
    label: 'Dashboard',
    icon: 'i-heroicons-home',
    to: '/'
  },
  {
    label: 'Matières',
    icon: 'i-heroicons-academic-cap',
    to: '/subjects'
  },
  {
    label: 'Modules',
    icon: 'i-heroicons-rectangle-stack',
    to: '/modules'
  },
  {
    label: 'Chapitres',
    icon: 'i-heroicons-document-text',
    to: '/chapters'
  }
]

const logout = () => {
  auth.logout()
}
</script>

