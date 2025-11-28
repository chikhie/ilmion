<template>
  <div class="min-h-screen bg-gray-900">
    <!-- Header -->
    <header class="bg-gray-800 shadow-sm border-b border-gray-700">
      <div class="container mx-auto px-4 py-4">
        <NuxtLink to="/" class="inline-flex items-center text-gray-300 hover:text-white transition-colors">
          ← Retour à l'accueil
        </NuxtLink>
      </div>
    </header>

    <div class="container mx-auto px-4 py-12">
      <!-- Hero -->
      <div class="text-center mb-16">
        <h1 class="text-4xl md:text-5xl font-bold text-white mb-4">
          Comment ça fonctionne ?
        </h1>
        <p class="text-xl text-gray-400 max-w-2xl mx-auto">
          Découvrez notre structure d'apprentissage simple et efficace
        </p>
      </div>

      <!-- Structure Visuelle -->
      <div class="max-w-4xl mx-auto mb-16">
        <div class="bg-gray-800 rounded-lg border border-gray-700 p-8">
          <h2 class="text-2xl font-bold text-white mb-8 text-center">Structure des cours</h2>
          
          <!-- Level 1: Matières -->
          <div class="mb-6">
            <div class="flex items-center mb-4">
              <div class="w-12 h-12 bg-blue-600 rounded-lg flex items-center justify-center text-2xl mr-4">
                📚
              </div>
              <div>
                <h3 class="text-xl font-bold text-white">1. Matières</h3>
                <p class="text-gray-400 text-sm">Mathématiques, Physique, Chimie, Biologie...</p>
              </div>
            </div>
            
            <!-- Arrow -->
            <div class="ml-6 mb-4">
              <div class="text-gray-600 text-2xl">↓</div>
            </div>

            <!-- Level 2: Modules -->
            <div class="ml-12 mb-6">
              <div class="flex items-center mb-4">
                <div class="w-12 h-12 bg-green-600 rounded-lg flex items-center justify-center text-2xl mr-4">
                  📖
                </div>
                <div>
                  <h3 class="text-xl font-bold text-white">2. Modules</h3>
                  <p class="text-gray-400 text-sm">Regroupement de chapitres sur un thème</p>
                </div>
              </div>
              
              <!-- Arrow -->
              <div class="ml-6 mb-4">
                <div class="text-gray-600 text-2xl">↓</div>
              </div>

              <!-- Level 3: Chapitres -->
              <div class="ml-12 mb-6">
                <div class="flex items-center mb-4">
                  <div class="w-12 h-12 bg-purple-600 rounded-lg flex items-center justify-center text-2xl mr-4">
                    📝
                  </div>
                  <div>
                    <h3 class="text-xl font-bold text-white">3. Chapitres</h3>
                    <p class="text-gray-400 text-sm">Leçon complète sur un sujet spécifique</p>
                  </div>
                </div>
                
                <!-- Arrow -->
                <div class="ml-6 mb-4">
                  <div class="text-gray-600 text-2xl">↓</div>
                </div>

                <!-- Level 4: Sections -->
                <div class="ml-12">
                  <div class="flex items-center">
                    <div class="w-12 h-12 bg-orange-600 rounded-lg flex items-center justify-center text-2xl mr-4">
                      ✨
                    </div>
                    <div>
                      <h3 class="text-xl font-bold text-white">4. Sections</h3>
                      <p class="text-gray-400 text-sm">Contenu : vidéos, textes, quiz, exercices...</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Exemple Concret -->
      <div class="max-w-4xl mx-auto mb-16">
        <h2 class="text-2xl font-bold text-white mb-6 text-center">Exemple concret</h2>
        <div class="bg-gray-800 rounded-lg border border-gray-700 p-8">
          <div class="space-y-4">
            <div class="flex items-center">
              <span class="text-blue-400 font-bold mr-3">Matière :</span>
              <span class="text-white">Physique</span>
            </div>
            <div class="flex items-center ml-6">
              <span class="text-green-400 font-bold mr-3">→ Module :</span>
              <span class="text-white">Mécanique</span>
            </div>
            <div class="flex items-center ml-12">
              <span class="text-purple-400 font-bold mr-3">→ Chapitre :</span>
              <span class="text-white">Les lois de Newton</span>
            </div>
            <div class="ml-18 space-y-2 mt-4 border-l-2 border-orange-600 pl-6">
              <div class="text-gray-300">
                <span class="text-orange-400 font-bold">→ Section 1 :</span> Vidéo d'introduction (10 min)
              </div>
              <div class="text-gray-300">
                <span class="text-orange-400 font-bold">→ Section 2 :</span> Explication de la 1ère loi
              </div>
              <div class="text-gray-300">
                <span class="text-orange-400 font-bold">→ Section 3 :</span> Illustration avec schémas
              </div>
              <div class="text-gray-300">
                <span class="text-orange-400 font-bold">→ Section 4 :</span> Quiz de compréhension
              </div>
              <div class="text-gray-300">
                <span class="text-orange-400 font-bold">→ Section 5 :</span> Exercices pratiques
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Types de Sections -->
      <div class="max-w-4xl mx-auto mb-16">
        <h2 class="text-2xl font-bold text-white mb-6 text-center">Types de contenu disponibles</h2>
        <div class="grid md:grid-cols-2 gap-6">
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">🎥</div>
            <h3 class="text-lg font-bold text-white mb-2">Vidéos</h3>
            <p class="text-gray-400 text-sm">Leçons vidéo HD avec explications détaillées</p>
          </div>
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">📄</div>
            <h3 class="text-lg font-bold text-white mb-2">Textes</h3>
            <p class="text-gray-400 text-sm">Paragraphes explicatifs et théorie</p>
          </div>
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">🖼️</div>
            <h3 class="text-lg font-bold text-white mb-2">Illustrations</h3>
            <p class="text-gray-400 text-sm">Schémas, graphiques et images</p>
          </div>
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">❓</div>
            <h3 class="text-lg font-bold text-white mb-2">Quiz</h3>
            <p class="text-gray-400 text-sm">Questions pour tester vos connaissances</p>
          </div>
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">💻</div>
            <h3 class="text-lg font-bold text-white mb-2">Code</h3>
            <p class="text-gray-400 text-sm">Exemples de code et algorithmes</p>
          </div>
          <div class="bg-gray-800 rounded-lg border border-gray-700 p-6">
            <div class="text-3xl mb-3">✍️</div>
            <h3 class="text-lg font-bold text-white mb-2">Exercices</h3>
            <p class="text-gray-400 text-sm">Problèmes à résoudre pour pratiquer</p>
          </div>
        </div>
      </div>

      <!-- CTA -->
      <div class="text-center">
        <h2 class="text-2xl font-bold text-white mb-4">Prêt à commencer ?</h2>
        <p class="text-gray-400 mb-8">Explorez nos matières et commencez votre apprentissage dès maintenant</p>
        <NuxtLink 
          to="/register" 
          class="inline-block px-8 py-3 bg-primary-600 hover:bg-primary-700 text-white rounded-lg font-medium transition-colors"
        >
          Commencer Votre Apprentissage
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
useHead({
  title: 'Comment ça fonctionne'
})
</script>

