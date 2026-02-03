<template>
  <div class="min-h-screen bg-brand-dark flex flex-col relative overflow-hidden font-sans-body">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.03]" style="background-image: url('/patterns/arabesque.png')"></div>
        <div class="absolute top-[-20%] left-[-10%] w-[600px] h-[600px] bg-brand-gold/5 rounded-full blur-[120px] animate-pulse-slow"></div>
        <div class="absolute bottom-[-20%] right-[-10%] w-[600px] h-[600px] bg-brand-wood/10 rounded-full blur-[120px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Global Header -->
    <AppHeader />

    <main class="flex-grow container mx-auto px-4 py-8 relative z-10 flex flex-col gap-6">
      
      <!-- Page Title (Compact) -->
      <div class="flex items-center gap-4 mb-2 animate-fade-in">
        <div class="w-12 h-12 rounded-full bg-brand-wood/20 border border-brand-gold/20 flex items-center justify-center text-2xl shadow-sm backdrop-blur-sm text-brand-gold">
          <i class="fas fa-user"></i>
        </div>
        <div>
          <h1 class="text-2xl md:text-3xl font-serif-title font-bold text-brand-parchment tracking-wide">Mon Profil</h1>
          <p class="text-brand-parchment/50 text-sm">Gérez votre identité et vos préférences</p>
        </div>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center items-center py-20 bg-brand-wood/5 rounded-3xl border border-brand-gold/5">
        <div class="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-brand-gold/50"></div>
      </div>

      <!-- Main Grid Layout -->
      <!-- Main Grid Layout -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-2 gap-6 animate-fade-in-up">
        
        <!-- Left Column: Identity & Personal Info -->
        <div class="space-y-6">
            
            <!-- Identity Card -->
            <div class="lg:col-span-1 space-y-4">
                <div class="p-4 backdrop-blur-md relative overflow-hidden group hover:border-brand-gold/30 transition-colors">
                    <!-- Decorative Glow -->
                    <div class="absolute top-0 right-0 w-24 h-24 bg-brand-gold/5 rounded-full -mr-25 -mt-25 blur-2xl group-hover:bg-brand-gold/10 transition-all duration-500"></div>
    
                    <div class="flex flex-col items-center text-center">
                        <div class="relative mb-3 group/photo cursor-pointer" @click="fileInput?.click()">
                            <img
                                v-if="user?.profilePicture"
                                :src="user.profilePicture"
                                alt="Photo de profil"
                                class="w-24 h-24 rounded-full object-cover border-2 border-brand-gold/20 shadow-lg group-hover/photo:border-brand-gold transition-colors"
                            />
                            <div v-else class="w-24 h-24 rounded-full bg-brand-dark/50 border-2 border-brand-gold/20 flex items-center justify-center shadow-lg group-hover/photo:border-brand-gold transition-colors">
                                <span class="text-3xl font-serif-title font-bold text-brand-gold/50">{{ getInitials() }}</span>
                            </div>
                            
                            <!-- Edit Badge (Persistent) -->
                            <div class="absolute bottom-0 right-0 bg-brand-gold text-brand-dark w-7 h-7 rounded-full flex items-center justify-center shadow-md border-2 border-brand-dark group-hover/photo:scale-110 transition-transform">
                                <i class="fas fa-pen text-[0.6rem]"></i>
                            </div>
    
                            <!-- Overlay Icon -->
                            <div class="absolute inset-0 bg-black/40 rounded-full opacity-0 group-hover/photo:opacity-100 flex items-center justify-center transition-opacity backdrop-blur-[1px]">
                                <i class="fas fa-camera text-white text-sm"></i>
                            </div>
                        </div>
    
                        <input
                            ref="fileInput"
                            type="file"
                            accept="image/*"
                            class="hidden"
                            @change="handleFileSelect"
                        />
    
                        <div class="flex gap-2 w-full justify-center" v-if="user?.profilePicture">
                             <button
                                @click="deletePhoto"
                                class="py-1.5 px-3 rounded-lg border border-red-500/20 text-red-400 bg-red-500/5 hover:bg-red-500/10 text-[0.65rem] font-bold uppercase tracking-wider transition-colors"
                            >
                                Supprimer photo
                            </button>
                        </div>
    
                        <div v-if="uploadMessage" class="mt-2 w-full p-2 rounded-lg border text-[0.65rem] font-bold text-center" :class="uploadError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
                            {{ uploadMessage }}
                        </div>
                    </div>
                </div>
            </div>

            <!-- Personal Info Form -->
            <div class="bg-brand-wood/10 border border-brand-gold/10 rounded-2xl p-6 md:p-8 backdrop-blur-md shadow-sm">
                <h3 class="text-lg font-serif-title font-bold text-brand-parchment mb-6 flex items-center gap-2">
                    <i class="fas fa-address-card text-brand-gold/60 text-sm"></i>
                    Informations Personnelles
                </h3>
                
                <form @submit.prevent="saveProfile" class="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Prénom</label>
                    <input v-model="profileForm.firstName" type="text" class="input-field" placeholder="Votre prénom" />
                  </div>

                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Nom</label>
                    <input v-model="profileForm.lastName" type="text" class="input-field" placeholder="Votre nom" />
                  </div>

                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Pseudonyme</label>
                    <input v-model="profileForm.userName" type="text" class="input-field" placeholder="Votre pseudo" />
                  </div>

                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Email</label>
                    <input :value="user?.email" type="email" disabled class="input-field opacity-60 cursor-not-allowed bg-brand-dark/50" />
                  </div>

                  <div class="md:col-span-2 pt-2">
                      <div v-if="profileMessage" class="mb-4 p-3 rounded-xl border text-xs font-bold text-center" :class="profileError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
                        {{ profileMessage }}
                      </div>
                      <button type="submit" :disabled="savingProfile" class="btn-primary w-full md:w-auto md:px-12 md:float-right">
                        {{ savingProfile ? 'Enregistrement...' : 'Enregistrer' }}
                      </button>
                  </div>
                </form>
            </div>
        </div>

        <!-- Right Column: Security (Full Height) -->
        <div class="space-y-6">
            <div class="bg-brand-wood/10 border border-brand-gold/10 rounded-2xl p-6 md:p-8 backdrop-blur-md shadow-sm h-full">
                <h3 class="text-lg font-serif-title font-bold text-brand-parchment mb-6 flex items-center gap-2">
                    <i class="fas fa-lock text-brand-gold/60 text-sm"></i>
                    Sécurité
                </h3>
                
                <form @submit.prevent="changePassword" class="flex flex-col gap-6 h-full">
                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Actuel</label>
                    <input v-model="passwordForm.currentPassword" type="password" class="input-field" placeholder="••••••••" />
                  </div>

                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Nouveau</label>
                    <input v-model="passwordForm.newPassword" type="password" class="input-field" placeholder="••••••••" />
                  </div>

                  <div class="space-y-1.5">
                    <label class="text-[10px] font-bold uppercase tracking-widest text-brand-parchment/50 ml-1">Confirmation</label>
                    <input v-model="passwordForm.confirmPassword" type="password" class="input-field" placeholder="••••••••" />
                  </div>

                  <div class="pt-2">
                     <div v-if="passwordMessage" class="mb-4 p-3 rounded-xl border text-xs font-bold text-center" :class="passwordError ? 'bg-red-500/10 border-red-500/20 text-red-400' : 'bg-green-500/10 border-green-500/20 text-green-400'">
                        {{ passwordMessage }}
                      </div>
                       <button type="submit" :disabled="changingPassword" class="btn-secondary w-full md:w-auto md:px-12 md:float-right">
                        {{ changingPassword ? 'Mise à jour...' : 'Modifier le mot de passe' }}
                      </button>
                  </div>
                </form>
            </div>
        </div>
      </div>
    </main>

    <!-- Global Footer -->
    <AppFooter />

  </div>
</template>

<script setup lang="ts">
import type { User } from '~/types'

definePageMeta({
  middleware: 'auth',
  layout: false // Custom layout control
})

useHead({
  title: 'Mon profil - Ilmanar'
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
  
  if (!authStore.isLoggedIn) {
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
    
    // Mettre à jour le store global pour la navbar
    await authStore.fetchUserProfile()
    
    uploadMessage.value = 'Photo mise à jour !'
  } catch (error: any) {
    uploadError.value = true
    uploadMessage.value = error.data?.message || 'Erreur upload'
  } finally {
    uploading.value = false
  }
}

const deletePhoto = async () => {
  if (!confirm('Supprimer votre photo ?')) return

  try {
    await api.deleteProfilePicture()
    user.value!.profilePicture = undefined
    await authStore.fetchUserProfile()
    uploadMessage.value = 'Photo supprimée'
    uploadError.value = false
  } catch (error) {
    uploadError.value = true
    uploadMessage.value = 'Erreur suppression'
  }
}

const saveProfile = async () => {
  savingProfile.value = true
  profileMessage.value = ''
  profileError.value = false

  try {
    const payload = {
      firstName: profileForm.value.firstName,
      lastName: profileForm.value.lastName,
      username: profileForm.value.userName,
      bio: profileForm.value.bio,
      dateOfBirth: profileForm.value.dateOfBirth || null
    }

    await api.updateProfile(payload)
    await authStore.fetchUserProfile()
    profileMessage.value = 'Profil enregistré.'
    user.value = await api.getProfile()
  } catch (error: any) {
    profileError.value = true
    profileMessage.value = error.data?.message || 'Erreur sauvegarde'
  } finally {
    savingProfile.value = false
  }
}

const changePassword = async () => {
  passwordMessage.value = ''
  passwordError.value = false

  if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
    passwordError.value = true
    passwordMessage.value = 'Mots de passe différents'
    return
  }

  if (passwordForm.value.newPassword.length < 6) {
    passwordError.value = true
    passwordMessage.value = 'Min 6 caractères'
    return
  }

  changingPassword.value = true

  try {
    await api.changePassword({
      currentPassword: passwordForm.value.currentPassword,
      newPassword: passwordForm.value.newPassword
    })
    
    passwordMessage.value = 'Mot de passe modifié.'
    passwordForm.value = { currentPassword: '', newPassword: '', confirmPassword: '' }
  } catch (error: any) {
    passwordError.value = true
    passwordMessage.value = error.data?.message || 'Erreur modification'
  } finally {
    changingPassword.value = false
  }
}
</script>

<style scoped>
.input-field {
    @apply w-full px-4 py-3 bg-brand-dark/30 border border-brand-gold/10 rounded-xl text-brand-parchment placeholder-brand-parchment/30 focus:border-brand-gold/50 focus:bg-brand-dark/50 transition-all outline-none text-sm;
}

.btn-primary {
    @apply py-3 px-6 bg-brand-gold text-brand-dark rounded-xl font-bold text-sm uppercase tracking-widest hover:bg-white transition-all shadow-lg active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed;
}

.btn-secondary {
    @apply py-3 px-6 bg-brand-wood/20 text-brand-parchment border border-brand-gold/20 rounded-xl font-bold text-sm uppercase tracking-widest hover:bg-brand-wood/40 hover:border-brand-gold/40 transition-all shadow-md active:scale-95 disabled:opacity-50 disabled:cursor-not-allowed;
}

.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
.animate-fade-in { animation: fadeIn 0.6s ease-out forwards; opacity: 0; }
.animate-fade-in-up { animation: fadeInUp 0.6s ease-out forwards; opacity: 0; }

@keyframes pulse { 0%, 100% { opacity: 0.2; } 50% { opacity: 0.1; } }
@keyframes fadeIn { to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
</style>

