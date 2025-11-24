# VideoPlayer Component - Formats supportés

## ✅ Formats vidéo supportés

### 1. **YouTube**
Le composant détecte automatiquement les URLs YouTube et affiche un iframe YouTube.

**Formats acceptés :**
- `https://www.youtube.com/watch?v=VIDEO_ID`
- `https://youtu.be/VIDEO_ID`
- `https://youtube.com/embed/VIDEO_ID`
- `https://youtube.com/v/VIDEO_ID`

**Exemple :**
```html
<VideoPlayer src="https://youtu.be/2kbAId70b_I" />
<VideoPlayer src="https://www.youtube.com/watch?v=2kbAId70b_I" />
```

### 2. **HLS (HTTP Live Streaming)**
Pour les vidéos hébergées avec des fichiers `.m3u8`

**Format accepté :**
- URLs se terminant par `.m3u8`

**Exemple :**
```html
<VideoPlayer src="https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8" />
```

## 🔧 Détection automatique

Le composant détecte automatiquement le type de vidéo :
- Si l'URL contient `youtube.com` ou `youtu.be` → Player YouTube
- Sinon → Player HLS

## 🐛 Debug

Si une vidéo YouTube ne s'affiche pas :
1. Vérifiez que l'URL est bien formatée
2. Ouvrez la console navigateur (F12) pour voir les erreurs
3. Vérifiez que l'ID vidéo a bien 11 caractères

## 📝 Notes

- Les vidéos YouTube sont chargées via iframe (pas de HLS)
- Les vidéos HLS utilisent la bibliothèque `hls.js`
- Safari supporte HLS nativement sans `hls.js`

