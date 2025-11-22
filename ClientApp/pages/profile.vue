<template>
  <div class="min-h-screen bg-gray-50">
    <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-900">Mon profil</h1>
        <p class="mt-2 text-gray-600">Gérez vos informations personnelles</p>
      </div>

      <!-- Loading -->
      <div v-if="loading" class="flex justify-center items-center py-12">
        <svg class="animate-spin h-12 w-12 text-primary-600" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>

      <div v-else class="space-y-6">
        <!-- Photo de profil -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Photo de profil</h2>
          
          <div class="flex items-center space-x-6">
            <div class="relative">
              <img
                v-if="user?.profilePicture"
                :src="user.profilePicture"
                alt="Photo de profil"
                class="w-24 h-24 rounded-full object-cover"
              />
              <div v-else class="w-24 h-24 rounded-full bg-primary-100 flex items-center justify-center">
                <span class="text-3xl text-primary-600">{{ getInitials() }}</span>
              </div>
            </div>

            <div class="flex-1">
              <input
                ref="fileInput"
                type="file"
                accept="image/*"
                class="hidden"
                @change="handleFileSelect"
              />
              
              <div class="flex space-x-3">
                <button
                  @click="$refs.fileInput.click()"
                  :disabled="uploading"
                  class="px-4 py-2 bg-primary-600 text-white rounded-lg text-sm font-medium hover:bg-primary-700 transition-colors disabled:opacity-50"
                >
                  {{ uploading ? 'Upload...' : 'Changer la photo' }}
                </button>
                
                <button
                  v-if="user?.profilePicture"
                  @click="deletePhoto"
                  class="px-4 py-2 bg-red-600 text-white rounded-lg text-sm font-medium hover:bg-red-700 transition-colors"
                >
                  Supprimer
                </button>
              </div>
              
              <p class="mt-2 text-xs text-gray-500">JPG, PNG ou GIF. Max 5MB.</p>
            </div>
          </div>

          <div v-if="uploadMessage" class="mt-4 p-3 rounded-lg" :class="uploadError ? 'bg-red-50 text-red-700' : 'bg-green-50 text-green-700'">
            {{ uploadMessage }}
          </div>
        </div>

        <!-- Informations personnelles -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Informations personnelles</h2>
          
          <form @submit.prevent="saveProfile" class="space-y-4">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Prénom</label>
                <input
                  v-model="profileForm.firstName"
                  type="text"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Jean"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Nom</label>
                <input
                  v-model="profileForm.lastName"
                  type="text"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Dupont"
                />
              </div>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Nom d'utilisateur</label>
              <input
                v-model="profileForm.userName"
                type="text"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="jeandupont"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
              <input
                :value="user?.email"
                type="email"
                disabled
                class="w-full px-4 py-2 border border-gray-300 rounded-lg bg-gray-50 text-gray-500 cursor-not-allowed"
              />
              <p class="mt-1 text-xs text-gray-500">L'email ne peut pas être modifié</p>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Bio</label>
              <textarea
                v-model="profileForm.bio"
                rows="3"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="Parlez-nous de vous..."
              ></textarea>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Date de naissance</label>
              <input
                v-model="profileForm.dateOfBirth"
                type="date"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
              />
            </div>

            <div v-if="profileMessage" class="p-3 rounded-lg" :class="profileError ? 'bg-red-50 text-red-700' : 'bg-green-50 text-green-700'">
              {{ profileMessage }}
            </div>

            <button
              type="submit"
              :disabled="savingProfile"
              class="w-full bg-primary-600 text-white py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors disabled:opacity-50"
            >
              {{ savingProfile ? 'Enregistrement...' : 'Enregistrer les modifications' }}
            </button>
          </form>
        </div>

        <!-- Changer le mot de passe -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Changer le mot de passe</h2>
          
          <form @submit.prevent="changePassword" class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Mot de passe actuel</label>
              <input
                v-model="passwordForm.currentPassword"
                type="password"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="••••••••"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Nouveau mot de passe</label>
              <input
                v-model="passwordForm.newPassword"
                type="password"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="••••••••"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Confirmer le nouveau mot de passe</label>
              <input
                v-model="passwordForm.confirmPassword"
                type="password"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="••••••••"
              />
            </div>

            <div v-if="passwordMessage" class="p-3 rounded-lg" :class="passwordError ? 'bg-red-50 text-red-700' : 'bg-green-50 text-green-700'">
              {{ passwordMessage }}
            </div>

            <button
              type="submit"
              :disabled="changingPassword"
              class="w-full bg-primary-600 text-white py-3 rounded-lg font-medium hover:bg-primary-700 transition-colors disabled:opacity-50"
            >
              {{ changingPassword ? 'Changement...' : 'Changer le mot de passe' }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { User } from '~/types'

definePageMeta({
  middleware: 'auth'
})

useHead({
  title: 'Mon profil'
})

const api = useApi()
const authStore = useAuthStore()

const loading = ref(true)
const user = ref<User | null>(null)

const profileForm = ref({
  firstName: '',
  lastName: '',
  userName: '',
  bio: '',
  dateOfBirth: ''
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

const uploading = ref(false)
const savingProfile = ref(false)
const changingPassword = ref(false)

const uploadMessage = ref('')
const uploadError = ref(false)
const profileMessage = ref('')
const profileError = ref(false)
const passwordMessage = ref('')
const passwordError = ref(false)

onMounted(async () => {
  const authStore = useAuthStore()
  
  // Vérifier si l'utilisateur est authentifié
  if (!authStore.isAuthenticated) {
    navigateTo('/login')
    return
  }
  
  try {
    user.value = await api.getProfile()
    
    // Remplir le formulaire
    profileForm.value = {
      firstName: user.value?.firstName || '',
      lastName: user.value?.lastName || '',
      userName: user.value?.username || '',
      bio: user.value?.bio || '',
      dateOfBirth: user.value?.dateOfBirth ? user.value.dateOfBirth.split('T')[0] : ''
    }
  } catch (error) {
    console.error('Error loading profile:', error)
    navigateTo('/login')
  } finally {
    loading.value = false
  }
})

const getInitials = () => {
  if (!user.value) return '?'
  return user.value.username?.[0]?.toUpperCase() || '?'
}

const handleFileSelect = async (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) return

  uploading.value = true
  uploadMessage.value = ''
  uploadError.value = false

  try {
    const response = await api.uploadProfilePicture(file)
    user.value!.profilePicture = response.profilePicture
    uploadMessage.value = 'Photo de profil mise à jour avec succès !'
  } catch (error: any) {
    uploadError.value = true
    uploadMessage.value = error.data?.message || 'Erreur lors de l\'upload de la photo'
  } finally {
    uploading.value = false
  }
}

const deletePhoto = async () => {
  if (!confirm('Êtes-vous sûr de vouloir supprimer votre photo de profil ?')) return

  try {
    await api.deleteProfilePicture()
    user.value!.profilePicture = undefined
    uploadMessage.value = 'Photo de profil supprimée'
    uploadError.value = false
  } catch (error) {
    uploadError.value = true
    uploadMessage.value = 'Erreur lors de la suppression'
  }
}

const saveProfile = async () => {
  savingProfile.value = true
  profileMessage.value = ''
  profileError.value = false

  try {
    await api.updateProfile(profileForm.value)
    profileMessage.value = 'Profil mis à jour avec succès !'
    
    // Rafraîchir les données
    user.value = await api.getProfile()
  } catch (error: any) {
    profileError.value = true
    profileMessage.value = error.data?.message || 'Erreur lors de la mise à jour du profil'
  } finally {
    savingProfile.value = false
  }
}

const changePassword = async () => {
  passwordMessage.value = ''
  passwordError.value = false

  if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
    passwordError.value = true
    passwordMessage.value = 'Les mots de passe ne correspondent pas'
    return
  }

  if (passwordForm.value.newPassword.length < 6) {
    passwordError.value = true
    passwordMessage.value = 'Le mot de passe doit contenir au moins 6 caractères'
    return
  }

  changingPassword.value = true

  try {
    await api.changePassword({
      currentPassword: passwordForm.value.currentPassword,
      newPassword: passwordForm.value.newPassword
    })
    
    passwordMessage.value = 'Mot de passe modifié avec succès !'
    
    // Réinitialiser le formulaire
    passwordForm.value = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
  } catch (error: any) {
    passwordError.value = true
    passwordMessage.value = error.data?.message || 'Erreur lors du changement de mot de passe'
  } finally {
    changingPassword.value = false
  }
}
</script>

