<template>
  <NuxtLink
    :to="`/subjects/${subject.id}`"
    class="block p-6 bg-white rounded-lg shadow-md hover:shadow-xl transition-shadow duration-300 border-2 border-transparent hover:border-primary-500"
  >
    <div class="flex items-center mb-4">
      <div class="text-4xl mr-4">{{ subject.icon || '📚' }}</div>
      <div class="flex-1">
        <h3 class="text-xl font-bold text-gray-800">{{ subject.name }}</h3>
        <p class="text-sm text-gray-500">{{ subject.moduleCount }} modules</p>
      </div>
    </div>
    <p class="text-gray-600">{{ subject.description }}</p>
  </NuxtLink>
</template>

<script setup lang="ts">
import type { Subject } from '~/types'

defineProps<{
  subject: Subject
}>()
</script>

