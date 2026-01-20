<template>
  <div class="fixed z-50 transition-all duration-300 pointer-events-none w-full h-full inset-0 flex flex-col justify-end items-end p-4 md:justify-start md:items-end md:p-6">
    
    <!-- Widget Container (Pointer events enabled for children) -->
    <div class="pointer-events-auto relative flex flex-col items-end">
        
        <!-- Toggle Button (Mobile Only or Collapsed) -->
        <button v-if="!isExpanded && isConnected" @click="isExpanded = true" 
            class="md:hidden w-12 h-12 rounded-full bg-brand-gold text-brand-dark shadow-2xl border-2 border-white/20 flex items-center justify-center animate-bounce-subtle">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"/></svg>
        </button>

        <!-- Main Panel -->
        <div v-show="isExpanded || !isConnected || !isMobile" class="bg-brand-dark/95 backdrop-blur-md border border-brand-gold/30 p-5 rounded-2xl shadow-2xl w-full max-w-[320px] mb-20 md:mb-0 transition-all duration-300 origin-bottom-right md:origin-top-right">
            
            <!-- Not Connected State -->
            <div v-if="!isConnected" class="flex flex-col gap-3">
                 <button @click="startJoin" class="w-full bg-brand-wood text-brand-parchment py-3 rounded-xl font-bold font-serif-title shadow-lg hover:bg-brand-dark hover:scale-[1.02] transition-all border border-brand-gold/20 flex items-center justify-center gap-2">
                    <svg class="w-5 h-5 opacity-60" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"/></svg>
                    Rejoindre un groupe
                 </button>
                 <button @click="createGroup" class="w-full bg-brand-gold text-brand-dark py-3 rounded-xl font-bold font-serif-title shadow-lg hover:bg-white hover:scale-[1.02] transition-all flex items-center justify-center gap-2">
                     <svg class="w-5 h-5 opacity-60" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"/></svg>
                     Créer un groupe
                 </button>
            </div>

            <!-- Active Lobby State -->
            <div v-else>
                 <div class="flex justify-between items-center mb-4 border-b border-brand-gold/10 pb-3">
                     <h3 class="font-serif-title font-bold text-xl text-brand-gold flex items-center gap-2">
                         <span class="w-2 h-2 rounded-full bg-green-500 animate-pulse"></span>
                         Mon Groupe
                     </h3>
                     <div class="flex gap-2">
                         <button @click="isExpanded = false" class="md:hidden text-brand-wood hover:text-white p-1">
                             <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"/></svg>
                         </button>
                         <button @click="leaveGroup" class="text-red-400 hover:text-red-300 bg-red-400/10 hover:bg-red-400/20 rounded-lg p-2 transition-colors" title="Quitter">
                            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/></svg>
                         </button>
                     </div>
                 </div>

                 <!-- CODE DISPLAY -->
                 <div class="bg-black/40 rounded-xl p-4 mb-4 text-center cursor-pointer group hover:bg-black/50 transition-colors border-2" :class="copied ? 'border-green-500/50' : 'border-dashed border-brand-wood/30'" @click="copyCode">
                     <p class="text-[10px] uppercase tracking-widest text-brand-wood mb-1 font-bold">Code d'invitation</p>
                     <p class="text-4xl font-mono font-bold text-white tracking-wider group-active:scale-95 transition-transform">{{ currentCode }}</p>
                     <p class="text-[10px] mt-2 font-bold transition-colors" :class="copied ? 'text-green-400' : 'text-brand-wood/50 group-hover:text-brand-gold'">
                         {{ copied ? 'COPIÉ !' : 'CLIQUER POUR COPIER' }}
                     </p>
                 </div>
 
                 <div class="space-y-2 max-h-[250px] overflow-y-auto custom-scrollbar pr-1">
                     <p class="text-xs font-bold text-brand-wood uppercase tracking-wider mb-2 sticky top-0 bg-brand-dark/95 py-1 z-10">Joueurs ({{ players.length }})</p>
                     
                     <div v-for="player in players" :key="player.username" class="flex items-center gap-3 bg-white/5 p-3 rounded-xl border border-white/5">
                         <div class="w-8 h-8 rounded-full flex items-center justify-center font-bold text-xs shadow-inner" :class="player.isHost ? 'bg-brand-gold text-brand-dark' : 'bg-brand-wood/20 text-brand-parchment'">
                             {{ player.username.substring(0, 2).toUpperCase() }}
                         </div>
                         <div class="flex-1 min-w-0">
                             <p class="truncate font-bold text-sm text-brand-parchment">{{ player.username }}</p>
                         </div>
                         <span v-if="player.isHost" class="text-[9px] uppercase bg-brand-gold/20 text-brand-gold px-2 py-1 rounded-full font-bold self-center">Hôte</span>
                     </div>
                 </div>
            </div>
        </div>
    </div>

    <!-- Modals (Join/Create) - Kept largely same but styled better -->
    <Transition name="fade">
        <div v-if="showJoinModal || showCreateModal" class="fixed inset-0 bg-black/80 flex items-center justify-center z-[100] backdrop-blur-md p-4 pointer-events-auto">
            <div class="bg-texture-parchment p-8 rounded-3xl shadow-2xl max-w-sm w-full border border-brand-gold/30 transform transition-all">
                <h3 class="text-3xl font-serif-title font-bold text-brand-dark mb-6 text-center">{{ showCreateModal ? 'Créer un groupe' : 'Rejoindre' }}</h3>
                
                <div v-if="(!authStore.isLoggedIn && (showCreateModal || showJoinModal))" class="mb-5">
                    <label class="block text-brand-wood text-xs font-bold uppercase tracking-widest mb-2">Choisir un Pseudo</label>
                    <input v-model="usernameInput" type="text" placeholder="Ex: Gandalf" class="w-full bg-white/80 border-2 border-brand-wood/20 rounded-xl px-4 py-3 text-brand-dark font-bold focus:outline-none focus:border-brand-gold transition-colors text-lg" maxlength="15" />
                </div>

                <div v-if="showJoinModal" class="mb-6">
                    <label class="block text-brand-wood text-xs font-bold uppercase tracking-widest mb-2">Code du groupe</label>
                    <input v-model="joinCodeInput" type="text" placeholder="A1B2C3" class="w-full bg-white/80 border-2 border-brand-wood/20 rounded-xl px-4 py-4 text-brand-dark font-bold font-mono text-center text-3xl focus:outline-none focus:border-brand-gold uppercase tracking-[0.2em] transition-colors" maxlength="6" />
                </div>
                
                <div v-if="joinError" class="text-red-500 text-sm font-bold mb-4 text-center bg-red-50 p-2 rounded-lg border border-red-100 flex items-center justify-center gap-2">
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                    {{ joinError }}
                </div>
                
                <div class="flex gap-3 mt-2">
                    <button @click="closeModals" class="flex-1 py-4 text-brand-wood font-bold hover:bg-brand-wood/10 rounded-xl transition-colors">Annuler</button>
                    <button @click="showCreateModal ? confirmCreate() : confirmJoin()" 
                        :disabled="(showJoinModal && !joinCodeInput) || (!authStore.isLoggedIn && !usernameInput) || joining" 
                        class="flex-[2] py-4 bg-brand-gold text-brand-dark font-bold rounded-xl shadow-lg hover:shadow-xl hover:bg-white transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none">
                        {{ joining ? 'Chargement...' : (showCreateModal ? 'Commencer' : 'Valider') }}
                    </button>
                </div>
            </div>
        </div>
    </Transition>

  </div>
</template>

<script setup lang="ts">


const { isConnected, currentCode, players, createLobby, joinLobby, leaveLobby } = useMultiplayer()
const authStore = useAuthStore()

const isExpanded = ref(false)
const showJoinModal = ref(false)
const showCreateModal = ref(false)
const joinCodeInput = ref('')
const usernameInput = ref('')
const joinError = ref('')
const joining = ref(false)
const copied = ref(false)

const isMobile = ref(false)

onMounted(() => {
    const updateMobile = () => {
        isMobile.value = window.innerWidth < 768
    }
    updateMobile()
    window.addEventListener('resize', updateMobile)
    
    // Auto expand on desktop if connected
    if (isConnected.value && !isMobile.value) {
        isExpanded.value = true
    }
})

// Watch connect state 
watch(isConnected, (newVal) => {
    if (newVal) {
        if (!isMobile.value) isExpanded.value = true
        if (isMobile.value) isExpanded.value = false 
    }
})

const createGroup = async () => {
    if (authStore.isLoggedIn) {
        await createLobby()
        isExpanded.value = true
    } else {
        usernameInput.value = ''
        showCreateModal.value = true
    }
}

const confirmCreate = async () => {
    if (!usernameInput.value) return
    await createLobby(usernameInput.value)
    showCreateModal.value = false
    isExpanded.value = true
}

const startJoin = () => {
    usernameInput.value = ''
    joinCodeInput.value = ''
    joinError.value = ''
    showJoinModal.value = true
}

const closeModals = () => {
    showJoinModal.value = false
    showCreateModal.value = false
}

const confirmJoin = async () => {
    if (!joinCodeInput.value) return
    if (!authStore.isLoggedIn && !usernameInput.value) return

    joining.value = true
    joinError.value = ''
    
    const success = await joinLobby(joinCodeInput.value.toUpperCase(), usernameInput.value)
    joining.value = false
    
    if (success) {
        closeModals()
        joinCodeInput.value = ''
        isExpanded.value = true
    } else {
        joinError.value = "Code invalide ou erreur de connexion."
    }
}

const leaveGroup = async () => {
    if (confirm("Voulez-vous vraiment quitter le groupe ?")) {
        await leaveLobby()
        isExpanded.value = false
    }
}

const copyCode = () => {
    if (currentCode.value) {
        navigator.clipboard.writeText(currentCode.value)
        copied.value = true
        setTimeout(() => copied.value = false, 2000)
    }
}
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 4px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.05);
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: rgba(212, 163, 115, 0.3);
  border-radius: 4px;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>

