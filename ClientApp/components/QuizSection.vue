<template>
  <div class="bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-700">
    <h2 class="text-2xl font-bold text-white mb-6">{{ title }}</h2>
    
    <div v-if="quizData" class="space-y-6">
      <div 
        v-for="question in quizData.questions" 
        :key="question.id" 
        class="bg-gray-900/50 p-5 rounded-lg border border-gray-700"
      >
        <p class="font-medium text-gray-200 mb-4 text-lg">
          <span class="text-[#C39712] mr-2">Question {{ question.id }}:</span>
          {{ question.question }}
        </p>
        
        <div class="space-y-3">
          <button
            v-for="(option, optIndex) in question.options"
            :key="optIndex"
            @click="handleAnswer(question.id, optIndex)"
            class="cursor-pointer text-gray-300 w-full text-left px-4 py-3 rounded-md border transition-all duration-200 flex items-center group hover:text-[#C39712] hover:border-[#C39712]"
            :class="getOptionClass(question.id, optIndex, question.correctAnswer)"
            :disabled="answers[question.id] !== undefined"
          >
            <span 
              class="w-6 h-6 rounded-full border-2 flex items-center justify-center mr-3 text-xs font-bold transition-colors" 
              :class="getRadioClass(question.id, optIndex, question.correctAnswer)"
            >
              <span v-if="answers[question.id] === optIndex" class="color-white w-3 h-3 rounded-full bg-current"></span>
            </span>
            <span class="flex-1">{{ option }}</span>
          </button>
        </div>
        
        <!-- Feedback -->
        <div v-if="answers[question.id] !== undefined" class="mt-4 p-4 rounded-lg border transition-all duration-300">
          <div v-if="answers[question.id] === question.correctAnswer" class="bg-green-900/30 border-green-600">
            <p class="text-green-400 font-medium flex items-center">
              <svg class="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
              </svg>
              Correct ! Bonne réponse 🎉
            </p>
          </div>
          <div v-else class="bg-red-900/30 border-red-600">
            <p class="text-red-400 font-medium flex items-center mb-2">
              <svg class="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd"/>
              </svg>
              Incorrect
            </p>
            <p class="text-gray-300 text-sm">
              La bonne réponse était : <span class="text-white font-medium">{{ question.options[question.correctAnswer] }}</span>
            </p>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Score -->
    <div v-if="allAnswered" class="mt-8 p-6 bg-gradient-to-r from-[#082540] to-[#0a3a5f] rounded-lg border border-[#C39712]">
      <div class="text-center">
        <p class="text-2xl font-bold text-white mb-2">
          Score : {{ score }} / {{ quizData?.questions.length }}
        </p>
        <p class="text-gray-300">
          {{ getScoreMessage() }}
        </p>
        <button 
          @click="resetQuiz" 
          class="mt-4 px-6 py-2 bg-[#C39712] text-[#082540] rounded-lg font-bold hover:bg-yellow-500 transition-colors"
        >
          Recommencer le quiz
        </button>
      </div>
    </div>
    
    <div v-if="!quizData" class="text-red-400">
      Erreur : Format de quiz invalide
    </div>
  </div>
</template>

<script setup lang="ts">
interface QuizQuestion {
  id: number
  question: string
  options: string[]
  correctAnswer: number
}

interface QuizData {
  questions: QuizQuestion[]
}

interface Props {
  title: string
  content: string
}

const props = defineProps<Props>()

const answers = ref<Record<number, number>>({})
const quizData = ref<QuizData | null>(null)

// Parser le contenu JSON
onMounted(() => {
  try {
    quizData.value = JSON.parse(props.content)
  } catch (error) {
    console.error('Erreur lors du parsing du quiz:', error)
  }
})

const handleAnswer = (questionId: number, optionIndex: number) => {
  if (answers.value[questionId] === undefined) {
    answers.value[questionId] = optionIndex
  }
}

const getOptionClass = (questionId: number, optionIndex: number, correctAnswer: number) => {
  const hasAnswered = answers.value[questionId] !== undefined
  const isSelected = answers.value[questionId] === optionIndex
  const isCorrect = optionIndex === correctAnswer
  
  if (!hasAnswered) {
    return 'bg-gray-800 border-gray-600 hover:bg-gray-700 hover:border-[#C39712] cursor-pointer'
  }
  
  if (isSelected && isCorrect) {
    return 'bg-green-900/30 border-green-600 text-green-400'
  }
  
  if (isSelected && !isCorrect) {
    return 'bg-red-900/30 border-red-600 text-red-400'
  }
  
  if (!isSelected && isCorrect) {
    return 'bg-green-900/20 border-green-700 text-green-400'
  }
  
  return 'bg-gray-800 border-gray-600 opacity-50 cursor-not-allowed'
}

const getRadioClass = (questionId: number, optionIndex: number, correctAnswer: number) => {
  const hasAnswered = answers.value[questionId] !== undefined
  const isSelected = answers.value[questionId] === optionIndex
  const isCorrect = optionIndex === correctAnswer
  
  if (!hasAnswered) {
    return 'border-gray-500 text-gray-500 group-hover:border-[#C39712] group-hover:text-[#C39712]'
  }
  
  if (isSelected && isCorrect) {
    return 'border-green-400 text-green-400'
  }
  
  if (isSelected && !isCorrect) {
    return 'border-red-400 text-red-400'
  }
  
  if (!isSelected && isCorrect) {
    return 'border-green-400 text-green-400'
  }
  
  return 'border-gray-600 text-gray-600'
}

const allAnswered = computed(() => {
  if (!quizData.value) return false
  return quizData.value.questions.every(q => answers.value[q.id] !== undefined)
})

const score = computed(() => {
  if (!quizData.value) return 0
  return quizData.value.questions.filter(q => answers.value[q.id] === q.correctAnswer).length
})

const getScoreMessage = () => {
  if (!quizData.value) return ''
  const percentage = (score.value / quizData.value.questions.length) * 100
  
  if (percentage === 100) return '🎉 Parfait ! Vous maîtrisez le sujet !'
  if (percentage >= 75) return '👏 Excellent ! Très bon résultat !'
  if (percentage >= 50) return '👍 Bien ! Continuez vos efforts !'
  return '💪 Révisez encore et réessayez !'
}

const resetQuiz = () => {
  answers.value = {}
}
</script>

