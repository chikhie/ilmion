<template>
  <div class="bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-700 mt-12">
    <h2 class="text-2xl font-bold text-white mb-6">{{ section.title }}</h2>
    <div class="space-y-6">
      <div v-for="question in section.questions" :key="question.id" class="bg-gray-750 p-5 rounded-lg border border-gray-700 bg-gray-900/50">
        <p class="font-medium text-gray-200 mb-4">{{ question.question }}</p>
        <div class="space-y-3">
          <button
            v-for="(option, optIndex) in question.options"
            :key="optIndex"
            @click="handleAnswer(question.id, optIndex)"
            class="w-full text-left px-4 py-3 rounded-md border transition-all duration-200 flex items-center"
            :class="getOptionClass(question.id, optIndex)"
            :disabled="answers[`${section.title}-${question.id}`] !== undefined"
          >
            <span class="w-6 h-6 rounded-full border flex items-center justify-center mr-3 text-xs" 
                  :class="getRadioClass(question.id, optIndex)">
              <span v-if="answers[`${section.title}-${question.id}`] === optIndex" class="w-3 h-3 rounded-full bg-current"></span>
            </span>
            {{ option }}
          </button>
        </div>
        <div v-if="answers[`${section.title}-${question.id}`] !== undefined" class="mt-4 p-3 rounded bg-gray-900/80 border border-gray-700">
          <p v-if="answers[`${section.title}-${question.id}`] === question.correctAnswer" class="text-green-400 font-medium flex items-center">
            <span class="mr-2">✓</span> Correct !
          </p>
          <p v-else class="text-red-400 font-medium flex items-center">
            <span class="mr-2">✗</span> Incorrect. La bonne réponse était : <span class="ml-1 text-white">{{ question.options[question.correctAnswer] }}</span>
          </p>
        </div>
      </div>
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

interface QuizSection {
  type: 'quiz'
  title: string
  questions: QuizQuestion[]
}

interface Props {
  section: QuizSection
}

const props = defineProps<Props>()

const answers = ref<Record<string, number>>({})

const handleAnswer = (questionId: number, answerIndex: number) => {
  const key = `${props.section.title}-${questionId}`
  if (answers.value[key] === undefined) {
    answers.value[key] = answerIndex
  }
}

const getOptionClass = (questionId: number, optionIndex: number) => {
  const key = `${props.section.title}-${questionId}`
  const userAnswer = answers.value[key]
  const question = props.section.questions.find((q) => q.id === questionId)
  
  if (!question) return ''
  
  if (userAnswer === undefined) {
    return 'border-gray-600 hover:bg-gray-700 hover:border-gray-500 text-gray-300'
  }
  
  if (optionIndex === question.correctAnswer) {
    return 'bg-green-900/30 border-green-600 text-green-300'
  }
  
  if (userAnswer === optionIndex && userAnswer !== question.correctAnswer) {
    return 'bg-red-900/30 border-red-600 text-red-300'
  }
  
  return 'border-gray-700 opacity-50 text-gray-500'
}

const getRadioClass = (questionId: number, optionIndex: number) => {
  const key = `${props.section.title}-${questionId}`
  const userAnswer = answers.value[key]
  const question = props.section.questions.find((q) => q.id === questionId)
  
  if (!question) return ''
  
  if (userAnswer === undefined) {
    return 'border-gray-500 text-gray-400'
  }
  
  if (optionIndex === question.correctAnswer) {
    return 'border-green-500 text-green-500'
  }
  
  if (userAnswer === optionIndex && userAnswer !== question.correctAnswer) {
    return 'border-red-500 text-red-500'
  }
  
  return 'border-gray-700 text-gray-700'
}
</script>

