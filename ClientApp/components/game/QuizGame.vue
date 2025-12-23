<template>
  <div class="max-w-3xl mx-auto p-8 bg-white/5 backdrop-blur-2xl rounded-[2.5rem] border border-white/10 shadow-2xl relative overflow-hidden group">
    <!-- Decor -->
    <div class="absolute top-0 right-0 w-32 h-32 bg-white/5 rounded-full -mr-16 -mt-16 blur-3xl group-hover:bg-white/10 transition-all duration-500"></div>

    <!-- Game Header -->
    <div class="mb-10 flex justify-between items-center relative z-10">
      <h2 class="text-xs font-black uppercase tracking-[0.2em] text-gray-500">{{ title }}</h2>
      <div class="flex gap-3">
        <!-- Timer -->
        <div v-if="!showResult && !answered" class="text-sm font-black px-4 py-2 rounded-2xl border transition-all"
             :class="timerClass">
          <div class="flex items-center gap-2">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            {{ timeRemaining }}s
          </div>
        </div>
        <!-- Score -->
        <div class="text-sm font-black text-[#C39712] bg-[#C39712]/10 px-4 py-2 rounded-2xl border border-[#C39712]/20">
          SCORE: {{ score }} / {{ questions.length }}
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-20 relative z-10">
      <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-white opacity-20 mx-auto"></div>
    </div>

    <!-- Result Screen -->
    <div v-else-if="showResult" class="text-center py-12 animate-fade-in relative z-10">
      <div class="mb-8">
        <div class="w-24 h-24 bg-white/5 rounded-3xl mx-auto flex items-center justify-center border border-white/10 shadow-2xl mb-6">
          <span class="text-5xl" v-if="score >= questions.length / 2">🏆</span>
          <span class="text-5xl" v-else>✨</span>
        </div>
      </div>
      <h3 class="text-3xl font-black tracking-tighter uppercase mb-2">Quiz Terminé</h3>
      <p class="text-gray-400 font-medium text-lg mb-10">
        Félicitations ! Vous avez obtenu <span class="text-white font-black">{{ score }}</span> sur {{ questions.length }}.
      </p>
      
      <div class="flex flex-col sm:flex-row gap-4 justify-center">
        <button @click="handleRetry" class="px-8 py-4 bg-white/5 border border-white/10 text-white rounded-2xl font-bold hover:bg-white/10 transition-all active:scale-95">
          Recommencer
        </button>
        <button @click="handleClose" class="px-8 py-4 bg-white text-[#082540] rounded-2xl font-black hover:bg-gray-100 transition-all shadow-xl active:scale-95">
          Retour aux jeux
        </button>
      </div>
    </div>

    <!-- Question View -->
    <div v-else class="min-h-[400px] flex flex-col justify-between relative z-10">
      <div>
        <div class="mb-10">
          <div class="flex justify-between items-end mb-3">
            <span class="text-[10px] font-black uppercase tracking-widest text-gray-500">
              Question {{ currentQuestionIndex + 1 }} <span class="opacity-30">/ {{ questions.length }}</span>
            </span>
            <span class="text-[10px] font-black uppercase tracking-widest text-[#C39712]">
              {{ Math.round(((currentQuestionIndex + 1) / questions.length) * 100) }}%
            </span>
          </div>
          <div class="w-full bg-white/5 rounded-full h-1.5 overflow-hidden">
            <div class="bg-white h-1.5 rounded-full transition-all duration-700 ease-out" 
                 :style="{ width: `${((currentQuestionIndex + 1) / questions.length) * 100}%` }"></div>
          </div>
        </div>

        <h3 class="text-2xl font-black tracking-tight text-white mb-10 leading-snug">
          {{ currentQuestion.text }}
        </h3>

        <!-- Choice Question -->
        <div v-if="currentQuestion.type === 'choice'" class="grid gap-4">
          <button 
            v-for="option in currentQuestion.options" 
            :key="option"
            @click="selectOption(option)"
            :disabled="answered"
            class="w-full text-left p-6 rounded-[1.5rem] border-2 transition-all duration-300 relative overflow-hidden group/opt"
            :class="getOptionClass(option)"
          >
            <span class="z-10 relative font-bold text-lg">{{ option }}</span>
            
            <div v-if="answered && option === currentQuestion.correctAnswer" class="absolute right-6 top-1/2 -translate-y-1/2">
              <div class="bg-green-500 rounded-full p-1 shadow-lg shadow-green-500/20">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" />
                </svg>
              </div>
            </div>
            
            <div v-if="answered && option === selectedAnswer && option !== currentQuestion.correctAnswer" class="absolute right-6 top-1/2 -translate-y-1/2">
              <div class="bg-red-500 rounded-full p-1 shadow-lg shadow-red-500/20">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </div>
            </div>
          </button>
        </div>

        <!-- Number Question -->
        <div v-if="currentQuestion.type === 'number'" class="mt-4">
          <input 
            type="number" 
            v-model="numberInput" 
            placeholder="Réponse numérique..."
            :disabled="answered"
            @keyup.enter="submitNumber"
            class="w-full p-6 bg-white/5 border-2 border-white/10 rounded-[1.5rem] text-white focus:ring-4 focus:ring-white/10 transition-all text-center text-4xl font-black tracking-tighter outline-none"
            :class="{ 'border-green-500/50 bg-green-500/10': answered && isCorrect, 'border-red-500/50 bg-red-500/10': answered && !isCorrect }"
          />
          <div v-if="!answered" class="mt-8 text-center">
            <button @click="submitNumber" class="px-12 py-4 bg-white text-[#082540] rounded-2xl font-black text-lg hover:shadow-2xl hover:shadow-white/10 transition-all active:scale-95">
              Valider la réponse
            </button>
          </div>
        </div>
      </div>

      <!-- Feedback Section -->
      <div v-if="answered" class="mt-10 p-6 rounded-3xl bg-white/5 border border-white/10 animate-fade-in-up">
        <div class="flex items-start gap-5">
          <div class="mt-1">
             <div v-if="isCorrect" class="w-10 h-10 bg-green-500/20 rounded-2xl flex items-center justify-center border border-green-500/30">
                <svg class="h-6 w-6 text-green-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" />
                </svg>
             </div>
             <div v-else class="w-10 h-10 bg-red-500/20 rounded-2xl flex items-center justify-center border border-red-500/30">
                <svg class="h-6 w-6 text-red-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M6 18L18 6M6 6l12 12" />
                </svg>
             </div>
          </div>
          <div class="flex-1">
            <h4 class="text-xs font-black uppercase tracking-widest mb-2" :class="isCorrect ? 'text-green-400' : 'text-red-400'">
              {{ isCorrect ? 'Excellente réponse !' : 'Oups, ce n\'est pas tout à fait ça' }}
            </h4>
            <p v-if="!isCorrect" class="text-sm text-gray-300 font-medium mb-3">
              La réponse correcte est : <span class="text-white font-black">{{ currentQuestion.correctAnswer }}</span>
            </p>
            <div class="bg-black/20 p-4 rounded-2xl border border-white/5">
              <p class="text-sm text-gray-400 leading-relaxed italic">
                <span class="text-white not-italic font-bold">Le saviez-vous ?</span> <br>
                {{ currentQuestion.explanation }}
              </p>
            </div>
          </div>
        </div>
        <div class="mt-8 flex justify-end">
          <button @click="nextQuestion" class="px-8 py-3 bg-white text-[#082540] rounded-xl font-black text-sm uppercase tracking-widest hover:bg-gray-100 transition-all flex items-center gap-2 group shadow-xl">
            {{ currentQuestionIndex < questions.length - 1 ? 'Question suivante' : 'Voir mon score' }}
            <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 transition-transform group-hover:translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M9 5l7 7-7 7" />
            </svg>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick, watch } from 'vue'

const props = defineProps({
  gameContent: {
    type: String,
    required: true
  },
  title: {
    type: String,
    default: 'Quiz'
  }
})

const emit = defineEmits(['close', 'retry'])

// State
const loading = ref(false)
const currentQuestionIndex = ref(0)
const score = ref(0)
const showResult = ref(false)
const answered = ref(false)
const selectedAnswer = ref<string | null>(null)
const numberInput = ref('')
const isCorrect = ref(false)

// Timer State
const timeRemaining = ref(30)
const timerInterval = ref<ReturnType<typeof setInterval> | null>(null)

// Parsing content
const questions = computed(() => {
  try {
    const data = JSON.parse(props.gameContent)
    return data.questions || []
  } catch (e) {
    console.error('Invalid JSON content', e)
    return []
  }
})

const currentQuestion = computed(() => questions.value[currentQuestionIndex.value])

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
  selectedAnswer.value = null
  numberInput.value = ''
  isCorrect.value = false
  startTimer()
}

// Timer Functions
function startTimer() {
  stopTimer()
  timeRemaining.value = 30
  
  timerInterval.value = setInterval(() => {
    if (timeRemaining.value > 0) {
      timeRemaining.value--
    } else {
      // Time's up - auto submit as incorrect
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
  
  // Auto advance after 2 seconds
  setTimeout(() => {
    nextQuestion()
  }, 2000)
}

const timerClass = computed(() => {
  if (timeRemaining.value <= 5) {
    return 'text-red-400 bg-red-500/20 border-red-500/30 animate-pulse'
  } else if (timeRemaining.value <= 10) {
    return 'text-orange-400 bg-orange-500/10 border-orange-500/20'
  }
  return 'text-white bg-white/10 border-white/20'
})

// Safe emit handlers with nextTick
async function handleClose() {
  stopTimer()
  await nextTick()
  emit('close')
}

async function handleRetry() {
  stopTimer()
  await nextTick()
  emit('retry')
}

function getOptionClass(option: string) {
  if (!answered.value) {
    return selectedAnswer.value === option 
      ? 'border-[#C39712]/50 bg-[#C39712]/10 text-white' 
      : 'border-white/5 hover:border-white/10 hover:bg-white/5 text-gray-400'
  }
  
  if (option === currentQuestion.value.correctAnswer) {
    return 'border-green-500/50 bg-green-500/10 text-white'
  }
  
  if (option === selectedAnswer.value && option !== currentQuestion.value.correctAnswer) {
    return 'border-red-500/50 bg-red-500/10 text-white'
  }
  
  return 'border-white/5 text-gray-600 opacity-40' // Dim incorrect options
}

// Lifecycle
onMounted(() => {
  startTimer()
})

onUnmounted(() => {
  stopTimer()
})

// Stop timer when answer is submitted
watch(answered, (newVal) => {
  if (newVal) {
    stopTimer()
  }
})
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.8s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
.animate-fade-in-up {
  animation: fadeInUp 0.5s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes fadeInUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
</style>
