<template>
  <div class="min-h-screen bg-[#1e1f22] text-white flex flex-col relative overflow-hidden font-sans">
    
    <!-- Background Texture -->
    <div class="absolute inset-0 z-0 pointer-events-none">
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 0 0;"></div>
        <div class="absolute inset-0 opacity-[0.05]" style="background-image: radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px; background-position: 20px 20px;"></div>
        <div class="absolute top-[10%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/10 rounded-full blur-[120px] animate-pulse-slow"></div>
    </div>

    <AppHeader />

    <main class="flex-grow container mx-auto px-4 py-12 relative z-10 max-w-7xl">
      <div class="text-center mb-12 md:mb-16 animate-fade-in">
        <div class="inline-flex items-center justify-center w-20 h-20 rounded-2xl bg-[#2b2d31]/80 border border-[#00B578]/30 text-[#00B578] shadow-lg shadow-[#00B578]/10 mb-6 backdrop-blur-sm">
            <i class="fas fa-atom text-4xl"></i>
        </div>
        <h1 class="text-4xl font-bold font-sans text-white sm:text-5xl lg:text-6xl tracking-wide mb-4">
          Thèmes & Modules
        </h1>
        <p class="max-w-xl mx-auto text-lg text-gray-400">
          Explorez nos thèmes scientifiques et testez vos connaissances.
        </p>
      </div>

      <div v-if="pending" class="flex justify-center py-20 relative z-20">
        <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#00B578]"></div>
      </div>

      <div v-else-if="error" class="text-center py-10 px-6 text-red-400 bg-red-900/20 rounded-2xl border border-red-500/30 backdrop-blur-sm relative z-20 max-w-2xl mx-auto">
        <i class="fas fa-bug text-3xl mb-3 text-red-500"></i>
        <p class="font-bold tracking-wide">Une erreur est survenue lors du chargement des thèmes.</p>
      </div>
      
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-8 animate-fade-in-up delay-200">
        <div v-for="theme in themes" :key="theme.id" class="group relative flex flex-col bg-[#2b2d31]/80 rounded-2xl border border-[#00B578]/20 overflow-hidden backdrop-blur-sm transition-all duration-300 hover:-translate-y-2 hover:border-[#00B578]/50 hover:shadow-xl hover:shadow-[#00B578]/10">
             
             <!-- Decorative glowing corner -->
             <div class="absolute top-0 right-0 w-32 h-32 bg-[#00B578]/10 rounded-full -mr-16 -mt-16 blur-2xl group-hover:bg-[#00B578]/20 transition-all duration-500"></div>

             <div class="relative z-10 p-6 md:p-8 flex-grow">
                 <div class="flex items-center gap-4 mb-4">
                    <div class="w-12 h-12 rounded-xl bg-[#1e1f22] border-2 border-[#00B578]/40 flex items-center justify-center text-[#00B578] shadow-inner flex-shrink-0 group-hover:border-[#00B578] transition-colors">
                        <i class="fas fa-laptop-code text-2xl"></i>
                    </div>
                    <h2 class="text-xl md:text-2xl font-bold font-sans text-white group-hover:text-[#00B578] transition-colors line-clamp-2">
                        {{ theme.title }}
                    </h2>
                 </div>
                 <p class="text-gray-400 text-sm leading-relaxed mb-6 line-clamp-3">{{ theme.description }}</p>

                 <div class="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg bg-[#1e1f22] border border-[#00B578]/20 text-[#00B578] text-xs font-bold uppercase tracking-widest">
                    <i class="fas fa-layer-group"></i>
                    <span>{{ theme.subjects?.length || 0 }} modules</span>
                 </div>
             </div>
             
             <div class="relative z-10 p-6 pt-0 mt-auto">
                 <NuxtLink :to="`/games/${theme.id}`" class="w-full block text-center bg-[#00B578]/10 text-[#00B578] border border-[#00B578]/30 rounded-xl py-3 px-6 font-bold text-xs uppercase tracking-widest hover:bg-[#00B578] hover:text-[#1e1f22] transition-colors duration-300">
                    Explorer
                 </NuxtLink>
             </div>
        </div>
      </div>
      
      <div v-if="!pending && (!themes || themes.length === 0)" class="text-center py-20 bg-[#2b2d31]/50 rounded-2xl border border-dashed border-white/10 relative z-20">
          <i class="fas fa-flask text-4xl text-gray-600 mb-4 opacity-50"></i>
          <p class="text-gray-500 text-lg italic">Aucun thème disponible pour le moment.</p>
      </div>

    </main>
    
    <AppFooter />
  </div>
</template>

<script setup>
const { data: themes, pending, error } = await useFetch('/api/theme')

useSeoMeta({
  title: 'Thèmes & Modules | Ilmanar',
  description: 'Explorez nos thèmes et testez vos connaissances scientifiques avec nos modules éducatifs.'
})
</script>

<style scoped>
.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
.animate-fade-in { animation: fadeIn 0.8s ease-out forwards; opacity: 0; }
.animate-fade-in-up { animation: fadeInUp 0.8s ease-out forwards; opacity: 0; }
.delay-200 { animation-delay: 0.2s; }

@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.05; } }
@keyframes fadeIn { to { opacity: 1; } }
@keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
</style>
