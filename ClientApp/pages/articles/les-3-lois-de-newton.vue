<template>
  <div class="shahadatayn-page interactive-physics-page" ref="pageRef">
    <!-- Background Effects -->
    <div class="bg-effects bg-[#1e1f22]">
        <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#00B578 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
        <div class="absolute top-[10%] left-[-10%] w-[500px] h-[500px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow"></div>
        <div class="absolute bottom-[20%] right-[-10%] w-[600px] h-[600px] bg-[#00B578]/10 rounded-full blur-[100px] animate-pulse-slow delay-1000"></div>
    </div>

    <!-- Navigation Bar -->
    <nav class="top-nav">
      <NuxtLink to="/" class="back-link">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M19 12H5M12 19l-7-7 7-7"/></svg>
        Accueil
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
    <header class="hero" :class="{ visible: true }">
      <div class="hero-badge" style="color: #00B578; border-color: rgba(0, 181, 120, 0.3); background: rgba(0, 181, 120, 0.05);">PHYSIQUE & DYNAMIQUE</div>
      <h1 class="hero-title pt-4">Les 3 Lois de Newton</h1>
      <p class="hero-subtitle">Le guide interactif sur la mécanique classique</p>
      <p class="hero-desc" style="color: #00B578;">Inertie, Forces et Réactions</p>
      <div class="scroll-indicator">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 5v14M5 12l7 7 7-7"/></svg>
      </div>
    </header>

    <!-- Introduction -->
    <section id="intro" class="intro-section visible" ref="introRef">
      <div class="section-container">
        <div class="intro-card">
          <div class="ornament">🍎</div>
          <p>
            Lorsque vous regardez une pomme tomber, une voiture freiner, ou une fusée décoller vers les étoiles, vous observez les mêmes règles fondamentales en action : <strong>les lois du mouvement de Newton</strong>.
          </p>
          <p class="mt-4 text-[#b5bac1] leading-relaxed">
            Publiées en 1687 par Sir Isaac Newton dans son ouvrage <em>Principia Mathematica</em>, ces trois lois sont le pilier absolu de la mécanique classique. Bref, c'est la <strong>notice de l'Univers !</strong>
          </p>
          <div class="border-2 border-dashed border-[#00B578]/50 rounded-2xl p-6 mt-6 bg-[#00B578]/5 flex flex-col items-center justify-center text-center">
            <span class="text-[#00B578] font-bold text-sm tracking-widest uppercase mb-2">Illustration à insérer ici</span>
            <p class="text-sm text-[#b5bac1]">Visuel attendu : Un objet au repos (ex: une pomme posée) vs un objet en mouvement (pomme qui tombe). Design épuré, nuances de vert émeraude.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Chapitre 1: Inertie -->
    <section id="chap1" class="part-header visible" ref="chap1Ref">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">1ÈRE LOI</span>
      <h2>Le principe d'Inertie</h2>
      <p class="part-translation">Un corps fainéant reste fainéant</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <p class="article-p text-center mb-8 font-semibold italic border-l-4 border-[#00B578] pl-4 text-white">« Tout corps persévère dans son état de repos ou de mouvement rectiligne uniforme, si les forces qui s'exercent sur lui se compensent. »</p>
        <p class="article-p"><strong>En clair :</strong> Un objet au repos reste immobile, et un objet en mouvement continue d'avancer en ligne droite pour toujours, à moins qu'une force extérieure ne vienne le perturber (comme les frottements de l'air ou la gravité).</p>
        
        <div class="border-2 border-dashed border-[#00B578]/50 rounded-2xl p-6 bg-[#00B578]/5 flex flex-col items-center justify-center text-center mb-8">
            <span class="text-[#00B578] font-bold text-sm tracking-widest uppercase mb-2">Illustration à insérer ici</span>
            <p class="text-sm text-[#b5bac1]">Visuel attendu : Un astronaute qui lâche une clé anglaise dans le vide spatial (elle continue tout droit) vs un ballon sur gazon (freiné). Dominante `#00B578`.</p>
        </div>

        <!-- INTERACTION 1 -->
        <div class="interactive-lab-card">
          <h3 class="lab-title text-[#00B578]">À vous de jouer : Poussez le bloc !</h3>
          <div class="lab-stage flex items-center h-32 relative border-b border-[#2b2d31] mb-6">
            <div 
              class="absolute bottom-0 w-16 h-16 bg-gradient-to-br from-[#00B578] to-[#008F5D] rounded-lg shadow-[0_0_15px_rgba(0,181,120,0.4)] flex items-center justify-center font-bold text-white transition-all duration-300 ease-out z-10"
              :style="{ transform: `translateX(${(pushForce1 > 50 ? (pushForce1 - 50) * 8 : 0)}px)` }"
            >10kg</div>
            <div class="absolute bottom-[-24px] left-0 right-0 text-center text-xs text-[#b5bac1]" v-if="pushForce1 > 50">
              Force ({{ pushForce1 }}N) > Frottements (50N). <span class="text-[#00B578]">Le bloc bouge !</span>
            </div>
            <div class="absolute bottom-[-24px] left-0 right-0 text-center text-xs text-[#b5bac1]" v-else>
              Le bloc reste au repos (force trop faible).
            </div>
          </div>
          <div class="flex flex-col items-center">
            <label class="text-[#dbdee1] mb-3 text-sm font-medium">Force appliquée : <span class="text-[#00B578] font-bold">{{ pushForce1 }} N</span></label>
            <input type="range" min="0" max="100" v-model="pushForce1" class="w-full max-w-sm accent-[#00B578] h-2 bg-[#1e1f22] rounded-lg appearance-none cursor-pointer">
          </div>
        </div>
      </div>
    </section>

    <!-- Chapitre 2: F=ma -->
    <section id="chap2" class="part-header visible" ref="chap2Ref">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">2ÈME LOI</span>
      <h2>Dynamique : F = m × a</h2>
      <p class="part-translation">Le Principe Fondamental</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <p class="article-p text-center mb-8 font-semibold italic border-l-4 border-[#00B578] pl-4 text-white">« L'accélération (a) d'un corps est proportionnelle à la force (F) résultante et inversement proportionnelle à sa masse (m). »</p>
        <p class="article-p"><strong>En clair :</strong> Plus un objet est massif, plus il est dur de le faire accélérer. Si vous poussez un vélo et une voiture avec la même force, le vélo accélérera beaucoup plus vite car sa masse est plus faible.</p>
        
        <div class="border-2 border-dashed border-[#00B578]/50 rounded-2xl p-6 bg-[#00B578]/5 flex flex-col items-center justify-center text-center mb-8">
            <span class="text-[#00B578] font-bold text-sm tracking-widest uppercase mb-2">Illustration à insérer ici</span>
            <p class="text-sm text-[#b5bac1]">Visuel attendu : Deux personnages poussant un chariot (l'un vide, l'autre rempli). Grosses flèches vectorielles vertes pour F et a.</p>
        </div>

        <!-- INTERACTION 2 -->
        <div class="interactive-lab-card">
          <h3 class="lab-title text-[#00B578] text-xl font-bold">Laboratoire F = ma</h3>
          
          <div class="lab-stage flex items-end h-32 relative border-b-2 border-slate-700/50 mb-8 pb-2">
            <div 
              class="absolute bottom-2 flex flex-col items-center justify-end transition-all duration-500 ease-out z-10"
              :style="{ transform: `translateX(${computedAcceleration * 15}px)` }"
            >
              <!-- Effets de vitesse -->
              <div class="absolute left-[-30px] flex flex-col gap-1 opacity-50" v-if="computedAcceleration > 5">
                 <div class="w-8 h-[2px] bg-[#00B578] rounded-full"></div>
                 <div class="w-5 h-[2px] bg-[#00B578] rounded-full"></div>
              </div>
              <!-- Boîte massique -->
              <div 
                class="bg-gradient-to-tr from-[#00B578] to-[#008F5D] rounded-md shadow-[0_0_15px_rgba(0,181,120,0.3)] flex items-center justify-center font-bold text-white border border-[#00B578]/50 transition-all duration-300"
                :style="{ width: `${40 + (mass2 * 8)}px`, height: `${40 + (mass2 * 8)}px` }"
              >
                {{ mass2 }}kg
              </div>
            </div>
            <div class="absolute left-0 bottom-0 top-0 w-[2px] bg-[#00B578]/50 border-r border-[#1e1f22] border-dashed"></div>
          </div>

          <div class="flex justify-center mb-8">
            <div class="bg-black/40 px-6 py-4 rounded-xl text-center border border-[#1e1f22] shadow-inner">
              <div class="text-sm text-[#b5bac1] uppercase tracking-wider mb-1">Accélération (a)</div>
              <div class="text-4xl font-black text-[#00B578] font-mono">
                {{ computedAcceleration }} <span class="text-lg">m/s²</span>
              </div>
              <p class="text-xs text-[#00B578] mt-2 font-mono">a = {{ force2 }} / {{ mass2 }}</p>
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-8 mb-6">
            <div class="flex flex-col">
              <label class="text-[#dbdee1] mb-3 text-sm font-medium flex justify-between">
                Masse (m) <span class="bg-[#1e1f22] border border-[#2b2d31] px-2 py-0.5 rounded text-[#00B578]">{{ mass2 }} kg</span>
              </label>
              <input type="range" min="1" max="20" v-model="mass2" class="w-full accent-[#00B578] h-2 bg-[#1e1f22] rounded-lg appearance-none cursor-pointer">
            </div>
            <div class="flex flex-col">
              <label class="text-[#dbdee1] mb-3 text-sm font-medium flex justify-between">
                Force (F) <span class="bg-[#1e1f22] border border-[#2b2d31] px-2 py-0.5 rounded text-[#00B578]">{{ force2 }} N</span>
              </label>
              <input type="range" min="0" max="100" v-model="force2" class="w-full accent-[#00B578] h-2 bg-[#1e1f22] rounded-lg appearance-none cursor-pointer">
            </div>
          </div>
          
          <div class="flex justify-center">
            <button @click="resetLab2" class="qcm-btn" style="background: rgba(0,181,120,0.1); color: #00B578; border: 1px solid rgba(0,181,120,0.3);">Réinitialiser</button>
          </div>
        </div>
      </div>
    </section>

    <!-- Chapitre 3: Action/Réaction -->
    <section id="chap3" class="part-header visible" ref="chap3Ref">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">3ÈME LOI</span>
      <h2>Action et Réaction</h2>
      <p class="part-translation">Symétrie universelle des forces</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <p class="article-p text-center mb-8 font-semibold italic border-l-4 border-[#00B578] pl-4 text-white">« Tout corps A exerçant une force sur un corps B subit une force d'intensité égale et de sens opposé de la part du corps B. »</p>
        <p class="article-p"><strong>En clair :</strong> Les forces marchent toujours par paires ! Poussez un mur, le mur vous pousse en retour exactement avec la même force. C'est le principe qui permet aux avions de voler et aux nageurs d'avancer.</p>
        
        <div class="border-2 border-dashed border-[#00B578]/50 rounded-2xl p-6 bg-[#00B578]/5 flex flex-col items-center justify-center text-center mb-8">
            <span class="text-[#00B578] font-bold text-sm tracking-widest uppercase mb-2">Illustration à insérer ici</span>
            <p class="text-sm text-[#b5bac1]">Visuel attendu : Fusée qui décolle avec des flèches Action / Réaction. Variations de teintes de vert.</p>
        </div>

        <!-- INTERACTION 3 -->
        <div class="interactive-lab-card">
          <h3 class="lab-title text-[#00B578] font-bold mb-2">Skateboard et symétrie</h3>
          <p class="text-center text-[#b5bac1] text-sm mb-6">Poussez le mur ! Action = Réaction.</p>
          
          <div class="lab-stage flex items-center h-32 relative border-b-2 border-slate-700/50 mb-8 overflow-hidden">
            <!-- Mur -->
            <div class="absolute left-0 bottom-0 h-24 w-4 bg-slate-600 rounded-t-sm shadow-md z-30 flex items-center justify-center">
              <div class="absolute top-[30px] w-0 flex items-center overflow-visible" v-if="pushForce3 > 0" :style="{ width: `${pushForce3 * 1.5}px` }">
                 <div class="h-1 bg-[#00B578] w-full"></div>
                 <div class="w-0 h-0 border-[6px] border-y-transparent border-l-transparent border-r-[#00B578] absolute left-0 translate-x-[-100%]"></div>
                 <span class="absolute -top-6 text-[#00B578] text-xs font-bold w-[100px] text-right right-0 opacity-80">Action {{ pushForce3 }}N</span>
              </div>
            </div>

            <!-- Skater -->
            <div 
              class="absolute bottom-[-1px] flex flex-col items-center transition-all duration-300 ease-out z-10"
              :style="{ transform: `translateX(${16 + pushForce3 * 3}px)` }"
            >
              <div class="absolute top-[30px] left-[50px] w-0 flex items-center overflow-visible z-20" v-if="pushForce3 > 0" :style="{ width: `${pushForce3 * 1.5}px` }">
                 <div class="h-1 bg-[#00B578] w-full pointer-events-none opacity-50"></div>
                 <div class="w-0 h-0 border-[6px] border-y-transparent border-r-transparent border-l-[#00B578] absolute right-0 translate-x-[100%] pointer-events-none opacity-50"></div>
                 <span class="absolute -top-6 text-[#00B578] text-xs font-bold w-[100px] left-0 pointer-events-none opacity-80">Réaction {{ pushForce3 }}N</span>
              </div>
              <div class="w-12 h-14 bg-[#00B578]/10 border border-[#00B578]/30 rounded-t-xl mb-1 flex items-center justify-center text-xl shadow-[0_0_10px_rgba(0,181,120,0.2)]">🧍</div>
              <div class="w-16 h-2 bg-gradient-to-r from-[#008F5D] to-[#00B578] rounded-full flex justify-between px-2">
                <div class="w-2 h-2 bg-slate-900 rounded-full -bottom-[3px] relative"></div>
                <div class="w-2 h-2 bg-slate-900 rounded-full -bottom-[3px] relative"></div>
              </div>
            </div>
          </div>

          <div class="flex flex-col items-center">
            <label class="text-[#dbdee1] mb-3 text-sm font-medium">Force de poussée : <span class="text-[#00B578] font-bold">{{ pushForce3 }} N</span></label>
            <input type="range" min="0" max="60" v-model="pushForce3" class="w-full max-w-sm accent-[#00B578] h-2 bg-[#1e1f22] rounded-lg appearance-none cursor-pointer">
          </div>
        </div>
      </div>
    </section>

    <!-- Résumé / Cheat Sheet -->
    <section id="chap4" class="part-header visible" ref="chap4Ref">
      <span class="part-number text-white border-white/40 bg-white/5">CHEMIN D'ACCÈS RAPIDE</span>
      <h2>Cheat Sheet : Le Récap'</h2>
    </section>

    <section class="text-content-section visible">
      <div class="section-container pb-10">
        <div class="exigences-grid">
          <div class="exig-card" style="border-top: 4px solid #00B578;">
            <div class="exig-icon text-[#00B578]">1️⃣</div>
            <div>
              <h4 class="text-white font-bold mb-2">Inertie</h4>
              <p>Au repos, j'y reste. En mouvement, je continue tout droit. (Tant que la force résultante est nulle).</p>
            </div>
          </div>
          <div class="exig-card" style="border-top: 4px solid #00B578;">
            <div class="exig-icon text-[#00B578]">2️⃣</div>
            <div>
              <h4 class="text-white font-bold mb-2">F = m × a</h4>
              <p>Plus un objet est lourd (m), plus il faut de force (F) pour l'accélérer (a). Formule clé de la dynamique.</p>
            </div>
          </div>
          <div class="exig-card" style="border-top: 4px solid #00B578;">
            <div class="exig-icon text-[#00B578]">3️⃣</div>
            <div>
              <h4 class="text-white font-bold mb-2">Action / Réaction</h4>
              <p>Les forces vont par paire. A pousse B = B pousse A dans le sens opposé avec exactement la même force.</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Bottom Decorative -->
    <div class="bottom-line" style="background: linear-gradient(90deg, #1e1f22, #00B578, #1e1f22);"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'

definePageMeta({ layout: false })
useSeoMeta({
  title: 'Les 3 Lois de Newton : Mécanique Gamifiée',
  description: 'Inertie, Dynamique, Action et Réaction expliquées avec des simulations interactives pour comprendre la mécanique classique.'
})

// === UI State ===
const pageRef = ref<HTMLElement | null>(null)
const tocOpen = ref(false)
const activeSection = ref('intro')

const tocItems = [
  { id: 'intro', label: 'Introduction', isPart: false, isSub: false },
  { id: 'chap1', label: '1ère Loi : Inertie', isPart: true, isSub: false },
  { id: 'chap2', label: '2ème Loi : F = m × a', isPart: true, isSub: false },
  { id: 'chap3', label: '3ème Loi : Action/Réaction', isPart: true, isSub: false },
  { id: 'chap4', label: 'Cheat Sheet (Récapitulative)', isPart: true, isSub: false },
]

function scrollToSection(id: string) {
  const el = document.getElementById(id)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' })
    tocOpen.value = false
  }
}

// === INTERACTION STATE ===
const pushForce1 = ref(0)
const mass2 = ref(5)
const force2 = ref(10)
const computedAcceleration = computed(() => Number((force2.value / mass2.value).toFixed(2)))
function resetLab2() { mass2.value = 5; force2.value = 10; }
const pushForce3 = ref(0)

// === LYFECYCLE (TOC observer & Layout config) ===
onMounted(() => {
  document.documentElement.style.overflow = 'hidden'
  document.body.style.overflow = 'hidden'
  document.documentElement.style.height = '100%'
  document.body.style.height = '100%'

  const sectionObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting && entry.target.id) {
        activeSection.value = entry.target.id
      }
    })
  }, { threshold: 0.3, rootMargin: '-80px 0px -40% 0px' })

  const sections = document.querySelectorAll('section[id]')
  sections.forEach(sec => sectionObserver.observe(sec))

  onUnmounted(() => {
    document.documentElement.style.overflow = ''
    document.body.style.overflow = ''
    document.documentElement.style.height = ''
    document.body.style.height = ''
    sectionObserver.disconnect()
  })
})
</script>

<style scoped>
/* BASE STYLES EXACTLY FROM SHAHADATAYN / ATOM PAGE */
.shahadatayn-page {
  position: relative; height: 100vh; overflow-y: auto; overflow-x: hidden;
  background: #1e1f22; color: #f2f3f5; font-family: 'Inter', sans-serif; padding-top: 58px;
}
.bg-effects { position: fixed; inset: 0; z-index: 0; pointer-events: none; }
.section-container { max-width: 800px; margin: 0 auto; padding: 0 1.5rem; }

/* NAV */
.top-nav {
  position: fixed; top: 0; left: 0; right: 0; z-index: 50; padding: 1rem 1.5rem;
  background: rgba(30, 31, 34, 0.85); backdrop-filter: blur(12px); border-bottom: 1px solid rgba(43, 45, 49, 1);
  display: flex; align-items: center; justify-content: space-between;
}
.back-link { display: inline-flex; align-items: center; gap: 0.5rem; color: #00B578; font-weight: 600; text-decoration: none; transition: opacity 0.2s; }
.back-link:hover { opacity: 0.8; }
.toc-toggle { width: 40px; height: 40px; border-radius: 0.75rem; border: 1px solid rgba(0, 181, 120, 0.3); background: rgba(43, 45, 49, 0.5); color: #00B578; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: all 0.25s; }
.toc-toggle:hover, .toc-toggle.active { background: rgba(0, 181, 120, 0.15); border-color: #00B578; }

/* TOC overlay */
.toc-overlay { position: fixed; inset: 0; z-index: 100; background: rgba(0, 0, 0, 0.5); backdrop-filter: blur(4px); }
.toc-panel { position: absolute; top: 0; right: 0; bottom: 0; width: 320px; max-width: 85vw; background: rgba(43, 45, 49, 0.97); backdrop-filter: blur(16px); border-left: 1px solid rgba(0, 181, 120, 0.2); padding: 1.5rem; overflow-y: auto; display: flex; flex-direction: column; }
.toc-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 1.5rem; padding-bottom: 1rem; border-bottom: 1px solid rgba(43, 45, 49, 1); }
.toc-header h3 { font-size: 1.25rem; font-weight: 700; color: #00B578; margin: 0; }
.toc-close { width: 32px; height: 32px; border-radius: 0.5rem; border: none; background: rgba(0, 181, 120, 0.1); color: #00B578; cursor: pointer; display: flex; align-items: center; justify-content: center; }
.toc-list { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 0.25rem; }
.toc-link { display: flex; align-items: center; gap: 0.75rem; padding: 0.65rem 0.75rem; border-radius: 0.75rem; text-decoration: none; color: #949ba4; transition: all 0.25s; font-size: 0.9rem; font-weight: 600; }
.toc-link.active { background: rgba(0, 181, 120, 0.12); color: #00B578; font-weight: 700; }
.toc-link.toc-part { font-weight: 800; font-size: 0.95rem; color: #b5bac1; margin-top: 0.75rem; }
.toc-dot { width: 6px; height: 6px; border-radius: 50%; flex-shrink: 0; background: rgba(0, 181, 120, 0.3); }
.toc-link.active .toc-dot { background: #00B578; box-shadow: 0 0 8px rgba(0, 181, 120, 0.5); width: 8px; height: 8px; }
.toc-enter-active, .toc-leave-active { transition: opacity 0.3s ease; }
.toc-enter-active .toc-panel, .toc-leave-active .toc-panel { transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1); }
.toc-enter-from, .toc-leave-to { opacity: 0; }
.toc-enter-from .toc-panel, .toc-leave-to .toc-panel { transform: translateX(100%); }

/* HERO */
.hero { position: relative; z-index: 1; text-align: center; padding: 5rem 1.5rem 3rem; animation: fadeInDown 1s ease-out; }
.hero-badge { display: inline-block; font-size: 1rem; font-weight: 800; margin-bottom: 2rem; letter-spacing: 0.1em; padding: 0.5rem 2rem; border: 1px solid; border-radius: 999px; }
.hero-title { font-size: clamp(2rem, 5vw, 3rem); font-weight: 900; color: #fff; margin-bottom: 1rem; background: linear-gradient(135deg, #fff 30%, #00B578 100%); -webkit-background-clip: text; background-clip: text; -webkit-text-fill-color: transparent; }
.hero-subtitle { font-size: clamp(1rem, 2.5vw, 1.25rem); font-weight: 700; color: #b5bac1; margin-bottom: 0.75rem; }
.hero-desc { font-size: 1.1rem; font-weight: 800; text-transform: uppercase; letter-spacing: 0.15em; }

/* SECTIONS (Intro, Part Headers, Cards) */
.intro-section { position: relative; z-index: 1; padding: 2rem 0; }
.intro-card { background: rgba(43, 45, 49, 0.5); border: 1px solid rgba(0, 181, 120, 0.2); border-radius: 1.5rem; padding: 2.5rem; text-align: center; backdrop-filter: blur(8px); }
.ornament { color: #00B578; font-size: 1.5rem; margin: 1rem 0; text-align: center; }

.part-header { position: relative; z-index: 1; text-align: center; padding: 4rem 1.5rem 2rem; }
.part-number { display: inline-block; font-size: 0.85rem; font-weight: 800; letter-spacing: 0.3em; text-transform: uppercase; padding: 0.4rem 1.5rem; border: 1px solid; border-radius: 999px; margin-bottom: 1.5rem; }
.part-header h2 { font-size: clamp(1.5rem, 4vw, 2.5rem); font-weight: 800; color: #fff; margin-bottom: 1rem; }
.part-translation { color: #b5bac1; font-size: 1.1rem; font-weight: 600; }

.text-content-section { position: relative; z-index: 1; padding: 1rem 0 4rem; }
.article-h3 { font-size: 1.5rem; font-weight: 800; color: #fff; margin-bottom: 1rem; }
.article-p { font-size: 1.1rem; color: #b5bac1; line-height: 1.8; margin-bottom: 1.5rem; }

/* PHYSICS INTERACTIVE CUSTOM LAB CARD */
.interactive-lab-card { background: rgba(43, 45, 49, 0.6); backdrop-filter: blur(12px); border: 1px solid rgba(0, 181, 120, 0.2); border-radius: 1.5rem; padding: 2.5rem; margin-top: 2rem; }
.lab-title { font-size: 1.25rem; font-weight: 800; margin-bottom: 1rem; text-align: center; }

/* CHEAT SHEET (Mimicking exigences grid) */
.exigences-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 1rem; }
.exig-card { border-radius: 1.25rem; padding: 1.75rem; background: rgba(43, 45, 49, 0.5); display: flex; flex-direction: column; gap: 1rem; backdrop-filter: blur(8px); }
.exig-icon { font-size: 2rem; font-weight: 800; }

.bottom-line { height: 6px; }
.qcm-btn { padding: 0.75rem 2rem; border-radius: 999px; font-weight: 700; font-size: 1.05rem; transition: all 0.2s; border: none; cursor: pointer; }
.qcm-btn:hover { background: rgba(255,255,255,0.2) !important; transform: translateY(-2px); }

/* ANIMATIONS */
.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.2; } }
@keyframes fadeInDown { from { opacity: 0; transform: translateY(-20px); } to { opacity: 1; transform: translateY(0); } }
</style>
