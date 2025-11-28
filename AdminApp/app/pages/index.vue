<template>
  <div>
    <h1 class="text-3xl font-bold mb-8">Tableau de bord</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <UCard>
        <div class="flex items-center gap-4">
          <div class="p-3 bg-blue-100 dark:bg-blue-900 rounded-lg">
            <UIcon name="i-heroicons-academic-cap" class="text-2xl text-blue-600 dark:text-blue-400" />
          </div>
          <div>
            <p class="text-sm text-gray-500 dark:text-gray-400">Matières</p>
            <p class="text-2xl font-bold">{{ stats?.subjects || 0 }}</p>
          </div>
        </div>
      </UCard>

      <UCard>
        <div class="flex items-center gap-4">
          <div class="p-3 bg-green-100 dark:bg-green-900 rounded-lg">
            <UIcon name="i-heroicons-rectangle-stack" class="text-2xl text-green-600 dark:text-green-400" />
          </div>
          <div>
            <p class="text-sm text-gray-500 dark:text-gray-400">Modules</p>
            <p class="text-2xl font-bold">{{ stats?.modules || 0 }}</p>
          </div>
        </div>
      </UCard>

      <UCard>
        <div class="flex items-center gap-4">
          <div class="p-3 bg-purple-100 dark:bg-purple-900 rounded-lg">
            <UIcon name="i-heroicons-users" class="text-2xl text-purple-600 dark:text-purple-400" />
          </div>
          <div>
            <p class="text-sm text-gray-500 dark:text-gray-400">Utilisateurs</p>
            <p class="text-2xl font-bold">{{ stats?.users || 0 }}</p>
          </div>
        </div>
      </UCard>
    </div>
  </div>
</template>

<script setup lang="ts">
const { data: stats } = await useFetch('/api/stats/dashboard')
</script>

