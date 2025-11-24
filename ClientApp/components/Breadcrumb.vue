<template>
  <nav class="flex items-center space-x-2 text-sm text-gray-400">
    <NuxtLink 
      v-for="(item, index) in items" 
      :key="index"
      :to="item.path"
      class="hover:text-white transition-colors flex items-center"
      :class="{ 'text-white font-medium': index === items.length - 1 }"
    >
      <span>{{ item.label }}</span>
      <span v-if="index < items.length - 1" class="mx-2">→</span>
    </NuxtLink>
  </nav>
</template>

<script setup lang="ts">
interface BreadcrumbItem {
  label: string
  path: string
}

interface Props {
  items: BreadcrumbItem[]
}

defineProps<Props>()
</script>

