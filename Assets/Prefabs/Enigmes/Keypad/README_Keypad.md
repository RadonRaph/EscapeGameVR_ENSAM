# Énigme : Keypad

Le joueur tape un code sur un clavier, puis appuie sur **OK**. Si le code est bon, la porte s'ouvre.

## Le prefab

| Objet | Rôle |
|---|---|
| `Keypad` | Le clavier. Script `Keypad` (à compléter). |
| `Touche_0` à `Touche_9`, `Touche_C`, `Touche_OK` | Les touches. Script `KeypadButton` : il envoie sa valeur au keypad quand on appuie dessus. |
| `Affichage` | Le texte de l'écran, qui montre les chiffres tapés. |

## Installation

1. Glissez `Keypad.prefab` dans la scène, contre un mur ou sur un socle, **flèche bleue vers le joueur**.
2. Dans le script `Keypad` :
   - `Code` : le code à trouver (ex : `4071`).
   - `Door To Open` : glissez la porte à ouvrir.
3. Cachez le code dans la salle : sur un papier, au dos d'un tableau, en chiffres lumineux...

Pour appuyer sur une touche : avec le **doigt** (index tendu) ou en visant avec le **rayon** puis gâchette.

## Le code à écrire

Ouvrez `Scripts/TP/Keypad.cs`. Chaque touche appelle la fonction `PressKey(string key)` avec sa valeur : `"0"` à `"9"`, `"C"` ou `"OK"`.

| TODO | À faire |
|---|---|
| 1 | Si `key` vaut `"C"` : vider `typed` (`typed = "";`). |
| 2 | Si `key` vaut `"OK"` : comparer `typed` avec `code`. Bon code → `Solve();`, sinon vider `typed`. |
| 3 | Sinon (un chiffre) : ajouter `key` à la fin de `typed`. |
| 4 | Afficher `typed` dans l'écran : `display.text = typed;` |

> **Astuce** : en C#, on compare deux textes avec `==`, et on colle deux textes avec `+`.
> Pour enchaîner les cas, utilisez `if (...) { } else if (...) { } else { }`.

## Tester

Scène `EscapeGame_Enigmes`, keypad de gauche (code `1234`). La Console affiche `Touche : ...` à chaque appui, puis `Énigme résolue : Keypad`.

## Pour aller plus loin

- Limiter le code à 4 chiffres.
- Afficher `ERREUR` en rouge quelques secondes si le code est faux (changer `display.color`).
