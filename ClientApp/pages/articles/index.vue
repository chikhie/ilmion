<template>
  <div class="articles-page">
    <!-- Nav -->
    <nav class="top-nav">
      <NuxtLink to="/" class="back-link">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M19 12H5M12 19l-7-7 7-7"/></svg>
        Accueil
      </NuxtLink>
    </nav>

    <!-- Header -->
    <header class="page-header">
      <div class="header-badge">⚛️ Bibliothèque ⚛️</div>
      <h1>Articles & Théories</h1>
      <p class="header-desc">Explorez les fondements à travers des articles détaillés et illustrés.</p>
    </header>

    <!-- Articles Grid -->
    <div class="articles-container">
      <NuxtLink
        v-for="(article, idx) in articles"
        :key="article.slug"
        :to="article.to"
        class="article-card"
        :style="{ animationDelay: `${idx * 0.1}s` }"
      >
        <div class="card-category">{{ article.category }}</div>
        <div class="card-icon">{{ article.icon }}</div>
        <h2 class="card-title">{{ article.title }}</h2>
        <p class="card-arabic">{{ article.arabic }}</p>
        <p class="card-desc">{{ article.description }}</p>
        <div class="card-footer">
          <span class="card-tag" v-for="tag in article.tags" :key="tag">{{ tag }}</span>
        </div>
        <div class="card-arrow">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </div>
      </NuxtLink>
    </div>

    <!-- Coming Soon -->
    <div class="coming-soon">
      <p>D'autres articles sont en cours de rédaction…</p>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: false })
useSeoMeta({
  title: 'Articles & Leçons | Ilmanar',
  description: 'Explorez les fondements de l\'islam à travers des articles détaillés et illustrés.'
})

const articles = [
  {
    slug: 'shahadatayn',
    to: '/articles/shahadatayn',
    title: 'La Double Attestation de Foi',
    arabic: 'الشهادتان',
    description: 'Significations, piliers et conditions de « Lā ilāha illa Allāh » et « Muhammad Rasūl Allāh ».',
    category: 'Aqida',
    icon: '🕌',
    tags: ['Tawhid', 'Piliers', 'Conditions'],
  },
  {
    slug: 'tawhid',
    to: '/articles/tawhid',
    title: "Le Tawhid — L'Unicité d'Allah",
    arabic: 'التوحيد',
    description: 'Définition, les trois catégories indissociables (Rububiyyah, Uluhiyyah, Asma wa Sifat), règles fondamentales et mérites.',
    category: 'Aqida',
    icon: '☪️',
    tags: ['Unicité', 'Catégories', 'Mérites'],
  },
]

onMounted(() => {
  document.documentElement.style.overflow = 'hidden'
  document.body.style.overflow = 'hidden'
  document.documentElement.style.height = '100%'
  document.body.style.height = '100%'
})

onUnmounted(() => {
  document.documentElement.style.overflow = ''
  document.body.style.overflow = ''
  document.documentElement.style.height = ''
  document.body.style.height = ''
})
</script>

<style scoped>

.articles-page {
  height: 100vh;
  overflow-y: auto;
  overflow-x: hidden;
  background: #1e1f22;
  color: #f2f3f5;
  font-family: 'Inter', sans-serif;
  padding-top: 58px;
  position: relative;
}

.articles-page::before {
  content: '';
  position: absolute; inset: 0; z-index: -1; pointer-events: none;
  background-image: radial-gradient(#00B578 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px);
  background-size: 40px 40px, 40px 40px;
  background-position: 0 0, 20px 20px;
  opacity: 0.07;
}

/* ── Nav ── */
.top-nav {
  position: fixed; top: 0; left: 0; right: 0; z-index: 50;
  padding: 1rem 1.5rem;
  background: rgba(30, 31, 34, 0.85); backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(43, 45, 49, 1);
}
.back-link {
  display: inline-flex; align-items: center; gap: 0.5rem;
  color: #00B578; font-weight: 600; text-decoration: none;
  transition: opacity 0.2s;
}
.back-link:hover { opacity: 0.8; }

/* ── Header ── */
.page-header {
  text-align: center; padding: 4rem 1.5rem 2rem; position: relative; z-index: 1;
}
.header-badge {
  display: inline-block; font-family: 'Inter', sans-serif; font-size: 1.1rem;
  font-weight: 800; color: #00B578; margin-bottom: 1.5rem; letter-spacing: 0.2em;
}
.page-header h1 {
  font-family: 'Inter', sans-serif; font-size: clamp(2.5rem, 5vw, 3.5rem);
  font-weight: 900; margin-bottom: 1rem;
  background: linear-gradient(135deg, #fff 30%, #00B578 100%);
  -webkit-background-clip: text; -webkit-text-fill-color: transparent;
  background-clip: text;
}
.header-desc {
  color: #b5bac1; font-size: 1.1rem; max-width: 550px; margin: 0 auto; font-weight: 500;
}

/* ── Articles Grid ── */
.articles-container {
  max-width: 900px; margin: 0 auto; padding: 2rem 1.5rem;
  display: grid; grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.5rem; position: relative; z-index: 1;
}
.article-card {
  position: relative;
  border-radius: 1.25rem; padding: 2rem;
  background: rgba(43, 45, 49, 0.5);
  border: 1px solid rgba(0, 181, 120, 0.15);
  backdrop-filter: blur(8px);
  text-decoration: none; color: #f2f3f5;
  transition: all 0.35s cubic-bezier(0.16, 1, 0.3, 1);
  opacity: 0; animation: cardAppear 0.6s forwards;
  display: flex; flex-direction: column; gap: 0.75rem;
}
.article-card:hover {
  transform: translateY(-6px);
  border-color: rgba(0, 181, 120, 0.4);
  box-shadow: 0 12px 40px rgba(0, 181, 120, 0.1);
}

@keyframes cardAppear {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.card-category {
  display: inline-block; align-self: flex-start;
  font-size: 0.75rem; font-weight: 800; text-transform: uppercase;
  letter-spacing: 0.15em; color: #00B578;
  padding: 0.25rem 0.75rem; border-radius: 999px;
  background: rgba(0, 181, 120, 0.1); border: 1px solid rgba(0, 181, 120, 0.2);
}
.card-icon { font-size: 2.5rem; }
.card-title {
  font-family: 'Inter', sans-serif; font-size: 1.3rem;
  font-weight: 800; color: #fff; line-height: 1.4;
}
.card-arabic {
  font-family: 'Amiri', serif; font-size: 1.5rem;
  color: #00B578; direction: rtl;
}
.card-desc {
  color: #b5bac1; font-size: 0.9rem; line-height: 1.6; font-weight: 500;
}
.card-footer {
  display: flex; flex-wrap: wrap; gap: 0.5rem; margin-top: auto;
}
.card-tag {
  font-size: 0.75rem; padding: 0.2rem 0.6rem; font-weight: 600;
  border-radius: 0.5rem; background: rgba(242, 243, 245, 0.06);
  color: #949ba4; border: 1px solid rgba(242, 243, 245, 0.08);
}
.card-arrow {
  position: absolute; top: 2rem; right: 2rem;
  color: rgba(0, 181, 120, 0.3);
  transition: all 0.3s;
}
.article-card:hover .card-arrow {
  color: #00B578; transform: translateX(4px);
}

/* ── Coming Soon ── */
.coming-soon {
  text-align: center; padding: 3rem 1.5rem 4rem;
  color: rgba(221, 225, 231, 0.4); font-style: italic;
}

/* ── Responsive ── */
@media (max-width: 640px) {
  .articles-container { grid-template-columns: 1fr; padding: 1.5rem 1rem; }
  .page-header { padding: 3rem 1rem 1.5rem; }
}
</style>
