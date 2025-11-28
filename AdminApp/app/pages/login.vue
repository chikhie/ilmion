<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
    <div class="max-w-md w-full space-y-8 p-8 bg-white dark:bg-gray-800 rounded-lg shadow-lg">
      <div class="text-center">
        <UIcon name="i-heroicons-lock-closed" class="mx-auto h-12 w-12 text-primary-500" />
        <h2 class="mt-6 text-3xl font-extrabold text-gray-900 dark:text-white">
          Connexion Admin
        </h2>
      </div>
      
      <form class="mt-8 space-y-6" @submit.prevent="onSubmit">
        <div class="rounded-md shadow-sm -space-y-px">
          <UFormGroup label="Email" name="email" class="mb-4">
            <UInput v-model="email" type="email" autocomplete="email" required />
          </UFormGroup>
          <UFormGroup label="Mot de passe" name="password" class="mb-4">
            <UInput v-model="password" type="password" autocomplete="current-password" required />
          </UFormGroup>
        </div>

        <div v-if="error" class="text-red-500 text-sm text-center">
          {{ error }}
        </div>

        <div>
          <UButton type="submit" block :loading="loading">
            Se connecter
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'empty'
})

const auth = useAuth()
const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')

const onSubmit = async () => {
  loading.value = true
  error.value = ''
  
  const success = await auth.login(email.value, password.value)
  
  if (success) {
    navigateTo('/')
  } else {
    error.value = 'Email ou mot de passe incorrect'
  }
  
  loading.value = false
}
</script>

