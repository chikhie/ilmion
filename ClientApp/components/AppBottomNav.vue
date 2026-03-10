<template>
  <nav class="lg:hidden fixed bottom-0 left-0 right-0 bg-[#1e1f22]/95 backdrop-blur-xl border-t border-[#2b2d31] pb-safe-area-inset-bottom z-50 shadow-[0_-5px_20px_rgba(30,31,34,0.3)]">
    <div class="flex justify-around items-center h-16">
      
      <!-- Jeux -->
      <NuxtLink to="/games" class="flex flex-col items-center justify-center w-full h-full text-[#949ba4] hover:text-[#00B578] active:text-[#00B578] transition-colors group" active-class="!text-[#00B578]">
         <div class="relative p-1 rounded-xl group-active:scale-95 transition-transform">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.752 11.168l-3.197-2.132A1 1 0 0010 9.87v4.263a1 1 0 001.555.832l3.197-2.132a1 1 0 000-1.664z" />
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
         </div>
        <span class="text-[10px] font-bold uppercase tracking-widest mt-0.5">Jeux</span>
      </NuxtLink>

      <!-- Magazine -->
      <NuxtLink to="/magazine" class="flex flex-col items-center justify-center w-full h-full text-[#949ba4] hover:text-[#00B578] active:text-[#00B578] transition-colors group" active-class="!text-[#00B578]">
         <div class="relative p-1 rounded-xl group-active:scale-95 transition-transform">
             <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" />
             </svg>
         </div>
         <span class="text-[10px] font-bold uppercase tracking-widest mt-0.5">Magazine</span>
      </NuxtLink>

      <!-- Accueil (Center) -->
      <NuxtLink to="/" class="flex flex-col items-center justify-center w-full h-full text-[#949ba4] hover:text-[#00B578] active:text-[#00B578] transition-colors group relative -top-3" active-class="!text-[#00B578]">
        <div class="relative p-3 bg-[#00B578] text-white border-4 border-[#1e1f22] rounded-full shadow-lg group-active:scale-95 transition-transform hover:bg-[#00895a] hover:scale-105">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
            </svg>
        </div>
        <span class="text-[10px] font-bold uppercase tracking-widest mt-0.5">Accueil</span>
      </NuxtLink>

      <!-- Vidéos -->
      <NuxtLink to="/videos" class="flex flex-col items-center justify-center w-full h-full text-[#949ba4] hover:text-[#00B578] active:text-[#00B578] transition-colors group" active-class="!text-[#00B578]">
         <div class="relative p-1 rounded-xl group-active:scale-95 transition-transform">
             <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
             </svg>
         </div>
         <span class="text-[10px] font-bold uppercase tracking-widest mt-0.5">Vidéos</span>
      </NuxtLink>

      <!-- Profil (Rightmost) -->
      <NuxtLink to="/profile" class="flex flex-col items-center justify-center w-full h-full text-[#949ba4] hover:text-[#00B578] active:text-[#00B578] transition-colors group" active-class="!text-[#00B578]">
         <div class="relative p-1 rounded-xl group-active:scale-95 transition-transform">
             <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
             </svg>
         </div>
         <span class="text-[10px] font-bold uppercase tracking-widest mt-0.5">Profil</span>
      </NuxtLink>
      
    </div>
  </nav>
</template>

<style scoped>
/* Safe area padding for iPhones with notch/home bar */
.pb-safe-area-inset-bottom {
  padding-bottom: env(safe-area-inset-bottom, 20px);
}
</style>
