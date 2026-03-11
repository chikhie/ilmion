<template>
  <div class="w-full mb-4">
    
    <!-- Widget Container -->
    <div class="w-full">
        
        <!-- Main Panel -->
        <div class="w-full bg-[#1e1f22]/95 backdrop-blur-md border border-[#00B578]/30 p-4 rounded-2xl shadow-xl transition-all duration-300">
            
            <!-- Not Connected State -->
            <div v-if="!isConnected" class="flex flex-col md:flex-row gap-3 justify-center items-center">
                 <button @click="startJoin" class="w-full md:w-auto px-6 bg-[#2b2d31] text-gray-300 py-2 rounded-xl font-bold font-sans shadow-lg hover:bg-[#1e1f22] hover:scale-[1.02] transition-all border border-[#00B578]/20 flex items-center justify-center gap-2 text-sm">
                    <svg class="w-4 h-4 opacity-60" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"/></svg>
                    Rejoindre un groupe
                 </button>
                 <button @click="createGroup" class="w-full md:w-auto px-6 bg-[#00B578] text-[#1e1f22] py-2 rounded-xl font-bold font-sans shadow-lg hover:bg-white hover:scale-[1.02] transition-all flex items-center justify-center gap-2 text-sm">
                     <svg class="w-4 h-4 opacity-60" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"/></svg>
                     Créer un groupe
                 </button>
            </div>

            <!-- Active Lobby State -->
            <div v-else>
                 <div class="flex justify-between items-center mb-2 border-b border-[#00B578]/10 pb-2">
                     <h3 class="font-sans font-bold text-lg text-[#00B578] flex items-center gap-2">
                         <span class="w-2 h-2 rounded-full bg-green-500 animate-pulse"></span>
                         Mon Groupe
                     </h3>
                     <div class="flex gap-2">
                         <button @click="leaveGroup" class="text-red-400 hover:text-red-300 bg-red-400/10 hover:bg-red-400/20 rounded-lg p-1.5 transition-colors" title="Quitter">
                            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/></svg>
                         </button>
                     </div>
                 </div>

                 <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                     <!-- CODE DISPLAY -->
                     <div class="bg-black/40 rounded-xl p-3 text-center cursor-pointer group hover:bg-black/50 transition-colors border-2 h-full flex flex-col justify-center" :class="copied ? 'border-green-500/50' : 'border-dashed border-[#2b2d31]/30'" @click="copyCode">
                         <p class="text-[9px] uppercase tracking-widest text-[#2b2d31] mb-0.5 font-bold">Code d'invitation</p>
                         <p class="text-xl font-mono font-bold text-white tracking-wider group-active:scale-95 transition-transform">{{ currentCode }}</p>
                         <p class="text-[9px] mt-1 font-bold transition-colors" :class="copied ? 'text-green-400' : 'text-[#2b2d31]/50 group-hover:text-[#00B578]'">
                             {{ copied ? 'COPIÉ !' : 'CLIQUER POUR COPIER' }}
                         </p>
                     </div>
     
                     <div class="space-y-2 max-h-[250px] overflow-y-auto custom-scrollbar pr-1">
                         <p class="text-xs font-bold text-[#2b2d31] uppercase tracking-wider mb-2 sticky top-0 bg-[#1e1f22]/95 py-1 z-10">Joueurs ({{ players.length }})</p>
                         
                         <div v-for="player in players" :key="player.username" class="flex items-center gap-3 bg-white/5 p-3 rounded-xl border border-white/5">
                             <div class="w-8 h-8 rounded-full flex items-center justify-center font-bold text-xs shadow-inner" :class="player.isHost ? 'bg-[#00B578] text-[#1e1f22]' : 'bg-[#2b2d31]/20 text-gray-300'">
                                 {{ player.username.substring(0, 2).toUpperCase() }}
                             </div>
                             <div class="flex-1 min-w-0">
                                 <p class="truncate font-bold text-sm text-gray-300">{{ player.username }}</p>
                             </div>
                             <span v-if="player.isHost" class="text-[9px] uppercase bg-[#00B578]/20 text-[#00B578] px-2 py-1 rounded-full font-bold self-center">Hôte</span>
                         </div>
                     </div>
                 </div>
            </div>
        </div>
    </div>

    <!-- Modals (Join/Create) - Kept largely same but styled better -->
    <Transition name="fade">
        <div v-if="showJoinModal || showCreateModal" class="fixed inset-0 bg-black/80 flex items-center justify-center z-[100] backdrop-blur-md p-4 pointer-events-auto">
            <div class="bg-[#2b2d31] p-8 rounded-3xl shadow-2xl max-w-sm w-full border border-[#00B578]/30 transform transition-all">
                <h3 class="text-3xl font-sans font-bold text-[#1e1f22] mb-6 text-center">{{ showCreateModal ? 'Créer un groupe' : 'Rejoindre' }}</h3>
                
                <div v-if="(!authStore.isLoggedIn && (showCreateModal || showJoinModal))" class="mb-5">
                    <label class="block text-[#2b2d31] text-xs font-bold uppercase tracking-widest mb-2">Choisir un Pseudo</label>
                    <input v-model="usernameInput" type="text" placeholder="Ex: Gandalf" class="w-full bg-white/80 border-2 border-[#2b2d31]/20 rounded-xl px-4 py-3 text-[#1e1f22] font-bold focus:outline-none focus:border-[#00B578] transition-colors text-lg" maxlength="15" />
                </div>

                <div v-if="showJoinModal" class="mb-6">
                    <label class="block text-[#2b2d31] text-xs font-bold uppercase tracking-widest mb-2">Code du groupe</label>
                    <input v-model="joinCodeInput" type="text" placeholder="A1B2C3" class="w-full bg-white/80 border-2 border-[#2b2d31]/20 rounded-xl px-4 py-4 text-[#1e1f22] font-bold font-mono text-center text-3xl focus:outline-none focus:border-[#00B578] uppercase tracking-[0.2em] transition-colors" maxlength="6" />
                </div>
                
                <div v-if="joinError" class="text-red-500 text-sm font-bold mb-4 text-center bg-red-50 p-2 rounded-lg border border-red-100 flex items-center justify-center gap-2">
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                    {{ joinError }}
                </div>
                
                <div class="flex gap-3 mt-2">
                    <button @click="closeModals" class="flex-1 py-4 text-[#2b2d31] font-bold hover:bg-[#2b2d31]/10 rounded-xl transition-colors">Annuler</button>
                    <button @click="showCreateModal ? confirmCreate() : confirmJoin()" 
                        :disabled="(showJoinModal && !joinCodeInput) || (!authStore.isLoggedIn && !usernameInput) || joining" 
                        class="flex-[2] py-4 bg-[#00B578] text-[#1e1f22] font-bold rounded-xl shadow-lg hover:shadow-xl hover:bg-white transition-all transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none">
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

const showJoinModal = ref(false)
const showCreateModal = ref(false)
const joinCodeInput = ref('')
const usernameInput = ref('')
const joinError = ref('')
const joining = ref(false)
const copied = ref(false)

const createGroup = async () => {
    if (authStore.isLoggedIn) {
        await createLobby()
    } else {
        usernameInput.value = ''
        showCreateModal.value = true
    }
}

const confirmCreate = async () => {
    if (!usernameInput.value) return
    await createLobby(usernameInput.value)
    showCreateModal.value = false
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
    } else {
        joinError.value = "Code invalide ou erreur de connexion."
    }
}

const leaveGroup = async () => {
    if (confirm("Voulez-vous vraiment quitter le groupe ?")) {
        await leaveLobby()
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

