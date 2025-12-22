<template>
  <div class="min-h-screen bg-[#082540] text-white font-sans relative overflow-hidden">
    <!-- Background Gradients (Modern & Minimal) -->
    <div class="fixed inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <!-- Header -->
    <div class="relative z-10 border-b border-white/5 bg-white/5 backdrop-blur-xl">
      <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
        <div class="mb-8 animate-fade-in">
          <NuxtLink to="/games" class="text-xs font-black uppercase tracking-widest text-gray-500 hover:text-white transition-all flex items-center gap-2 group">
            <svg class="w-4 h-4 transition-transform group-hover:-translate-x-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" /></svg>
            Retour aux jeux
          </NuxtLink>
        </div>
        
        <div class="flex items-center space-x-6 animate-fade-in">
          <div class="w-20 h-20 rounded-3xl bg-white/5 flex items-center justify-center border border-white/10 text-4xl shadow-2xl">
            👤
          </div>
          <div>
            <h1 class="text-4xl font-black tracking-tighter uppercase text-white">Mon profil</h1>
            <p class="mt-1 text-gray-400 font-medium">Gérez votre identité et vos préférences</p>
          </div>
        </div>
      </div>
    </div>

    <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-12 relative z-10">
      <!-- Loading -->
      <div v-if="loading" class="flex justify-center items-center py-20">
        <svg class="animate-spin h-10 w-10 text-white opacity-20" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>

      <div v-else class="space-y-8 animate-fade-in-up">
        <!-- Photo de profil -->
        <div class="bg-white/5 backdrop-blur-2xl rounded-3xl p-8 border border-white/10 shadow-2xl overflow-hidden relative group">
          <div class="absolute top-0 right-0 w-32 h-32 bg-white/5 rounded-full -mr-16 -mt-16 blur-3xl group-hover:bg-white/10 transition-all duration-500"></div>
          
          <div class="flex items-center mb-8">
            <h2 class="text-xs font-black uppercase tracking-widest text-gray-500">Photo de profil</h2>
          </div>
          
          <div class="flex flex-col sm:flex-row items-center sm:items-end space-y-6 sm:space-y-0 sm:space-x-8">
            <div class="relative">
              <img
                v-if="user?.profilePicture"
                :src="user.profilePicture"
                alt="Photo de profil"
                class="w-32 h-32 rounded-3xl object-cover border-2 border-white/10 shadow-2xl"
              />
              <div v-else class="w-32 h-32 rounded-3xl bg-white/5 border-2 border-white/10 flex items-center justify-center shadow-2xl">
                <span class="text-4xl font-black text-white/20">{{ getInitials() }}</span>
              </div>
            </div>

            <div class="flex-1 text-center sm:text-left">
              <input
                ref="fileInput"
                type="file"
                accept="image/*"
                class="hidden"
                @change="handleFileSelect"
              />
              
              <div class="flex flex-col sm:flex-row gap-4 mb-4">
                <button
                  @click="fileInput?.click()"
                  :disabled="uploading"
                  class="px-8 py-3 bg-white text-[#082540] rounded-2xl text-sm font-bold hover:bg-gray-100 transition-all shadow-xl active:scale-95 disabled:opacity-50"
                >
                  {{ uploading ? 'Upload...' : 'Changer la photo' }}
                </button>
                
                <button
                  v-if="user?.profilePicture"
                  @click="deletePhoto"
                  class="px-8 py-3 bg-red-500/10 text-red-400 border border-red-500/20 rounded-2xl text-sm font-bold hover:bg-red-500/20 transition-all"
                >
                  Supprimer
                </button>
              </div>
              
              <p class="text-[10px] font-bold uppercase tracking-widest text-gray-500">Format JPG, PNG ou GIF &bull; Max 5MB</p>
            </div>
          </div>

          <div v-if="uploadMessage" class="mt-6 p-4 rounded-2xl border text-sm font-bold text-center" :class="uploadError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
            {{ uploadMessage }}
          </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <!-- Informations personnelles -->
          <div class="bg-white/5 backdrop-blur-2xl rounded-3xl p-8 border border-white/10 shadow-2xl">
            <div class="flex items-center mb-8">
              <h2 class="text-xs font-black uppercase tracking-widest text-gray-500">Informations personnelles</h2>
            </div>
            
            <form @submit.prevent="saveProfile" class="space-y-6">
              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Prénom</label>
                <input
                  v-model="profileForm.firstName"
                  type="text"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="Jean"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Nom</label>
                <input
                  v-model="profileForm.lastName"
                  type="text"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="Dupont"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Nom d'utilisateur</label>
                <input
                  v-model="profileForm.userName"
                  type="text"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="jeandupont"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Email (Non modifiable)</label>
                <input
                  :value="user?.email"
                  type="email"
                  disabled
                  class="w-full px-5 py-4 bg-black/20 border border-white/5 rounded-2xl text-gray-500 cursor-not-allowed outline-none"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Bio</label>
                <textarea
                  v-model="profileForm.bio"
                  rows="3"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none resize-none"
                  placeholder="Parlez-nous de vous..."
                ></textarea>
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Date de naissance</label>
                <input
                  v-model="profileForm.dateOfBirth"
                  type="date"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white focus:ring-2 focus:ring-white/20 transition-all outline-none [color-scheme:dark]"
                />
              </div>

              <div v-if="profileMessage" class="p-4 rounded-2xl border text-sm font-bold text-center" :class="profileError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
                {{ profileMessage }}
              </div>

              <button
                type="submit"
                :disabled="savingProfile"
                class="w-full bg-white text-[#082540] py-4 rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-gray-100 transition-all shadow-xl active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {{ savingProfile ? 'Envoi...' : 'Enregistrer' }}
              </button>
            </form>
          </div>

          <!-- Changer le mot de passe -->
          <div class="bg-white/5 backdrop-blur-2xl rounded-3xl p-8 border border-white/10 shadow-2xl h-fit">
            <div class="flex items-center mb-8">
              <h2 class="text-xs font-black uppercase tracking-widest text-gray-500">Mot de passe</h2>
            </div>
            
            <form @submit.prevent="changePassword" class="space-y-6">
              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Actuel</label>
                <input
                  v-model="passwordForm.currentPassword"
                  type="password"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="••••••••"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Nouveau</label>
                <input
                  v-model="passwordForm.newPassword"
                  type="password"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="••••••••"
                />
              </div>

              <div class="space-y-2">
                <label class="block text-[10px] font-black uppercase tracking-widest text-gray-500 ml-1">Confirmation</label>
                <input
                  v-model="passwordForm.confirmPassword"
                  type="password"
                  class="w-full px-5 py-4 bg-white/5 border border-white/10 rounded-2xl text-white placeholder-gray-600 focus:ring-2 focus:ring-white/20 transition-all outline-none"
                  placeholder="••••••••"
                />
              </div>

              <div v-if="passwordMessage" class="p-4 rounded-2xl border text-sm font-bold text-center" :class="passwordError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
                {{ passwordMessage }}
              </div>

              <button
                type="submit"
                :disabled="changingPassword"
                class="w-full bg-white text-[#082540] py-4 rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-gray-100 transition-all shadow-xl active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {{ changingPassword ? 'Envoi...' : 'Mettre à jour' }}
              </button>
            </form>
          </div>
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
const fileInput = ref<HTMLInputElement | null>(null)

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
    // Nettoyer les données avant l'envoi
    const payload = {
      firstName: profileForm.value.firstName,
      lastName: profileForm.value.lastName,
      username: profileForm.value.userName,
      bio: profileForm.value.bio,
      dateOfBirth: profileForm.value.dateOfBirth || null // Utiliser null si vide pour éviter l'erreur de conversion DateTime
    }

    await api.updateProfile(payload)
    
    // Mettre à jour le store d'authentification immédiatement pour refléter les changements (ex: username dans le header)
    await authStore.fetchUserProfile()
    
    profileMessage.value = 'Profil mis à jour avec succès !'
    
    // Rafraîchir les données locales
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

