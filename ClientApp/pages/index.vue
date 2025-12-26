<template>
  <div class="flex-grow flex flex-col items-center justify-center relative overflow-hidden py-12">
    <!-- Background Layers -->
    <div class="absolute inset-0 z-0 pointer-events-none">
       <!-- Geometric Pattern -->
       <div class="absolute inset-0 pattern-geometric opacity-[0.03]"></div>
       <!-- Wood Texture Overlay (already in body/app but reinforced here for depth) -->
       <div class="absolute inset-0 bg-texture-wood opacity-50 mix-blend-overlay"></div>
       
       <!-- Ambient Glows -->
       <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-brand-gold/10 blur-[150px] rounded-full mix-blend-screen"></div>
       <div class="absolute bottom-1/4 right-1/4 w-96 h-96 bg-brand-wood/30 blur-[150px] rounded-full"></div>
    </div>

    <!-- Hero Section -->
    <section class="relative z-10 w-full max-w-7xl mx-auto px-4 py-10 md:py-20 text-center md:text-left">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-12 items-center">
        <!-- Left Column: Content -->
        <div class="space-y-10 animate-fade-in">
            <div class="mb-6 relative inline-block group">
               <div class="absolute inset-0 bg-brand-gold/20 blur-xl rounded-full scale-75 group-hover:scale-100 transition-transform duration-700"></div>
               <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="relative h-24 w-24 md:h-32 md:w-32 mx-auto md:mx-0 drop-shadow-2xl transition-transform duration-500 group-hover:rotate-6" />
            </div>

            <h1 class="text-5xl md:text-8xl font-serif-title font-bold tracking-tight text-brand-parchment drop-shadow-lg leading-tight">
              <span class="bg-clip-text text-transparent bg-gradient-to-b from-brand-parchment to-brand-parchment/70">ILMANAR</span>
            </h1>

            <div class="border-t border-b border-brand-gold/20 py-4 my-6 bg-brand-dark/20 backdrop-blur-sm inline-block">
                <p class="text-xl md:text-2xl text-brand-parchment/80 font-serif-title italic">
                “L’histoire éclairée par le savoir”
                </p>
            </div>

            <p class="text-lg md:text-xl text-brand-parchment/60 max-w-xl font-sans-body leading-relaxed mx-auto md:mx-0">
              Une plateforme immersive pour redécouvrir l'âge d'or de la civilisation
              et maîtriser la langue arabe à travers le jeu.
            </p>

            <div class="flex flex-col sm:flex-row items-center justify-center md:justify-start gap-4 pt-4">
              <NuxtLink v-if="!authStore.isAuthenticated" to="/register" 
                class="px-8 py-3 bg-texture-parchment text-brand-dark text-lg font-bold font-serif-title rounded shadow-lg hover:shadow-brand-gold/20 hover:scale-105 transition-all duration-300 border border-brand-gold/50">
                Commencer l'aventure
              </NuxtLink>

              <NuxtLink v-if="authStore.isAuthenticated" to="/games" 
                class="px-8 py-3 bg-texture-parchment text-brand-dark text-lg font-bold font-serif-title rounded shadow-lg hover:shadow-brand-gold/20 hover:scale-105 transition-all duration-300 border border-brand-gold/50">
                Reprendre l'exploration
              </NuxtLink>
              
              <NuxtLink to="/login" v-if="!authStore.isAuthenticated"
                 class="px-8 py-3 border border-brand-parchment/20 text-brand-parchment text-lg font-bold font-sans-body rounded hover:bg-brand-parchment/5 transition-all duration-300">
                Connexion
              </NuxtLink>
            </div>
        </div>

        <!-- Right Column: Hook Quiz -->
        <div class="animate-fade-in-down delay-300 relative" v-if="!authStore.isAuthenticated">
             <div class="absolute -top-10 -right-10 w-20 h-20 bg-brand-gold/10 rounded-full blur-xl animate-pulse"></div>
             <QuizSection 
                title="Quiz Découverte" 
                :content="demoQuizContent"
                :is-demo="true"
             />
             <p class="text-center text-brand-parchment/40 text-xs mt-4 italic">Testez vos connaissances en 3 questions</p>
        </div>
        
        <!-- Placeholder for right column (image or illustration) if authenticated -->
        <div v-else class="hidden md:flex justify-center items-center animate-fade-in">
             <div class="relative w-full aspect-square max-w-md bg-brand-gold/5 rounded-full flex items-center justify-center border border-brand-gold/10 overflow-hidden">
                  <div class="absolute inset-0 pattern-geometric opacity-20 animate-spin-slow"></div>
                  <!-- Abstract Stylized Lighthouse/Knowledge Icon via CSS/SVG could go here -->
                  <span class="text-9xl opacity-20">🕌</span>
             </div>
        </div>

      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
useHead({
  title: 'Ilmanar - Apprenez Autrement',
  meta: [
    { name: 'description', content: 'Plateforme moderne d\'apprentissage interactive.' }
  ]
})

const authStore = useAuthStore()

definePageMeta({
  middleware: 'guest'
})

const features = [
  {
    title: "Interactif",
    description: "Des jeux conçus pour mémoriser durablement.",
    icon: "🎲"
  },
  {
    title: "Premium",
    description: "Magazines et contenus exclusifs.",
    icon: "⭐"
  },
  {
    title: "Suivi",
    description: "Mesurez vos progrès en temps réel.",
    icon: "📊"
  }
]

const demoQuizContent = JSON.stringify({
  questions: [
    {
      id: 1,
      question: "Quelle ville était la capitale de l'Andalousie omeyyade ?",
      options: ["Damas", "Cordoue", "Bagdad", "Le Caire"],
      correctAnswer: 1
    },
    {
      id: 2,
      question: "Qui est considéré comme le père de l'algèbre ?",
      options: ["Al-Khwarizmi", "Ibn Sina", "Al-Biruni", "Ibn Rushd"],
      correctAnswer: 0
    },
    {
      id: 3,
      question: "Quelle célèbre bibliothèque se trouvait à Bagdad ?",
      options: ["La Maison de la Sagesse", "La Bibliothèque d'Alexandrie", "Al-Qarawiyyin", "Dar al-Ilm"],
      correctAnswer: 0
    }
  ]
})
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 1s ease-out forwards;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.bg-radial-gradient {
  background: radial-gradient(circle at center, var(--tw-gradient-from), var(--tw-gradient-to));
}

@keyframes fade-in-down {
  0% { opacity: 0; transform: translateY(-10px); }
  100% { opacity: 1; transform: translateY(0); }
}

.animate-fade-in-down {
  animation: fade-in-down 0.8s ease-out;
}
</style>
