<template>
  <div class="shahadatayn-page" ref="pageRef">
    <!-- Background Effects -->
    <div class="bg-effects bg-[#1e1f22]">
        <div class="absolute inset-0 opacity-[0.07]" style="background-image: radial-gradient(#00B578 1px, transparent 1px), radial-gradient(#00B578 1px, transparent 1px); background-size: 40px 40px, 40px 40px; background-position: 0 0, 20px 20px;"></div>
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
    <header class="hero" :class="{ visible: heroVisible }">
      <div class="hero-badge">SCIENCES & CHIMIE</div>
      <h1 class="hero-title">De quoi est constitué un atome ?</h1>
      <p class="hero-subtitle">Le guide complet et définitif sur la structure de la matière</p>
      <p class="hero-desc">Protons, Quarks et Quantique</p>
      <div class="scroll-indicator">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 5v14M5 12l7 7 7-7"/></svg>
      </div>
    </header>

    <!-- Introduction -->
    <section id="intro" class="intro-section" :class="{ visible: introVisible }" ref="introRef">
      <div class="section-container">
        <div class="intro-card">
          <div class="ornament">⚛️</div>
          <p>
            Lorsque vous regardez votre main, l'écran de votre ordinateur, une goutte d'eau ou même les étoiles scintillant dans le ciel nocturne, vous observez la même chose sous des formes différentes : des <strong>atomes</strong>.
          </p>
          <p class="mt-4 text-[#b5bac1] leading-relaxed">
            Mais une question vertigineuse se pose lorsque l'on observe l'infiniment petit : <strong>de quoi est constitué un atome ?</strong> Si l'atome forme la matière, de quoi est-il lui-même formé ?
          </p>
          <p class="mt-4 text-[#949ba4] text-sm">
            Dans ce guide exhaustif, nous allons disséquer l'atome. De sa composition classique à la mécanique quantique, jusqu'à son origine cosmique.
          </p>
          <div class="ornament">⚛️</div>
        </div>
      </div>
    </section>

    <!-- Chapitre 1 -->
    <section id="chap1" class="part-header" :class="{ visible: true }" ref="chap1Ref">
      <span class="part-number">CHAPITRE 1</span>
      <h2>Les bases de l'atome</h2>
      <p class="part-translation">Une question d'échelle</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <h3 class="article-h3">Qu'est-ce qu'un atome ?</h3>
        <p class="article-p">En termes simples, un atome est la plus petite unité constitutive de la matière ordinaire qui possède les propriétés d'un <strong>élément chimique</strong>. Chaque élément chimique de notre univers, répertorié dans le célèbre <strong>tableau périodique des éléments</strong> (ou tableau de Mendeleïev), est défini par un type précis d'atome.</p>

        <h3 class="article-h3 mt-10">La taille d'un atome : L'infiniment petit</h3>
        <p class="article-p">Il est presque impossible pour le cerveau humain de se représenter la taille réelle d'un atome. La taille moyenne d'un atome est d'environ un dixième de nanomètre, soit 10<sup>-10</sup> mètre (un angström).</p>
        <ul class="article-ul">
          <li>Une simple goutte d'eau contient environ 1,5 trilliard d'atomes (1 500 000 000 000 000 000 000 atomes).</li>
          <li>Si vous vouliez agrandir les atomes d'une pomme pour qu'ils aient la taille d'une pomme normale, la pomme originale devrait être agrandie pour atteindre la taille de la planète Terre !</li>
          <li>Un cheveu humain a une épaisseur d'environ un million d'atomes de carbone.</li>
        </ul>
      </div>
    </section>

    <!-- Chapitre 2 : Pillars style -->
    <section id="chap2" class="part-header" :class="{ visible: true }" ref="chap2Ref">
      <span class="part-number">CHAPITRE 2</span>
      <h2>La composition classique</h2>
      <p class="part-translation">Le triptyque de base</p>
    </section>

    <section class="pillars-section visible">
      <div class="section-container">
        <p class="section-intro text-center mb-8">La science classique définit l'atome autour de trois particules subatomiques fondamentales.</p>
        
        <div class="pillars-grid mb-12">
          <!-- Protons -->
          <div class="pillar-card abd">
            <div class="text-4xl mb-4">🔴</div>
            <h4>Le Proton</h4>
            <p class="text-[#00B578] font-bold mb-2">Charge Positive (+)</p>
            <p>Il définit l'identité chimique de l'atome (Z). Masse : ~1,672 × 10<sup>-27</sup> kg.</p>
          </div>
          <div class="pillar-separator">
            <div class="separator-line"></div>
            <span class="separator-icon">+</span>
            <div class="separator-line"></div>
          </div>
          <!-- Neutrons -->
          <div class="pillar-card rasul">
            <div class="text-4xl mb-4">⚪</div>
            <h4>Le Neutron</h4>
            <p class="text-[#00B578] font-bold mb-2">Charge Neutre (0)</p>
            <p>La "colle" stabilisatrice qui maintient le noyau uni grâce à l'interaction forte.</p>
          </div>
        </div>
        
        <div class="pillars-summary mb-12">
          <p>Ces deux particules (Nucléons) forment le <strong>Noyau Atomique</strong> qui concentre > 99,9% de la masse.</p>
        </div>

        <div class="pillars-grid">
           <!-- Electrons -->
           <div class="pillar-card abd w-full max-w-md mx-auto">
            <div class="text-4xl mb-4">⚡</div>
            <h4>L'Électron</h4>
            <p class="text-[#0D99FF] font-bold mb-2">Charge Négative (-)</p>
            <p>Le nuage périphérique. Incroyablement léger (1836 fois moins qu'un proton), il forme les liaisons chimiques.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Chapitre 3 -->
    <section id="chap3" class="part-header" :class="{ visible: true }" ref="chap3Ref">
      <span class="part-number">CHAPITRE 3</span>
      <h2>La structure lacunaire</h2>
      <p class="part-translation">Pourquoi l'atome est-il fait de vide ?</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <div class="intro-card text-left mb-10">
          <h3 class="text-[#00B578] font-bold text-xl mb-3">L'analogie du stade de football</h3>
          <p class="text-[#b5bac1] leading-relaxed">
            Si vous agrandissiez un atome pour qu'il ait la taille du Stade de France, le noyau serait de la taille d'une petite coccinelle posée au centre du terrain. Les électrons, quant à eux, ne seraient pas plus gros que des têtes d'épingle orbitant tout en haut des gradins.
          </p>
          <p class="text-white mt-4 font-bold">
            Que trouve-t-on entre le noyau et les électrons ? Absolument rien. L'atome est constitué à 99,9999999 % de vide.
          </p>
        </div>

        <h3 class="article-h3">Pourquoi la matière nous semble-t-elle solide ?</h3>
        <p class="article-p">La réponse réside dans les forces électromagnétiques quantiques. Lorsque vous touchez un objet, les électrons des atomes de votre main s'approchent des électrons des atomes de l'objet. Les électrons ayant tous une charge négative, ils se repoussent violemment avant même de se toucher physiquement. Ce que vous ressentez comme étant "solide" n'est en fait que la <strong>répulsion électromagnétique</strong> entre des nuages d'électrons !</p>
      </div>
    </section>

    <!-- Chapitre 4 & 5 (Subatomique) -->
    <section id="chap4" class="part-header" :class="{ visible: true }" ref="chap4Ref">
      <span class="part-number">CHAPITRE 4 & 5</span>
      <h2>Mécanique quantique et Quarks</h2>
      <p class="part-translation">Le Modèle Standard de la physique</p>
    </section>

    <section class="exigences-section visible">
      <div class="section-container">
        <p class="article-p text-center mb-10">L'image du système solaire miniature (Bohr) est fausse. Place à la physique quantique et au Modèle Standard.</p>
        
        <div class="exigences-grid mb-12">
          <div class="exig-card" style="animation-delay: 0.1s">
            <div class="exig-icon">🎲</div>
            <div>
                <h4 class="text-white font-bold mb-2">Principe d'incertitude</h4>
                <p>Il est impossible de connaître simultanément la position exacte et la vitesse exacte d'un électron (Heisenberg).</p>
            </div>
          </div>
          <div class="exig-card" style="animation-delay: 0.2s">
            <div class="exig-icon">☁️</div>
            <div>
                <h4 class="text-white font-bold mb-2">Orbitales de probabilité</h4>
                <p>Les électrons ne sont pas sur des orbites, mais dans des "nuages fluides" (orbitales s, p, d, f) définissant leur probabilité de présence.</p>
            </div>
          </div>
          <div class="exig-card" style="animation-delay: 0.3s">
            <div class="exig-icon">🧱</div>
            <div>
                <h4 class="text-white font-bold mb-2">Les Quarks (Up & Down)</h4>
                <p>Les protons (uud) et les neutrons (udd) sont des particules composites faites de quarks inséparables.</p>
            </div>
          </div>
          <div class="exig-card" style="animation-delay: 0.4s">
            <div class="exig-icon">🔗</div>
            <div>
                <h4 class="text-white font-bold mb-2">Les Gluons</h4>
                <p>Ils propagent "l'interaction nucléaire forte", la super-glue de l'univers qui maintient les quarks ensemble.</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Chronologie en carrousel (Chap 6) -->
    <section id="chap6" class="part-header" :class="{ visible: true }" ref="chap6Ref">
      <span class="part-number">CHAPITRE 6</span>
      <h2>L'histoire de la découverte</h2>
      <p class="part-translation">2500 ans de réflexion scientifique</p>
    </section>

    <section class="carousel-section visible">
      <div class="section-container">
        <div class="carousel-wrapper">
          <div class="carousel-track" :style="{ transform: `translateX(-${carouselHistory * 100}%)` }">
            <div class="carousel-slide">
              <div class="meaning-card">
                <div class="card-icon" style="background: rgba(0, 181, 120, 0.2); color: #00B578;">V</div>
                <h4>Antiquité : Démocrite</h4>
                <p>L'Atomisme ("atomos" : insécable). Une particule fondamentale impossible à diviser, basée sur la philosophie.</p>
              </div>
            </div>
            <div class="carousel-slide">
              <div class="meaning-card">
                <div class="card-icon" style="background: rgba(0, 181, 120, 0.2); color: #00B578;">1803</div>
                <h4>John Dalton</h4>
                <p>Ravive l'atome avec la chimie moderne, modélisant les éléments comme des sphères de masses différentes.</p>
              </div>
            </div>
            <div class="carousel-slide">
              <div class="meaning-card">
                <div class="card-icon" style="background: rgba(0, 181, 120, 0.2); color: #00B578;">1897</div>
                <h4>J.J. Thomson (Plum Pudding)</h4>
                <p>Découverte de l'électron. L'atome n'est plus indivisible.</p>
              </div>
            </div>
            <div class="carousel-slide">
              <div class="meaning-card">
                <div class="card-icon" style="background: rgba(0, 181, 120, 0.2); color: #00B578;">1911</div>
                <h4>Ernest Rutherford</h4>
                <p>L'expérience de la feuille d'or : découverte du noyau atomique dense et positif au milieu du vide.</p>
              </div>
            </div>
            <div class="carousel-slide">
              <div class="meaning-card">
                <div class="card-icon" style="background: rgba(0, 181, 120, 0.2); color: #00B578;">1913+</div>
                <h4>Bohr & l'ère quantique</h4>
                <p>Niveaux d'énergie de Niels Bohr, puis Chadwick découvrant le neutron (1932), jusqu'aux orbitales de Schrödinger/Heisenberg.</p>
              </div>
            </div>
          </div>
          <div class="carousel-controls">
            <button @click="prevHistory" :disabled="carouselHistory === 0" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M15 18l-6-6 6-6"/></svg>
            </button>
            <div class="carousel-dots">
              <span v-for="i in 5" :key="i" class="dot" :class="{ active: carouselHistory === i - 1 }" @click="carouselHistory = i - 1"></span>
            </div>
            <button @click="nextHistory" :disabled="carouselHistory === 4" class="carousel-btn">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 18l6-6-6-6"/></svg>
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Rest of content (Chap 7-12) condensed -->
    <section id="chap7" class="part-header" :class="{ visible: true }" ref="chap7Ref">
      <span class="part-number">CHAPITRE 7 - 12</span>
      <h2>Radioactivité, Étoiles et Antimatière</h2>
      <p class="part-translation">De l'échelle cosmique au tableau périodique</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <h3 class="article-h3">Isotopes et Ions</h3>
        <p class="article-p">Un <strong>isotope</strong> est une variante d'un élément ayant le même nombre de protons mais un nombre différent de neutrons (ex: Carbone 14). Un <strong>ion</strong> est un atome chargé électriquement ayant perdu ou gagné des électrons (Cation = +, Anion = -).</p>

        <h3 class="article-h3 mt-10">L'origine cosmique des atomes</h3>
        <p class="article-p">D'où viennent-ils ? "Nous sommes de la poussière d'étoiles" (Carl Sagan). L'Hydrogène et l'Hélium proviennent du Big Bang. Les atomes plus lourds (jusqu'au fer) sont "forgés" par fusion nucléaire au cœur des étoiles. Les éléments très lourds (or, uranium) naissent lors des explosions de Supernovas et fusions d'étoiles à neutrons.</p>

        <h3 class="article-h3 mt-10">Fission, Fusion et Antimatière</h3>
        <ul class="article-ul">
            <li><strong>Fission :</strong> Briser un atome lourd (Uranium) pour libérer de l'énergie thermique (Centrales nucléaires, E=mc²).</li>
            <li><strong>Fusion :</strong> Fusionner des atomes légers pour libérer encore plus d'énergie (Énergie des étoiles, projet ITER).</li>
            <li><strong>Antimatière :</strong> Le miroir sombre. Rencontrer la matière provoque une annihilation totale en énergie pure (Positrons, Anti-protons).</li>
        </ul>

        <h3 class="article-h3 mt-10">Le Tableau Périodique</h3>
        <p class="article-p">Inventé par Dmitri Mendeleïev en 1869, il répertorie les 118 éléments chimiques connus (du léger Hydrogène à l'artificiel Oganesson) classés par le nombre de protons (numéro atomique Z).</p>
      </div>
    </section>
    <!-- Quiz (QCM) -->
    <section id="qcm" class="part-header" :class="{ visible: true }" ref="qcmRef">
      <span class="part-number">TESTEZ-VOUS</span>
      <h2>QCM - De quoi est constitué un atome ?</h2>
      <p class="part-translation">Testez vos connaissances sur la structure atomique</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <div class="qcm-card">
          <div v-if="!quizCompleted">
            <div class="qcm-header">
              <span class="qcm-progress">Question {{ currentQuestionIndex + 1 }} / {{ questions.length }}</span>
              <span class="qcm-score">Score: {{ score }}</span>
            </div>
            
            <h3 class="qcm-question">{{ questions[currentQuestionIndex].question }}</h3>
            
            <div class="qcm-options">
              <button 
                v-for="(option, index) in questions[currentQuestionIndex].options" 
                :key="index"
                class="qcm-option"
                :class="{ 
                  'selected': selectedOption === option,
                  'correct': showExplanation && option === questions[currentQuestionIndex].reponse_correcte,
                  'wrong': showExplanation && selectedOption === option && option !== questions[currentQuestionIndex].reponse_correcte
                }"
                @click="selectOption(option)"
                :disabled="showExplanation"
              >
                <span class="option-letter">{{ ['A', 'B', 'C', 'D'][index] }}</span>
                <span class="option-text">{{ option }}</span>
              </button>
            </div>
            
            <Transition name="fade">
              <div v-if="showExplanation" class="qcm-explanation" :class="isCorrect ? 'bg-[#00B578]/10 border-[#00B578]/30' : 'bg-red-500/10 border-red-500/30'">
                <h4 :class="isCorrect ? 'text-[#00B578]' : 'text-red-400'">
                  {{ isCorrect ? '✅ Bonne réponse !' : '❌ Mauvaise réponse.' }}
                </h4>
                <p>{{ questions[currentQuestionIndex].explication }}</p>
              </div>
            </Transition>
            
            <div class="qcm-actions">
              <button 
                v-if="!showExplanation" 
                class="qcm-btn validate-btn" 
                @click="validateAnswer" 
                :disabled="!selectedOption"
              >
                Valider
              </button>
              <button 
                v-else 
                class="qcm-btn next-btn" 
                @click="nextQuestion"
              >
                {{ currentQuestionIndex < questions.length - 1 ? 'Question suivante' : 'Voir le résultat' }}
              </button>
            </div>
          </div>
          
          <div v-else class="qcm-result">
            <div class="text-6xl mb-4 text-center">🏆</div>
            <h3 class="text-2xl font-bold text-white mb-2 text-center">Quiz terminé !</h3>
            <p class="text-xl text-[#b5bac1] mb-6 text-center">Votre score final est de <span class="text-[#00B578] font-bold">{{ score }}</span> sur {{ questions.length }}.</p>
            <div class="flex justify-center">
              <button class="qcm-btn validate-btn" @click="resetQuiz">Recommencer le quiz</button>
            </div>
          </div>
        </div>
      </div>
    </section>



    <!-- Bottom Decorative -->
    <div class="bottom-line"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'

definePageMeta({ layout: false })
useSeoMeta({
  title: 'De quoi est constitué un atome ? Le guide complet (Protons, Quarks, Quantique)',
  description: 'Vous voulez savoir de quoi est constitué un atome ? Plongez au cœur de la matière : protons, neutrons, électrons, quarks, physique quantique et histoire.'
})

const pageRef = ref<HTMLElement | null>(null)
const heroVisible = ref(true)
const introVisible = ref(true)
const tocOpen = ref(false)
const activeSection = ref('intro')
const carouselHistory = ref(0)

const questions = [
  {
    question: "De quoi est constitué plus de 99,99 % du volume d'un atome ?",
    options: ["De protons", "D'électrons", "De vide", "De plasma"],
    reponse_correcte: "De vide",
    explication: "L'atome a une structure dite lacunaire, l'immense majorité de son volume est un espace totalement vide entre le noyau et le nuage électronique."
  },
  {
    question: "Où est concentrée presque toute la masse d'un atome ?",
    options: ["Dans le nuage électronique", "Dans le noyau central", "Répartie uniformément dans l'atome", "Dans les couches de valence"],
    reponse_correcte: "Dans le noyau central",
    explication: "Le noyau concentre plus de 99,9 % de la masse de l'atome, car les protons et les neutrons sont environ 2000 fois plus lourds que les électrons."
  },
  {
    question: "Quelles particules trouve-t-on dans le noyau d'un atome ?",
    options: ["Des protons et des électrons", "Des électrons et des neutrons", "Uniquement des protons", "Des protons et des neutrons"],
    reponse_correcte: "Des protons et des neutrons",
    explication: "Le noyau est composé de nucléons, qui regroupent les protons (charge positive) et les neutrons (charge neutre)."
  },
  {
    question: "Quelle est la charge électrique d'un électron ?",
    options: ["Positive", "Négative", "Neutre", "Variable selon la température"],
    reponse_correcte: "Négative",
    explication: "L'électron est une particule élémentaire porteuse d'une charge électrique négative."
  },
  {
    question: "Qu'est-ce qui détermine la nature d'un élément chimique (son numéro atomique Z) ?",
    options: ["Le nombre de neutrons", "Le nombre d'électrons", "Le nombre de protons", "La masse totale de l'atome"],
    reponse_correcte: "Le nombre de protons",
    explication: "Le nombre de protons définit l'élément chimique. Par exemple, un atome avec 1 proton est toujours de l'hydrogène, avec 6 c'est du carbone."
  },
  {
    question: "Que signifie le mot grec ancien 'atomos' dont dérive le mot atome ?",
    options: ["Invisible", "Léger", "Énergétique", "Indivisible"],
    reponse_correcte: "Indivisible",
    explication: "Le philosophe Démocrite a inventé ce mot pour désigner une particule si petite qu'elle ne pourrait plus être coupée ou divisée."
  },
  {
    question: "Comment appelle-t-on des atomes qui ont le même nombre de protons mais un nombre différent de neutrons ?",
    options: ["Des ions", "Des isotopes", "Des isomères", "Des cations"],
    reponse_correcte: "Des isotopes",
    explication: "Les isotopes sont des variantes d'un même élément. Le Carbone 12 et le Carbone 14 sont des isotopes du carbone."
  },
  {
    question: "Qu'est-ce qu'un ion ?",
    options: ["Un atome qui a perdu ou gagné un ou plusieurs électrons", "Un atome qui a explosé", "Un atome sans noyau", "Un atome d'antimatière"],
    reponse_correcte: "Un atome qui a perdu ou gagné un ou plusieurs électrons",
    explication: "Lorsqu'un atome perd ou gagne un électron, il n'est plus électriquement neutre, il devient un ion (cation si positif, anion si négatif)."
  },
  {
    question: "Selon la physique moderne, comment se déplacent les électrons autour du noyau ?",
    options: ["Sur des orbites circulaires parfaites comme des planètes", "En ligne droite de bout en bout", "Dans des orbitales formant un nuage de probabilité", "Ils sont immobiles"],
    reponse_correcte: "Dans des orbitales formant un nuage de probabilité",
    explication: "La mécanique quantique et le principe d'incertitude de Heisenberg indiquent que l'électron occupe des zones de probabilité appelées orbitales atomiques."
  },
  {
    question: "De quelles particules élémentaires les protons et les neutrons sont-ils constitués ?",
    options: ["Les photons", "Les quarks", "Les bosons de Higgs", "Les muons"],
    reponse_correcte: "Les quarks",
    explication: "Les protons et neutrons ne sont pas élémentaires ; ils sont constitués de quarks (Up et Down)."
  },
  {
    question: "Quelle est la composition exacte d'un proton en termes de quarks ?",
    options: ["3 quarks Down", "2 quarks Up et 1 quark Down", "1 quark Up et 2 quarks Down", "2 quarks Charm et 1 quark Strange"],
    reponse_correcte: "2 quarks Up et 1 quark Down",
    explication: "Un proton est formé de deux quarks Up (charge +2/3) et d'un quark Down (charge -1/3), donnant une charge totale de +1."
  },
  {
    question: "Quelle particule maintient les quarks ensemble à l'intérieur du noyau (véhiculant la force nucléaire forte) ?",
    options: ["Le gluon", "Le graviton", "Le photon", "L'électron"],
    reponse_correcte: "Le gluon",
    explication: "Les gluons (de 'glue' = colle) sont les bosons qui transmettent l'interaction forte reliant les quarks entre eux."
  },
  {
    question: "Quel scientifique a découvert l'existence du noyau atomique grâce à l'expérience de la feuille d'or en 1911 ?",
    options: ["J.J. Thomson", "Niels Bohr", "Ernest Rutherford", "Marie Curie"],
    reponse_correcte: "Ernest Rutherford",
    explication: "En bombardant une feuille d'or avec des particules, Rutherford a constaté que certaines rebondissaient, prouvant l'existence d'un noyau dense."
  },
  {
    question: "Quelle est l'antiparticule de l'électron dans l'antimatière ?",
    options: ["L'antiquark", "Le proton", "Le positron", "Le neutrino"],
    reponse_correcte: "Le positron",
    explication: "Le positron a la même masse que l'électron, mais possède une charge électrique positive."
  },
  {
    question: "Où ont été créés les atomes les plus lourds de l'univers (comme l'or ou l'uranium) ?",
    options: ["Lors du Big Bang", "Dans le manteau terrestre", "Dans les étoiles ordinaires comme notre Soleil", "Lors d'explosions de supernovas ou fusions d'étoiles à neutrons"],
    reponse_correcte: "Lors d'explosions de supernovas ou fusions d'étoiles à neutrons",
    explication: "La nucléosynthèse des éléments plus lourds que le fer requiert une énergie colossale qui n'est dégagée que lors d'événements cosmiques extrêmes."
  },
  {
    question: "Comment s'appelle le processus où un gros noyau instable se casse en deux noyaux plus petits en libérant de l'énergie ?",
    options: ["La fusion nucléaire", "La fission nucléaire", "L'ionisation", "L'annihilation"],
    reponse_correcte: "La fission nucléaire",
    explication: "La fission nucléaire est le fait de briser un atome lourd (ex: Uranium), c'est le processus utilisé dans nos centrales nucléaires."
  },
  {
    question: "Comment appelle-t-on le tableau qui classe tous les éléments chimiques de l'univers ?",
    options: ["Le tableau d'Einstein", "La table de Newton", "Le tableau périodique de Mendeleïev", "Le registre quantique"],
    reponse_correcte: "Le tableau périodique de Mendeleïev",
    explication: "Inventé en 1869 par Dmitri Mendeleïev, il classe les éléments selon leur numéro atomique et leurs propriétés chimiques."
  },
  {
    question: "Lequel de ces éléments est l'atome le plus simple et le plus abondant dans l'univers ?",
    options: ["L'Oxygène", "Le Carbone", "L'Hydrogène", "L'Hélium"],
    reponse_correcte: "L'Hydrogène",
    explication: "L'hydrogène (Z=1), composé d'un proton et d'un électron, représente environ 75 % de la matière baryonique de l'univers."
  },
  {
    question: "Quelles particules sont impliquées lorsqu'une liaison chimique se forme entre deux atomes pour créer une molécule ?",
    options: ["Les protons", "Les neutrons du noyau", "Les quarks", "Les électrons de la couche externe"],
    reponse_correcte: "Les électrons de la couche externe",
    explication: "Ce sont les électrons de valence (sur la couche la plus externe) qui sont partagés ou échangés lors des réactions chimiques."
  },
  {
    question: "Que se passe-t-il si un atome de matière rencontre un atome d'antimatière ?",
    options: ["Ils fusionnent pour créer un atome super-lourd", "Ils se repoussent magnétiquement", "Ils s'annihilent et se transforment à 100 % en énergie pure", "Ils deviennent invisibles"],
    reponse_correcte: "Ils s'annihilent et se transforment à 100 % en énergie pure",
    explication: "La rencontre matière-antimatière provoque une annihilation totale, convertissant toute leur masse en énergie selon l'équation E=mc2."
  }
]

const currentQuestionIndex = ref(0)
const selectedOption = ref<string | null>(null)
const score = ref(0)
const showExplanation = ref(false)
const quizCompleted = ref(false)
const isCorrect = ref(false)

function selectOption(option: string) {
  if (showExplanation.value) return
  selectedOption.value = option
}

function validateAnswer() {
  if (!selectedOption.value) return
  const correct = questions[currentQuestionIndex.value].reponse_correcte
  isCorrect.value = selectedOption.value === correct
  if (isCorrect.value) {
    score.value++
  }
  showExplanation.value = true
}

function nextQuestion() {
  if (currentQuestionIndex.value < questions.length - 1) {
    currentQuestionIndex.value++
    selectedOption.value = null
    showExplanation.value = false
  } else {
    quizCompleted.value = true
  }
}

function resetQuiz() {
  currentQuestionIndex.value = 0
  selectedOption.value = null
  score.value = 0
  showExplanation.value = false
  quizCompleted.value = false
}

const tocItems = [
  { id: 'intro', label: 'Introduction', isPart: false, isSub: false },
  { id: 'chap1', label: 'Chapitre 1 : Les bases', isPart: true, isSub: false },
  { id: 'chap2', label: 'Chapitre 2 : Composition', isPart: true, isSub: false },
  { id: 'chap3', label: 'Chapitre 3 : Le Vide', isPart: true, isSub: false },
  { id: 'chap4', label: 'Chapitres 4 & 5 : Quantique', isPart: true, isSub: false },
  { id: 'chap6', label: 'Chapitre 6 : Histoire', isPart: true, isSub: false },
  { id: 'chap7', label: 'Chapitres 7 - 12 : Cosmique', isPart: true, isSub: false },
  { id: 'qcm', label: 'Quiz : Testez-vous', isPart: true, isSub: false },
]

function scrollToSection(id: string) {
  const el = document.getElementById(id)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' })
    tocOpen.value = false
  }
}

function prevHistory() {
  if (carouselHistory.value > 0) carouselHistory.value--
}
function nextHistory() {
  if (carouselHistory.value < 4) carouselHistory.value++
}

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
/* Base Styles matching shahadatayn.vue */
.shahadatayn-page {
  position: relative; height: 100vh; overflow-y: auto; overflow-x: hidden;
  background: #1e1f22; color: #f2f3f5; font-family: 'Inter', sans-serif; padding-top: 58px;
}
.bg-effects { position: fixed; inset: 0; z-index: 0; pointer-events: none; }
.section-container { max-width: 800px; margin: 0 auto; padding: 0 1.5rem; }

/* Article specific text styles */
.article-h3 { font-size: 1.5rem; font-weight: 800; color: #fff; margin-bottom: 1rem; }
.article-p { font-size: 1.1rem; color: #b5bac1; line-height: 1.8; margin-bottom: 1.5rem; }
.article-ul { list-style: disc; padding-left: 1.5rem; color: #b5bac1; font-size: 1.1rem; line-height: 1.8; margin-bottom: 1.5rem; }
.article-ul li { margin-bottom: 0.5rem; }
.text-content-section { position: relative; z-index: 1; padding: 2rem 0 4rem; }

/* Nav */
.top-nav {
  position: fixed; top: 0; left: 0; right: 0; z-index: 50; padding: 1rem 1.5rem;
  background: rgba(30, 31, 34, 0.85); backdrop-filter: blur(12px); border-bottom: 1px solid rgba(43, 45, 49, 1);
  display: flex; align-items: center; justify-content: space-between;
}
.back-link { display: inline-flex; align-items: center; gap: 0.5rem; color: #00B578; font-weight: 600; text-decoration: none; transition: opacity 0.2s; }
.back-link:hover { opacity: 0.8; }
.toc-toggle { width: 40px; height: 40px; border-radius: 0.75rem; border: 1px solid rgba(0, 181, 120, 0.3); background: rgba(43, 45, 49, 0.5); color: #00B578; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: all 0.25s; }
.toc-toggle:hover, .toc-toggle.active { background: rgba(0, 181, 120, 0.15); border-color: #00B578; }

/* TOC */
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

/* Hero */
.hero { position: relative; z-index: 1; text-align: center; padding: 5rem 1.5rem 3rem; animation: fadeInDown 1s ease-out; }
.hero-badge { display: inline-block; font-size: 1rem; font-weight: 800; color: #00B578; margin-bottom: 2rem; letter-spacing: 0.1em; padding: 0.5rem 2rem; border: 1px solid rgba(0, 181, 120, 0.3); border-radius: 999px; background: rgba(0, 181, 120, 0.05); }
.hero-title { font-size: clamp(2rem, 5vw, 3rem); font-weight: 900; color: #fff; margin-bottom: 1rem; background: linear-gradient(135deg, #fff 30%, #00B578 100%); -webkit-background-clip: text; background-clip: text; -webkit-text-fill-color: transparent; }
.hero-subtitle { font-size: clamp(1rem, 2.5vw, 1.25rem); font-weight: 700; color: #b5bac1; margin-bottom: 0.75rem; }
.hero-desc { font-size: 1.1rem; color: #00B578; font-weight: 800; text-transform: uppercase; letter-spacing: 0.15em; }

/* Intro / Part Headers / Cards */
.intro-section { position: relative; z-index: 1; padding: 2rem 0; }
.intro-card { background: rgba(43, 45, 49, 0.5); border: 1px solid rgba(0, 181, 120, 0.2); border-radius: 1.5rem; padding: 2.5rem; text-align: center; backdrop-filter: blur(8px); }
.ornament { color: #00B578; font-size: 1.5rem; margin: 1rem 0; text-align: center; }

.part-header { position: relative; z-index: 1; text-align: center; padding: 4rem 1.5rem 2rem; }
.part-number { display: inline-block; font-size: 0.85rem; font-weight: 800; color: #00B578; letter-spacing: 0.3em; text-transform: uppercase; padding: 0.4rem 1.5rem; border: 1px solid rgba(0, 181, 120, 0.4); border-radius: 999px; margin-bottom: 1.5rem; background: rgba(0, 181, 120, 0.05); }
.part-header h2 { font-size: clamp(1.5rem, 4vw, 2.5rem); font-weight: 800; color: #fff; margin-bottom: 1rem; }
.part-translation { color: #b5bac1; font-size: 1.1rem; font-weight: 600; }

/* Pillars (Composition) */
.pillars-section { position: relative; z-index: 1; padding: 1rem 0 3rem; }
.pillars-grid { display: flex; align-items: stretch; gap: 1rem; flex-wrap: wrap; justify-content: center; }
.pillar-card { flex: 1; min-width: 250px; border-radius: 1.25rem; padding: 2rem; text-align: center; border: 1px solid rgba(0, 181, 120, 0.2); background: rgba(43, 45, 49, 0.5); backdrop-filter: blur(8px); transition: transform 0.3s; }
.pillar-card:hover { transform: translateY(-4px); box-shadow: 0 8px 30px rgba(0, 181, 120, 0.1); }
.pillar-card h4 { color: #fff; font-size: 1.3rem; margin-bottom: 0.75rem; font-weight: 800; }
.pillar-card p { color: #b5bac1; }
.pillar-separator { display: flex; flex-direction: column; align-items: center; justify-content: center; gap: 0.5rem; padding: 1rem 0; }
.separator-line { width: 2px; height: 30px; background: rgba(0, 181, 120, 0.3); }
.separator-icon { width: 32px; height: 32px; border-radius: 50%; background: rgba(0, 181, 120, 0.15); color: #00B578; display: flex; align-items: center; justify-content: center; font-weight: 700; font-size: 1.1rem; }
.pillars-summary { text-align: center; padding: 1.25rem; border-radius: 1rem; background: rgba(0, 181, 120, 0.08); border: 1px solid rgba(0, 181, 120, 0.2); color: #00B578; font-weight: 700; }

/* Exigences (Quantique) */
.exigences-section { position: relative; z-index: 1; padding: 1rem 0 3rem; }
.exigences-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 1rem; }
.exig-card { border-radius: 1.25rem; padding: 1.75rem; background: rgba(43, 45, 49, 0.5); border: 1px solid rgba(0, 181, 120, 0.15); display: flex; align-items: flex-start; gap: 1rem; backdrop-filter: blur(8px); }
.exig-icon { font-size: 2rem; flex-shrink: 0; }

/* Carousel (Histoire) */
.carousel-section { position: relative; z-index: 1; padding: 1rem 0 3rem; }
.carousel-wrapper { overflow: hidden; border-radius: 1.25rem; }
.carousel-track { display: flex; transition: transform 0.5s cubic-bezier(0.16, 1, 0.3, 1); }
.carousel-slide { min-width: 100%; padding: 0 0.25rem; box-sizing: border-box; }
.meaning-card { border-radius: 1.25rem; padding: 2.5rem 2rem; min-height: 280px; display: flex; flex-direction: column; align-items: center; justify-content: center; text-align: center; gap: 0.75rem; border: 1px solid rgba(0, 181, 120, 0.2); background: rgba(43, 45, 49, 0.5); }
.card-icon { width: 64px; height: 64px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 1.2rem; font-weight: 800; }
.meaning-card h4 { font-size: 1.4rem; color: #fff; font-weight: 800; }
.meaning-card p { color: #b5bac1; font-size: 1.1rem; }
.carousel-controls { display: flex; align-items: center; justify-content: center; gap: 1rem; padding: 1.5rem 0; }
.carousel-btn { width: 40px; height: 40px; border-radius: 50%; border: 1px solid rgba(0, 181, 120, 0.3); background: rgba(43, 45, 49, 0.5); color: #00B578; cursor: pointer; display: flex; align-items: center; justify-content: center; }
.carousel-btn:disabled { opacity: 0.3; }
.carousel-dots { display: flex; gap: 0.5rem; }
.dot { width: 8px; height: 8px; border-radius: 50%; background: rgba(0, 181, 120, 0.3); cursor: pointer; }
.dot.active { background: #00B578; transform: scale(1.3); }

/* Conclusion */
.conclusion-section { padding: 4rem 0; }
.conclusion-card { text-align: center; border: 1px solid rgba(0, 181, 120, 0.3); border-radius: 1.5rem; padding: 3rem; background: linear-gradient(180deg, rgba(43, 45, 49, 0.8) 0%, rgba(0, 181, 120, 0.05) 100%); }
.conclusion-card h3 { font-size: 1.8rem; font-weight: 800; color: #00B578; margin-bottom: 1rem; }
.conclusion-card p { color: #b5bac1; font-size: 1.15rem; line-height: 1.8; }
.bottom-line { height: 6px; background: linear-gradient(90deg, #1e1f22, #00B578, #1e1f22); }

/* QCM / Quiz Styles */
.qcm-card { background: rgba(43, 45, 49, 0.6); backdrop-filter: blur(12px); border: 1px solid rgba(0, 181, 120, 0.2); border-radius: 1.5rem; padding: 2.5rem; margin-top: 2rem; }
.qcm-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; border-bottom: 1px solid rgba(229, 231, 235, 0.05); padding-bottom: 1rem; }
.qcm-progress { font-weight: 700; color: #b5bac1; font-size: 0.95rem; }
.qcm-score { font-weight: 800; color: #00B578; font-size: 1.1rem; }
.qcm-question { font-size: 1.3rem; font-weight: 800; color: #e5e7eb; margin-bottom: 2rem; line-height: 1.5; }
.qcm-options { display: flex; flex-direction: column; gap: 0.75rem; margin-bottom: 2rem; }
.qcm-option { display: flex; align-items: center; gap: 1rem; width: 100%; text-align: left; padding: 1rem 1.25rem; border-radius: 1rem; border: 1px solid rgba(229, 231, 235, 0.08); background: rgba(30, 31, 34, 0.5); color: #e5e7eb; font-size: 1.05rem; transition: all 0.2s; cursor: pointer; }
.qcm-option:hover:not(:disabled) { background: rgba(0, 181, 120, 0.05); border-color: rgba(0, 181, 120, 0.3); }
.qcm-option.selected { border-color: #00B578; background: rgba(0, 181, 120, 0.1); }
.qcm-option.correct { border-color: #00B578; background: rgba(0, 181, 120, 0.15); color: #00B578; font-weight: 700; }
.qcm-option.wrong { border-color: #ef4444; background: rgba(239, 68, 68, 0.1); color: #ef4444; font-weight: 700; }
.option-letter { display: flex; align-items: center; justify-content: center; width: 32px; height: 32px; border-radius: 50%; background: #2b2d31; font-weight: 800; font-size: 0.9rem; flex-shrink: 0; color: #e5e7eb; }
.qcm-option.selected .option-letter { background: #00B578; color: #1e1f22; }
.qcm-option.correct .option-letter { background: #00B578; color: #1e1f22; }
.qcm-option.wrong .option-letter { background: #ef4444; color: #e5e7eb; }
.qcm-explanation { padding: 1.5rem; border-radius: 1rem; border: 1px solid; margin-bottom: 2rem; }
.qcm-explanation h4 { font-weight: 800; font-size: 1.1rem; margin-bottom: 0.5rem; }
.qcm-explanation p { color: #e5e7eb; font-size: 1rem; line-height: 1.6; }
.qcm-actions { display: flex; justify-content: flex-end; }
.qcm-btn { padding: 0.75rem 2rem; border-radius: 999px; font-weight: 700; font-size: 1.05rem; transition: all 0.2s; border: none; cursor: pointer; }
.qcm-btn:disabled { opacity: 0.5; cursor: not-allowed; }
.validate-btn { background: #00B578; color: #1e1f22; }
.validate-btn:hover:not(:disabled) { background: #00c785; transform: translateY(-2px); }
.next-btn { background: #00B578; color: #1e1f22; }
.next-btn:hover { background: #00c785; transform: translateY(-2px); }
.fade-enter-active, .fade-leave-active { transition: opacity 0.3s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }
</style>
