<template>
  <div class="flex-grow flex flex-col items-center justify-center relative overflow-hidden py-12">
    <!-- Background Gradients (Modern & Minimal) - Adjusted for overlay -->
    <div class="absolute inset-0 z-0 pointer-events-none">
      <div class="absolute inset-0 bg-radial-gradient from-[#0B3152] to-[#082540]"></div>
      <div class="absolute -top-[20%] -left-[10%] w-[60%] h-[60%] bg-[#C39712]/10 blur-[120px] rounded-full"></div>
      <div class="absolute -bottom-[20%] -right-[10%] w-[60%] h-[60%] bg-[#0B3152]/40 blur-[120px] rounded-full"></div>
    </div>

    <!-- Hero Section -->
    <section class="relative z-10 w-full max-w-6xl mx-auto px-4 py-20 text-center">
      <div class="space-y-8 animate-fade-in">
        <div class="mb-4">
          <img src="/Ilmanar.svg" alt="Ilmanar Logo" class="h-24 w-24 mx-auto drop-shadow-2xl" />
        </div>

        <h1 class="text-6xl md:text-9xl font-extrabold tracking-tighter">
          <span class="bg-clip-text text-transparent bg-gradient-to-r from-white via-white to-gray-500">ILMANAR</span>
        </h1>

        <p class="text-xl md:text-2xl text-gray-400 max-w-2xl mx-auto font-medium leading-relaxed">
          Apprenez de façon ludique et interactive.
          <br/>
          Une plateforme moderne dédiée à l'apprentissage de l'histoire islamique et de la langue arabe.
        </p>

        <div class="flex flex-col sm:flex-row items-center justify-center gap-6 pt-10">
          <NuxtLink v-if="!authStore.isAuthenticated" to="/register" 
            class="px-10 py-4 bg-white text-[#082540] text-lg font-bold rounded-full hover:bg-gray-100 transition-all duration-300 shadow-2xl">
            Commencer maintenant
          </NuxtLink>

          <NuxtLink v-if="authStore.isAuthenticated" to="/games" 
            class="px-10 py-4 bg-white text-[#082540] text-lg font-bold rounded-full hover:bg-gray-100 transition-all duration-300 shadow-2xl">
            Reprendre les jeux
          </NuxtLink>
          
          <NuxtLink to="/login" v-if="!authStore.isAuthenticated"
             class="px-10 py-4 border border-white/20 text-white text-lg font-bold rounded-full hover:bg-white/5 transition-all duration-300">
            Se connecter
          </NuxtLink>
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
