# Énigme : Molettes

Trois molettes portent chacune les symboles A, B, C et D. Chaque appui fait tourner une molette d'un cran. L'énigme est résolue quand chaque molette montre le bon symbole **en haut**, sous le repère rouge.

## Le prefab

| Objet | Rôle |
|---|---|
| `Molettes` | Le panneau. Script `RotationPuzzle` (à compléter). |
| `Molette_0`, `Molette_1`, `Molette_2` | Les molettes, de gauche à droite. Chacune a un `XR Simple Interactable` dont `Select Entered` appelle `RotationPuzzle.TurnDial` avec son numéro. |

Sélectionnez `Molette_1` : dans `XR Simple Interactable` → `Interactable Events` → `Select Entered`, vous voyez `RotationPuzzle.TurnDial` avec la valeur `1`.

## Installation

1. Glissez `Molettes.prefab` contre un mur ou sur un socle, **flèche bleue vers le joueur**.
2. Dans le script `RotationPuzzle` :
   - `Dials` : déjà rempli avec les 3 molettes.
   - `Solution` : le symbole attendu pour chaque molette (ex : `1, 3, 2` = B, D, C).
   - `On Solved` : `+`, glissez la `Grille` de la porte, `GameObject` → `SetActive`, case **décochée**.
3. Cachez la solution dans la salle : trois tableaux avec une lettre, des livres de couleur...

Pour tourner une molette : avec le **doigt** ou en visant avec le **rayon** puis gâchette.

## Le code à écrire

Ouvrez `Scripts/TP/RotationPuzzle.cs`. Le début de `TurnDial` (déjà écrit) tourne la molette et met à jour `current` : `current[i]` est le symbole en haut de la molette `i` (0 = A, 1 = B...). À vous de vérifier si **toutes** les molettes sont bonnes.

| TODO | À faire |
|---|---|
| 1 | Une boucle `for` sur toutes les molettes : `for (int i = 0; i < dials.Length; i++) { }` |
| 2 | Dans la boucle : si `current[i]` est différent de `solution[i]`, ce n'est pas bon : `return;` |
| 3 | Après la boucle : toutes les molettes sont bonnes, `onSolved.Invoke();` |

> **Astuce** : `current[i]` est le symbole de la molette numéro `i`, et `solution[i]` le symbole attendu pour elle. Les deux tableaux ont la même taille.

## Tester

Scène `EscapeGame_Enigmes`, troisième énigme (solution `B D C`). Tournez les molettes : la grille s'ouvre quand les trois lettres sont bonnes.

## Pour aller plus loin

- Ajouter une 4e molette : dupliquez une molette, changez la valeur de son `Select Entered` (3) et ajoutez-la à `Dials` et `Solution`.
- Remplacer les lettres par des symboles ou des couleurs.
