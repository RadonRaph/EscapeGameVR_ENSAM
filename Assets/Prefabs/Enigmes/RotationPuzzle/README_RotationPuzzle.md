# Énigme : Molettes

Trois molettes portent chacune les symboles A, B, C et D. Chaque appui fait tourner une molette d'un cran. L'énigme est résolue quand chaque molette montre le bon symbole **en haut**, sous le repère rouge.

## Le prefab

| Objet | Rôle |
|---|---|
| `Molettes` | Le panneau. Script `RotationPuzzle` (à compléter). |
| `Molette_0`, `Molette_1`, `Molette_2` | Les molettes, de gauche à droite. Script `RotatingDial`. |

Chaque `RotatingDial` a un `Current Index` : le numéro du symbole en haut (0 = A, 1 = B, 2 = C, 3 = D). À chaque cran, la molette appelle `OnDialTurned()` sur le `RotationPuzzle`.

## Installation

1. Glissez `Molettes.prefab` contre un mur ou sur un socle, **flèche bleue vers le joueur**.
2. Dans le script `RotationPuzzle` :
   - `Dials` : déjà rempli avec les 3 molettes.
   - `Solution` : le symbole attendu pour chaque molette (ex : `1, 3, 2` = B, D, C).
   - `Door To Open` : glissez la porte à ouvrir.
3. Cachez la solution dans la salle : trois tableaux avec une lettre, des livres de couleur...

Pour tourner une molette : avec le **doigt** ou en visant avec le **rayon** puis gâchette.

## Le code à écrire

Ouvrez `Scripts/TP/RotationPuzzle.cs`. La fonction `OnDialTurned()` doit vérifier si **toutes** les molettes sont sur le bon symbole.

| TODO | À faire |
|---|---|
| 1 | Une boucle `for` sur toutes les molettes : `for (int i = 0; i < dials.Length; i++) { }` |
| 2 | Dans la boucle : si `dials[i].currentIndex` est différent de `solution[i]`, ce n'est pas bon : `return;` |
| 3 | Après la boucle : toutes les molettes sont bonnes, `Solve();` |

> **Astuce** : `dials[i]` est la molette numéro `i`, et `solution[i]` le symbole attendu pour elle. Les deux tableaux doivent avoir la même taille.

## Tester

Scène `EscapeGame_Enigmes`, troisième énigme (solution `B D C`). Tournez les molettes : la grille s'ouvre quand les trois lettres sont bonnes.

## Pour aller plus loin

- Mettre 4 molettes, ou 6 symboles par molette (`Symbol Count`).
- Remplacer les lettres par des symboles ou des couleurs.
