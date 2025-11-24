-- Script pour ajouter des matières par défaut
-- Exécutez ce script si votre base de données est vide

-- Supprimer les matières existantes (optionnel)
-- DELETE FROM Subjects;

-- Ajouter les matières
INSERT OR IGNORE INTO Subjects (Id, Label) VALUES 
(1, 'Mathématiques'),
(2, 'Physique'),
(3, 'Chimie'),
(4, 'Biologie');

-- Vérifier
SELECT * FROM Subjects;

