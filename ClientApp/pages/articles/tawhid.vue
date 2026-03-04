<template>
  <div class="tawhid-page" ref="pageRef">
    <!-- Background Effects -->
    <div class="bg-effects">
      <div class="arabesque-overlay"></div>
    </div>

    <!-- Navigation Bar -->
    <nav class="top-nav">
      <NuxtLink to="/articles" class="back-link">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M19 12H5M12 19l-7-7 7-7"/></svg>
        Articles
      </NuxtLink>
      <button class="toc-toggle" @click="tocOpen = !tocOpen" :class="{ active: tocOpen }" aria-label="Navigation rapide">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M4 6h16M4 12h10M4 18h14"/></svg>
      </button>
    </nav>

    <!-- Table of Contents -->
    <Transition name="toc">
      <div v-if="tocOpen" class="toc-overlay" @click.self="tocOpen = false">
        <div class="toc-panel">
          <div class="toc-header">
            <h3>Sommaire</h3>
            <button class="toc-close" @click="tocOpen = false">✕</button>
          </div>
          <ul class="toc-list">
            <li v-for="item in tocItems" :key="item.id">
              <a
                :href="`#${item.id}`"
                class="toc-link"
                :class="{ active: activeSection === item.id, 'toc-part': item.isPart, 'toc-sub': item.isSub }"
                @click.prevent="scrollToSection(item.id)"
              >
                <span class="toc-dot"></span>
                <span class="toc-label">{{ item.label }}</span>
              </a>
            </li>
          </ul>
        </div>
      </div>
    </Transition>

    <!-- Hero Section -->
    <header class="hero" :class="{ visible: heroVisible }">
      <div class="hero-badge">بسم الله الرحمن الرحيم</div>
      <h1 class="hero-title">Le Tawhid</h1>
      <p class="hero-subtitle">التوحيد — L'Unicité d'Allah</p>
      <p class="hero-desc">Définition, Catégories et Mérites</p>
      <div class="scroll-indicator">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 5v14M5 12l7 7 7-7"/></svg>
      </div>
    </header>

    <!-- Introduction -->
    <section id="intro" class="intro-section" :class="{ visible: introVisible }" ref="introRef">
      <div class="section-container">
        <div class="intro-card">
          <div class="ornament">✦</div>
          <p>Le Tawhid est le <strong>fondement de l'Islam</strong>. C'est la première obligation, la condition d'acceptation des œuvres et la clé du Paradis.</p>
          <div class="ornament">✦</div>
        </div>
      </div>
    </section>

    <!-- Section 1: Qu'est-ce que le Tawhid ? -->
    <section id="definition" class="part-header" :class="{ visible: defVisible }" ref="defRef">
      <span class="part-number">SECTION 1</span>
      <h2>Qu'est-ce que le Tawhid ?</h2>
    </section>

    <!-- Définition Linguistique -->
    <section id="linguistique" class="content-section" :class="{ visible: lingVisible }" ref="lingRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">1</span>Définition Linguistique</h3>
        <div class="intro-card">
          <p>Le terme vient de la racine arabe signifiant <strong>« rendre une chose unique »</strong>.</p>
          <div class="example-box">
            <div class="example-label">Exemple</div>
            <p>Si vous dites « Personne ne sort de la maison sauf Muhammad », vous avez <em>unifié</em> (isolé) Muhammad dans l'action de sortir.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Définition Religieuse -->
    <section id="religieuse" class="content-section" :class="{ visible: relVisible }" ref="relRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">2</span>Définition Religieuse (Shar'i)</h3>
        <div class="intro-card">
          <p>C'est le fait d'<strong>unifier Allah</strong> (Lui vouer l'exclusivité) dans trois domaines :</p>
          <div class="three-domains">
            <div class="domain-item">
              <span class="domain-num">1</span>
              <span>La Seigneurie <em>(Rububiyyah)</em></span>
            </div>
            <div class="domain-item">
              <span class="domain-num">2</span>
              <span>L'Adoration / Divinité <em>(Uluhiyyah)</em></span>
            </div>
            <div class="domain-item">
              <span class="domain-num">3</span>
              <span>Les Noms et Attributs <em>(Al-Asma wa As-Sifat)</em></span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Section 2: Les 3 Catégories -->
    <section id="categories" class="part-header" :class="{ visible: catVisible }" ref="catRef">
      <span class="part-number">SECTION 2</span>
      <h2>Les Trois Catégories de Tawhid</h2>
      <p class="part-translation">Ces trois catégories sont indissociables. Pour être croyant, il faut valider les trois sans exception.</p>
    </section>

    <!-- Carousel des 3 Catégories -->
    <section id="cat-detail" class="carousel-section" :class="{ visible: catDetailVisible }" ref="catDetailRef">
      <div class="section-container">
        <div class="carousel-wrapper" @touchstart="handleTouchStart" @touchend="handleTouchEnd">
          <div class="carousel-track" :style="{ transform: `translateX(-${carousel1 * 100}%)` }">
            <!-- Cat 1: Rububiyyah -->
            <div class="carousel-slide">
              <div class="category-card">
                <div class="cat-number">1</div>
                <div class="cat-arabic">الربوبية</div>
                <h4>Tawhid Ar-Rububiyyah</h4>
                <p class="cat-subtitle">L'Unicité dans la Seigneurie</p>
                <div class="cat-divider"></div>
                <p class="cat-definition">C'est unifier Allah dans <strong>Ses actions à Lui</strong> : la Création, la Royauté et la Gestion de l'univers.</p>
                <p class="cat-example">C'est croire qu'Allah est le seul Créateur, le seul Pourvoyeur, Celui qui donne la vie et la mort, et Celui qui gère toutes les affaires du monde.</p>
              </div>
            </div>
            <!-- Cat 2: Uluhiyyah -->
            <div class="carousel-slide">
              <div class="category-card">
                <div class="cat-number">2</div>
                <div class="cat-arabic">الألوهية</div>
                <h4>Tawhid Al-Uluhiyyah</h4>
                <p class="cat-subtitle">L'Unicité dans l'Adoration</p>
                <div class="cat-divider"></div>
                <p class="cat-definition">C'est unifier Allah par <strong>les actions des serviteurs</strong>. C'est vouer tout acte d'adoration à Allah seul.</p>
                <p class="cat-example">La prière, le jeûne, le pèlerinage (Hajj), le vœu pieux, mais aussi les actes du cœur comme la peur, l'espoir et la confiance (Tawakkul) ne doivent être dirigés que vers Allah.</p>
              </div>
            </div>
            <!-- Cat 3: Asma wa Sifat -->
            <div class="carousel-slide">
              <div class="category-card">
                <div class="cat-number">3</div>
                <div class="cat-arabic">الأسماء والصفات</div>
                <h4>Tawhid Al-Asma wa As-Sifat</h4>
                <p class="cat-subtitle">L'Unicité des Noms et Attributs</p>
                <div class="cat-divider"></div>
                <p class="cat-definition">C'est décrire Allah comme <strong>Il S'est décrit Lui-même</strong> ou comme Son Messager ﷺ L'a décrit.</p>
                <div class="prohibitions">
                  <p>On affirme Ses Noms et Attributs de perfection sans :</p>
                  <div class="prohib-grid">
                    <div class="prohib-item"><span class="prohib-icon">✗</span> Altérer le sens <em>(Tahrif)</em></div>
                    <div class="prohib-item"><span class="prohib-icon">✗</span> Nier ou rejeter <em>(Ta'til)</em></div>
                    <div class="prohib-item"><span class="prohib-icon">✗</span> Demander « comment » <em>(Takyif)</em></div>
                    <div class="prohib-item"><span class="prohib-icon">✗</span> Ressembler aux créatures <em>(Tamthil)</em></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="carousel-controls">
            <button @click="prevSlide()" :disabled="carousel1 === 0" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M15 18l-6-6 6-6"/></svg>
            </button>
            <div class="carousel-dots">
              <span v-for="i in 3" :key="i" class="dot" :class="{ active: carousel1 === i - 1 }" @click="carousel1 = i - 1"></span>
            </div>
            <button @click="nextSlide()" :disabled="carousel1 === 2" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 18l6-6-6-6"/></svg>
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Section 3: Règles Fondamentales -->
    <section id="regles" class="part-header" :class="{ visible: reglesVisible }" ref="reglesRef">
      <span class="part-number">SECTION 3</span>
      <h2>Trois Règles Fondamentales</h2>
    </section>

    <section id="regles-detail" class="content-section" :class="{ visible: reglesDetailVisible }" ref="reglesDetailRef">
      <div class="section-container">
        <div class="rules-grid">
          <div class="rule-card" v-for="(rule, idx) in rules" :key="idx" :style="{ animationDelay: `${idx * 0.15}s` }">
            <div class="rule-num">{{ idx + 1 }}</div>
            <div class="rule-icon">{{ rule.icon }}</div>
            <h4>{{ rule.title }}</h4>
            <p>{{ rule.desc }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Section 4: Importance et Mérites -->
    <section id="merites" class="part-header" :class="{ visible: meritesVisible }" ref="meritesRef">
      <span class="part-number">SECTION 4</span>
      <h2>L'Importance et les Mérites du Tawhid</h2>
      <p class="part-translation">Pourquoi le Tawhid est-il si important ? Voici 5 raisons majeures.</p>
    </section>

    <section id="merites-detail" class="content-section" :class="{ visible: meritesDetailVisible }" ref="meritesDetailRef">
      <div class="section-container">
        <div class="merites-list">
          <div class="merite-card" v-for="(merite, idx) in merites" :key="idx" :style="{ animationDelay: `${idx * 0.12}s` }">
            <div class="merite-num">{{ idx + 1 }}</div>
            <div class="merite-content">
              <h4>{{ merite.title }}</h4>
              <p>{{ merite.desc }}</p>
              <blockquote v-if="merite.hadith" class="merite-hadith">
                {{ merite.hadith }}
              </blockquote>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Conclusion -->
    <section id="conclusion" class="conclusion-section" :class="{ visible: conclVisible }" ref="conclRef">
      <div class="section-container">
        <div class="conclusion-card">
          <div class="ornament gold">✦ ✦ ✦</div>
          <h3>Résumé</h3>
          <p>Le Tawhid ne consiste pas seulement à croire qu'Allah existe (ce que même les polythéistes admettaient).</p>
          <p class="conclusion-highlight">Le véritable Tawhid qui sauve l'individu consiste à <strong>unifier Allah dans l'Adoration</strong>, en Lui vouant tous nos actes, sans rien Lui associer, tout en affirmant Ses Noms et Attributs parfaits.</p>
          <div class="ornament gold">✦ ✦ ✦</div>
        </div>
      </div>
    </section>

    <!-- Bottom Decorative -->
    <div class="bottom-line"></div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ layout: false })
useSeoMeta({
  title: 'Le Tawhid — L\'Unicité d\'Allah | Ilmanar',
  description: 'Définition, Catégories et Mérites du Tawhid. Apprenez ce qui constitue le fondement de l\'Islam.'
})

const pageRef = ref<HTMLElement | null>(null)
const introRef = ref<HTMLElement | null>(null)
const defRef = ref<HTMLElement | null>(null)
const lingRef = ref<HTMLElement | null>(null)
const relRef = ref<HTMLElement | null>(null)
const catRef = ref<HTMLElement | null>(null)
const catDetailRef = ref<HTMLElement | null>(null)
const reglesRef = ref<HTMLElement | null>(null)
const reglesDetailRef = ref<HTMLElement | null>(null)
const meritesRef = ref<HTMLElement | null>(null)
const meritesDetailRef = ref<HTMLElement | null>(null)
const conclRef = ref<HTMLElement | null>(null)

const heroVisible = ref(false)
const introVisible = ref(false)
const defVisible = ref(false)
const lingVisible = ref(false)
const relVisible = ref(false)
const catVisible = ref(false)
const catDetailVisible = ref(false)
const reglesVisible = ref(false)
const reglesDetailVisible = ref(false)
const meritesVisible = ref(false)
const meritesDetailVisible = ref(false)
const conclVisible = ref(false)

const carousel1 = ref(0)
const tocOpen = ref(false)
const activeSection = ref('intro')

const tocItems = [
  { id: 'intro', label: 'Introduction', isPart: false, isSub: false },
  { id: 'definition', label: 'Section 1 — Définition', isPart: true, isSub: false },
  { id: 'linguistique', label: 'Définition Linguistique', isPart: false, isSub: true },
  { id: 'religieuse', label: 'Définition Religieuse', isPart: false, isSub: true },
  { id: 'categories', label: 'Section 2 — Les 3 Catégories', isPart: true, isSub: false },
  { id: 'cat-detail', label: 'Détail des Catégories', isPart: false, isSub: true },
  { id: 'regles', label: 'Section 3 — Règles Fondamentales', isPart: true, isSub: false },
  { id: 'merites', label: 'Section 4 — Mérites du Tawhid', isPart: true, isSub: false },
  { id: 'conclusion', label: 'Résumé', isPart: false, isSub: false },
]

function scrollToSection(id: string) {
  const el = document.getElementById(id)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' })
    tocOpen.value = false
  }
}

function prevSlide() { if (carousel1.value > 0) carousel1.value-- }
function nextSlide() { if (carousel1.value < 2) carousel1.value++ }

const rules = [
  { icon: '🔗', title: "L'Indissociabilité", desc: "Les trois types de Tawhid sont liés. Celui qui en accepte une partie mais en rejette une autre n'est pas considéré comme monothéiste (Muwahhid)." },
  { icon: '⚠️', title: 'Le Cas des Polythéistes', desc: "Les mécréants de l'époque du Prophète ﷺ acceptaient le Tawhid Ar-Rububiyyah (ils savaient qu'Allah était le Créateur), mais cela ne suffisait pas car ils adoraient des idoles." },
  { icon: '🕌', title: 'Le Cœur de la Mission', desc: "Le Tawhid Al-Uluhiyyah (l'adoration) est la base sur laquelle toutes les œuvres sont construites. C'est le sujet principal de la dispute entre les Prophètes et leurs peuples." },
]

const merites = [
  {
    title: `Le plus grand pilier de l'Islam`,
    desc: `C'est la base absolue. Personne ne peut entrer en Islam sans attester de l'unicité d'Allah.`,
    hadith: `« L'Islam est bâti sur cinq piliers : l'attestation qu'il n'y a de divinité qu'Allah... »`
  },
  {
    title: `La priorité absolue des priorités`,
    desc: `C'est la première chose qu'il faut apprendre et enseigner.`,
    hadith: `« Que la première chose à laquelle tu les appelles soit l'attestation qu'il n'y a de divinité qu'Allah. »`
  },
  {
    title: `La condition de validité des œuvres`,
    desc: `Les adorations ne sont acceptées qu'avec le Tawhid. Le Tawhid est à l'adoration ce que la purification (ablutions) est à la prière.`,
    hadith: null
  },
  {
    title: `La source de la sécurité et de la guidée`,
    desc: `Ceux qui réalisent le Tawhid parfaitement obtiennent la sécurité totale dans l'au-delà et la guidée dans ce bas-monde.`,
    hadith: `« Ceux qui ont cru et n'ont point troublé la pureté de leur foi par quelque iniquité... ceux-là ont la sécurité ; et ce sont eux les bien-guidés. » (Sourate Al-An'am)`
  },
  {
    title: `La cause de l'entrée au Paradis`,
    desc: `C'est le moyen ultime pour être sauvé du Feu et entrer au Paradis.`,
    hadith: `« Allah a interdit le Feu à celui qui dit 'La ilaha illa Allah' en recherchant par cela le Visage d'Allah. »`
  },
]

// Touch swipe support
let touchStartX = 0
function handleTouchStart(e: TouchEvent) { touchStartX = e.touches[0].clientX }
function handleTouchEnd(e: TouchEvent) {
  const diff = touchStartX - e.changedTouches[0].clientX
  if (Math.abs(diff) > 50) {
    if (diff > 0) nextSlide()
    else prevSlide()
  }
}

onMounted(() => {
  document.documentElement.style.overflow = 'hidden'
  document.body.style.overflow = 'hidden'
  document.documentElement.style.height = '100%'
  document.body.style.height = '100%'

  heroVisible.value = true

  const observerOptions = { threshold: 0.15, rootMargin: '0px 0px -50px 0px' }
  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (!entry.isIntersecting) return
      const el = entry.target
      if (el === introRef.value) introVisible.value = true
      if (el === defRef.value) defVisible.value = true
      if (el === lingRef.value) lingVisible.value = true
      if (el === relRef.value) relVisible.value = true
      if (el === catRef.value) catVisible.value = true
      if (el === catDetailRef.value) catDetailVisible.value = true
      if (el === reglesRef.value) reglesVisible.value = true
      if (el === reglesDetailRef.value) reglesDetailVisible.value = true
      if (el === meritesRef.value) meritesVisible.value = true
      if (el === meritesDetailRef.value) meritesDetailVisible.value = true
      if (el === conclRef.value) conclVisible.value = true
    })
  }, observerOptions)

  const sectionObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting && entry.target.id) {
        activeSection.value = entry.target.id
      }
    })
  }, { threshold: 0.3, rootMargin: '-80px 0px -40% 0px' })

  const refs = [introRef, defRef, lingRef, relRef, catRef, catDetailRef, reglesRef, reglesDetailRef, meritesRef, meritesDetailRef, conclRef]
  refs.forEach(r => {
    if (r.value) {
      observer.observe(r.value)
      sectionObserver.observe(r.value)
    }
  })

  onUnmounted(() => {
    document.documentElement.style.overflow = ''
    document.body.style.overflow = ''
    document.documentElement.style.height = ''
    document.body.style.height = ''
    observer.disconnect()
    sectionObserver.disconnect()
  })
})
</script>

<style scoped>
/* ── Base ── */
.tawhid-page {
  position: relative;
  height: 100vh;
  overflow-y: auto;
  overflow-x: hidden;
  background: #082540;
  color: #DDE1E7;
  font-family: 'Inter', 'Source Sans 3', sans-serif;
  padding-top: 58px;
}

/* ── Background ── */
.bg-effects { position: fixed; inset: 0; z-index: 0; pointer-events: none; }
.arabesque-overlay {
  position: absolute; inset: 0; opacity: 0.03;
  background-image: url('/patterns/arabesque.png');
}

/* ── Section Container ── */
.section-container { max-width: 800px; margin: 0 auto; padding: 0 1.5rem; }

/* ── Nav ── */
.top-nav {
  position: fixed; top: 0; left: 0; right: 0; z-index: 50; padding: 1rem 1.5rem;
  background: rgba(8, 37, 64, 0.85); backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(195, 151, 18, 0.15);
  display: flex; align-items: center; justify-content: space-between;
}
.back-link {
  display: inline-flex; align-items: center; gap: 0.5rem;
  color: #C39712; font-weight: 600; text-decoration: none; transition: opacity 0.2s;
}
.back-link:hover { opacity: 0.8; }
.toc-toggle {
  width: 40px; height: 40px; border-radius: 0.75rem; border: 1px solid rgba(195, 151, 18, 0.3);
  background: rgba(26, 60, 90, 0.3); color: #C39712; cursor: pointer;
  display: flex; align-items: center; justify-content: center; transition: all 0.25s;
}
.toc-toggle:hover, .toc-toggle.active { background: rgba(195, 151, 18, 0.15); border-color: #C39712; }

/* ── TOC ── */
.toc-overlay { position: fixed; inset: 0; z-index: 100; background: rgba(0, 0, 0, 0.5); backdrop-filter: blur(4px); }
.toc-panel {
  position: absolute; top: 0; right: 0; bottom: 0; width: 320px; max-width: 85vw;
  background: rgba(8, 37, 64, 0.97); backdrop-filter: blur(16px);
  border-left: 1px solid rgba(195, 151, 18, 0.2); padding: 1.5rem; overflow-y: auto;
  display: flex; flex-direction: column;
}
.toc-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 1.5rem; padding-bottom: 1rem; border-bottom: 1px solid rgba(195, 151, 18, 0.15); }
.toc-header h3 { font-family: 'Merriweather', serif; font-size: 1.25rem; color: #C39712; margin: 0; }
.toc-close { width: 32px; height: 32px; border-radius: 0.5rem; border: none; background: rgba(195, 151, 18, 0.1); color: #C39712; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: background 0.2s; }
.toc-close:hover { background: rgba(195, 151, 18, 0.2); }
.toc-list { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 0.25rem; }
.toc-link { display: flex; align-items: center; gap: 0.75rem; padding: 0.65rem 0.75rem; border-radius: 0.75rem; text-decoration: none; color: rgba(221, 225, 231, 0.6); transition: all 0.25s; font-size: 0.9rem; }
.toc-link:hover { background: rgba(195, 151, 18, 0.08); color: #DDE1E7; }
.toc-link.active { background: rgba(195, 151, 18, 0.12); color: #C39712; font-weight: 600; }
.toc-link.toc-part { font-weight: 700; font-size: 0.95rem; color: rgba(221, 225, 231, 0.85); margin-top: 0.75rem; }
.toc-link.toc-part.active { color: #C39712; }
.toc-link.toc-sub { padding-left: 1.75rem; font-size: 0.85rem; }
.toc-dot { width: 6px; height: 6px; border-radius: 50%; flex-shrink: 0; background: rgba(195, 151, 18, 0.3); transition: all 0.25s; }
.toc-link.active .toc-dot { background: #C39712; box-shadow: 0 0 8px rgba(195, 151, 18, 0.5); width: 8px; height: 8px; }
.toc-link.toc-part .toc-dot { width: 8px; height: 8px; }
.toc-enter-active, .toc-leave-active { transition: opacity 0.3s ease; }
.toc-enter-active .toc-panel, .toc-leave-active .toc-panel { transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1); }
.toc-enter-from, .toc-leave-to { opacity: 0; }
.toc-enter-from .toc-panel, .toc-leave-to .toc-panel { transform: translateX(100%); }

/* ── Hero ── */
.hero {
  position: relative; z-index: 1; text-align: center; padding: 6rem 1.5rem 4rem;
  opacity: 0; transform: translateY(30px); transition: all 1s cubic-bezier(0.16, 1, 0.3, 1);
}
.hero.visible { opacity: 1; transform: translateY(0); }
.hero-badge {
  display: inline-block; font-family: 'Amiri', serif; font-size: 1.5rem;
  color: #C39712; margin-bottom: 2rem; letter-spacing: 0.1em;
  padding: 0.5rem 2rem; border: 1px solid rgba(195, 151, 18, 0.3); border-radius: 999px; background: rgba(195, 151, 18, 0.05);
}
.hero-title {
  font-family: 'Merriweather', serif; font-size: clamp(2rem, 5vw, 3.5rem);
  font-weight: 800; color: #fff; margin-bottom: 1rem;
  background: linear-gradient(135deg, #fff 30%, #C39712 100%);
  -webkit-background-clip: text; -webkit-text-fill-color: transparent; background-clip: text;
}
.hero-subtitle { font-family: 'Amiri', serif; font-size: clamp(1rem, 2.5vw, 1.25rem); color: rgba(221, 225, 231, 0.8); margin-bottom: 0.75rem; }
.hero-desc { font-size: 1.1rem; color: #C39712; font-weight: 600; text-transform: uppercase; letter-spacing: 0.15em; }
.scroll-indicator { margin-top: 3rem; color: rgba(195, 151, 18, 0.5); animation: bounce 2s infinite; }
@keyframes bounce { 0%, 100% { transform: translateY(0); } 50% { transform: translateY(10px); } }

/* ── Intro ── */
.intro-section, .content-section { position: relative; z-index: 1; padding: 3rem 0; opacity: 0; transform: translateY(40px); transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1); }
.intro-section.visible, .content-section.visible { opacity: 1; transform: translateY(0); }
.intro-card {
  background: rgba(26, 60, 90, 0.3); border: 1px solid rgba(195, 151, 18, 0.2);
  border-radius: 1.5rem; padding: 2.5rem; text-align: center; backdrop-filter: blur(8px);
}
.ornament { color: #C39712; font-size: 1.5rem; margin: 1rem 0; }

/* ── Part Headers ── */
.part-header {
  position: relative; z-index: 1; text-align: center; padding: 5rem 1.5rem 3rem;
  opacity: 0; transform: translateY(40px); transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.part-header.visible { opacity: 1; transform: translateY(0); }
.part-number {
  display: inline-block; font-size: 0.85rem; font-weight: 700; color: #C39712;
  letter-spacing: 0.3em; text-transform: uppercase;
  padding: 0.4rem 1.5rem; border: 1px solid rgba(195, 151, 18, 0.4); border-radius: 999px; margin-bottom: 1.5rem;
}
.part-header h2 { font-family: 'Merriweather', serif; font-size: clamp(1.5rem, 4vw, 2.5rem); font-weight: 700; color: #fff; margin-bottom: 1rem; }
.part-translation { color: rgba(221, 225, 231, 0.6); font-size: 1.1rem; max-width: 600px; margin: 0 auto; }

/* ── Section Title ── */
.section-title {
  font-family: 'Merriweather', serif; font-size: 1.5rem; font-weight: 700;
  color: #fff; margin-bottom: 1.5rem; display: flex; align-items: center; gap: 0.75rem;
}
.title-num {
  display: inline-flex; align-items: center; justify-content: center;
  width: 36px; height: 36px; border-radius: 50%;
  background: linear-gradient(135deg, #C39712, #a07d0e);
  color: #082540; font-weight: 800; font-size: 1rem; font-family: 'Inter', sans-serif; flex-shrink: 0;
}

/* ── Example Box ── */
.example-box {
  margin-top: 1.5rem; padding: 1.25rem; border-radius: 1rem;
  background: rgba(195, 151, 18, 0.06); border: 1px solid rgba(195, 151, 18, 0.15);
  text-align: left;
}
.example-label {
  display: inline-block; font-size: 0.75rem; font-weight: 700; text-transform: uppercase;
  letter-spacing: 0.15em; color: #C39712; margin-bottom: 0.5rem;
  padding: 0.2rem 0.6rem; border-radius: 999px; background: rgba(195, 151, 18, 0.1);
}

/* ── Three Domains ── */
.three-domains { display: flex; flex-direction: column; gap: 0.75rem; margin-top: 1.5rem; text-align: left; }
.domain-item {
  display: flex; align-items: center; gap: 0.75rem; padding: 0.75rem 1rem;
  border-radius: 0.75rem; background: rgba(195, 151, 18, 0.06); border: 1px solid rgba(195, 151, 18, 0.12);
}
.domain-num {
  width: 28px; height: 28px; border-radius: 50%; flex-shrink: 0;
  background: linear-gradient(135deg, #C39712, #a07d0e); color: #082540;
  font-weight: 800; font-size: 0.85rem; display: flex; align-items: center; justify-content: center;
}
.domain-item em { color: #C39712; }

/* ── Carousel ── */
.carousel-section { position: relative; z-index: 1; padding: 1rem 0 3rem; opacity: 0; transform: translateY(40px); transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1); }
.carousel-section.visible { opacity: 1; transform: translateY(0); }
.carousel-wrapper { overflow: hidden; border-radius: 1.25rem; }
.carousel-track { display: flex; transition: transform 0.5s cubic-bezier(0.16, 1, 0.3, 1); }
.carousel-slide { min-width: 100%; padding: 0 0.25rem; box-sizing: border-box; }

/* ── Category Cards ── */
.category-card {
  border-radius: 1.25rem; padding: 2.5rem 2rem; min-height: 380px;
  display: flex; flex-direction: column; align-items: center;
  justify-content: center; text-align: center; gap: 0.5rem;
  background: rgba(26, 60, 90, 0.25); border: 1px solid rgba(195, 151, 18, 0.2); backdrop-filter: blur(8px);
}
.cat-number {
  width: 44px; height: 44px; border-radius: 50%;
  background: linear-gradient(135deg, #C39712, #a07d0e); color: #082540;
  font-weight: 800; font-size: 1.2rem; display: flex; align-items: center; justify-content: center;
}
.cat-arabic { font-family: 'Amiri', serif; font-size: 2rem; color: #C39712; direction: rtl; }
.category-card h4 { font-family: 'Merriweather', serif; font-size: 1.2rem; color: #fff; margin: 0; }
.cat-subtitle { color: rgba(221, 225, 231, 0.6); font-size: 0.95rem; }
.cat-divider { width: 60px; height: 2px; background: rgba(195, 151, 18, 0.3); margin: 0.5rem 0; }
.cat-definition { color: rgba(221, 225, 231, 0.9); line-height: 1.7; }
.cat-example { color: rgba(221, 225, 231, 0.6); font-size: 0.9rem; line-height: 1.6; font-style: italic; }

/* ── Prohibitions Grid ── */
.prohibitions { text-align: left; margin-top: 0.5rem; }
.prohibitions > p { color: rgba(221, 225, 231, 0.8); margin-bottom: 0.5rem; }
.prohib-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0.5rem; }
.prohib-item { display: flex; align-items: center; gap: 0.5rem; font-size: 0.85rem; color: rgba(221, 225, 231, 0.7); padding: 0.4rem 0.6rem; border-radius: 0.5rem; background: rgba(239, 68, 68, 0.06); border: 1px solid rgba(239, 68, 68, 0.15); }
.prohib-icon { color: #EF4444; font-weight: 700; }
.prohib-item em { color: #C39712; }

.carousel-controls { display: flex; align-items: center; justify-content: center; gap: 1rem; padding: 1.5rem 0; }
.carousel-btn {
  width: 40px; height: 40px; border-radius: 50%; border: 1px solid rgba(195, 151, 18, 0.3);
  background: rgba(26, 60, 90, 0.3); color: #C39712; cursor: pointer;
  display: flex; align-items: center; justify-content: center; transition: all 0.2s;
}
.carousel-btn:hover:not(:disabled) { background: rgba(195, 151, 18, 0.15); border-color: #C39712; }
.carousel-btn:disabled { opacity: 0.3; cursor: not-allowed; }
.carousel-dots { display: flex; gap: 0.5rem; }
.dot { width: 8px; height: 8px; border-radius: 50%; background: rgba(195, 151, 18, 0.3); cursor: pointer; transition: all 0.3s; }
.dot.active { background: #C39712; transform: scale(1.3); }

/* ── Rules Grid ── */
.rules-grid {
  display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 1rem;
}
.rule-card {
  border-radius: 1.25rem; padding: 1.75rem; text-align: center;
  background: rgba(26, 60, 90, 0.25); border: 1px solid rgba(195, 151, 18, 0.15);
  backdrop-filter: blur(8px); transition: transform 0.3s, box-shadow 0.3s;
  opacity: 0; animation: cardFadeIn 0.6s forwards;
}
.content-section.visible .rule-card { opacity: 1; }
.rule-card:hover { transform: translateY(-4px); box-shadow: 0 8px 30px rgba(195, 151, 18, 0.1); }
.rule-num {
  width: 32px; height: 32px; border-radius: 50%; margin: 0 auto 0.75rem;
  background: rgba(195, 151, 18, 0.15); color: #C39712;
  display: flex; align-items: center; justify-content: center; font-weight: 700;
}
.rule-icon { font-size: 2rem; margin-bottom: 0.75rem; }
.rule-card h4 { font-family: 'Merriweather', serif; color: #fff; font-size: 1.05rem; margin-bottom: 0.75rem; }
.rule-card p { color: rgba(221, 225, 231, 0.7); line-height: 1.6; font-size: 0.9rem; }

/* ── Merites List ── */
.merites-list { display: flex; flex-direction: column; gap: 1.25rem; }
.merite-card {
  display: flex; gap: 1.25rem; padding: 1.75rem; border-radius: 1.25rem;
  background: rgba(26, 60, 90, 0.2); border: 1px solid rgba(195, 151, 18, 0.12);
  backdrop-filter: blur(8px); transition: transform 0.3s;
  opacity: 0; animation: cardFadeIn 0.6s forwards;
}
.content-section.visible .merite-card { opacity: 1; }
.merite-card:hover { transform: translateX(4px); }
.merite-num {
  width: 40px; height: 40px; border-radius: 50%; flex-shrink: 0;
  background: linear-gradient(135deg, #C39712, #a07d0e); color: #082540;
  font-weight: 800; font-size: 1.1rem; display: flex; align-items: center; justify-content: center;
}
.merite-content h4 { font-family: 'Merriweather', serif; color: #fff; font-size: 1.1rem; margin-bottom: 0.5rem; }
.merite-content p { color: rgba(221, 225, 231, 0.7); line-height: 1.6; }
.merite-hadith {
  margin-top: 0.75rem; padding: 0.75rem 1rem; border-radius: 0.75rem;
  background: rgba(195, 151, 18, 0.06); border-left: 3px solid #C39712;
  color: #C39712; font-style: italic; font-size: 0.9rem; line-height: 1.6;
}

@keyframes cardFadeIn {
  from { opacity: 0; transform: translateY(15px); }
  to { opacity: 1; transform: translateY(0); }
}

/* ── Conclusion ── */
.conclusion-section {
  position: relative; z-index: 1; padding: 4rem 0 3rem;
  opacity: 0; transform: translateY(40px); transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.conclusion-section.visible { opacity: 1; transform: translateY(0); }
.conclusion-card {
  text-align: center; padding: 3rem 2rem; border-radius: 1.5rem;
  background: linear-gradient(135deg, rgba(195, 151, 18, 0.08), rgba(26, 60, 90, 0.2));
  border: 1px solid rgba(195, 151, 18, 0.25); backdrop-filter: blur(8px);
}
.conclusion-card h3 { font-family: 'Merriweather', serif; font-size: 1.75rem; color: #fff; margin-bottom: 1.5rem; }
.conclusion-card p { color: rgba(221, 225, 231, 0.85); line-height: 1.8; margin-bottom: 1rem; }
.conclusion-card strong { color: #C39712; }
.conclusion-highlight {
  font-size: 1.1rem; color: #C39712; font-weight: 600; padding: 1rem; border-radius: 0.75rem;
  background: rgba(195, 151, 18, 0.08); margin-top: 1rem;
}
.ornament.gold { color: #C39712; font-size: 1.25rem; letter-spacing: 0.5em; }

/* ── Bottom ── */
.bottom-line { height: 3px; background: linear-gradient(90deg, transparent, #C39712, transparent); position: relative; z-index: 1; }

/* ── Responsive ── */
@media (max-width: 640px) {
  .hero { padding: 4rem 1rem 3rem; }
  .section-container { padding: 0 1rem; }
  .category-card { padding: 2rem 1.25rem; min-height: 320px; }
  .rules-grid { grid-template-columns: 1fr; }
  .merite-card { flex-direction: column; gap: 0.75rem; }
  .prohib-grid { grid-template-columns: 1fr; }
}
</style>
