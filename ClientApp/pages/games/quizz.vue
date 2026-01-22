<template>
  <div class="min-h-screen bg-[#F0F2F5] font-sans-body relative overflow-hidden flex flex-col items-center justify-center p-4">
    
    <!-- Background Gradient similar to screenshot -->
    <div class="absolute inset-0 bg-gradient-to-br from-violet-500 via-purple-500 to-fuchsia-500 z-0"></div>

    <!-- MAIN CONTAINER -->
    <div class="relative z-10 w-full max-w-md bg-white/10 backdrop-blur-md rounded-[40px] shadow-2xl border border-white/20 h-[850px] flex flex-col overflow-hidden">
        
        <!-- === SETUP SCREEN === -->
        <div v-if="!gameStarted" class="flex flex-col items-center justify-center h-full p-8 text-center space-y-8 animate-fade-in">
            <h1 class="text-4xl font-bold text-white drop-shadow-md">Culture Quiz</h1>
            <p class="text-white/90 text-lg">Combien de questions ?</p>
            
            <div class="grid grid-cols-2 gap-4 w-full">
                <button v-for="count in [5, 10, 15, 20]" :key="count"
                    @click="startGame(count)"
                    class="bg-white hover:bg-purple-50 text-purple-800 font-bold py-4 rounded-2xl transition-all duration-300 transform hover:scale-105 shadow-lg border border-purple-200"
                >
                    {{ count }}
                </button>
            </div>
        </div>

        <!-- === GAME SCREEN === -->
        <div v-else-if="quiz.length > 0 && currentQuestionIndex < quiz.length" class="flex flex-col h-full w-full">
            
            <!-- HEADER -->
            <div class="px-6 pt-8 pb-4 flex items-center justify-between">
                <button class="w-10 h-10 rounded-full bg-white text-gray-800 flex items-center justify-center shadow-md hover:bg-gray-100" @click="goBack">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                    </svg>
                </button>
                <span class="text-white font-semibold text-lg drop-shadow-sm">Question {{ currentQuestionIndex + 1 }}/{{ quiz.length }}</span>
                <button class="w-10 h-10 rounded-full bg-white text-gray-800 flex items-center justify-center shadow-md hover:bg-gray-100">
                     <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M5 4a2 2 0 012-2h6a2 2 0 012 2v14l-5-2.5L5 18V4z" />
                    </svg>
                </button>
            </div>

            <!-- QUESTION CARD -->
            <div class="px-6 mb-2">
                 <div class="bg-white rounded-3xl p-8 min-h-[240px] flex items-center justify-center text-center shadow-lg border border-purple-100">
                    <h2 class="text-gray-800 text-2xl font-bold leading-tight">
                        {{ quiz[currentQuestionIndex].text }}
                    </h2>
                 </div>
            </div>

            <!-- TIMER / PROGRESS -->
             <!-- Simple visual placeholder for the timeline in the screenshot -->
            <div class="px-8 py-4 flex items-center gap-3 mx-6">
                 <span class="text-white font-bold text-sm">Time</span>
                 <div class="flex-1 h-2 bg-white/30 rounded-full overflow-hidden">
                     <div class="h-full bg-white w-3/4 rounded-full"></div>
                 </div>
                 <span class="text-white font-bold text-sm">00:12</span>
            </div>

            <!-- ANSWERS LIST -->
            <div class="flex-1 px-6 pb-8 overflow-y-auto space-y-3">
                <!-- Choice Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'choice'">
                     <button v-for="(option, idx) in quiz[currentQuestionIndex].options" :key="option"
                        @click="selectOption(option)"
                        :class="[
                            'w-full p-4 rounded-2xl flex items-center gap-4 transition-all duration-200 border-2 font-semibold text-lg relative group',
                             getOptionClass(option, idx)
                        ]"
                        :disabled="hasAnswered"
                     >
                        <span class="flex-shrink-0 w-8 font-bold opacity-60">{{ String.fromCharCode(65 + idx) }}</span>
                        <span class="text-left flex-1">{{ option }}</span>
                        
                        <!-- Checkmark/Circle Icon -->
                        <div class="w-6 h-6 rounded-full border-2 border-current flex items-center justify-center">
                            <div v-if="getOptionState(option) === 'correct'" class="w-3 h-3 bg-current rounded-full"></div>
                        </div>
                    </button>
                </template>

                <!-- Boolean Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'boolean'">
                    <button @click="selectOption('true')"
                        :class="['w-full p-6 rounded-2xl border-2 font-bold text-xl mb-4', getOptionClass('true', 0)]"
                        :disabled="hasAnswered">
                        Vrai
                    </button>
                    <button @click="selectOption('false')"
                         :class="['w-full p-6 rounded-2xl border-2 font-bold text-xl', getOptionClass('false', 1)]"
                        :disabled="hasAnswered">
                        Faux
                    </button>
                </template>

                 <!-- Number Type -->
                <template v-if="quiz[currentQuestionIndex].type === 'number'">
                    <div class="bg-white rounded-2xl p-6 shadow-sm">
                        <input type="number" 
                          v-model="numberInput" 
                          @keyup.enter="selectOption(numberInput.toString())"
                          class="w-full text-center text-3xl font-bold text-gray-800 border-b-2 border-gray-200 focus:border-purple-500 outline-none p-2 mb-4 bg-transparent"
                          placeholder="0"
                        />
                         <button @click="selectOption(numberInput.toString())"
                            :disabled="hasAnswered || !numberInput"
                            class="w-full bg-purple-600 text-white rounded-xl py-3 font-bold hover:bg-purple-700 transition"
                         >
                            Valider
                         </button>
                    </div>
                </template>

                <!-- Explanation / Feedback -->
                <div v-if="hasAnswered" class="mt-4 p-4 rounded-xl bg-white/90 shadow-sm animate-fade-in-up">
                    <p :class="isCorrect ? 'text-green-600' : 'text-red-500'" class="font-bold mb-1">
                        {{ isCorrect ? 'Bravo !' : 'Oups...' }}
                    </p>
                    <p class="text-gray-700 text-sm leading-relaxed">{{ quiz[currentQuestionIndex].explanation }}</p>
                    
                    <button @click="nextQuestion" class="mt-3 w-full bg-gray-900 text-white py-2 rounded-lg font-bold text-sm hover:bg-black">
                        {{ currentQuestionIndex < quiz.length - 1 ? 'Suivant' : 'Terminer' }}
                    </button>
                </div>

            </div>
        </div>

        <!-- === END SCREEN === -->
        <div v-else-if="gameStarted && currentQuestionIndex >= quiz.length" class="flex flex-col items-center justify-center h-full p-8 text-center animate-scale-in">
             <div class="bg-white rounded-3xl p-8 shadow-xl w-full max-w-sm">
                <h2 class="text-3xl font-bold text-gray-800 mb-2">Partie Terminée!</h2>
                <div class="text-6xl font-black text-purple-600 mb-4">{{ score }}/{{ quiz.length }}</div>
                <p class="text-gray-500 mb-8">Score final</p>
                <button @click="resetGame" class="w-full bg-purple-600 text-white font-bold py-4 rounded-xl shadow-lg hover:bg-purple-700 transition">
                    Rejouer
                </button>
             </div>
        </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { GameService } from '../../services/game.service';
import type { Quiz } from '../../types';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const gameService = new GameService()
const router = useRouter()

// State
const gameStarted = ref(false)
const quiz = ref<Quiz[]>([])
const currentQuestionIndex = ref(0)
const hasAnswered = ref(false)
const selectedAnswer = ref<string | null>(null)
const isCorrect = ref<boolean>(false)
const numberInput = ref<number | ''>('')
const score = ref(0)

// Methods
const startGame = async (count: number) => {
    try {
        quiz.value = await gameService.getQuizzForSoloPlayer(count);
        gameStarted.value = true;
        currentQuestionIndex.value = 0;
        resetQuestionState();
    } catch (e) {
        console.error("Failed to load quiz", e);
    }
}

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
    currentQuestionIndex.value = 0;
}

const goBack = () => {
    if (gameStarted.value) {
        // Confirm exit? For now just go back to setup
        gameStarted.value = false; 
    } else {
        router.back();
    }
}

// Styling helpers
const getOptionState = (option: string) => {
    if (!hasAnswered.value) return 'default';
    if (selectedAnswer.value === option) {
        return isCorrect.value ? 'correct' : 'wrong';
    }
    // Show correct answer if wrong was selected
    const correctAns = quiz.value[currentQuestionIndex.value].correctAnswer.toString();
    if (option.toString().toLowerCase() === correctAns.toLowerCase()) {
        return 'correct';
    }
    return 'default';
}

const getOptionClass = (option: string, idx: number) => {
    const state = getOptionState(option);
    switch (state) {
        case 'correct':
            return 'bg-green-100 border-green-500 text-green-700';
        case 'wrong':
            return 'bg-red-100 border-red-500 text-red-700';
        default:
            return 'bg-white text-gray-700 border-transparent shadow-sm hover:shadow-md hover:bg-gray-50';
    }
}

</script>

<style scoped>
.animate-fade-in { animation: fadeIn 0.5s ease-out; }
.animate-fade-in-up { animation: fadeInUp 0.4s ease-out; }
.animate-scale-in { animation: scaleIn 0.3s cubic-bezier(0.2, 0.8, 0.2, 1.2); }

@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(10px); } to { opacity: 1; transform: translateY(0); } }
@keyframes scaleIn { from { opacity: 0; transform: scale(0.9); } to { opacity: 1; transform: scale(1); } }
</style>