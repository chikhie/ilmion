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
      <div class="hero-badge" style="color: #00B578; border-color: rgba(0, 181, 120, 0.3); background: rgba(0, 181, 120, 0.05);">PHYSIQUE & CINÉMATIQUE</div>
      <h1 class="hero-title pt-4">La Cinématique</h1>
      <p class="hero-subtitle">Mouvements, Vitesses et Accélérations</p>
      <p class="hero-desc" style="color: #00B578;">Observer le mouvement sans se soucier des forces</p>
      <div class="scroll-indicator">
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 5v14M5 12l7 7 7-7"/></svg>
      </div>
    </header>

    <!-- Introduction & Théorie -->
    <section id="theorie" class="intro-section visible" ref="theorieRef">
      <div class="section-container">
        <div class="intro-card mb-12">
          <div class="ornament">🚗</div>
          <p>
            La <strong>cinématique</strong> est la branche de la physique qui décrit le mouvement des objets <em>sans s'intéresser à leurs causes</em> (les forces).
          </p>
          <p class="mt-4 text-[#00B578] font-bold text-lg">
            👉 Elle répond à une seule question : « Comment un objet bouge-t-il ? »
          </p>
        </div>

        <h3 class="article-h3 border-l-4 border-[#00B578] pl-4 mb-6">1. Grandeurs Fondamentales</h3>
        <div class="exigences-grid mb-12">
          <!-- Position & Temps -->
          <div class="exig-card" style="border-top: 2px solid #00B578;">
            <div class="text-[#00B578] font-mono text-xl mb-2 font-bold">x(t) & Δt</div>
            <h4 class="text-white font-bold mb-2">Position & Temps</h4>
            <p class="text-sm text-[#b5bac1]"><strong>Position :</strong> Notée <code>x(t)</code>, représente la place d'un objet à un instant précis.<br/>
            <strong>Temps :</strong> <code>Δt = t₂ - t₁</code> (l'intervalle de temps écoulé).</p>
          </div>
          <!-- Distance -->
          <div class="exig-card" style="border-top: 2px solid #00B578;">
            <div class="text-[#00B578] font-mono text-xl mb-2 font-bold">d = |x₂ - x₁|</div>
            <h4 class="text-white font-bold mb-2">Distance vs Parcours</h4>
            <p class="text-sm text-[#b5bac1]">La distance <code>d</code> est l'écart direct entre deux positions. La "distance parcourue" est la longueur réelle (totale) du trajet.</p>
          </div>
          <!-- Vitesse -->
          <div class="exig-card" style="border-top: 2px solid #00B578;">
            <div class="text-[#00B578] font-mono text-xl mb-2 font-bold">v = dx/dt</div>
            <h4 class="text-white font-bold mb-2">La Vitesse</h4>
            <p class="text-sm text-[#b5bac1]">Vitesse d'un objet qui se déplace :<br/>
            Vitesse moyenne = <code>Distance / Δt</code><br/>
            Vitesse instantanée = <code>v = dx/dt</code>. C'est la dérivée de la position !</p>
          </div>
          <!-- Accélération -->
          <div class="exig-card" style="border-top: 2px solid #00B578;">
            <div class="text-[#00B578] font-mono text-xl mb-2 font-bold">a = dv/dt</div>
            <h4 class="text-white font-bold mb-2">L'Accélération</h4>
            <p class="text-sm text-[#b5bac1]">Désigne le changement de vitesse. C'est la dérivée de la vitesse : <code>a = dv/dt</code>.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Types de Mouvements -->
    <section id="mouvements" class="part-header visible" ref="mouvementsRef">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">MODÈLES</span>
      <h2>Types de Mouvements</h2>
      <p class="part-translation">Les formules magiques de la mécanique</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        <div class="grid md:grid-cols-2 gap-8 mb-12">
          <!-- MRU -->
          <div class="bg-black/30 border border-[#2b2d31] rounded-2xl p-6 shadow-xl">
            <h3 class="text-2xl font-bold text-white mb-2 flex items-center gap-2">🚗 MRU</h3>
            <p class="text-[#00B578] text-sm tracking-wider uppercase mb-4">Mouvement Rectiligne Uniforme</p>
            <ul class="text-[#b5bac1] space-y-2 mb-6 text-sm">
              <li>✓ Ligne droite</li>
              <li>✓ Vitesse <strong>constante</strong></li>
              <li>✓ Accélération <strong>nulle (0)</strong></li>
            </ul>
            <div class="bg-[#1e1f22] border border-[#00B578]/30 rounded-xl p-4 text-center font-mono text-white text-sm">
               x(t) = x₀ + v·t
            </div>
          </div>
          
          <!-- MRUA -->
          <div class="bg-black/30 border border-[#2b2d31] rounded-2xl p-6 shadow-xl">
            <h3 class="text-2xl font-bold text-white mb-2 flex items-center gap-2">🚀 MRUA</h3>
            <p class="text-[#00B578] text-sm tracking-wider uppercase mb-4">Mouvement Rectiligne Uniformément Accéléré</p>
            <ul class="text-[#b5bac1] space-y-2 mb-6 text-sm">
              <li>✓ Ligne droite</li>
              <li>✓ Accélération <strong>constante</strong></li>
            </ul>
            <div class="flex flex-col gap-3">
              <div class="bg-[#1e1f22] border border-[#00B578]/30 rounded-xl p-3 text-center font-mono text-white text-sm">
                v(t) = v₀ + a·t
              </div>
              <div class="bg-[#1e1f22] border border-[#00B578]/30 rounded-xl p-3 text-center font-mono text-white text-sm">
                x(t) = x₀ + v₀·t + ½·a·t²
              </div>
              <div class="bg-[#1e1f22] border border-[#00B578]/30 rounded-xl p-3 text-center font-mono text-white text-sm">
                v² = v₀² + 2a(x - x₀)
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Partie Pratique / Exercices -->
    <section id="exercices" class="part-header visible" ref="exercicesRef">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">LABORATOIRE</span>
      <h2>Exercices Pratiques</h2>
      <p class="part-translation">Entraînez-vous avec l'IA Cinématique</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container">
        
        <!-- EXERCICE 1 -->
        <div class="interactive-lab-card mb-8">
          <h3 class="lab-title text-left text-[#00B578] mb-4"><span class="bg-[#00B578]/20 px-2 rounded mr-2 h-7 inline-flex items-center text-sm font-bold">Ex 1</span> Mouvement Rectiligne Uniforme (MRU)</h3>
          <div class="p-4 bg-black/40 rounded-xl border border-[#2b2d31] mb-6 font-mono text-sm text-[#dbdee1]">
            <p><strong>Données :</strong> x₀ = 0 m | v = 5 m/s | t = 4 s</p>
            <p class="mt-2 text-white">Quelle est la position finale (x) en mètres ?</p>
          </div>
          
          <div class="flex items-center gap-4 mb-6">
            <input type="number" v-model="ex1Ans" placeholder="Votre réponse" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-4 py-2 rounded-lg outline-none focus:border-[#00B578] w-40 transition-colors">
            <button class="qcm-btn bg-[#00B578] text-[#1e1f22]" @click="evalEx(1, ex1Ans, 20)">Vérifier</button>
          </div>
          
          <Transition name="fade">
            <div v-if="ex1Res" class="p-4 rounded-xl border mb-4" :class="bgForRes(ex1Res)">
              <p class="font-bold mb-2">{{ ex1Res.text }}</p>
              <p class="text-sm">x = 0 + (5 × 4) = 20 m</p>
              
              <div class="mt-4 pt-4 border-t border-white/10" v-if="ex1Res.exact || ex1Res.close">
                <p class="text-xs uppercase mb-2">Graphique x(t) - Position linéaire</p>
                <svg viewBox="0 0 200 100" class="w-full h-32 bg-[#1e1f22] rounded-lg border border-[#2b2d31]">
                  <line x1="0" y1="100" x2="200" y2="100" stroke="#b5bac1" stroke-width="2"/>
                  <line x1="0" y1="0" x2="0" y2="100" stroke="#b5bac1" stroke-width="2"/>
                  <path d="M0,100 L200,0" fill="none" class="stroke-[#00B578]" stroke-width="3" stroke-dasharray="0 500" :style="ex1Res.exact ? 'animation: draw 1.5s ease-out forwards;' : 'stroke-dashoffset:0;'"/>
                </svg>
              </div>
            </div>
          </Transition>
        </div>

        <!-- EXERCICE 2 -->
        <div class="interactive-lab-card mb-8">
          <h3 class="lab-title text-left text-[#00B578] mb-4"><span class="bg-[#00B578]/20 px-2 rounded mr-2 h-7 inline-flex items-center text-sm font-bold">Ex 2</span> Mouvement Accéléré (MRUA)</h3>
          <div class="p-4 bg-black/40 rounded-xl border border-[#2b2d31] mb-6 font-mono text-sm text-[#dbdee1]">
            <p><strong>Données :</strong> x₀ = 0 m | v₀ = 0 m/s | a = 2 m/s² | t = 3 s</p>
            <p class="mt-2 text-white">Quelle est la position finale (x) en mètres ?</p>
          </div>
          
          <div class="flex items-center gap-4 mb-6">
            <input type="number" v-model="ex2Ans" placeholder="Votre réponse" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-4 py-2 rounded-lg outline-none focus:border-[#00B578] w-40 transition-colors">
            <button class="qcm-btn bg-[#00B578] text-[#1e1f22]" @click="evalEx(2, ex2Ans, 9)">Vérifier</button>
          </div>
          
          <Transition name="fade">
            <div v-if="ex2Res" class="p-4 rounded-xl border mb-4" :class="bgForRes(ex2Res)">
              <p class="font-bold mb-2">{{ ex2Res.text }}</p>
              <p class="text-sm">x = ½ × 2 × (3)² = 9 m</p>
              
              <div class="mt-4 pt-4 border-t border-white/10 flex gap-4" v-if="ex2Res.exact || ex2Res.close">
                <div class="flex-1">
                  <p class="text-xs uppercase mb-2 text-center text-white/70">x(t) : Parabole</p>
                  <svg viewBox="0 0 100 100" class="w-full h-24 bg-[#1e1f22] rounded-lg">
                    <path d="M0,100 Q 50,100 100,0" fill="none" class="stroke-[#00B578]" stroke-width="3" />
                  </svg>
                </div>
                <div class="flex-1">
                  <p class="text-xs uppercase mb-2 text-center text-white/70">v(t) : Droite</p>
                  <svg viewBox="0 0 100 100" class="w-full h-24 bg-[#1e1f22] rounded-lg">
                    <path d="M0,100 L100,0" fill="none" class="stroke-[#00B578]" stroke-width="3" />
                  </svg>
                </div>
              </div>
            </div>
          </Transition>
        </div>
        
        <!-- EXERCICE 3 & 4 (Grid) -->
        <div class="grid md:grid-cols-2 gap-8">
          <!-- EX 3 -->
          <div class="interactive-lab-card mt-0">
            <h3 class="lab-title text-left text-sm text-[#00B578] mb-4"><span class="bg-[#00B578]/20 px-2 rounded mr-2 font-bold">Ex 3</span> Trouver la Vitesse</h3>
            <div class="text-xs bg-black/40 p-3 rounded mb-4 font-mono">Distance = 100m | t = 10s</div>
            <div class="flex gap-2">
              <input type="number" v-model="ex3Ans" placeholder="v=?" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-3 py-2 rounded w-full outline-none focus:border-[#00B578]">
              <button class="bg-[#00B578] text-[#1e1f22] px-4 py-2 rounded-lg font-bold" @click="evalEx(3, ex3Ans, 10)">Ok</button>
            </div>
            <p v-if="ex3Res" class="mt-2 text-sm font-bold" :class="ex3Res.exact ? 'text-[#00B578]' : 'text-rose-400'">{{ ex3Res.text }}</p>
          </div>
          <!-- EX 4 -->
          <div class="interactive-lab-card mt-0">
            <h3 class="lab-title text-left text-sm text-[#00B578] mb-4"><span class="bg-[#00B578]/20 px-2 rounded mr-2 font-bold">Ex 4</span> Trouver l'Accélération</h3>
            <div class="text-xs bg-black/40 p-3 rounded mb-4 font-mono">v₀ = 0 | v = 20 m/s | t = 4s</div>
            <div class="flex gap-2">
              <input type="number" v-model="ex4Ans" placeholder="a=?" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-3 py-2 rounded w-full outline-none focus:border-[#00B578]">
              <button class="bg-[#00B578] text-[#1e1f22] px-4 py-2 rounded-lg font-bold" @click="evalEx(4, ex4Ans, 5)">Ok</button>
            </div>
            <p v-if="ex4Res" class="mt-2 text-sm font-bold" :class="ex4Res.exact ? 'text-[#00B578]' : 'text-rose-400'">{{ ex4Res.text }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Simulateur Global -->
    <section id="simulateur" class="part-header visible" ref="simulateurRef">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">SIMULATEUR</span>
      <h2>Simulateur Cinématique</h2>
      <p class="part-translation">Générez n'importe quel mouvement</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container pb-16">
        <div class="interactive-lab-card">
          <div class="grid md:grid-cols-4 gap-4 mb-8">
            <div class="flex flex-col">
              <label class="text-xs text-[#b5bac1] mb-1">x₀ (Pos. Initiale)</label>
              <input type="number" v-model.number="simX0" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-3 py-2 rounded transition-colors focus:border-[#00B578] outline-none">
            </div>
            <div class="flex flex-col">
              <label class="text-xs text-[#b5bac1] mb-1">v₀ (Vitesse Init)</label>
              <input type="number" v-model.number="simV0" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-3 py-2 rounded transition-colors focus:border-[#00B578] outline-none">
            </div>
            <div class="flex flex-col">
              <label class="text-xs text-[#b5bac1] mb-1">a (Accélération)</label>
              <input type="number" v-model.number="simA" class="bg-[#1e1f22] border border-[#2b2d31] text-white px-3 py-2 rounded transition-colors focus:border-[#00B578] outline-none">
            </div>
            <div class="flex flex-col">
              <label class="text-xs text-[#b5bac1] mb-1">t (Temps d'étude)</label>
              <input type="number" v-model.number="simT" class="bg-[#1e1f22] border border-[#00B578]/50 text-[#00B578] font-bold px-3 py-2 rounded transition-colors focus:border-[#00B578] outline-none" min="1" max="20">
            </div>
          </div>

          <div class="flex justify-between items-center bg-black/40 p-4 rounded-xl border border-[#00B578]/20 mb-8 font-mono">
            <div>
              <span class="text-xs text-[#b5bac1]">Position finale x(t)</span>
              <div class="text-3xl text-white">{{ computedSimX.toFixed(2) }} m</div>
            </div>
            <div class="text-right">
              <span class="text-xs text-[#b5bac1]">Vitesse finale v(t)</span>
              <div class="text-3xl text-[#00B578]">{{ computedSimV.toFixed(2) }} m/s</div>
            </div>
          </div>

          <!-- Animation Stage -->
          <div class="h-20 border-b border-[#2b2d31] relative overflow-visible mb-6 bg-[#1e1f22]/50 rounded-t-lg">
            <div class="absolute inset-0 opacity-[0.2]" style="background-image: linear-gradient(90deg, #b5bac1 1px, transparent 1px); background-size: 50px 100%;"></div>
            
            <!-- Véhicule animé -->
            <div 
              class="absolute bottom-[-10px] w-12 h-12 bg-[#00B578] rounded-full shadow-[0_0_20px_rgba(0,181,120,0.6)] flex items-center justify-center text-[#1e1f22] font-bold"
              style="transition: left 0.1s linear"
              :style="{ left: `min(max(${simCurrentXPercent}%, 0%), 95%)` }"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><path d="M12 16v-4m0-4h.01"/></svg>
            </div>
          </div>

          <div class="flex justify-center mb-8">
            <button @click="playSim" :disabled="isPlaying" class="qcm-btn bg-[#00B578] text-[#1e1f22] flex items-center gap-2">
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="currentColor"><path d="M5 3l14 9-14 9V3z"/></svg>
              Jouer l'animation ({{simT}}s)
            </button>
          </div>

          <!-- Graphes SVG -->
          <div class="grid md:grid-cols-2 gap-6">
            <div class="bg-[#1e1f22] p-4 rounded-xl border border-[#2b2d31]">
              <p class="text-xs uppercase text-center mb-2 font-bold text-white/80">Graphique x(t)</p>
              <svg viewBox="0 -10 100 120" class="w-full h-32 overflow-visible">
                <line x1="0" y1="100" x2="100" y2="100" stroke="#b5bac1" stroke-width="1"/>
                <line x1="0" y1="0" x2="0" y2="100" stroke="#b5bac1" stroke-width="1"/>
                <path :d="pathDataX" fill="none" class="stroke-[#00B578]" stroke-width="2" />
              </svg>
            </div>
            <div class="bg-[#1e1f22] p-4 rounded-xl border border-[#2b2d31]">
              <p class="text-xs uppercase text-center mb-2 font-bold text-white/80">Graphique v(t)</p>
              <svg viewBox="0 -10 100 120" class="w-full h-32 overflow-visible">
                <line x1="0" y1="100" x2="100" y2="100" stroke="#b5bac1" stroke-width="1"/>
                <line x1="0" y1="0" x2="0" y2="100" stroke="#b5bac1" stroke-width="1"/>
                <path :d="pathDataV" fill="none" class="stroke-[#00B578]" stroke-width="2" />
              </svg>
            </div>
          </div>

        </div>
      </div>
    </section>

    <!-- Quiz (QCM) -->
    <section id="qcm" class="part-header visible" ref="qcmRef">
      <span class="part-number text-[#00B578] border-[#00B578]/40 bg-[#00B578]/5">TEST FINAL</span>
      <h2>QCM - L'Épreuve de Cinématique</h2>
      <p class="part-translation">Testez vos connaissances en 20 questions</p>
    </section>

    <section class="text-content-section visible">
      <div class="section-container pb-10">
        <div class="qcm-card">
          <div v-if="!quizCompleted">
            <div class="qcm-header">
              <span class="qcm-progress">Question {{ currentQuestionIndex + 1 }} / {{ quizQuestions.length }}</span>
              <span class="qcm-score">Score: {{ score }}</span>
            </div>
            
            <h3 class="qcm-question text-xl">{{ quizQuestions[currentQuestionIndex].question }}</h3>
            
            <div class="qcm-options">
              <button 
                v-for="(option, index) in quizQuestions[currentQuestionIndex].options" 
                :key="index"
                class="qcm-option"
                :class="{ 
                  'selected': selectedOption === option,
                  'correct': showExplanation && option === quizQuestions[currentQuestionIndex].answer,
                  'wrong': showExplanation && selectedOption === option && option !== quizQuestions[currentQuestionIndex].answer
                }"
                @click="selectOption(option)"
                :disabled="showExplanation"
              >
                <span class="option-letter">{{ ['A', 'B', 'C', 'D'][index] }}</span>
                <span class="option-text">{{ option }}</span>
              </button>
            </div>
            
            <Transition name="fade">
              <div v-if="showExplanation" class="qcm-explanation mt-6" :class="isCorrect ? 'bg-[#00B578]/10 border-[#00B578]/30' : 'bg-red-500/10 border-red-500/30'">
                <h4 :class="isCorrect ? 'text-[#00B578]' : 'text-red-400'">
                  {{ isCorrect ? '✅ Bonne réponse !' : '❌ Mauvaise réponse.' }}
                </h4>
              </div>
            </Transition>
            
            <div class="qcm-actions mt-6">
              <button 
                v-if="!showExplanation" 
                class="qcm-btn validate-btn" 
                @click="validateQuizAnswer" 
                :disabled="!selectedOption"
              >
                Valider
              </button>
              <button 
                v-else 
                class="qcm-btn next-btn" 
                @click="nextQuestion"
              >
                {{ currentQuestionIndex < quizQuestions.length - 1 ? 'Question suivante' : 'Voir le résultat' }}
              </button>
            </div>
          </div>
          
          <div v-else class="qcm-result">
            <div class="text-6xl mb-4 text-center">🏆</div>
            <h3 class="text-2xl font-bold text-white mb-2 text-center">Quiz terminé !</h3>
            <p class="text-xl text-[#b5bac1] mb-6 text-center">Votre score final est de <span class="text-[#00B578] font-bold">{{ score }}</span> sur {{ quizQuestions.length }}.</p>
            <div class="flex justify-center">
              <button class="qcm-btn validate-btn" @click="resetQuiz">Recommencer le quiz</button>
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
  title: 'Cinématique : Mouvements, Vitesses et Accélérations',
  description: 'Apprenez la cinématique avec des exercices interactifs, des graphiques dynamiques et un simulateur complet MRU/MRUA.'
})

const pageRef = ref<HTMLElement | null>(null)
const tocOpen = ref(false)
const activeSection = ref('theorie')

const tocItems = [
  { id: 'theorie', label: '1. Définition & Théorie', isPart: false, isSub: false },
  { id: 'mouvements', label: '2. Types de mouvements', isPart: true, isSub: false },
  { id: 'exercices', label: '3. Exercices Pratiques', isPart: true, isSub: false },
  { id: 'simulateur', label: '4. Simulateur Cinématique', isPart: true, isSub: false },
  { id: 'qcm', label: '5. Quiz : Testez-vous', isPart: true, isSub: false },
]

function scrollToSection(id: string) {
  const el = document.getElementById(id)
  if (el) { el.scrollIntoView({ behavior: 'smooth', block: 'start' }); tocOpen.value = false; }
}

// === EXERCISE LOGIC ===
type ExRes = { text: string, exact: boolean, close: boolean } | null;

function evaluateAnswer(userAns: number | string | undefined, correctAns: number): ExRes {
  if (userAns === undefined || userAns === null || userAns === '') return null;
  const val = Number(userAns);
  if (isNaN(val)) return null;
  
  if (val === correctAns) return { text: "✅ Correct !", exact: true, close: true }
  
  const diff = Math.abs(val - correctAns);
  if (diff <= 0.05 * Math.abs(correctAns)) {
    return { text: "⚠️ Proche ! (Erreur < 5%)", exact: false, close: true }
  }
  return { text: "❌ Incorrect", exact: false, close: false }
}

function bgForRes(res: ExRes) {
  if (!res) return ''
  if (res.exact) return 'bg-[#00B578]/10 border-[#00B578]/50 text-[#00B578]'
  if (res.close) return 'bg-yellow-500/10 border-yellow-500/50 text-yellow-400'
  return 'bg-rose-500/10 border-rose-500/50 text-rose-400'
}

const ex1Ans = ref<number>()
const ex1Res = ref<ExRes>(null)
const ex2Ans = ref<number>()
const ex2Res = ref<ExRes>(null)
const ex3Ans = ref<number>()
const ex3Res = ref<ExRes>(null)
const ex4Ans = ref<number>()
const ex4Res = ref<ExRes>(null)

function evalEx(num: number, ans: any, correct: number) {
  if (num === 1) ex1Res.value = evaluateAnswer(ans, correct)
  if (num === 2) ex2Res.value = evaluateAnswer(ans, correct)
  if (num === 3) ex3Res.value = evaluateAnswer(ans, correct)
  if (num === 4) ex4Res.value = evaluateAnswer(ans, correct)
}

// === GLOBAL SIMULATOR LOGIC ===
const simX0 = ref(0)
const simV0 = ref(0)
const simA = ref(2)
const simT = ref(4)

const isPlaying = ref(false)
const currentTime = ref(0)

const computedSimX = computed(() => {
  const t = simT.value || 0
  return (simX0.value || 0) + ((simV0.value || 0) * t) + (0.5 * (simA.value || 0) * t * t)
})
const computedSimV = computed(() => {
  return (simV0.value || 0) + ((simA.value || 0) * (simT.value || 0))
})

const currentRealX = computed(() => {
  const t = currentTime.value || 0
  return (simX0.value || 0) + ((simV0.value || 0) * t) + (0.5 * (simA.value || 0) * t * t)
})

const simCurrentXPercent = computed(() => {
  const maxAbsX = Math.max(Math.abs(computedSimX.value), 1)
  const mapped = (currentRealX.value / maxAbsX) * 100
  return mapped;
})

let animInterval: any;
function playSim() {
  if (isPlaying.value) return;
  isPlaying.value = true;
  currentTime.value = 0;
  
  const steps = 60; 
  const durationMs = 3000;
  const frameTime = durationMs / steps;
  const timeStep = (simT.value || 1) / steps;

  animInterval = setInterval(() => {
    currentTime.value += timeStep;
    if (currentTime.value >= (simT.value || 1)) {
      currentTime.value = simT.value || 1;
      isPlaying.value = false;
      clearInterval(animInterval);
    }
  }, frameTime)
}

const pathDataX = computed(() => {
  const x0 = Number(simX0.value) || 0, v0 = Number(simV0.value) || 0, a = Number(simA.value) || 0, tmax = Number(simT.value) || 1
  let d = ""
  let maxV = -Infinity, minV = Infinity;
  for (let i = 0; i <= 10; i++) {
    const t = (i/10) * tmax;
    const y = x0 + v0*t + 0.5*a*t*t;
    if (y > maxV) maxV = y;
    if (y < minV) minV = y;
  }
  const range = (maxV - minV) || 1;
  const scale = 100 / range;
  
  for (let i = 0; i <= 20; i++) {
    const t = (i/20) * tmax;
    const px = (t / tmax) * 100;
    const realY = x0 + v0*t + 0.5*a*t*t;
    const py = 100 - ((realY - minV) * scale);
    if (i === 0) d += `M ${px},${py} `
    else d += `L ${px},${py} `
  }
  return d
})

const pathDataV = computed(() => {
  const v0 = Number(simV0.value) || 0, a = Number(simA.value) || 0, tmax = Number(simT.value) || 1
  const endV = v0 + a*tmax;
  const minV = Math.min(v0, endV, 0)
  const fullRange = Math.max(v0, endV) - minV || 1;
  
  const scale = 100 / fullRange;
  const yStart = 100 - ((v0 - minV) * scale);
  const yEnd = 100 - ((endV - minV) * scale);
  
  return `M 0,${yStart} L 100,${yEnd}`;
})

// === QCM LOGIC ===
const quizQuestions = [
  { "question": "Qu'est-ce que la cinématique ?", "options": ["Étude des forces", "Description du mouvement", "Étude de l'énergie", "Étude de la masse"], "answer": "Description du mouvement" },
  { "question": "Que mesure la position ?", "options": ["La vitesse", "La localisation", "L'accélération", "Le temps"], "answer": "La localisation" },
  { "question": "Formule de la vitesse moyenne ?", "options": ["v = at", "v = d/t", "v = t/d", "v = a/t"], "answer": "v = d/t" },
  { "question": "MRU signifie ?", "options": ["Mouvement rapide uniforme", "Mouvement rectiligne uniforme", "Mouvement ralenti uniforme", "Mouvement rotatif uniforme"], "answer": "Mouvement rectiligne uniforme" },
  { "question": "Dans un MRU, l'accélération est :", "options": ["Positive", "Négative", "Nulle", "Variable"], "answer": "Nulle" },
  { "question": "Formule du MRU ?", "options": ["x = vt", "x = x0 + vt", "x = at²", "x = v²"], "answer": "x = x0 + vt" },
  { "question": "Dans un MRUA, l'accélération est :", "options": ["Variable", "Nulle", "Constante", "Infinie"], "answer": "Constante" },
  { "question": "Formule vitesse MRUA ?", "options": ["v = vt", "v = v0 + at", "v = at²", "v = x/t"], "answer": "v = v0 + at" },
  { "question": "Formule position MRUA ?", "options": ["x = x0 + vt", "x = x0 + v0t + 1/2 at²", "x = at", "x = vt²"], "answer": "x = x0 + v0t + 1/2 at²" },
  { "question": "La distance est :", "options": ["Toujours négative", "Toujours positive", "Toujours nulle", "Variable"], "answer": "Toujours positive" },
  { "question": "La vitesse vectorielle inclut :", "options": ["La masse", "La direction", "La température", "La pression"], "answer": "La direction" },
  { "question": "Accélération signifie :", "options": ["Changer position", "Changer vitesse", "Changer masse", "Changer direction seulement"], "answer": "Changer vitesse" },
  { "question": "Un MRU a :", "options": ["Accélération constante", "Accélération nulle", "Vitesse variable", "Trajectoire courbe"], "answer": "Accélération nulle" },
  { "question": "Une trajectoire rectiligne est :", "options": ["Courbe", "Droite", "Circulaire", "Spirale"], "answer": "Droite" },
  { "question": "Si v augmente, on a :", "options": ["Décélération", "Accélération", "Repos", "MRU"], "answer": "Accélération" },
  { "question": "La vitesse s'exprime en :", "options": ["m", "s", "m/s", "kg"], "answer": "m/s" },
  { "question": "L'accélération s'exprime en :", "options": ["m/s", "m/s²", "kg", "N"], "answer": "m/s²" },
  { "question": "Distance parcourue est :", "options": ["Vecteur", "Scalaire", "Force", "Accélération"], "answer": "Scalaire" },
  { "question": "Un mouvement circulaire a :", "options": ["Direction constante", "Direction variable", "Vitesse nulle", "Accélération nulle"], "answer": "Direction variable" },
  { "question": "Le temps mesure :", "options": ["La distance", "Un intervalle", "Une force", "Une masse"], "answer": "Un intervalle" }
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

function validateQuizAnswer() {
  if (!selectedOption.value) return
  const correct = quizQuestions[currentQuestionIndex.value].answer
  isCorrect.value = selectedOption.value === correct
  if (isCorrect.value) {
    score.value++
  }
  showExplanation.value = true
}

function nextQuestion() {
  if (currentQuestionIndex.value < quizQuestions.length - 1) {
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

// === INTERSECTION (TOC) ===
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
    clearInterval(animInterval)
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

.exigences-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 1rem; }
.exig-card { border-radius: 1.25rem; padding: 1.75rem; background: rgba(43, 45, 49, 0.5); display: flex; flex-direction: column; gap: 1rem; backdrop-filter: blur(8px); }
.exig-icon { font-size: 2rem; font-weight: 800; }

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
.qcm-actions { display: flex; justify-content: flex-end; }
.qcm-btn { padding: 0.75rem 2rem; border-radius: 999px; font-weight: 700; font-size: 1.05rem; transition: all 0.2s; border: none; cursor: pointer; }
.qcm-btn:disabled { opacity: 0.5; cursor: not-allowed; }
.validate-btn { background: #00B578; color: #1e1f22; }
.validate-btn:hover:not(:disabled) { background: #00c785; transform: translateY(-2px); }
.next-btn { background: #00B578; color: #1e1f22; }
.next-btn:hover { background: #00c785; transform: translateY(-2px); }

.bottom-line { height: 6px; }

/* ANIMATIONS */
.animate-pulse-slow { animation: pulse 8s cubic-bezier(0.4, 0, 0.6, 1) infinite; }
@keyframes pulse { 0%, 100% { opacity: 0.1; } 50% { opacity: 0.2; } }
@keyframes fadeInDown { from { opacity: 0; transform: translateY(-20px); } to { opacity: 1; transform: translateY(0); } }
@keyframes draw { to { stroke-dashoffset: 0; } }
.fade-enter-active, .fade-leave-active { transition: opacity 0.3s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }
</style>
