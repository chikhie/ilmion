<template>
  <div class="h-screen max-h-screen bg-[#1e1f22] text-white font-sans relative flex flex-col overflow-hidden">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <div class="absolute top-[10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/10 rounded-full blur-[120px] animate-pulse-slow"></div>
    </div>

    <!-- MAIN CONTENT WRAPPER -->
    <div class="relative z-10 w-full flex-grow flex flex-col">
        
        <!-- === SETUP SCREEN === -->
        <div v-if="!gameStarted" class="flex flex-col items-center justify-center flex-grow p-6 text-center space-y-12 animate-fade-in max-w-2xl mx-auto w-full">
            <div class="space-y-4">
                <div class="inline-flex items-center justify-center w-20 h-20 rounded-2xl bg-[#2b2d31]/80 border border-[#00B578]/30 text-[#00B578] shadow-lg shadow-[#00B578]/10 mb-2 backdrop-blur-sm">
                    <i class="fas fa-brain text-4xl"></i>
                </div>
                <h1 class="text-3xl md:text-5xl font-bold tracking-wide text-white drop-shadow-md">Module d'entraînement</h1>
                
                <!-- Language Selector -->
                <div class="flex justify-center gap-3 mt-4">
                    <button 
                        v-for="loc in availableLocales" 
                        :key="loc.code"
                        @click="changeLocale(loc.code)"
                        :class="[
                            'px-4 py-2 rounded-xl border transition-all duration-200 flex items-center gap-2 font-bold text-sm tracking-widest uppercase',
                            locale === loc.code 
                                ? 'bg-[#00B578] text-[#1e1f22] border-[#00B578]' 
                                : 'bg-[#2b2d31]/50 text-gray-400 border-[#00B578]/20 hover:border-[#00B578]/50 hover:text-white'
                        ]"
                    >
                        <span>{{ loc.code === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
                        <span>{{ loc.name }}</span>
                    </button>
                </div>
                
                <p class="text-gray-400 text-lg md:text-xl font-medium mt-4">{{ $t('quiz.howManyQuestions') }}</p>
            </div>
            
            <div class="grid grid-cols-2 lg:grid-cols-4 gap-4 w-full">
                <button v-for="count in [5, 10, 15, 20]" :key="count"
                    @click="startGame(count)"
                    class="bg-[#2b2d31]/80 hover:bg-[#00B578] hover:text-[#1e1f22] text-[#00B578] text-xl md:text-2xl font-black py-4 md:py-6 rounded-2xl transition-all duration-300 transform hover:-translate-y-1 shadow-lg border border-[#00B578]/30 hover:shadow-[#00B578]/20 group"
                >
                    <span class="block text-sm font-bold text-gray-500 group-hover:text-[#1e1f22]/70 uppercase tracking-widest mb-1">Série de</span>
                    {{ count }}
                </button>
            </div>
            
            <button @click="handleExit()" class="text-gray-500 hover:text-[#00B578] font-bold uppercase tracking-widest text-sm underline decoration-[#00B578]/30 outline-none underline-offset-4 transition-colors flex items-center gap-2">
                <i class="fas fa-arrow-left"></i>
                {{ $t('quiz.backToHome') }}
            </button>
        </div>

        <!-- === GAME SCREEN === -->
        <div v-else-if="quiz.length > 0 && currentQuestionIndex < quiz.length" class="flex flex-col h-full w-full max-w-4xl mx-auto">
            
            <!-- HEADER -->
            <div class="px-6 pt-6 pb-4 md:py-8 flex items-center justify-between">
                <button class="w-12 h-12 rounded-xl bg-[#2b2d31] text-[#00B578] border border-[#00B578]/30 flex items-center justify-center shadow-md hover:bg-[#1e1f22] hover:border-[#00B578] transition-colors" @click="goBack">
                    <i class="fas fa-times text-xl"></i>
                </button>
                <div class="flex items-center gap-3">
                    <span class="text-white font-bold text-lg tracking-wide bg-[#2b2d31] px-4 py-1.5 rounded-lg border border-white/5 shadow-inner">
                        <span class="text-[#00B578] mr-1">Q{{ currentQuestionIndex + 1 }}</span>
                        <span class="text-gray-500 text-sm">/ {{ totalQuestions }}</span>
                    </span>
                </div>
                <div class="w-12 h-12 flex items-center justify-center bg-[#2b2d31]/50 rounded-xl border border-white/5 font-mono text-[#00B578] font-bold">
                    <!-- Placeholder Timer or Score snippet -->
                    {{ score }}
                </div>
            </div>

            <!-- QUESTION AREA -->
            <div class="px-6 mb-6 md:mb-10 flex justify-center">
                 <div class="w-full relative py-4 md:py-12 flex flex-col items-center text-center">
                    <h2 class="text-white text-xl md:text-3xl font-bold leading-tight md:leading-snug max-w-3xl drop-shadow-sm tracking-wide">
                        {{ quiz[currentQuestionIndex].text }}
                    </h2>
                 </div>
            </div>

            <!-- ANSWERS AREA -->
            <div class="flex-1 px-4 md:px-0 pb-8 overflow-y-auto space-y-4 custom-scrollbar w-full max-w-3xl mx-auto">
                
                <!-- Choice Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'choice'">
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                     <button v-for="(option, idx) in quiz[currentQuestionIndex].options" :key="option"
                        @click="selectOption(option)"
                        :class="[
                            'w-full p-4 md:p-6 rounded-2xl flex items-center gap-4 transition-all duration-200 border relative group backdrop-blur-sm text-left shadow-lg',
                             getOptionClass(option, idx)
                        ]"
                        :disabled="hasAnswered"
                     >
                        <div :class="['flex-shrink-0 w-10 h-10 rounded-xl border-2 flex items-center justify-center font-bold text-lg transition-colors',
                                      getOptionLetterClass(option)]">
                             {{ String.fromCharCode(65 + idx) }}
                        </div>
                        <span class="flex-1 text-base md:text-lg font-medium leading-snug">{{ option }}</span>
                        
                        <!-- Status Icon -->
                         <div v-if="hasAnswered" class="absolute top-1/2 -translate-y-1/2 right-4">
                            <div v-if="getOptionState(option) === 'correct'" class="text-[#00B578] bg-[#00B578]/20 p-1 rounded-full">
                                <i class="fas fa-check"></i>
                            </div>
                            <div v-else-if="selectedAnswer === option && getOptionState(option) === 'wrong'" class="text-red-400 bg-red-400/20 p-1 rounded-full">
                                <i class="fas fa-times"></i>
                            </div>
                        </div>
                    </button>
                    </div>
                </template>

                <template v-if="quiz[currentQuestionIndex].type === 'boolean'">
                    <div class="flex flex-col sm:flex-row gap-4 justify-center py-8">
                    <button @click="selectOption('true')"
                        :class="['w-full sm:w-64 p-8 rounded-2xl border font-bold text-xl uppercase tracking-widest transition-all duration-200 flex flex-col items-center gap-4 shadow-lg', getOptionClass('true', 0)]"
                        :disabled="hasAnswered">
                        <i class="fas fa-check-circle text-3xl mb-2 opacity-50"></i>
                        <span>{{ $t('quiz.true') }}</span>
                    </button>
                    <button @click="selectOption('false')"
                         :class="['w-full sm:w-64 p-8 rounded-2xl border font-bold text-xl uppercase tracking-widest transition-all duration-200 flex flex-col items-center gap-4 shadow-lg', getOptionClass('false', 1)]"
                        :disabled="hasAnswered">
                        <i class="fas fa-times-circle text-3xl mb-2 opacity-50"></i>
                        <span>{{ $t('quiz.false') }}</span>
                    </button>
                    </div>
                </template>

                 <!-- Number Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'number'">
                    <div class="max-w-md mx-auto w-full bg-[#2b2d31]/80 rounded-3xl p-8 shadow-xl border border-[#00B578]/20 backdrop-blur-md">
                        <input type="number" 
                          v-model="numberInput" 
                          @keyup.enter="selectOption(numberInput.toString())"
                          :disabled="hasAnswered"
                          class="w-full text-center text-4xl md:text-5xl font-bold text-[#00B578] border-b-2 border-white/10 focus:border-[#00B578] outline-none p-4 mb-8 bg-transparent placeholder-gray-600 disabled:opacity-50 disabled:cursor-not-allowed font-mono"
                          placeholder="0"
                        />
                         <button @click="selectOption(numberInput.toString())"
                            :disabled="hasAnswered || !numberInput"
                            class="w-full bg-[#00B578] text-[#1e1f22] rounded-xl py-4 text-lg font-black tracking-widest uppercase hover:bg-emerald-400 transition-colors shadow-lg shadow-[#00B578]/20 disabled:opacity-30 disabled:cursor-not-allowed disabled:shadow-none"
                         >
                             SOUMETTRE
                         </button>
                    </div>
                </template>

                <!-- Explanation / Feedback (Floating Bottom Sheet on Mobile, centered card on Desktop) -->
                <div v-if="hasAnswered" class="fixed bottom-0 left-0 right-0 p-6 bg-[#2b2d31]/95 backdrop-blur-xl border-t border-[#00B578]/30 shadow-2xl z-50 md:relative md:bg-transparent md:border-t-0 md:shadow-none md:p-0 md:mt-8 md:backdrop-blur-none animate-fade-in-up">
                    <div class="max-w-3xl mx-auto md:bg-[#2b2d31]/80 md:p-6 md:rounded-2xl md:border md:border-[#00B578]/20 md:backdrop-blur-md">
                        <div class="flex items-start gap-4">
                            <div :class="['w-1.5 mt-1.5 h-12 rounded-full', isCorrect ? 'bg-[#00B578]' : 'bg-red-500']"></div>
                            <div class="flex-1">
                                <p :class="isCorrect ? 'text-[#00B578]' : 'text-red-400'" class="font-bold mb-1 text-lg uppercase tracking-widest flex items-center gap-2">
                                    <i :class="isCorrect ? 'fas fa-check-circle' : 'fas fa-times-circle'"></i>
                                    {{ isCorrect ? $t('quiz.correct') : $t('quiz.incorrect') }}
                                </p>
                                <p class="text-gray-300 text-base leading-relaxed mb-6">{{ quiz[currentQuestionIndex].explanation }}</p>
                                
                                <button @click="nextQuestion" class="w-full md:w-auto px-8 bg-[#00B578] text-[#1e1f22] py-3 rounded-xl font-bold tracking-widest uppercase text-sm hover:bg-emerald-400 transition-colors shadow-lg shadow-[#00B578]/20 flex items-center justify-center gap-2">
                                    {{ currentQuestionIndex < quiz.length - 1 ? $t('quiz.next') : $t('quiz.gameOver') }}
                                    <i class="fas fa-arrow-right"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
             
             <!-- PROGRESS LINE ABSOLUTE BOTTOM -->
             <div class="fixed top-0 left-0 right-0 h-1.5 bg-[#2b2d31] z-50">
                <div class="h-full bg-[#00B578] transition-all duration-500 ease-out shadow-[0_0_15px_rgba(0,181,120,0.8)]" :style="{ width: `${((currentQuestionIndex) / totalQuestions) * 100}%` }"></div>
            </div>

        </div>

        <!-- === END SCREEN === -->
        <div v-else-if="gameStarted && currentQuestionIndex >= quiz.length" class="flex flex-col items-center justify-center flex-grow p-6 text-center animate-scale-in max-w-lg mx-auto w-full">
             <div class="bg-[#2b2d31]/80 border border-[#00B578]/30 rounded-3xl p-10 shadow-2xl w-full backdrop-blur-md relative overflow-hidden">
                <!-- Background glow -->
                <div class="absolute top-0 right-0 w-64 h-64 bg-[#00B578]/10 rounded-full blur-[80px] -mr-32 -mt-32"></div>
                <div class="absolute bottom-0 left-0 w-64 h-64 bg-[#00B578]/5 rounded-full blur-[80px] -ml-32 -mb-32"></div>
                
                <div class="inline-flex items-center justify-center w-20 h-20 rounded-2xl bg-[#1e1f22] border border-[#00B578]/50 text-[#00B578] mb-6 shadow-inner relative z-10">
                    <i class="fas fa-trophy text-4xl"></i>
                </div>
                
                <h2 class="text-xl md:text-2xl font-bold uppercase tracking-widest text-gray-400 mb-2 relative z-10">{{ $t('quiz.gameOver') }}</h2>
                <div class="text-6xl md:text-7xl font-black text-white mb-2 drop-shadow-lg relative z-10 font-mono tracking-tighter">
                    {{ score }}<span class="text-3xl text-gray-500">/{{ totalQuestions }}</span>
                </div>
                
                <p class="text-lg text-[#00B578] font-bold relative z-10 mb-8 max-w-sm mx-auto bg-[#00B578]/10 py-2 px-4 rounded-xl border border-[#00B578]/20 mt-4">
                    {{ getScoreMessage() }}
                </p>

                <div class="space-y-4 relative z-10 mt-8">
                     <button @click="handleShareResult" class="w-full bg-[#2b2d31] border border-[#00B578]/30 text-[#00B578] font-bold py-3 md:py-4 text-sm tracking-widest uppercase rounded-xl hover:bg-[#00B578]/10 hover:border-[#00B578]/50 flex items-center justify-center gap-3 transition-all duration-200">
                        <i class="fas fa-share-alt"></i>
                        Partager le score
                    </button>
                    <button @click="resetGame" class="w-full bg-[#00B578] text-[#1e1f22] font-black py-4 text-lg tracking-widest uppercase rounded-xl shadow-lg shadow-[#00B578]/20 hover:bg-emerald-400 transform transition-all duration-200 flex items-center justify-center gap-2">
                        <i class="fas fa-redo-alt"></i>
                        Rejouer
                    </button>
                    <button @click="handleExit()" class="w-full text-gray-400 hover:text-white font-bold py-3 text-sm tracking-widest uppercase underline decoration-gray-600 underline-offset-4 transition-colors">
                        Retour au réseau
                    </button>
                </div>
             </div>
        </div>

        <!-- Toast Notification -->
        <div v-if="showToast" class="fixed top-24 left-1/2 transform -translate-x-1/2 z-50 animate-fade-in-down">
             <div class="px-6 py-4 bg-[#2b2d31]/95 backdrop-blur-md border border-[#00B578] rounded-2xl shadow-2xl shadow-[#00B578]/20 flex items-center gap-3">
                 <div class="bg-[#00B578]/20 p-1.5 rounded-full text-[#00B578]">
                     <i class="fas fa-clipboard-check"></i>
                 </div>
                 <span class="text-white font-bold text-sm tracking-wide">Résultat copié !</span>
             </div>
        </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { GameService } from '../services/game.service';
import { progressionService } from '../services/progression.service';
import { useAuthStore } from '../stores/auth';
import type { Quiz } from '../types';
import { ref, computed, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';

const { locale, locales, setLocale, t } = useI18n()

useSeoMeta({
  title: 'Entraînement | Ilmion',
  description: 'Testez vos connaissances scientifiques avec nos modules interactifs.'
})

// Computed for available locales with proper typing
const availableLocales = computed(() => 
    (locales.value as Array<{ code: string; name: string }>)
)

const changeLocale = (code: string) => {
    setLocale(code as 'fr' | 'en')
}

const gameService = new GameService()
const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()
const themeId = computed(() => route.query.themeId as string | undefined)
const partId = computed(() => route.query.partId as string | undefined)

// State
const gameStarted = ref(false)
const quiz = ref<Quiz[]>([])
const currentQuestionIndex = ref(0)
const hasAnswered = ref(false)
const selectedAnswer = ref<string | null>(null)
const isCorrect = ref<boolean>(false)
const numberInput = ref<number | ''>('')
const score = ref(0)
const totalQuestions = ref(0)
const showToast = ref(false)
const loading = ref(false)
const startTime = ref<number>(0)

// Methods
const startGame = async (count?: number) => {
    loading.value = true
    try {
        // If partId or themeId is present, we ignore count and fetch by those filters
        // If neither is present, we use count (Global Mode)
        quiz.value = await gameService.getQuizzForSoloPlayer(count, locale.value, themeId.value, partId.value);
        
        if (quiz.value.length > 0) {
            totalQuestions.value = quiz.value.length;
            gameStarted.value = true;
            currentQuestionIndex.value = 0;
            startTime.value = Date.now();
            resetQuestionState();
        } else {
            console.warn("No questions found");
            // Handle empty quiz context?
        }
    } catch (e) {
        console.error("Failed to load quiz", e);
    } finally {
        loading.value = false
    }
}

// Auto-start if themeId or partId provided
onMounted(() => {
    if (themeId.value || partId.value) {
        startGame() // No count needed for targeted mode
    }
})

const selectOption = (option: string) => {
    if (hasAnswered.value) return;

    selectedAnswer.value = option;
    hasAnswered.value = true;
    
    // Normalize comparison (handle string vs number)
    const correctAnswer = quiz.value[currentQuestionIndex.value].correctAnswer.toString();
    isCorrect.value = option.toLowerCase() === correctAnswer.toLowerCase();

    if (isCorrect.value) score.value++;
}

const nextQuestion = () => {
    currentQuestionIndex.value++;
    if (currentQuestionIndex.value >= quiz.value.length) {
        saveGame();
    } else {
        resetQuestionState();
    }
}

const saveGame = async () => {
    if (!authStore.isLoggedIn || !authStore.token) return;

    const duration = Math.round((Date.now() - startTime.value) / 1000);
    try {
        await progressionService.saveResult(authStore.token, {
            score: score.value,
            totalQuestions: totalQuestions.value,
            themeId: themeId.value,
            partId: partId.value,
            durationSeconds: duration
        });
        // Optionally refresh profile/stats if we store them locally
    } catch (e) {
        console.error("Failed to save result", e);
    }
}

const resetQuestionState = () => {
    hasAnswered.value = false;
    selectedAnswer.value = null;
    isCorrect.value = false;
    numberInput.value = '';
}

const resetGame = () => {
    gameStarted.value = false;
    quiz.value = [];
    score.value = 0;
    totalQuestions.value = 0;
    currentQuestionIndex.value = 0;
}

const handleShareResult = async () => {
    const text = `J'ai obtenu ${score.value}/${totalQuestions.value} au module Ilmion ! Peux-tu faire mieux ?`
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

const handleExit = () => {
    if (authStore.isLoggedIn) {
        router.push('/dashboard')
    } else {
        router.push('/')
    }
}

const goBack = () => {
    if (gameStarted.value) {
        // Confirm exit? For now just go back to setup
        gameStarted.value = false; 
    } else {
        handleExit();
    }
}

const getScoreMessage = () => {
    if (totalQuestions.value === 0) return '';
    const percentage = (score.value / totalQuestions.value) * 100;
    if (percentage === 100) return "Remarquable analytique! Score parfait."; //t('quiz.scoreMessages.perfect');
    if (percentage >= 80) return "D'excellents résultats expérimentaux."; //t('quiz.scoreMessages.great');
    if (percentage >= 50) return "Compréhension de base vérifiée."; //t('quiz.scoreMessages.good');
    return "Plus de données et de pratique requises."; //t('quiz.scoreMessages.keep');
}

definePageMeta({
  layout: 'game'
})

// Styling helpers
const getOptionState = (option: string) => {
    if (!hasAnswered.value) return 'default';
    
    const optionStr = option.toString().toLowerCase();
    const correctAns = quiz.value[currentQuestionIndex.value].correctAnswer.toString().toLowerCase();
    
    if (selectedAnswer.value === option) {
        return isCorrect.value ? 'correct' : 'wrong';
    }
    
    // Show correct answer even if we picked wrong
    if (optionStr === correctAns) {
        return 'correct';
    }
    
    return 'disabled';
}

const getOptionClass = (option: string, idx: number) => {
    const state = getOptionState(option);
    
    switch (state) {
        case 'correct':
            return 'bg-[#00B578]/10 border-[#00B578] text-white shadow-[0_0_15px_rgba(0,181,120,0.3)]';
        case 'wrong':
            return 'bg-red-500/10 border-red-500 text-white opacity-80';
        case 'disabled':
            return 'bg-[#2b2d31]/50 border-white/5 text-gray-500 grayscale opacity-40'; 
        default:
            return 'bg-[#2b2d31]/80 text-gray-200 border-[#00B578]/20 hover:bg-[#1e1f22] hover:border-[#00B578]/60 hover:shadow-lg hover:shadow-[#00B578]/10 hover:-translate-y-1';
    }
}

const getOptionLetterClass = (option: string) => {
    const state = getOptionState(option);
    
    switch (state) {
        case 'correct':
            return 'border-[#00B578] text-[#00B578] bg-[#00B578]/20';
        case 'wrong':
            return 'border-red-400 text-red-400 bg-red-400/20';
        case 'disabled':
            return 'border-gray-600 text-gray-600'; 
        default:
            return 'border-gray-500 text-gray-400 group-hover:border-[#00B578] group-hover:text-[#00B578]';
    }
}

</script>

<style scoped>
/* Hide number input spinners */
input[type=number]::-webkit-inner-spin-button, 
input[type=number]::-webkit-outer-spin-button { 
  -webkit-appearance: none; 
  margin: 0; 
}
input[type=number] {
  -moz-appearance: textfield;
  appearance: textfield;
}

.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
.animate-fade-in { animation: fadeIn 0.4s ease-out; }
.animate-fade-in-up { animation: fadeInUp 0.4s ease-out; }
.animate-fade-in-down { animation: fadeInDown 0.4s ease-out; }
.animate-scale-in { animation: scaleIn 0.4s cubic-bezier(0.2, 0.8, 0.2, 1.2); }

@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.05; } }
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes fadeInDown { from { opacity: 0; transform: translateY(-20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes scaleIn { from { opacity: 0; transform: scale(0.9); } to { opacity: 1; transform: scale(1); } }

/* Custom Scrollbar for the answers list */
.custom-scrollbar::-webkit-scrollbar {
  width: 6px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: rgba(43, 45, 49, 0.5);
  border-radius: 8px;
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: rgba(0, 181, 120, 0.3);
  border-radius: 8px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: rgba(0, 181, 120, 0.6);
}
</style>
