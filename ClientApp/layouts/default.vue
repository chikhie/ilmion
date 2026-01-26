<template>
  <div class="min-h-screen bg-brand-dark text-white flex flex-col font-sans-body">
    <AppHeader />
    <main class="flex-grow pb-24 lg:pb-0">
      <slot />
    </main>
    <AppFooter class="hidden lg:block" />
    <AppBottomNav />
  </div>
</template>
