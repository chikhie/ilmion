<template>
  <div class="max-w-4xl mx-auto font-sans">
    <!-- Outer Card with Pattern Border (Matching Mockup) -->
    <div class="bg-[#Fdfbf7] p-2 rounded-2xl md:rounded-[2.5rem] shadow-2xl relative overflow-hidden transform transition-all hover:scale-[1.005] duration-500 border border-[#00B578]/20">
        <!-- Pattern Texture on Border -->
        <div class="absolute inset-0 pattern-geometric opacity-10 pointer-events-none"></div>

        <!-- Global Multiplayer Error Overlay -->
        <div v-if="isMultiplayer && mpError" class="absolute inset-0 z-50 flex items-center justify-center bg-white/95 backdrop-blur-sm rounded-xl md:rounded-[2rem]">
            <div class="text-center p-8 max-w-md animate-scale-in">
                <div class="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
                    <svg class="w-8 h-8 text-red-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                </div>
                <h3 class="text-2xl font-sans font-bold text-[#1e1f22] mb-2">Fin de la partie</h3>
                <p class="text-[#2b2d31] font-medium mb-8 leading-relaxed">{{ mpError }}</p>
                <button @click="handleClose" class="px-8 py-3 bg-[#1e1f22] text-gray-300 rounded-xl font-bold uppercase tracking-widest shadow-lg hover:bg-black transition-all">
                    Retour aux jeux
                </button>
            </div>
        </div>
        
        <!-- Inner Card with Line Border -->
        <div class="bg-white rounded-xl md:rounded-[2rem] border-2 border-[#1e1f22]/80 p-3 md:p-10 relative z-10 flex flex-col min-h-[350px] md:min-h-[600px]">
            
            <!-- Game Header (Compact) -->
            <div class="flex justify-between items-center mb-2 md:mb-8 md:border-b md:border-[#2b2d31]/10 md:pb-6 relative w-full">
                <!-- Left: Timer / Lobby Status -->
                <div class="flex items-center gap-2">
                     <div v-if="!showResult && !answered && (!isMultiplayer || mpGameStarted)" class="w-10 h-10 md:w-12 md:h-12 rounded-full border-2 flex items-center justify-center font-bold text-sm md:text-lg shadow-inner bg-gray-50"
                          :class="timerClass">
                        {{ timeRemaining }}
                     </div>
                     <div v-else-if="isMultiplayer && !mpGameStarted">
                        <!-- Removed 'En Ligne' label as requested -->
                     </div>
                </div>

                <!-- Right: Score / Player Count -->
                <div class="flex flex-col items-end">
                    <div class="text-base md:text-xl font-bold text-[#1e1f22]">
                         <span v-if="!(isMultiplayer && !mpGameStarted)">
                             <span class="text-[#00B578]">{{ score }}</span><span class="text-gray-400 text-xs mx-0.5">/</span>{{ questions.length }}
                         </span>
                    </div>
                </div>
            </div>

            <!-- Loading State -->
            <div v-if="loading" class="text-center py-20 animate-pulse">
               <p class="text-[#2b2d31] font-sans italic text-xl">Recherche dans les archives...</p>
            </div>

            <!-- MULTIPLAYER LOBBY -->
            <div v-else-if="isMultiplayer && !mpGameStarted" class="flex-grow flex flex-col items-center justify-center animate-fade-in relative z-10 w-full">
                
                <!-- GUEST USERNAME PROMPT -->
                <div v-if="!hasMultiplayerIdentity" class="w-full max-w-sm bg-white p-8 rounded-2xl shadow-xl border border-[#00B578]/20 text-center">
                     <h3 class="text-xl font-sans font-bold text-[#1e1f22] mb-4">Qui êtes-vous ?</h3>
                     <p class="text-sm text-[#2b2d31]/60 mb-6">Entrez un nom pour rejoindre la caravane</p>
                     
                     <input v-model="guestUsername" @keyup.enter="confirmIdentity" placeholder="Votre nom..." class="w-full p-4 mb-4 bg-gray-50 border-2 border-[#2b2d31]/10 rounded-xl text-center font-bold text-gray-900 focus:border-[#00B578] outline-none" maxlength="15" />
                     
                     <button @click="confirmIdentity" :disabled="!guestUsername" class="w-full py-3 bg-[#1e1f22] text-gray-300 rounded-xl font-bold uppercase tracking-widest hover:bg-black disabled:opacity-50 transition-all">
                         Continuer
                     </button>
                </div>

                <!-- Initial Choice (Lobby Actions) -->
                <div v-else-if="!currentCode" class="w-full max-w-md space-y-6">
                    <h3 class="text-3xl font-sans font-bold text-center text-[#1e1f22] mb-8">Rejoindre la Caravane</h3>
                    
                    <button @click="handleCreateLobby" class="w-full py-5 bg-[#1e1f22] text-gray-300 rounded-2xl font-bold uppercase tracking-widest shadow-lg hover:bg-black hover:scale-[1.02] transition-all flex items-center justify-center gap-3">
                         <span>Créer une partie</span>
                         <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"/></svg>
                    </button>
                    
                    <div class="relative">
                        <div class="absolute inset-0 flex items-center"><div class="w-full border-t border-[#2b2d31]/20"></div></div>
                        <div class="relative flex justify-center text-xs uppercase tracking-widest text-[#2b2d31]/60"><span class="bg-white px-2">Ou</span></div>
                    </div>

                    <div class="flex flex-col sm:flex-row gap-2">
                        <input v-model="joinCodeDisplay" placeholder="Code de partie..." class="flex-1 p-4 bg-gray-50 border-2 border-[#2b2d31]/10 rounded-xl text-center font-bold uppercase tracking-widest text-[#1e1f22] focus:border-[#00B578] outline-none" maxlength="6" />
                        <button @click="handleJoinLobby" :disabled="!joinCodeDisplay" class="w-full sm:w-auto px-6 py-4 bg-[#2b2d31] text-gray-300 rounded-xl font-bold uppercase tracking-widest hover:bg-[#00B578] disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
                            Rejoindre
                        </button>
                    </div>
                    
                    <p v-if="mpError" class="text-red-500 text-center text-sm font-bold mt-4">{{ mpError }}</p>
                </div>

                <!-- Waiting Room -->
                <div v-else class="w-full max-w-lg text-center">
                    <div class="bg-[#00B578]/10 rounded-2xl p-6 mb-8 border border-[#00B578]/30 relative group">
                        <p class="text-xs uppercase tracking-widest text-[#2b2d31]/60 mb-1">Code de la partie</p>
                        <p class="text-2xl md:text-3xl font-sans font-bold text-[#1e1f22] tracking-[0.2em] mb-4">{{ currentCode }}</p>
                        
                        <div class="flex justify-center gap-3 mt-2 transition-opacity duration-300">
                             <button @click="copyCode" class="text-xs font-bold uppercase tracking-widest text-[#2b2d31] hover:text-[#1e1f22] flex items-center gap-1 px-3 py-2 bg-white/50 rounded-lg hover:bg-white transition-colors">
                                 <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 5H6a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2v-1M8 5a2 2 0 002 2h2a2 2 0 002-2M8 5a2 2 0 012-2h2a2 2 0 012 2m0 0h2a2 2 0 012 2v3m2 4H10m0 0l3-3m-3 3l3 3" /></svg>
                                 Copier
                             </button>
                             <button @click="copyInviteLink" class="text-xs font-bold uppercase tracking-widest text-[#2b2d31] hover:text-[#1e1f22] flex items-center gap-1 px-3 py-2 bg-white/50 rounded-lg hover:bg-white transition-colors">
                                 <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z" /></svg>
                                 Inviter
                             </button>
                        </div>
                    </div>

                    <h4 class="text-sm font-bold uppercase tracking-widest text-[#2b2d31] mb-4">Joueurs en attente ({{ players.length }})</h4>
                    
                    <div class="flex flex-wrap justify-center gap-2 mb-6">
                        <div v-for="player in players" :key="player.connectionId" class="bg-gray-50 px-3 py-1.5 rounded-lg border border-gray-100 flex items-center gap-2 animate-scale-in">
                            <div class="w-5 h-5 rounded-full bg-[#1e1f22] text-gray-300 flex items-center justify-center font-bold text-[10px]">
                                {{ player.username.charAt(0).toUpperCase() }}
                            </div>
                            <span class="text-xs font-bold text-gray-900 truncate max-w-[80px]">{{ player.username }}</span>
                        </div>
                    </div>
                    
                    <div class="space-y-3">
                         <button v-if="isHost" @click="handleStartGame" class="w-full py-4 bg-[#00B578] text-[#1e1f22] rounded-xl font-bold uppercase tracking-widest shadow-lg hover:bg-yellow-500 hover:scale-[1.02] transition-all relative overflow-hidden group">
                             <span class="relative z-10">Lancer la caravane</span>
                             <div class="absolute inset-0 bg-white/20 translate-y-full group-hover:translate-y-0 transition-transform"></div>
                         </button>
                         <p class="text-xs text-[#2b2d31]/50 italic">{{ isHost ? 'Attendez que tous les joueurs soient prêts' : 'En attente du chef de caravane...' }}</p>
                    </div>
                </div>
            </div>

            <!-- Result Screen -->
            <div v-else-if="showResult" class="text-center flex-grow flex flex-col justify-center items-center py-12 animate-fade-in relative z-10">
                <div class="mb-8 relative">
                    <div class="absolute inset-0 bg-[#00B578]/10 rounded-full blur-2xl animate-pulse"></div>
                    <span class="text-8xl drop-shadow-xl relative z-10 block animate-bounce-slow">
                        {{ score >= questions.length / 2 ? '🏆' : '🕯️' }}
                    </span>
                 </div>
                 
                 <h3 class="text-4xl font-sans font-bold text-[#1e1f22] mb-4">Quiz Terminé</h3>
                 <p class="text-[#2b2d31] font-medium text-lg mb-8 max-w-md mx-auto leading-relaxed">
                    {{ getScoreMessage() }}
                 </p>

                 <!-- Multiplayer Leaderboard Summary (Simple) -->
                 <div v-if="isMultiplayer" class="w-full max-w-md mb-8 bg-gray-50 rounded-2xl p-4 border border-gray-100">
                     <h4 class="text-xs uppercase tracking-widest text-[#2b2d31]/60 mb-4">Classement de la Caravane</h4>
                     <div class="space-y-2">
                         <div v-for="(p, i) in sortedPlayers" :key="p.connectionId" class="flex justify-between items-center p-2 rounded-lg" :class="players[i]?.connectionId === myConnectionId ? 'bg-[#00B578]/20 border border-[#00B578]/30' : ''">
                             <div class="flex items-center gap-2">
                                 <span class="font-bold text-[#1e1f22] w-6">{{ i + 1 }}.</span>
                                 <span class="text-sm font-medium text-gray-900">{{ p.username }}</span>
                             </div>
                             <span class="font-bold text-[#1e1f22]">{{ p.score }} pts</span>
                         </div>
                     </div>
                 </div>
                 
                    <button @click="handleShareResult" class="w-full py-4 mb-4 bg-[#00B578] text-[#1e1f22] rounded-xl font-bold uppercase tracking-widest shadow-lg hover:bg-[#d4a81e] transition-all flex items-center justify-center gap-2 group relative overflow-hidden">
                        <span class="relative z-10">Partager mon résultat</span>
                        <svg class="w-5 h-5 relative z-10" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z" /></svg>
                        <div class="absolute inset-0 bg-white/20 translate-y-full group-hover:translate-y-0 transition-transform duration-300"></div>
                     </button>

                     <div class="flex flex-col sm:flex-row gap-4 w-full max-w-md mx-auto">
                        <button @click="handleRetry" class="flex-1 py-4 border-2 border-[#2b2d31]/20 text-[#2b2d31] rounded-xl font-bold hover:bg-[#2b2d31]/5 hover:border-[#2b2d31]/40 transition-all uppercase tracking-widest text-xs">
                          Recommencer
                        </button>
                        <button @click="handleClose" class="flex-1 py-4 bg-[#1e1f22] text-gray-300 rounded-xl font-bold hover:bg-black transition-all shadow-xl hover:shadow-2xl uppercase tracking-widest text-xs">
                          Retour aux jeux
                        </button>
                     </div>
            </div>

            <!-- Question View (Standard Game) -->
            <div v-else class="flex-grow flex flex-col relative z-10">
                
                <!-- Question Text -->
                <h3 class="text-2xl md:text-3xl font-sans font-bold text-center text-[#1e1f22] mb-10 leading-relaxed drop-shadow-sm min-h-[4rem] flex items-center justify-center">
                  {{ currentQuestion.text }}
                </h3>

                <!-- Choice Question -->
                <div v-if="currentQuestion.type === 'choice'" class="grid gap-4 max-w-2xl mx-auto w-full">
                  <button 
                    v-for="(option, idx) in currentQuestion.options" 
                    :key="option"
                    @click="selectOption(option)"
                    :disabled="answered"
                    class="w-full relative group transition-all duration-300"
                  >
                        <!-- Ornamental Button Container (Chamfered) -->
                        <div class="relative overflow-hidden rounded-xl border-2 transition-all duration-300 shadow-sm group-hover:shadow-md"
                             :class="getOptionStyles(option).container">
                             
                             <!-- Button Content -->
                             <div class="relative z-10 px-6 py-5 flex items-center w-full">
                                 <!-- Letter Block -->
                                 <span class="mr-5 font-sans font-bold opacity-60 text-sm tracking-wider w-6">
                                     {{ String.fromCharCode(65 + (idx as number)) }}.
                                 </span>
                                 
                                 <!-- Text -->
                                 <span class="font-medium text-lg flex-1 text-left" :class="getOptionStyles(option).text">
                                    {{ option }}
                                 </span>
                                 
                                 <!-- Status Icons -->
                                 <div v-if="answered && showFeedback && option === currentQuestion.correctAnswer" class="ml-2 text-green-600 bg-green-100 rounded-full p-1 animate-scale-in">
                                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" /></svg>
                                 </div>
                                 <div v-else-if="answered && showFeedback && option === selectedAnswer && option !== currentQuestion.correctAnswer" class="ml-2 text-red-600 bg-red-100 rounded-full p-1 animate-scale-in">
                                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M6 18L18 6M6 6l12 12" /></svg>
                                 </div>
                             </div>

                             <!-- Decorative Corners (CSS) -->
                             <div class="absolute top-0 left-0 w-2 h-2 border-t-2 border-l-2 transition-colors" :class="getOptionStyles(option).corner"></div>
                             <div class="absolute top-0 right-0 w-2 h-2 border-t-2 border-r-2 transition-colors" :class="getOptionStyles(option).corner"></div>
                             <div class="absolute bottom-0 left-0 w-2 h-2 border-b-2 border-l-2 transition-colors" :class="getOptionStyles(option).corner"></div>
                             <div class="absolute bottom-0 right-0 w-2 h-2 border-b-2 border-r-2 transition-colors" :class="getOptionStyles(option).corner"></div>
                        </div>
                  </button>
                </div>

                <!-- Number Question -->
                <div v-if="currentQuestion.type === 'number'" class="mt-4 max-w-xl mx-auto w-full">
                  <div class="relative">
                      <input 
                        type="number" 
                        v-model="numberInput" 
                        placeholder="?"
                        :disabled="answered"
                        @keyup.enter="submitNumber"
                        class="w-full p-8 bg-[#F0F4F8] border-2 border-[#2b2d31]/20 rounded-[1.5rem] text-[#1e1f22] focus:border-[#00B578] focus:bg-white transition-all text-center text-5xl font-sans font-bold tracking-tighter outline-none shadow-inner"
                        :class="{ 
                            'border-green-500 bg-green-50': answered && showFeedback && isCorrect, 
                            'border-red-500 bg-red-50': answered && showFeedback && !isCorrect,
                            'border-[#00B578] bg-white shadow-md': answered && !showFeedback 
                        }"
                      />
                      <div class="absolute inset-0 pointer-events-none border border-black/5 rounded-[1.5rem]"></div>
                  </div>
                  
                  <div v-if="!answered" class="mt-8 text-center">
                    <button @click="submitNumber" class="px-12 py-4 bg-[#1e1f22] text-gray-300 rounded-xl font-bold shadow-lg hover:shadow-xl hover:scale-105 transition-all uppercase tracking-widest text-sm relative overflow-hidden group">
                      <span class="relative z-10">Valider ma réponse</span>
                      <div class="absolute inset-0 bg-[#00B578] opacity-0 group-hover:opacity-10 transition-opacity"></div>
                    </button>
                  </div>
                </div>

                <!-- Feedback Section -->
                <div v-if="answered" class="mt-8 mx-auto max-w-2xl w-full animate-fade-in-up">
                    
                    <!-- Waiting for others message (Sync Mode) -->
                    <div v-if="isMultiplayer && !showFeedback" class="bg-[#2b2d31]/5 rounded-2xl p-6 border border-[#2b2d31]/10 text-center animate-pulse">
                         <p class="text-[#2b2d31] font-medium italic text-sm">
                            En attente des autres voyageurs...
                         </p>
                    </div>

                    <!-- Result (Only when showFeedback is true) -->
                    <div v-else class="bg-[#2b2d31]/5 rounded-2xl p-6 border border-[#2b2d31]/10 flex gap-4 items-start relative overflow-hidden">
                        <div class="absolute top-0 left-0 w-1 h-full" :class="isCorrect ? 'bg-green-500' : 'bg-red-500'"></div>
                        
                        <div class="flex-1 pl-2">
                             <div class="flex justify-between items-center mb-2">
                                 <h4 class="font-bold uppercase tracking-wider text-xs" :class="isCorrect ? 'text-green-700' : 'text-red-700'">
                                    {{ isCorrect ? 'Excellente réponse !' : 'Incorrect' }}
                                 </h4>
                                 <span v-if="!isCorrect" class="text-xs text-[#1e1f22] font-bold bg-white px-2 py-1 rounded border border-[#1e1f22]/10">
                                     Réponse : {{ currentQuestion.correctAnswer }}
                                 </span>
                             </div>
                             
                             <p class="text-[#2b2d31] font-medium italic text-sm leading-relaxed">
                                {{ currentQuestion.explanation }}
                             </p>
                        </div>
                    </div>
                    
                    <!-- Next Button (Host Only in MP, or Everyone if Single) -->
                    <div v-if="showFeedback" class="mt-6 flex justify-center">
                         <button 
                            v-if="!isMultiplayer || isHost"
                            @click="handleNextQuestionClick" 
                            class="px-8 py-3 bg-[#00B578] text-[#1e1f22] font-bold rounded-full shadow-lg hover:bg-[#d4a81e] transition-all flex items-center gap-2 group animate-bounce-subtle">
                            <span>{{ currentQuestionIndex < questions.length - 1 ? 'Question suivante' : 'Voir les résultats' }}</span>
                            <svg class="w-4 h-4 transition-transform group-hover:translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
                         </button>
                         <p v-else class="text-xs text-[#2b2d31]/50 italic mt-2 animate-pulse">En attente du chef de caravane...</p>
                    </div>
                </div>

                <!-- Footer Slider Progress -->
                <div class="mt-auto pt-10 px-4">
                    <div class="w-full h-1 bg-[#1e1f22]/5 rounded-full relative">
                        <!-- Filled Part -->
                        <div class="absolute top-0 left-0 h-full bg-[#00B578] rounded-full transition-all duration-700"
                             :style="{ width: `${((currentQuestionIndex + 1) / questions.length) * 100}%` }"></div>
                        <!-- Slider Knot -->
                        <div class="absolute top-1/2 w-6 h-6 bg-white border-2 border-[#00B578] rounded-full shadow-md flex items-center justify-center transition-all duration-700 transform -translate-y-1/2 -translate-x-1/2 z-10"
                             :style="{ left: `${((currentQuestionIndex + 1) / questions.length) * 100}%` }">
                             <div class="w-1.5 h-1.5 bg-[#00B578] rounded-full"></div>
                        </div>
                    </div>
                </div>

            </div> <!-- Close Question View -->
        </div> <!-- Close Inner Card -->
        
        <!-- Toast Notification -->
        <div v-if="showToast" class="fixed top-24 left-1/2 transform -translate-x-1/2 z-50 animate-fade-in-down">
             <div class="px-6 py-4 bg-[#1e1f22]/95 backdrop-blur-md border border-[#00B578] rounded-full shadow-2xl flex items-center gap-3">
                 <div class="bg-green-500/20 p-1 rounded-full">
                     <svg class="w-5 h-5 text-[#00B578]" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                 </div>
                 <span class="text-gray-300 font-bold text-sm tracking-wide">Résultat copié !</span>
             </div>
        </div>

    </div> <!-- Close Pattern Card -->
  </div> <!-- Close Root -->
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick, watch } from 'vue'

const props = defineProps({
  gameContent: {
    type: [String, Object], // Accept both
    required: false,
    default: null
  },
  questions: {
    type: Array,
    required: false,
    default: () => []
  },
  title: {
    type: String,
    default: 'Quiz'
  },
  isMultiplayer: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['close', 'retry'])

// Composables
const route = useRoute()
const { 
    createLobby, 
    joinLobby, 
    startGame: mpStartGame, 
    updateScore: mpUpdateScore, 
    submitAnswer: mpSubmitAnswer,
    requestNextQuestion: mpRequestNextQuestion,
    currentCode, 
    players, 
    gameStarted: mpGameStarted, 
    error: mpError,
    disconnect: mpDisconnect,
    isHost,
    showReveal: mpShowReveal,
    moveToNext: mpMoveToNext,
    mpQuestions
} = useMultiplayer()

// State
const loading = ref(false)
const currentQuestionIndex = ref(0)
const score = ref(0)
const showResult = ref(false)
const answered = ref(false) // User has clicked
const showFeedback = ref(false) // Results are revealed
const selectedAnswer = ref<string | null>(null)
const numberInput = ref('')
const isCorrect = ref(false)
const joinCodeDisplay = ref('')
const guestUsername = ref('')
const hasPseudo = ref(false) // Track if guest pseudo is set
const showToast = ref(false)

const authStore = useAuthStore()
const hasMultiplayerIdentity = computed(() => {
    return !!authStore.user?.username || hasPseudo.value
})

// Timer State
const timeRemaining = ref(30)
const timerInterval = ref<ReturnType<typeof setInterval> | null>(null)

// Parsing content
const questions = computed(() => {
  // 1. Direct prop (New standard)
  if (props.questions && props.questions.length > 0) {
      return props.questions
  }

  // 2. Multiplayer State (Fallback)
  if (props.isMultiplayer && mpQuestions && mpQuestions.value && mpQuestions.value.length > 0) {
      return mpQuestions.value
  }
  
  // 3. GameContent parsing (Legacy)
  try {
    if(!props.gameContent) return []
    // Check if it's already an object
    const data = typeof props.gameContent === 'string' 
        ? JSON.parse(props.gameContent) 
        : props.gameContent
        
    return data.questions || []
  } catch (e) {
    console.error('Invalid content', e)
    return []
  }
})

const currentQuestion = computed(() => questions.value[currentQuestionIndex.value])
const sortedPlayers = computed(() => [...players.value].sort((a, b) => b.score - a.score))
const myConnectionId = computed(() => players.value.find(p => p.username === 'Moi')?.connectionId) 

// Methods
function selectOption(option: string) {
  if (answered.value) return
  
  selectedAnswer.value = option
  checkAnswer(option)
}

function submitNumber() {
  if (answered.value || !numberInput.value) return
  
  selectedAnswer.value = numberInput.value.toString()
  checkAnswer(selectedAnswer.value)
}

function checkAnswer(answer: string) {
  answered.value = true
  const correct = currentQuestion.value.correctAnswer.toString().toLowerCase().trim()
  const userAns = answer.toString().toLowerCase().trim()
  
  if (userAns === correct) {
    isCorrect.value = true
    score.value++
  } else {
    isCorrect.value = false
  }

  // Multiplayer Update
  if (props.isMultiplayer) {
      console.log('[QuizGame] Submitting MP Answer. Waiting for signal...')
      mpUpdateScore(score.value)
      mpSubmitAnswer(answer)
      // Wait for server signal to show feedback
      showFeedback.value = false 
  } else {
      console.log('[QuizGame] Solo Answer. Showing feedback immediately.')
      showFeedback.value = true
  }
}

function handleNextQuestionClick() {
    if (props.isMultiplayer) {
        if (isHost.value) {
            mpRequestNextQuestion()
        }
    } else {
        nextQuestion()
    }
}

function nextQuestion() {
  if (currentQuestionIndex.value < questions.value.length - 1) {
    currentQuestionIndex.value++
    resetQuestionState()
  } else {
    showResult.value = true
  }
}

function resetQuestionState() {
  answered.value = false
  showFeedback.value = false
  selectedAnswer.value = null
  numberInput.value = ''
  isCorrect.value = false
  startTimer()
}

// Timer Functions
function startTimer() {
  stopTimer()
  // Only start timer loop if not multiplayer OR if we decide MP controls client timers autonomously for now
  // For V1 MP: Local timer per client is acceptable, assuming everyone started roughly same time
  // If MP, maybe we don't auto-timeout locally to keep sync simple, or we do and send submit
  timeRemaining.value = 30
  
  timerInterval.value = setInterval(() => {
    if (timeRemaining.value > 0) {
      timeRemaining.value--
    } else {
      stopTimer()
      if (!answered.value) {
        handleTimeOut()
      }
    }
  }, 1000)
}

function stopTimer() {
  if (timerInterval.value) {
    clearInterval(timerInterval.value)
    timerInterval.value = null
  }
}

function handleTimeOut() {
  answered.value = true
  isCorrect.value = false
  if (props.isMultiplayer) {
      mpUpdateScore(score.value) // No points added
      mpSubmitAnswer('') // Signal done
      showFeedback.value = false
  } else {
      showFeedback.value = true
  }
}

const timerClass = computed(() => {
  if (timeRemaining.value <= 5) return 'text-red-600 border-red-200 bg-red-50 animate-pulse'
  if (timeRemaining.value <= 10) return 'text-orange-500 border-orange-200 bg-orange-50'
  return 'text-[#1e1f22] border-[#1e1f22]/20 bg-gray-50'
})

// Multiplayer Actions
async function handleCreateLobby() {
    try {
        const gameId = route.params.id as string
        await createLobby(gameId, guestUsername.value)
    } catch (e) {
        console.error(e)
    }
}

async function handleJoinLobby() {
    if(!joinCodeDisplay.value) return
    try {
        await joinLobby(joinCodeDisplay.value, guestUsername.value)
    } catch(e) {
        console.error(e)
    }
}

function confirmIdentity() {
    if (guestUsername.value) {
        localStorage.setItem('ilmion_guest_username', guestUsername.value)
        hasPseudo.value = true
    }
}

async function handleStartGame() {
    await mpStartGame(route.params.id as string)
}

async function copyCode() {
    if (currentCode.value) {
        await navigator.clipboard.writeText(currentCode.value)
        // Could show a toast here
    }
}

async function copyInviteLink() {
    if (currentCode.value) {
        const url = `${window.location.origin}${window.location.pathname}?code=${currentCode.value}`
        await navigator.clipboard.writeText(url)
        // Could show a toast here
    }
}

// Watch for Game Start in MP
watch(mpGameStarted, (started) => {
    if (started) {
        // Reset local game state to begin
        score.value = 0
        currentQuestionIndex.value = 0
        resetQuestionState()
        
        // DEBUG: Verify Order
        if (questions.value.length > 0) {
            console.log(`[QuizGame DEBUG] Total Questions: ${questions.value.length}`)
            console.log(`[QuizGame DEBUG] First Question: "${questions.value[0].text}"`)
            console.log(`[QuizGame DEBUG] Last Question: "${questions.value[questions.value.length-1].text}"`)
        } else {
             console.warn('[QuizGame DEBUG] No questions found on GameStart!')
        }
    }
})

// Watch for Sync Signals
watch(mpShowReveal, (reveal) => {
    if (reveal && props.isMultiplayer) {
        showFeedback.value = true
    }
})

watch(mpMoveToNext, (next) => {
    if (next && props.isMultiplayer) {
        nextQuestion()
    }
})


// Safe emit handlers
async function handleClose() {
  stopTimer()
  if (props.isMultiplayer) {
      await mpDisconnect()
  }
  await nextTick()
  emit('close')
}

async function handleRetry() {
  stopTimer()
  if (props.isMultiplayer) {
      // In MP, retry means back to lobby? 
  }
  score.value = 0
  currentQuestionIndex.value = 0
  showResult.value = false
  resetQuestionState()
  await nextTick()
  emit('retry') 
}

async function handleShareResult() {
    const text = `J'ai obtenu ${score.value}/${questions.value.length} au quiz "${props.title}" sur Ilmion ! Peux-tu faire mieux ?`
    const url = window.location.href

    if (navigator.share) {
        try {
            await navigator.share({
                title: 'Mon score Ilmion',
                text: text,
                url: url
            })
        } catch (err) {
            console.error('Erreur partage:', err)
        }
    } else {
        // Fallback clipboard
        try {
            await navigator.clipboard.writeText(`${text} ${url}`)
            showToast.value = true
            setTimeout(() => showToast.value = false, 3000)
        } catch (err) {
            console.error('Erreur copie:', err)
        }
    }
}


// Styling Logic (Standardized)
function getOptionStyles(option: string) {
    const isSelected = selectedAnswer.value === option
    const isThisCorrect = option === currentQuestion.value.correctAnswer

    // Default
    let styles = {
        container: 'bg-[#F0F4F8] border-[#cfd9e0] hover:border-[#C39712] hover:bg-white cursor-pointer',
        text: 'text-[#082540]',
        corner: 'border-[#082540] opacity-0'
    }

    // Only show results if feedback is revealed
    if (!showFeedback.value) {
        if (answered.value && isSelected) {
            // Selected but waiting for reveal -> Show as "Selected/Pending"
             styles = {
                container: 'bg-[#F0F4F8] border-[#00B578] shadow-md',
                text: 'text-[#1e1f22] font-bold',
                corner: 'border-[#00B578] opacity-100' // Gold corners to show selection
             }
             return styles
        }
        return styles // Hide correctness
    }

    // Feedback revealed
    if (isSelected) {
        if (isCorrect.value) {
            styles = {
                container: 'bg-[#082540] border-[#082540] shadow-md transform scale-[1.02]',
                text: 'text-white font-bold',
                corner: 'border-[#C39712] opacity-100' 
            }
        } else {
            styles = {
                container: 'bg-red-50 border-red-500',
                text: 'text-red-800',
                corner: 'border-red-600 opacity-50' 
            }
        }
    } else if (isThisCorrect) {
        styles = {
            container: 'bg-green-50 border-green-500 border-dashed',
            text: 'text-green-800',
            corner: 'border-green-600 opacity-50'
        }
    } else {
        styles = {
            container: 'bg-[F0F4F8] border-gray-100 opacity-50',
            text: 'text-gray-400',
            corner: 'opacity-0'
        }
    }
    return styles
}

function getScoreMessage() {
  const percentage = (score.value / questions.value.length) * 100
  if (percentage === 100) return 'Une sagesse digne des plus grands savants ! Vos connaissances sont une lumière.'
  if (percentage >= 80) return 'Excellent travail ! Vous maîtrisez le sujet avec brio.'
  if (percentage >= 50) return 'Bien joué ! Continuez à explorer pour enrichir votre savoir.'
  return 'Le chemin du savoir est long et pavé d\'apprentissage. Ne lâchez rien !'
}

// Lifecycle
onMounted(() => {
  if (!props.isMultiplayer) {
      startTimer()
  }
  
  // Check for stored guest username
  const storedName = localStorage.getItem('ilmion_guest_username')
  if (storedName) {
      guestUsername.value = storedName
      hasPseudo.value = true
  }

  // Check for code in URL
  if (route.query.code) {
      joinCodeDisplay.value = route.query.code as string
  }
})

onUnmounted(() => {
  stopTimer()
  if (props.isMultiplayer) {
      mpDisconnect() // Safe cleanup
  }
})

watch(answered, (newVal) => {
  if (newVal) stopTimer()
})
</script>

<style scoped>
.pattern-geometric {
    background-image: radial-gradient(#C39712 1px, transparent 1px);
    background-size: 20px 20px;
}
.animate-fade-in {
  animation: fadeIn 0.6s ease-out forwards;
}
.animate-fade-in-up {
  animation: fadeInUp 0.5s ease-out forwards;
}
.animate-scale-in {
    animation: scaleIn 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275) forwards;
}
.animate-bounce-slow {
    animation: bounce 3s infinite;
}
.animate-bounce-subtle {
    animation: bounceSubtle 2s infinite;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes fadeInUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
@keyframes scaleIn {
    from { transform: scale(0); opacity: 0; }
    to { transform: scale(1); opacity: 1; }
}
@keyframes bounce {
  0%, 100% { transform: translateY(-5%); }
  50% { transform: translateY(0); }
}
@keyframes bounceSubtle {
  0%, 100% { transform: translateY(-3px); }
  50% { transform: translateY(0); }
}
</style>
