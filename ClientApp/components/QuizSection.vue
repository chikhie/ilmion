<template>
  <div class="relative w-full max-w-lg mx-auto font-sans">
    <!-- Outer Card with Pattern Border -->
    <div class="bg-[#Fdfbf7] p-2 rounded-[2rem] shadow-2xl relative overflow-hidden transform transition-all hover:scale-[1.01] duration-500">
        <!-- Pattern Texture on Border -->
        <div class="absolute inset-0 pattern-geometric opacity-10 pointer-events-none"></div>
        
        <!-- Inner Card with Line Border -->
        <div class="bg-white rounded-[1.5rem] border-2 border-[#1e1f22]/80 p-6 md:p-8 relative z-10 flex flex-col min-h-[500px]">
            
            <!-- Top Calligraphy Icon -->
            <div class="flex justify-center mb-6">
                 <!-- SVG Icon representing 'Ilm' / Knowledge -->
                <svg class="h-16 w-16 text-[#C39712] opacity-80 drop-shadow-sm" viewBox="0 0 100 100" fill="currentColor">
                    <!-- A stylized calligraphy shape -->
                    <path d="M50 20 C60 15 75 15 85 25 C90 35 85 45 75 45 C65 45 60 35 60 25 M60 25 C60 55 60 75 50 85 M50 85 C40 75 40 55 50 25" stroke="currentColor" stroke-width="3" fill="none" stroke-linecap="round" />
                    <path d="M55 22 Q65 12 75 22" stroke="currentColor" fill="none" stroke-width="2" />
                    <!-- Dot -->
                    <circle cx="65" cy="15" r="3" fill="currentColor" />
                </svg>
            </div>

            <!-- Header / Title -->
             <div class="text-center mb-8 h-20 flex items-center justify-center">
                 <h2 class="text-2xl font-sans font-bold text-[#1e1f22] leading-tight animate-fade-in">
                    {{ isFinished ? 'Résultats du Quiz' : currentQuestion?.question }}
                 </h2>
             </div>
            
            <!-- Loading -->
            <div v-if="!quizData" class="text-[#2b2d31] font-medium text-center p-10 animate-pulse">
              Chargement de la sagesse ancienne...
            </div>

            <!-- Quiz Content -->
            <div v-else class="flex-grow flex flex-col relative z-10">
                
                <!-- Question Options -->
                <div v-if="!isFinished" class="flex-grow flex flex-col justify-center space-y-4">
                    <button
                        v-for="(option, optIndex) in currentQuestion.options"
                        :key="optIndex"
                        @click="handleAnswer(optIndex)"
                        :disabled="hasAnsweredCurrent"
                        class="w-full relative group transition-all duration-300"
                    >
                        <!-- Ornamental Button Container -->
                        <div class="relative overflow-hidden rounded-xl border-2 transition-all duration-300 shadow-sm group-hover:shadow-md"
                             :class="getButtonStyles(optIndex).container">
                             
                             <!-- Button Content -->
                             <div class="relative z-10 px-6 py-4 flex items-center w-full">
                                 <!-- Letter Block -->
                                 <span class="mr-4 font-sans font-bold opacity-70 text-sm tracking-wider w-6">
                                     {{ String.fromCharCode(65 + optIndex) }}.
                                 </span>
                                 
                                 <!-- Text -->
                                 <span class="font-medium text-lg flex-1 text-left" :class="getButtonStyles(optIndex).text">
                                    {{ option }}
                                 </span>
                                 
                                 <!-- Selection Checkmark -->
                                 <div v-if="currentAnswer === optIndex" class="ml-2 text-[#00B578] animate-scale-in">
                                    <svg class="w-6 h-6 bg-white rounded-full p-1 shadow-sm" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" />
                                    </svg>
                                 </div>
                             </div>

                             <!-- Decorative Corners (CSS) -->
                             <div class="absolute top-0 left-0 w-2 h-2 border-t-2 border-l-2 opacity-50 transition-colors" :class="getButtonStyles(optIndex).corner"></div>
                             <div class="absolute top-0 right-0 w-2 h-2 border-t-2 border-r-2 opacity-50 transition-colors" :class="getButtonStyles(optIndex).corner"></div>
                             <div class="absolute bottom-0 left-0 w-2 h-2 border-b-2 border-l-2 opacity-50 transition-colors" :class="getButtonStyles(optIndex).corner"></div>
                             <div class="absolute bottom-0 right-0 w-2 h-2 border-b-2 border-r-2 opacity-50 transition-colors" :class="getButtonStyles(optIndex).corner"></div>
                        </div>
                    </button>

                    <!-- Feedback Area (Inline) -->
                    <div v-if="hasAnsweredCurrent" class="mt-2 text-center animate-fade-in font-sans min-h-[1.5em] flex items-center justify-center gap-2"
                         :class="isCurrentCorrect ? 'text-green-700' : 'text-red-700'">
                        <span v-if="isCurrentCorrect">✨ Correct !</span>
                        <span v-else>⚠️ La réponse était {{ currentQuestion.options[currentQuestion.correctAnswer] }}</span>
                    </div>
                </div>

                <!-- Result Step -->
                <div v-else class="text-center flex-grow flex flex-col justify-center items-center animate-fade-in">
                    <div class="mb-4 relative">
                         <div class="absolute inset-0 bg-[#00B578]/20 blur-xl rounded-full animate-pulse"></div>
                         <div class="text-7xl relative drop-shadow-md py-4">
                            {{ score >= (quizData.questions.length / 2) ? '🏆' : '🕯️' }}
                         </div>
                    </div>
                    
                    <p class="text-3xl font-sans font-bold text-[#1e1f22] mb-4">
                      Score : <span class="text-[#00B578] text-4xl">{{ score }}</span> / {{ quizData.questions.length }}
                    </p>
                    
                    <p class="text-[#2b2d31]/80 font-sans italic mb-8 max-w-xs mx-auto">
                        {{ getScoreMessage() }}
                    </p>
                    
                    <div v-if="isDemo" class="w-full">
                        <NuxtLink to="/register" class="group relative block w-full py-4 overflow-hidden rounded-xl bg-[#1e1f22] text-white shadow-lg transition-all hover:scale-[1.02] hover:shadow-[#00B578]/20">
                           <div class="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent translate-x-[-100%] group-hover:translate-x-[100%] transition-transform duration-1000"></div>
                           <span class="relative font-bold uppercase tracking-widest flex items-center justify-center gap-2">
                               Créer un compte
                               <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
                           </span>
                        </NuxtLink>
                        <div class="mt-4 flex items-center justify-center gap-2 text-xs text-[#2b2d31]/60">
                             <svg class="w-3 h-3" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
                             <span>Sauvegardez votre progression</span>
                        </div>
                    </div>
                    <div v-else class="w-full">
                         <button @click="resetQuiz" class="w-full py-3 border-2 border-[#00B578] text-[#1e1f22] font-bold rounded-lg hover:bg-[#00B578] hover:text-white transition-colors uppercase tracking-wider">
                             Rejouer
                         </button>
                    </div>
                </div>

                <!-- Footer / Progress -->
                <div class="mt-auto pt-8 flex flex-col items-center justify-center relative">
                     <!-- Next Button (Floating above progress) -->
                     <div class="h-10 mb-2 w-full flex justify-end">
                         <button 
                            v-if="hasAnsweredCurrent && !isFinished"
                            @click="nextQuestion"
                            class="px-6 py-2 bg-[#00B578] text-[#1e1f22] font-bold text-sm rounded-full shadow-lg hover:bg-[#d4a81e] hover:shadow-xl transition-all transform hover:-translate-y-1 flex items-center gap-2 animate-fade-in-up"
                         >
                            <span>Suivant</span>
                            <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/></svg>
                         </button>
                     </div>
                    
                    <!-- Custom Slider Progress -->
                    <div class="w-full h-1 bg-[#E2E8F0] rounded-full relative overflow-visible">
                        <!-- Filled Part -->
                        <div class="absolute top-0 left-0 h-full bg-[#C39712] rounded-full transition-all duration-500"
                             :style="{ width: `${((currentQuestionIndex + 1) / quizData.questions.length) * 100}%` }"></div>
                        
                        <!-- Slider Knob (The 'Knot') -->
                        <div class="absolute top-1/2 w-8 h-8 bg-white border-2 border-[#C39712] rounded-full shadow-md flex items-center justify-center transition-all duration-500 transform -translate-y-1/2 -translate-x-1/2 z-10"
                             :style="{ left: `${((currentQuestionIndex + 1) / quizData.questions.length) * 100}%` }">
                             <div class="w-2 h-2 bg-[#C39712] rounded-full"></div>
                        </div>
                    </div>
                    
                    <!-- Small decorative lighthouse bottom right -->
                    <div class="absolute bottom-0 right-[-10px] opacity-10 pointer-events-none">
                         <svg class="w-8 h-8" viewBox="0 0 24 24" fill="currentColor">
                             <path d="M12 3L14 8H10L12 3Z" />
                             <rect x="9" y="8" width="6" height="10" rx="1" />
                             <line x1="6" y1="21" x2="18" y2="21" stroke="currentColor" stroke-width="2"/>
                         </svg>
                    </div>
                </div>

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

interface QuizData {
  questions: QuizQuestion[]
}

interface Props {
  title: string
  content: string
  isDemo?: boolean
}

const props = defineProps<Props>()
const answers = ref<Record<number, number>>({})
const quizData = ref<QuizData | null>(null)
const currentQuestionIndex = ref(0)
const isFinished = ref(false)

onMounted(() => {
  try {
    quizData.value = JSON.parse(props.content)
  } catch (error) {
    console.error('Erreur quiz:', error)
  }
})

// Computeds
const currentQuestion = computed(() => quizData.value?.questions[currentQuestionIndex.value])
const currentAnswer = computed(() => currentQuestion.value ? answers.value[currentQuestion.value.id] : undefined)
const hasAnsweredCurrent = computed(() => currentAnswer.value !== undefined)
const isCurrentCorrect = computed(() => {
    if (!currentQuestion.value || !hasAnsweredCurrent.value) return false
    return currentAnswer.value === currentQuestion.value.correctAnswer
})
const score = computed(() => {
    if (!quizData.value) return 0
    return quizData.value.questions.filter(q => answers.value[q.id] === q.correctAnswer).length
})

// Actions
const handleAnswer = (idx: number) => {
    if (hasAnsweredCurrent.value || !currentQuestion.value) return
    answers.value[currentQuestion.value.id] = idx
}

const nextQuestion = () => {
    if (!quizData.value) return
    if (currentQuestionIndex.value < quizData.value.questions.length - 1) {
        currentQuestionIndex.value++
    } else {
        isFinished.value = true
    }
}

const resetQuiz = () => {
    answers.value = {}
    currentQuestionIndex.value = 0
    isFinished.value = false
}

// Styling Helper
const getButtonStyles = (optIndex: number) => {
    const isSelected = currentAnswer.value === optIndex
    const isCorrect = optIndex === currentQuestion.value?.correctAnswer

    // Default State (Blue/Grey clean look)
    // bg-[#F0F4F8] -> Light Slate
    // border-[#cfd9e0]
    let styles = {
        container: 'bg-[#F0F4F8] border-[#cfd9e0] hover:border-[#C39712] hover:bg-white',
        text: 'text-[#082540]',
        corner: 'border-[#082540] opacity-0'
    }

    if (isSelected) {
        // Selected State (Dark Blue like mockup)
        styles = {
            container: 'bg-[#082540] border-[#082540] shadow-md transform scale-[1.02]',
            text: 'text-white font-bold',
            corner: 'border-[#C39712] opacity-100' // Gold corners on selection
        }
    } else if (hasAnsweredCurrent.value && isCorrect) {
         // Correct State (Green hint if revealed)
         styles = {
            container: 'bg-green-50 border-green-500',
            text: 'text-green-800',
            corner: 'border-green-600 opacity-50'
        }
    } else if (hasAnsweredCurrent.value && !isCorrect && optIndex === currentQuestion.value?.correctAnswer) {
        // Reveal correct answer if wrong was picked
         styles = {
            container: 'bg-green-50 border-green-500 border-dashed',
            text: 'text-green-800',
            corner: 'border-green-600 opacity-30'
        }
    }

    return styles
}

const getScoreMessage = () => {
  if (!quizData.value) return ''
  const percentage = (score.value / quizData.value.questions.length) * 100
  if (percentage === 100) return 'Une sagesse digne des plus grands savants !'
  if (percentage >= 60) return 'Un esprit vif et éclairé.'
  return 'Le chemin du savoir est pavé d\'apprentissage.'
}
</script>

<style scoped>
.pattern-geometric {
    background-image: radial-gradient(#C39712 1px, transparent 1px);
    background-size: 20px 20px;
}
.animate-fade-in {
    animation: fadeIn 0.5s ease-out forwards;
}
.animate-fade-in-up {
    animation: fadeInUp 0.4s ease-out forwards;
}
.animate-scale-in {
    animation: scaleIn 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275) forwards;
}

@keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
}
@keyframes fadeInUp {
    from { opacity: 0; transform: translateY(10px); }
    to { opacity: 1; transform: translateY(0); }
}
@keyframes scaleIn {
    from { transform: scale(0); opacity: 0; }
    to { transform: scale(1); opacity: 1; }
}
</style>
