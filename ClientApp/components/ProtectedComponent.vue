<template>
  <div class="protected-component-wrapper">
    <!-- Loading -->
    <div v-if="loading" class="flex flex-col justify-center items-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-600 mb-4" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
      <span class="text-gray-600 font-medium">Chargement du contenu interactif...</span>
      <span class="text-gray-400 text-sm mt-2">Déchiffrement en cours</span>
    </div>

    <!-- Error - Access Denied -->
    <div v-else-if="error" class="bg-gradient-to-br from-red-50 to-orange-50 border border-red-200 rounded-xl p-6 shadow-sm">
      <div class="flex items-start">
        <div class="flex-shrink-0">
          <svg class="h-6 w-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path>
          </svg>
        </div>
        <div class="ml-3 flex-1">
          <h3 class="text-lg font-semibold text-red-800 mb-2">🔒 Contenu Protégé</h3>
          <p class="text-red-700 mb-4">{{ error }}</p>
          
          <!-- Purchase Button -->
          <button 
            v-if="requiresPurchase && moduleId" 
            @click="goToPurchase"
            class="inline-flex items-center px-6 py-3 bg-gradient-to-r from-primary-600 to-primary-700 text-white rounded-lg font-semibold hover:from-primary-700 hover:to-primary-800 transition-all shadow-md hover:shadow-lg transform hover:-translate-y-0.5"
          >
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"></path>
            </svg>
            Acheter ce module
          </button>

          <!-- Login Button -->
          <button 
            v-else-if="!requiresPurchase"
            @click="goToLogin"
            class="inline-flex items-center px-6 py-3 bg-gray-800 text-white rounded-lg font-semibold hover:bg-gray-900 transition-all"
          >
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"></path>
            </svg>
            Se connecter
          </button>
        </div>
      </div>
    </div>

    <!-- Composant chargé et rendu -->
    <div v-else-if="componentHtml" class="interactive-component">
      <div v-html="componentHtml" class="component-content"></div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  sectionId: string
}>()

const api = useApi()
const loading = ref(true)
const error = ref<string | null>(null)
const requiresPurchase = ref(false)
const moduleId = ref<string | null>(null)
const componentHtml = ref<string | null>(null)

onMounted(async () => {
  await loadProtectedComponent()
})

const loadProtectedComponent = async () => {
  loading.value = true
  error.value = null
  
  try {
    // Récupérer le composant déchiffré depuis le backend
    const response = await api.getProtectedComponent(props.sectionId)
    
    // Pour l'instant, on affiche simplement le code dans une balise pre
    // Dans une vraie implémentation, vous compileriez le composant Vue
    componentHtml.value = renderComponentAsHtml(response.componentCode)
    
  } catch (err: any) {
    console.error('Erreur lors du chargement du composant protégé:', err)
    
    if (err.status === 401 || err.status === 403) {
      error.value = err.data?.message || 'Accès refusé à ce contenu'
      requiresPurchase.value = err.data?.requiresPurchase || false
      moduleId.value = err.data?.moduleId
    } else if (err.status === 404) {
      error.value = 'Contenu introuvable'
    } else {
      error.value = 'Erreur lors du chargement du contenu. Veuillez réessayer.'
    }
  } finally {
    loading.value = false
  }
}

/**
 * Rend le code du composant en HTML
 * AVERTISSEMENT: Cette fonction est simplifiée pour la démo.
 * En production, utilisez un compilateur Vue approprié ou un sandboxing sécurisé.
 */
const renderComponentAsHtml = (componentCode: string): string => {
  // Extraire le template du composant Vue
  const templateMatch = componentCode.match(/<template>([\s\S]*?)<\/template>/)
  
  if (templateMatch) {
    return templateMatch[1]
  }
  
  // Si pas de template, afficher le code source (pour debug)
  return `
    <div class="bg-gray-50 rounded-lg p-6 border border-gray-200">
      <div class="flex items-center mb-4">
        <svg class="w-6 h-6 text-primary-600 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 20l4-16m4 4l4 4-4 4M6 16l-4-4 4-4"></path>
        </svg>
        <span class="font-semibold text-gray-700">Composant Interactif Chargé</span>
      </div>
      <pre class="bg-white p-4 rounded border border-gray-300 overflow-x-auto text-sm"><code>${escapeHtml(componentCode.substring(0, 500))}...</code></pre>
      <p class="text-sm text-gray-500 mt-2">💡 Le composant a été déchiffré avec succès. Intégration complète en cours de développement.</p>
    </div>
  `
}

const escapeHtml = (text: string): string => {
  const map: Record<string, string> = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '&quot;',
    "'": '&#039;'
  }
  return text.replace(/[&<>"']/g, (m) => map[m])
}

const goToPurchase = () => {
  if (moduleId.value) {
    navigateTo(`/checkout/${moduleId.value}`)
  }
}

const goToLogin = () => {
  navigateTo('/login')
}
</script>

<style scoped>
.protected-component-wrapper {
  @apply min-h-[200px];
}

.component-content {
  @apply w-full;
}

/* Styles pour le contenu interactif */
.interactive-component :deep(button) {
  @apply transition-all;
}

.interactive-component :deep(input),
.interactive-component :deep(select),
.interactive-component :deep(textarea) {
  @apply border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-primary-500;
}
</style>

