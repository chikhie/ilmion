<template>
  <div class="h-screen max-h-screen bg-brand-dark font-sans-body relative flex flex-col overflow-hidden">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 opacity-20 pointer-events-none">
        <div class="absolute top-0 left-0 w-full h-full bg-[url('/patterns/arabesque.png')]"></div>
    </div>

    <!-- MAIN CONTENT WRAPPER -->
    <div class="relative z-10 w-full flex-grow flex flex-col">
        
        <!-- === SETUP SCREEN === -->
        <div v-if="!gameStarted" class="flex flex-col items-center justify-center flex-grow p-6 text-center space-y-12 animate-fade-in max-w-2xl mx-auto w-full">
            <div class="space-y-4">
                <h1 class="text-3xl md:text-6xl font-serif-title font-bold text-brand-gold drop-shadow-md">{{ $t('quiz.title') }}</h1>
                
                <!-- Language Selector -->
                <div class="flex justify-center gap-3">
                    <button 
                        v-for="loc in availableLocales" 
                        :key="loc.code"
                        @click="changeLocale(loc.code)"
                        :class="[
                            'px-4 py-2 rounded-xl border transition-all duration-200 flex items-center gap-2',
                            locale === loc.code 
                                ? 'bg-brand-gold text-brand-dark border-brand-gold' 
                                : 'bg-brand-wood/30 text-brand-parchment/70 border-brand-gold/30 hover:border-brand-gold/60'
                        ]"
                    >
                        <span class="text-xl">{{ loc.code === 'fr' ? '🇫🇷' : '🇬🇧' }}</span>
                        <span class="font-medium">{{ loc.name }}</span>
                    </button>
                </div>
                
                <p class="text-brand-parchment/80 text-xl md:text-2xl">{{ $t('quiz.howManyQuestions') }}</p>
            </div>
            
            <div class="grid grid-cols-2 lg:grid-cols-4 gap-4 w-full">
                <button v-for="count in [5, 10, 15, 20]" :key="count"
                    @click="startGame(count)"
                    class="bg-brand-wood/40 hover:bg-brand-gold hover:text-brand-dark text-brand-gold text-xl md:text-2xl font-bold py-4 md:py-8 rounded-2xl transition-all duration-300 transform hover:scale-105 shadow-lg border border-brand-gold/30 hover:shadow-brand-gold/20"
                >
                    {{ count }}
                </button>
            </div>
            
            <button @click="router.push('/')" class="text-brand-parchment/50 hover:text-brand-gold underline decoration-brand-gold/30 underline-offset-4 transition-colors">
                {{ $t('quiz.backToHome') }}
            </button>
        </div>

        <!-- === GAME SCREEN === -->
        <div v-else-if="quiz.length > 0 && currentQuestionIndex < quiz.length" class="flex flex-col h-full w-full max-w-4xl mx-auto">
            
            <!-- HEADER -->
            <div class="px-6 pt-6 pb-4 md:py-8 flex items-center justify-between">
                <button class="w-12 h-12 rounded-full bg-brand-wood/50 text-brand-gold border border-brand-gold/30 flex items-center justify-center shadow-md hover:bg-brand-wood transition-colors" @click="goBack">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                    </svg>
                </button>
                <span class="text-brand-parchment font-serif-title font-semibold text-xl drop-shadow-sm tracking-wide">{{ $t('quiz.question') }} {{ currentQuestionIndex + 1 }} <span class="text-brand-parchment/50 text-base">/ {{ totalQuestions }}</span></span>
                <div class="w-12 h-12"></div> <!-- Spacer -->
            </div>

            <!-- QUESTION AREA -->
            <div class="px-6 mb-6 md:mb-10 flex justify-center">
                 <div class="w-full relative py-4 md:py-12 flex flex-col items-center text-center">
                    
                    <!-- Decorative Element -->
                    <div class="w-24 h-1 bg-gradient-to-r from-transparent via-brand-gold/50 to-transparent mb-8"></div>

                    <h2 class="text-brand-parchment text-xl md:text-4xl font-serif-title font-bold leading-tight md:leading-snug max-w-3xl drop-shadow-sm">
                        {{ quiz[currentQuestionIndex].text }}
                    </h2>
                    
                    <!-- Decorative Element -->
                     <div class="w-24 h-1 bg-gradient-to-r from-transparent via-brand-gold/50 to-transparent mt-8"></div>
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
                            'w-full p-4 md:p-8 rounded-2xl flex items-center gap-4 transition-all duration-200 border relative group backdrop-blur-sm text-left',
                             getOptionClass(option, idx)
                        ]"
                        :disabled="hasAnswered"
                     >
                        <div class="flex-shrink-0 w-10 h-10 rounded-full border border-current flex items-center justify-center opacity-60 group-hover:opacity-100 transition-opacity">
                             <span :class="['font-serif-title font-bold text-lg']">{{ String.fromCharCode(65 + idx) }}</span>
                        </div>
                        <span class="flex-1 font-sans-body text-base md:text-xl font-medium leading-snug">{{ option }}</span>
                        
                        <!-- Status Icon -->
                         <div v-if="hasAnswered" class="absolute top-4 right-4">
                            <div v-if="getOptionState(option) === 'correct'" class="text-green-400">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" viewBox="0 0 20 20" fill="currentColor">
                                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                                </svg>
                            </div>
                            <div v-else-if="selectedAnswer === option && getOptionState(option) === 'wrong'" class="text-red-400">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" viewBox="0 0 20 20" fill="currentColor">
                                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
                                </svg>
                            </div>
                        </div>
                    </button>
                    </div>
                </template>

                <template v-if="quiz[currentQuestionIndex].type === 'boolean'">
                    <div class="flex flex-col sm:flex-row gap-4 justify-center py-8">
                    <button @click="selectOption('true')"
                        :class="['w-full sm:w-64 p-8 rounded-2xl border font-bold text-2xl transition-all duration-200 flex flex-col items-center gap-4', getOptionClass('true', 0)]"
                        :disabled="hasAnswered">
                        <span>{{ $t('quiz.true') }}</span>
                    </button>
                    <button @click="selectOption('false')"
                         :class="['w-full sm:w-64 p-8 rounded-2xl border font-bold text-2xl transition-all duration-200 flex flex-col items-center gap-4', getOptionClass('false', 1)]"
                        :disabled="hasAnswered">
                        <span>{{ $t('quiz.false') }}</span>
                    </button>
                    </div>
                </template>

                 <!-- Number Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'number'">
                    <div class="max-w-md mx-auto w-full bg-brand-wood/30 rounded-3xl p-8 shadow-sm border border-brand-gold/20 backdrop-blur-md">
                        <input type="number" 
                          v-model="numberInput" 
                          @keyup.enter="selectOption(numberInput.toString())"
                          :disabled="hasAnswered"
                          class="w-full text-center text-4xl md:text-5xl font-bold font-serif-title text-brand-gold border-b-2 border-brand-gold/30 focus:border-brand-gold outline-none p-4 mb-8 bg-transparent placeholder-brand-wood/50 disabled:opacity-50 disabled:cursor-not-allowed"
                          placeholder="0"
                        />
                         <button @click="selectOption(numberInput.toString())"
                            :disabled="hasAnswered || !numberInput"
                            class="w-full bg-brand-gold text-brand-dark rounded-xl py-4 text-xl font-bold hover:bg-white transition-colors shadow-lg disabled:opacity-50 disabled:cursor-not-allowed disabled:bg-brand-wood/50"
                         >
                             {{ $t('quiz.submit') }}
                         </button>
                    </div>
                </template>

                <!-- Explanation / Feedback (Floating Bottom Sheet on Mobile, centered card on Desktop) -->
                <div v-if="hasAnswered" class="fixed bottom-0 left-0 right-0 p-6 bg-brand-dark border-t border-brand-gold/30 shadow-2xl z-50 md:relative md:bg-transparent md:border-t-0 md:shadow-none md:p-0 md:mt-8 animate-fade-in-up">
                    <div class="max-w-3xl mx-auto md:bg-brand-wood/40 md:p-6 md:rounded-2xl md:border md:border-brand-gold/20 md:backdrop-blur-md">
                        <div class="flex items-start gap-4">
                            <div :class="['w-1 mt-1.5 h-12 rounded-full', isCorrect ? 'bg-green-500' : 'bg-red-500']"></div>
                            <div class="flex-1">
                                <p :class="isCorrect ? 'text-green-400' : 'text-red-400'" class="font-bold mb-1 text-lg font-serif-title">
                                    {{ isCorrect ? $t('quiz.correct') : $t('quiz.incorrect') }}
                                </p>
                                <p class="text-brand-parchment text-lg leading-relaxed mb-6">{{ quiz[currentQuestionIndex].explanation }}</p>
                                
                                <button @click="nextQuestion" class="w-full md:w-auto px-8 bg-brand-gold text-brand-dark py-3 rounded-xl font-bold text-lg hover:bg-white transition-colors">
                                    {{ currentQuestionIndex < quiz.length - 1 ? $t('quiz.next') : $t('quiz.gameOver') }}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
             
             <!-- PROGRESS LINE ABSOLUTE BOTTOM -->
             <div class="fixed top-0 left-0 right-0 h-1.5 bg-brand-wood z-50">
                <div class="h-full bg-brand-gold transition-all duration-500 ease-out shadow-[0_0_10px_rgba(195,151,18,0.5)]" :style="{ width: `${((currentQuestionIndex) / totalQuestions) * 100}%` }"></div>
            </div>

        </div>

        <!-- === END SCREEN === -->
        <div v-else-if="gameStarted && currentQuestionIndex >= quiz.length" class="flex flex-col items-center justify-center flex-grow p-6 text-center animate-scale-in max-w-lg mx-auto w-full">
             <div class="bg-brand-wood/40 border border-brand-gold/30 rounded-[3rem] p-12 shadow-2xl w-full backdrop-blur-md relative overflow-hidden">
                <!-- Background glow -->
                <div class="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-full h-full bg-brand-gold/5 blur-3xl rounded-full"></div>
                
                <h2 class="text-2xl md:text-3xl font-serif-title font-bold text-brand-parchment mb-2 relative z-10">{{ $t('quiz.gameOver') }}</h2>
                <div class="text-6xl md:text-[8rem] leading-none font-black text-brand-gold mb-2 drop-shadow-2xl relative z-10 font-serif-title">{{ score }}<span class="text-4xl text-brand-gold/50">/{{ totalQuestions }}</span></div>
                
                <p class="text-xl text-brand-parchment/90 font-serif-title italic relative z-10 mb-8 max-w-sm mx-auto">
                    {{ getScoreMessage() }}
                </p>

                <div class="space-y-4 relative z-10 mt-8">
                     <button @click="handleShareResult" class="w-full bg-brand-gold text-brand-dark font-bold py-3 md:py-5 text-lg md:text-xl rounded-2xl shadow-xl hover:bg-white flex items-center justify-center gap-3 transform transition-all duration-200">
                        <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z" /></svg>
                        Partager mon résultat
                    </button>
                    <button @click="resetGame" class="w-full bg-brand-gold text-brand-dark font-bold py-3 md:py-5 text-lg md:text-xl rounded-2xl shadow-xl hover:bg-white transform transition-all duration-200">
                        {{ $t('quiz.playAgain') }}
                    </button>
                    <button @click="router.push('/')" class="w-full bg-brand-wood text-white hover:bg-brand-wood/90 font-bold py-3 md:py-4 rounded-xl transition-all duration-200 shadow-md border border-brand-parchment/20">
                        {{ $t('quiz.backToHomeBtn') }}
                    </button>
                </div>
             </div>
        </div>

        <!-- Toast Notification -->
        <div v-if="showToast" class="fixed top-24 left-1/2 transform -translate-x-1/2 z-50 animate-fade-in-down">
             <div class="px-6 py-4 bg-brand-dark/95 backdrop-blur-md border border-brand-gold rounded-full shadow-2xl flex items-center gap-3">
                 <div class="bg-green-500/20 p-1 rounded-full">
                     <svg class="w-5 h-5 text-brand-gold" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                 </div>
                 <span class="text-brand-parchment font-bold text-sm tracking-wide">Résultat copié !</span>
             </div>
        </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { GameService } from '../services/game.service';
import type { Quiz } from '../types';
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

const { locale, locales, setLocale, t } = useI18n()

// Computed for available locales with proper typing
const availableLocales = computed(() => 
    (locales.value as Array<{ code: string; name: string }>)
)

const changeLocale = (code: string) => {
    setLocale(code as 'fr' | 'en')
}

const gameService = new GameService()
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
    resetQuestionState();
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
    const text = `J'ai obtenu ${score.value}/${totalQuestions.value} au quiz sur Ilmanar ! Peux-tu faire mieux ?`
    const url = window.location.href

    if (navigator.share) {
        try {
            await navigator.share({
                title: 'Mon score Ilmanar',
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

const goBack = () => {
    if (gameStarted.value) {
        // Confirm exit? For now just go back to setup
        gameStarted.value = false; 
    } else {
        router.back();
    }
}

const getScoreMessage = () => {
    if (totalQuestions.value === 0) return '';
    const percentage = (score.value / totalQuestions.value) * 100;
    if (percentage === 100) return t('quiz.scoreMessages.perfect');
    if (percentage >= 80) return t('quiz.scoreMessages.great');
    if (percentage >= 50) return t('quiz.scoreMessages.good');
    return t('quiz.scoreMessages.keep');
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
            return 'bg-green-500/20 border-green-500 text-green-400';
        case 'wrong':
            return 'bg-red-500/20 border-red-500 text-red-400';
        case 'disabled':
            return 'bg-brand-wood/20 border-brand-gold/5 text-brand-parchment/30 grayscale opacity-50'; 
        default:
            return 'bg-brand-wood/30 text-brand-parchment border-brand-gold/10 hover:bg-brand-wood/60 hover:border-brand-gold/30 hover:scale-[1.01]';
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

.animate-fade-in { animation: fadeIn 0.6s ease-out; }
.animate-fade-in-up { animation: fadeInUp 0.5s ease-out; }
.animate-scale-in { animation: scaleIn 0.4s cubic-bezier(0.2, 0.8, 0.2, 1.2); }

@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes scaleIn { from { opacity: 0; transform: scale(0.9); } to { opacity: 1; transform: scale(1); } }

/* Custom Scrollbar for the answers list */
.custom-scrollbar::-webkit-scrollbar {
  width: 4px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.05);
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: rgba(195, 151, 18, 0.3); /* brand-gold with opacity */
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: rgba(195, 151, 18, 0.6);
}
</style>
