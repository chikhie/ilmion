<template>
  <div class="shahadatayn-page" ref="pageRef">
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

    <!-- Floating Table of Contents -->
    <Transition name="toc">
      <div v-if="tocOpen" class="toc-overlay" @click.self="tocOpen = false">
        <div class="toc-panel">
          <div class="toc-header">
            <h3>Sommaire</h3>
            <button class="toc-close" @click="tocOpen = false">
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M18 6L6 18M6 6l12 12"/></svg>
            </button>
          </div>
          <ul class="toc-list">
            <li v-for="item in tocItems" :key="item.id">
              <a
                :href="'#' + item.id"
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
      <h1 class="hero-title">La Double Attestation de Foi</h1>
      <p class="hero-subtitle">Ash-Shahadatayn — أشهد أن لا إله إلا الله وأشهد أن محمداً رسول الله</p>
      <p class="hero-desc">Significations, Piliers et Conditions</p>
      <div class="scroll-indicator">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 5v14M5 12l7 7 7-7"/></svg>
      </div>
    </header>

    <!-- Introduction -->
    <section id="intro" class="intro-section" :class="{ visible: introVisible }" ref="introRef">
      <div class="section-container">
        <div class="intro-card">
          <div class="ornament">✦</div>
          <p>La clé de voûte de l'islam réside dans la double attestation :</p>
          <blockquote class="arabic-quote">
            « أشهد أن لا إله إلا الله وأشهد أن محمداً رسول الله »
          </blockquote>
          <p class="translation">« Je témoigne qu'il n'y a de divinité digne d'adoration qu'Allah et je témoigne que Muhammad est le Messager d'Allah »</p>
          <div class="ornament">✦</div>
        </div>
      </div>
    </section>

    <!-- PART 1 Header -->
    <section id="part1" class="part-header" :class="{ visible: part1Visible }" ref="part1Ref">
      <span class="part-number">PARTIE 1</span>
      <h2>La Parole « Lā ilāha illa Allāh »</h2>
      <p class="part-arabic">لا إله إلا الله</p>
      <p class="part-translation">Il n'y a de dieu qu'Allah</p>
    </section>

    <!-- Signification Carousel -->
    <section id="signification" class="carousel-section" :class="{ visible: signifVisible }" ref="signifRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">1</span>La Signification Réelle</h3>
        <div class="carousel-wrapper">
          <div class="carousel-track" :style="{ transform: `translateX(-${carousel1 * 100}%)` }">
            <!-- Slide 1: Correct meaning -->
            <div class="carousel-slide">
              <div class="meaning-card correct">
                <div class="card-icon">✓</div>
                <h4>Le sens correct</h4>
                <p class="arabic-inline">لا معبود بحق إلا الله</p>
                <p>« Nul n'est digne d'être adoré en dehors d'Allah. »</p>
                <p class="card-note">C'est le sens précis qui ne doit pas être altéré.</p>
              </div>
            </div>
            <!-- Slide 2: Wrong interpretation 1 -->
            <div class="carousel-slide">
              <div class="meaning-card wrong">
                <div class="card-icon">✗</div>
                <h4>Interprétation erronée n°1</h4>
                <p>« Nul dieu n'existe sauf Allah »</p>
                <p class="card-note">C'est faux, car il existe de nombreuses fausses divinités adorées par les hommes, bien qu'elles soient vaines.</p>
              </div>
            </div>
            <!-- Slide 3: Wrong interpretation 2 -->
            <div class="carousel-slide">
              <div class="meaning-card wrong">
                <div class="card-icon">✗</div>
                <h4>Interprétation erronée n°2</h4>
                <p>« Nul créateur sauf Allah »</p>
                <p class="card-note">C'est vrai mais insuffisant. Les polythéistes admettaient qu'Allah était le créateur, mais adoraient d'autres idoles.</p>
              </div>
            </div>
            <!-- Slide 4: Wrong interpretation 3 -->
            <div class="carousel-slide">
              <div class="meaning-card wrong">
                <div class="card-icon">✗</div>
                <h4>Interprétation erronée n°3</h4>
                <p>« Nulle souveraineté sauf celle d'Allah »</p>
                <p class="card-note">C'est une partie de la vérité, mais ce n'est pas le sens complet visé par l'attestation.</p>
              </div>
            </div>
          </div>
          <div class="carousel-controls">
            <button @click="prevSlide(1)" :disabled="carousel1 === 0" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M15 18l-6-6 6-6"/></svg>
            </button>
            <div class="carousel-dots">
              <span v-for="i in 4" :key="i" class="dot" :class="{ active: carousel1 === i - 1 }" @click="carousel1 = i - 1"></span>
            </div>
            <button @click="nextSlide(1, 4)" :disabled="carousel1 === 3" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 18l6-6-6-6"/></svg>
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Piliers Section -->
    <section id="piliers" class="pillars-section" :class="{ visible: pillarVisible }" ref="pillarRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">2</span>Les Deux Piliers Fondamentaux</h3>
        <div class="pillars-grid">
          <div class="pillar-card isbat">
            <div class="pillar-label">الإثبات</div>
            <h4>L'Affirmation (Al-Isbat)</h4>
            <p class="pillar-arabic">« إلا الله »</p>
            <p>Confirmer et réserver l'adoration à Allah seul, sans aucun associé.</p>
          </div>
          <div class="pillar-separator">
            <div class="separator-line"></div>
            <span class="separator-icon">+</span>
            <div class="separator-line"></div>
          </div>
            <div class="pillar-card nafi">
            <div class="pillar-label">النفي</div>
            <h4>La Négation (An-Nafi)</h4>
            <p class="pillar-arabic">« لا إله »</p>
            <p>Rejeter et nier tout ce qui est adoré en dehors d'Allah : idoles, statues, passions…</p>
          </div>
        </div>
        <div class="pillars-summary">
          <p>On nie la divinité pour toute la création et on l'affirme exclusivement pour Allah.</p>
        </div>
      </div>
    </section>

    <!-- Conditions Carousel -->
    <section id="conditions" class="conditions-section" :class="{ visible: condVisible }" ref="condRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">3</span>Les 8 Conditions</h3>
        <p class="section-intro">Prononcer cette parole ne suffit pas pour être sauvé ; il faut en remplir les conditions.</p>
        <div class="carousel-wrapper conditions-carousel">
          <div class="carousel-track" :style="{ transform: `translateX(-${carousel2 * 100}%)` }">
            <div class="carousel-slide" v-for="(cond, idx) in conditions" :key="idx">
              <div class="condition-card">
                <div class="cond-number">{{ idx + 1 }}</div>
                <div class="cond-arabic">{{ cond.arabic }}</div>
                <h4>{{ cond.title }}</h4>
                <p>{{ cond.desc }}</p>
              </div>
            </div>
          </div>
          <div class="carousel-controls">
            <button @click="prevSlide(2)" :disabled="carousel2 === 0" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M15 18l-6-6 6-6"/></svg>
            </button>
            <div class="carousel-dots">
              <span v-for="i in 8" :key="i" class="dot" :class="{ active: carousel2 === i - 1 }" @click="carousel2 = i - 1"></span>
            </div>
            <button @click="nextSlide(2, 8)" :disabled="carousel2 === 7" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 18l6-6-6-6"/></svg>
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- PART 2 Header -->
    <section id="part2" class="part-header part2" :class="{ visible: part2Visible }" ref="part2Ref">
      <span class="part-number">PARTIE 2</span>
      <h2>La Parole « Muhammad Rasūl Allāh »</h2>
      <p class="part-arabic">محمد رسول الله</p>
      <p class="part-translation">Muhammad est le Messager d'Allah</p>
    </section>

    <!-- Signification Part 2 -->
    <section id="signification2" class="signif2-section" :class="{ visible: signif2Visible }" ref="signif2Ref">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">1</span>La Signification</h3>
        <div class="intro-card signif2-card">
          <p>Cela signifie croire fermement dans le cœur et proclamer par la langue que :</p>
          <p class="highlight-text">Muhammad ﷺ est le serviteur d'Allah et Son Messager envoyé à l'ensemble de la création (les humains et les djinns).</p>
        </div>
      </div>
    </section>

    <!-- Piliers Part 2 -->
    <section id="piliers2" class="pillars-section pillars2" :class="{ visible: pillar2Visible }" ref="pillar2Ref">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">2</span>Les Deux Piliers</h3>
        <div class="pillars-grid">
          <div class="pillar-card abd">
            <div class="pillar-label">عبده</div>
            <h4>Serviteur ('Abdouhu)</h4>
            <p>Il est une créature soumise à Allah. Il ne possède aucune caractéristique divine et ne doit pas être adoré.</p>
          </div>
          <div class="pillar-separator">
            <div class="separator-line"></div>
            <span class="separator-icon">&</span>
            <div class="separator-line"></div>
          </div>
          <div class="pillar-card rasul">
            <div class="pillar-label">رسوله</div>
            <h4>Messager (Rasūluhu)</h4>
            <p>Il a reçu la révélation. Il ne doit pas être démenti ; il doit être écouté et suivi.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Exigences Carousel -->
    <section id="exigences" class="exigences-section" :class="{ visible: exigVisible }" ref="exigRef">
      <div class="section-container">
        <h3 class="section-title"><span class="title-num">3</span>Les Exigences Concrètes</h3>
        <div class="exigences-grid">
          <div class="exig-card" v-for="(exig, idx) in exigences" :key="idx" :style="{ animationDelay: `${idx * 0.15}s` }">
            <div class="exig-num">{{ idx + 1 }}</div>
            <div class="exig-icon">{{ exig.icon }}</div>
            <p>{{ exig.text }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Conclusion -->
    <section id="conclusion" class="conclusion-section" :class="{ visible: conclVisible }" ref="conclRef">
      <div class="section-container">
        <div class="conclusion-card">
          <div class="ornament gold">✦ ✦ ✦</div>
          <h3>Conclusion</h3>
          <p>L'islam ne se résume pas à une simple prononciation verbale. Il repose sur la <strong>connaissance du sens du Tawhid</strong> (Unicité), la <strong>conviction du cœur</strong>, et la <strong>mise en pratique par les actes</strong> (obéissance et suivi du Prophète ﷺ).</p>
          <p class="conclusion-highlight">C'est uniquement en réunissant ces aspects que l'attestation de foi profite réellement à l'individu.</p>
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
useHead({ title: "L'Attestation de Foi" })

const pageRef = ref<HTMLElement | null>(null)
const introRef = ref<HTMLElement | null>(null)
const part1Ref = ref<HTMLElement | null>(null)
const signifRef = ref<HTMLElement | null>(null)
const pillarRef = ref<HTMLElement | null>(null)
const condRef = ref<HTMLElement | null>(null)
const part2Ref = ref<HTMLElement | null>(null)
const signif2Ref = ref<HTMLElement | null>(null)
const pillar2Ref = ref<HTMLElement | null>(null)
const exigRef = ref<HTMLElement | null>(null)
const conclRef = ref<HTMLElement | null>(null)

const heroVisible = ref(false)
const introVisible = ref(false)
const part1Visible = ref(false)
const signifVisible = ref(false)
const pillarVisible = ref(false)
const condVisible = ref(false)
const part2Visible = ref(false)
const signif2Visible = ref(false)
const pillar2Visible = ref(false)
const exigVisible = ref(false)
const conclVisible = ref(false)

const carousel1 = ref(0)
const carousel2 = ref(0)
const tocOpen = ref(false)
const activeSection = ref('intro')

const tocItems = [
  { id: 'intro', label: 'Introduction', isPart: false, isSub: false },
  { id: 'part1', label: 'Partie 1 — Lā ilāha illa Allāh', isPart: true, isSub: false },
  { id: 'signification', label: 'Signification', isPart: false, isSub: true },
  { id: 'piliers', label: 'Les Deux Piliers', isPart: false, isSub: true },
  { id: 'conditions', label: 'Les 8 Conditions', isPart: false, isSub: true },
  { id: 'part2', label: 'Partie 2 — Muhammad Rasūl Allāh', isPart: true, isSub: false },
  { id: 'signification2', label: 'Signification', isPart: false, isSub: true },
  { id: 'piliers2', label: 'Les Deux Piliers', isPart: false, isSub: true },
  { id: 'exigences', label: 'Les Exigences', isPart: false, isSub: true },
  { id: 'conclusion', label: 'Conclusion', isPart: false, isSub: false },
]

function scrollToSection(id: string) {
  const el = document.getElementById(id)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' })
    tocOpen.value = false
  }
}

function prevSlide(c: number) {
  if (c === 1 && carousel1.value > 0) carousel1.value--
  if (c === 2 && carousel2.value > 0) carousel2.value--
}
function nextSlide(c: number, max: number) {
  if (c === 1 && carousel1.value < max - 1) carousel1.value++
  if (c === 2 && carousel2.value < max - 1) carousel2.value++
}

const conditions = [
  { arabic: 'العِلم', title: 'La Science (Al-ʿIlm)', desc: 'Connaître le sens de cette parole (ce qu\'elle nie et ce qu\'elle affirme) pour dissiper l\'ignorance.' },
  { arabic: 'اليَقين', title: 'La Certitude (Al-Yaqīn)', desc: 'Une foi ferme dans le cœur qui ne laisse aucune place au doute.' },
  { arabic: 'القَبول', title: 'L\'Acceptation (Al-Qabūl)', desc: 'Accepter tout ce que cette parole implique, par le cœur et la langue, sans orgueil ni rejet.' },
  { arabic: 'الانقِياد', title: 'La Soumission (Al-Inqiyād)', desc: 'Se soumettre physiquement et spirituellement aux commandements d\'Allah.' },
  { arabic: 'الصِّدق', title: 'La Véracité (Aṣ-Ṣidq)', desc: 'Être sincère dans sa parole, afin que l\'intérieur (le cœur) soit en accord avec l\'extérieur (la langue).' },
  { arabic: 'الإخلاص', title: 'La Sincérité (Al-Ikhlāṣ)', desc: 'Purifier ses actes de toute forme d\'association (Shirk). Agir uniquement pour Allah.' },
  { arabic: 'المَحبّة', title: 'L\'Amour (Al-Maḥabba)', desc: 'Aimer cette parole, aimer Allah et Son Messager, et aimer les gens qui obéissent à Allah.' },
  { arabic: 'الكُفر', title: 'Le Désaveu (Al-Kufr bimā yuʿbadu min dūnillāh)', desc: 'Mécroire en tout ce qui est adoré en dehors d\'Allah et se désavouer des fausses divinités.' },
]

const exigences = [
  { icon: '📖', text: 'Croire en tout ce dont il nous a informés (les récits du passé, du futur, de l\'invisible).' },
  { icon: '⚡', text: 'Lui obéir dans tout ce qu\'il a ordonné.' },
  { icon: '🚫', text: 'S\'écarter de tout ce qu\'il a interdit et blâmé.' },
  { icon: '🕌', text: 'N\'adorer Allah que de la manière qu\'il a légiférée (suivre la Sunna, pas de Bidʿa).' },
]

// Touch swipe support
let touchStartX = 0
let activeCarousel = 0

function handleTouchStart(e: TouchEvent) {
  touchStartX = e.touches[0].clientX
  const target = e.target as HTMLElement
  if (target.closest('.conditions-carousel')) activeCarousel = 2
  else if (target.closest('.carousel-wrapper')) activeCarousel = 1
  else activeCarousel = 0
}

function handleTouchEnd(e: TouchEvent) {
  if (!activeCarousel) return
  const diff = touchStartX - e.changedTouches[0].clientX
  if (Math.abs(diff) > 50) {
    if (diff > 0) nextSlide(activeCarousel, activeCarousel === 1 ? 4 : 8)
    else prevSlide(activeCarousel)
  }
}

onMounted(() => {
  // Lock body scroll for this page only
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
      if (el === part1Ref.value) part1Visible.value = true
      if (el === signifRef.value) signifVisible.value = true
      if (el === pillarRef.value) pillarVisible.value = true
      if (el === condRef.value) condVisible.value = true
      if (el === part2Ref.value) part2Visible.value = true
      if (el === signif2Ref.value) signif2Visible.value = true
      if (el === pillar2Ref.value) pillar2Visible.value = true
      if (el === exigRef.value) exigVisible.value = true
      if (el === conclRef.value) conclVisible.value = true
    })
  }, observerOptions)

  // Active section tracker for the TOC
  const sectionObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting && entry.target.id) {
        activeSection.value = entry.target.id
      }
    })
  }, { threshold: 0.3, rootMargin: '-80px 0px -40% 0px' })

  const refs = [introRef, part1Ref, signifRef, pillarRef, condRef, part2Ref, signif2Ref, pillar2Ref, exigRef, conclRef]
  refs.forEach(r => {
    if (r.value) {
      observer.observe(r.value)
      sectionObserver.observe(r.value)
    }
  })

  document.addEventListener('touchstart', handleTouchStart, { passive: true })
  document.addEventListener('touchend', handleTouchEnd, { passive: true })

  onUnmounted(() => {
    // Restore body scroll
    document.documentElement.style.overflow = ''
    document.body.style.overflow = ''
    document.documentElement.style.height = ''
    document.body.style.height = ''
    observer.disconnect()
    sectionObserver.disconnect()
    document.removeEventListener('touchstart', handleTouchStart)
    document.removeEventListener('touchend', handleTouchEnd)
  })
})
</script>

<style scoped>
/* ── Base ── */
.shahadatayn-page {
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
  color: #C39712; font-weight: 600; text-decoration: none;
  transition: opacity 0.2s;
}
.back-link:hover { opacity: 0.8; }
.toc-toggle {
  width: 40px; height: 40px; border-radius: 0.75rem; border: 1px solid rgba(195, 151, 18, 0.3);
  background: rgba(26, 60, 90, 0.3); color: #C39712; cursor: pointer;
  display: flex; align-items: center; justify-content: center;
  transition: all 0.25s;
}
.toc-toggle:hover, .toc-toggle.active {
  background: rgba(195, 151, 18, 0.15); border-color: #C39712;
}

/* ── Table of Contents ── */
.toc-overlay {
  position: fixed; inset: 0; z-index: 100;
  background: rgba(0, 0, 0, 0.5); backdrop-filter: blur(4px);
}
.toc-panel {
  position: absolute; top: 0; right: 0; bottom: 0;
  width: 320px; max-width: 85vw;
  background: rgba(8, 37, 64, 0.97); backdrop-filter: blur(16px);
  border-left: 1px solid rgba(195, 151, 18, 0.2);
  padding: 1.5rem; overflow-y: auto;
  display: flex; flex-direction: column;
}
.toc-header {
  display: flex; align-items: center; justify-content: space-between;
  margin-bottom: 1.5rem; padding-bottom: 1rem;
  border-bottom: 1px solid rgba(195, 151, 18, 0.15);
}
.toc-header h3 {
  font-family: 'Merriweather', serif; font-size: 1.25rem;
  color: #C39712; margin: 0;
}
.toc-close {
  width: 32px; height: 32px; border-radius: 0.5rem; border: none;
  background: rgba(195, 151, 18, 0.1); color: #C39712; cursor: pointer;
  display: flex; align-items: center; justify-content: center;
  transition: background 0.2s;
}
.toc-close:hover { background: rgba(195, 151, 18, 0.2); }
.toc-list {
  list-style: none; padding: 0; margin: 0;
  display: flex; flex-direction: column; gap: 0.25rem;
}
.toc-link {
  display: flex; align-items: center; gap: 0.75rem;
  padding: 0.65rem 0.75rem; border-radius: 0.75rem;
  text-decoration: none; color: rgba(221, 225, 231, 0.6);
  transition: all 0.25s; font-size: 0.9rem;
}
.toc-link:hover { background: rgba(195, 151, 18, 0.08); color: #DDE1E7; }
.toc-link.active {
  background: rgba(195, 151, 18, 0.12); color: #C39712; font-weight: 600;
}
.toc-link.toc-part {
  font-weight: 700; font-size: 0.95rem; color: rgba(221, 225, 231, 0.85);
  margin-top: 0.75rem;
}
.toc-link.toc-part.active { color: #C39712; }
.toc-link.toc-sub { padding-left: 1.75rem; font-size: 0.85rem; }
.toc-dot {
  width: 6px; height: 6px; border-radius: 50%; flex-shrink: 0;
  background: rgba(195, 151, 18, 0.3); transition: all 0.25s;
}
.toc-link.active .toc-dot {
  background: #C39712; box-shadow: 0 0 8px rgba(195, 151, 18, 0.5);
  width: 8px; height: 8px;
}
.toc-link.toc-part .toc-dot { width: 8px; height: 8px; }

/* TOC Transitions */
.toc-enter-active, .toc-leave-active { transition: opacity 0.3s ease; }
.toc-enter-active .toc-panel, .toc-leave-active .toc-panel { transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1); }
.toc-enter-from, .toc-leave-to { opacity: 0; }
.toc-enter-from .toc-panel { transform: translateX(100%); }
.toc-leave-to .toc-panel { transform: translateX(100%); }

/* ── Hero ── */
.hero {
  position: relative; z-index: 1;
  text-align: center; padding: 6rem 1.5rem 4rem;
  opacity: 0; transform: translateY(30px);
  transition: all 1s cubic-bezier(0.16, 1, 0.3, 1);
}
.hero.visible { opacity: 1; transform: translateY(0); }
.hero-badge {
  display: inline-block; font-family: 'Amiri', serif; font-size: 1.5rem;
  color: #C39712; margin-bottom: 2rem; letter-spacing: 0.1em;
  padding: 0.5rem 2rem; border: 1px solid rgba(195, 151, 18, 0.3);
  border-radius: 999px; background: rgba(195, 151, 18, 0.05);
}
.hero-title {
  font-family: 'Merriweather', serif; font-size: clamp(2rem, 5vw, 3.5rem);
  font-weight: 800; color: #fff; margin-bottom: 1rem;
  background: linear-gradient(135deg, #fff 30%, #C39712 100%);
  -webkit-background-clip: text; -webkit-text-fill-color: transparent;
  background-clip: text;
}
.hero-subtitle {
  font-family: 'Amiri', serif; font-size: clamp(1rem, 2.5vw, 1.25rem);
  color: rgba(221, 225, 231, 0.8); margin-bottom: 0.75rem; direction: ltr;
}
.hero-desc {
  font-size: 1.1rem; color: #C39712; font-weight: 600;
  text-transform: uppercase; letter-spacing: 0.15em;
}
.scroll-indicator {
  margin-top: 3rem; color: rgba(195, 151, 18, 0.5);
  animation: bounce 2s infinite;
}
@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(10px); }
}

/* ── Intro ── */
.intro-section { position: relative; z-index: 1; padding: 4rem 0; opacity: 0; transform: translateY(40px); transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1); }
.intro-section.visible { opacity: 1; transform: translateY(0); }
.intro-card {
  background: rgba(26, 60, 90, 0.3); border: 1px solid rgba(195, 151, 18, 0.2);
  border-radius: 1.5rem; padding: 2.5rem; text-align: center;
  backdrop-filter: blur(8px);
}
.ornament { color: #C39712; font-size: 1.5rem; margin: 1rem 0; }
.arabic-quote {
  font-family: 'Amiri', serif; font-size: clamp(1.2rem, 3vw, 1.8rem);
  color: #C39712; margin: 1.5rem 0; padding: 1rem; line-height: 2;
  border-right: 3px solid #C39712; border-left: 3px solid #C39712;
  direction: rtl;
}
.translation { font-style: italic; color: rgba(221, 225, 231, 0.7); font-size: 0.95rem; }

/* ── Part Headers ── */
.part-header {
  position: relative; z-index: 1; text-align: center;
  padding: 5rem 1.5rem 3rem;
  opacity: 0; transform: translateY(40px);
  transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.part-header.visible { opacity: 1; transform: translateY(0); }
.part-number {
  display: inline-block; font-size: 0.85rem; font-weight: 700;
  color: #C39712; letter-spacing: 0.3em; text-transform: uppercase;
  padding: 0.4rem 1.5rem; border: 1px solid rgba(195, 151, 18, 0.4);
  border-radius: 999px; margin-bottom: 1.5rem;
}
.part-header h2 {
  font-family: 'Merriweather', serif; font-size: clamp(1.5rem, 4vw, 2.5rem);
  font-weight: 700; color: #fff; margin-bottom: 1rem;
}
.part-arabic {
  font-family: 'Amiri', serif; font-size: 2.5rem; color: #C39712;
  margin-bottom: 0.5rem; direction: rtl;
}
.part-translation { color: rgba(221, 225, 231, 0.6); font-size: 1.1rem; }

/* ── Section Title ── */
.section-title {
  font-family: 'Merriweather', serif; font-size: 1.5rem;
  font-weight: 700; color: #fff; margin-bottom: 1.5rem;
  display: flex; align-items: center; gap: 0.75rem;
}
.title-num {
  display: inline-flex; align-items: center; justify-content: center;
  width: 36px; height: 36px; border-radius: 50%;
  background: linear-gradient(135deg, #C39712, #a07d0e);
  color: #082540; font-weight: 800; font-size: 1rem;
  font-family: 'Inter', sans-serif; flex-shrink: 0;
}
.section-intro { color: rgba(221, 225, 231, 0.7); margin-bottom: 1.5rem; }

/* ── Carousel ── */
.carousel-section, .conditions-section, .exigences-section, .signif2-section {
  position: relative; z-index: 1; padding: 3rem 0;
  opacity: 0; transform: translateY(40px);
  transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.carousel-section.visible, .conditions-section.visible,
.exigences-section.visible, .signif2-section.visible { opacity: 1; transform: translateY(0); }

.carousel-wrapper { overflow: hidden; border-radius: 1.25rem; }
.carousel-track {
  display: flex; transition: transform 0.5s cubic-bezier(0.16, 1, 0.3, 1);
}
.carousel-slide { min-width: 100%; padding: 0 0.25rem; box-sizing: border-box; }

/* ── Meaning Cards ── */
.meaning-card {
  border-radius: 1.25rem; padding: 2.5rem 2rem; min-height: 280px;
  display: flex; flex-direction: column; align-items: center;
  justify-content: center; text-align: center; gap: 0.75rem;
  border: 1px solid rgba(195, 151, 18, 0.2);
  backdrop-filter: blur(8px);
}
.meaning-card.correct { background: rgba(16, 185, 129, 0.08); border-color: rgba(16, 185, 129, 0.3); }
.meaning-card.wrong { background: rgba(239, 68, 68, 0.08); border-color: rgba(239, 68, 68, 0.2); }
.card-icon {
  width: 48px; height: 48px; border-radius: 50%; display: flex;
  align-items: center; justify-content: center; font-size: 1.5rem; font-weight: 700;
}
.correct .card-icon { background: rgba(16, 185, 129, 0.2); color: #10B981; }
.wrong .card-icon { background: rgba(239, 68, 68, 0.2); color: #EF4444; }
.meaning-card h4 { font-family: 'Merriweather', serif; font-size: 1.2rem; color: #fff; }
.meaning-card p { color: rgba(221, 225, 231, 0.8); line-height: 1.6; }
.arabic-inline { font-family: 'Amiri', serif; font-size: 1.3rem; color: #C39712; direction: rtl; }
.card-note { font-size: 0.9rem; color: rgba(221, 225, 231, 0.6); font-style: italic; }

.carousel-controls {
  display: flex; align-items: center; justify-content: center;
  gap: 1rem; padding: 1.5rem 0;
}
.carousel-btn {
  width: 40px; height: 40px; border-radius: 50%; border: 1px solid rgba(195, 151, 18, 0.3);
  background: rgba(26, 60, 90, 0.3); color: #C39712; cursor: pointer;
  display: flex; align-items: center; justify-content: center;
  transition: all 0.2s;
}
.carousel-btn:hover:not(:disabled) { background: rgba(195, 151, 18, 0.15); border-color: #C39712; }
.carousel-btn:disabled { opacity: 0.3; cursor: not-allowed; }
.carousel-dots { display: flex; gap: 0.5rem; }
.dot {
  width: 8px; height: 8px; border-radius: 50%;
  background: rgba(195, 151, 18, 0.3); cursor: pointer;
  transition: all 0.3s;
}
.dot.active { background: #C39712; transform: scale(1.3); }

/* ── Pillar Cards ── */
.pillars-section {
  position: relative; z-index: 1; padding: 3rem 0;
  opacity: 0; transform: translateY(40px);
  transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.pillars-section.visible { opacity: 1; transform: translateY(0); }
.pillars-grid {
  display: flex; align-items: stretch; gap: 1rem;
  flex-wrap: wrap; justify-content: center;
}
.pillar-card {
  flex: 1; min-width: 250px; border-radius: 1.25rem;
  padding: 2rem; text-align: center;
  border: 1px solid rgba(195, 151, 18, 0.2);
  background: rgba(26, 60, 90, 0.2); backdrop-filter: blur(8px);
  transition: transform 0.3s, box-shadow 0.3s;
}
.pillar-card:hover { transform: translateY(-4px); box-shadow: 0 8px 30px rgba(195, 151, 18, 0.1); }
.pillar-label {
  font-family: 'Amiri', serif; font-size: 2rem; color: #C39712;
  margin-bottom: 0.75rem; direction: rtl;
}
.pillar-card h4 { font-family: 'Merriweather', serif; color: #fff; font-size: 1.1rem; margin-bottom: 0.75rem; }
.pillar-arabic { font-family: 'Amiri', serif; font-size: 1.3rem; color: #C39712; margin-bottom: 0.75rem; direction: rtl; }
.pillar-separator {
  display: flex; flex-direction: column; align-items: center;
  justify-content: center; gap: 0.5rem; padding: 1rem 0;
}
.separator-line { width: 2px; height: 30px; background: rgba(195, 151, 18, 0.3); }
.separator-icon {
  width: 32px; height: 32px; border-radius: 50%;
  background: rgba(195, 151, 18, 0.15); color: #C39712;
  display: flex; align-items: center; justify-content: center;
  font-weight: 700; font-size: 1.1rem;
}
.pillars-summary {
  text-align: center; margin-top: 2rem; padding: 1.25rem;
  border-radius: 1rem; background: rgba(195, 151, 18, 0.08);
  border: 1px solid rgba(195, 151, 18, 0.2);
  color: #C39712; font-weight: 600;
}

/* ── Condition Cards ── */
.condition-card {
  border-radius: 1.25rem; padding: 2.5rem 2rem; min-height: 300px;
  display: flex; flex-direction: column; align-items: center;
  justify-content: center; text-align: center; gap: 0.75rem;
  background: rgba(26, 60, 90, 0.25); border: 1px solid rgba(195, 151, 18, 0.2);
  backdrop-filter: blur(8px);
}
.cond-number {
  width: 40px; height: 40px; border-radius: 50%;
  background: linear-gradient(135deg, #C39712, #a07d0e);
  color: #082540; font-weight: 800; font-size: 1.1rem;
  display: flex; align-items: center; justify-content: center;
}
.cond-arabic {
  font-family: 'Amiri', serif; font-size: 2rem; color: #C39712; direction: rtl;
}
.condition-card h4 { font-family: 'Merriweather', serif; color: #fff; font-size: 1.1rem; }
.condition-card p { color: rgba(221, 225, 231, 0.8); line-height: 1.7; max-width: 400px; }

/* ── Signif2 Card ── */
.signif2-card { text-align: left; }
.highlight-text {
  font-size: 1.15rem; color: #C39712; font-weight: 600;
  padding: 1rem; border-left: 3px solid #C39712;
  background: rgba(195, 151, 18, 0.05); border-radius: 0 0.75rem 0.75rem 0;
  margin-top: 1rem;
}

/* ── Exigences ── */
.exigences-grid {
  display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1rem;
}
.exig-card {
  border-radius: 1.25rem; padding: 1.75rem;
  background: rgba(26, 60, 90, 0.25); border: 1px solid rgba(195, 151, 18, 0.15);
  display: flex; align-items: flex-start; gap: 1rem;
  backdrop-filter: blur(8px); transition: transform 0.3s, box-shadow 0.3s;
  opacity: 0; animation: cardFadeIn 0.6s forwards;
}
.exigences-section.visible .exig-card { opacity: 1; }
.exig-card:hover { transform: translateY(-2px); box-shadow: 0 6px 24px rgba(195, 151, 18, 0.08); }
.exig-num {
  width: 28px; height: 28px; border-radius: 50%; flex-shrink: 0;
  background: rgba(195, 151, 18, 0.15); color: #C39712;
  display: flex; align-items: center; justify-content: center;
  font-weight: 700; font-size: 0.85rem;
}
.exig-icon { font-size: 1.5rem; flex-shrink: 0; }
.exig-card p { color: rgba(221, 225, 231, 0.8); line-height: 1.6; }

@keyframes cardFadeIn {
  from { opacity: 0; transform: translateY(15px); }
  to { opacity: 1; transform: translateY(0); }
}

/* ── Conclusion ── */
.conclusion-section {
  position: relative; z-index: 1; padding: 4rem 0 3rem;
  opacity: 0; transform: translateY(40px);
  transition: all 0.8s cubic-bezier(0.16, 1, 0.3, 1);
}
.conclusion-section.visible { opacity: 1; transform: translateY(0); }
.conclusion-card {
  text-align: center; padding: 3rem 2rem; border-radius: 1.5rem;
  background: linear-gradient(135deg, rgba(195, 151, 18, 0.08), rgba(26, 60, 90, 0.2));
  border: 1px solid rgba(195, 151, 18, 0.25);
  backdrop-filter: blur(8px);
}
.conclusion-card h3 {
  font-family: 'Merriweather', serif; font-size: 1.75rem;
  color: #fff; margin-bottom: 1.5rem;
}
.conclusion-card p { color: rgba(221, 225, 231, 0.85); line-height: 1.8; margin-bottom: 1rem; }
.conclusion-card strong { color: #C39712; }
.conclusion-highlight {
  font-size: 1.1rem; color: #C39712; font-weight: 600;
  padding: 1rem; border-radius: 0.75rem;
  background: rgba(195, 151, 18, 0.08); margin-top: 1rem;
}
.ornament.gold { color: #C39712; font-size: 1.25rem; letter-spacing: 0.5em; }

/* ── Bottom ── */
.bottom-line {
  height: 3px; background: linear-gradient(90deg, transparent, #C39712, transparent);
  position: relative; z-index: 1;
}

/* ── Responsive ── */
@media (max-width: 640px) {
  .hero { padding: 4rem 1rem 3rem; }
  .pillars-grid { flex-direction: column; }
  .pillar-separator { flex-direction: row; padding: 0 1rem; }
  .separator-line { width: 30px; height: 2px; }
  .meaning-card, .condition-card { padding: 2rem 1.25rem; min-height: 240px; }
  .section-container { padding: 0 1rem; }
}
</style>
