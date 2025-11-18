# Dossier Watermark

Ce dossier contient les images de filigrane (watermark) utilisées pour le streaming HLS.

## Configuration

Le fichier par défaut doit s'appeler `default.png` et sera automatiquement appliqué sur toutes les vidéos en streaming.

### Format recommandé
- Format : PNG avec transparence
- Taille recommandée : 200x50 pixels (ou similaire)
- Position : Coin inférieur droit avec 10px de marge
- Transparence : 60% (configuré dans le code)

### Personnalisation

Pour changer le filigrane, remplacez simplement le fichier `default.png` par votre propre image.

Pour créer un filigrane simple avec ImageMagick :
```bash
magick -size 200x50 xc:none -pointsize 30 -fill "rgba(255,255,255,0.8)" -annotate +10+35 "KitabStock" default.png
```

Ou utilisez un outil en ligne comme :
- Canva (https://www.canva.com)
- Photopea (https://www.photopea.com)
- GIMP (logiciel gratuit)

### Note
Si le fichier `default.png` n'existe pas, les vidéos seront générées sans filigrane.

