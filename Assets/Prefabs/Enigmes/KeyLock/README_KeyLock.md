# Énigme : Clé et serrure

Le joueur trouve une clé, la prend en main et la pose dans la serrure. Si c'est la bonne clé, la porte s'ouvre.

## Les prefabs

| Prefab | Rôle |
|---|---|
| `Cle_Rouge` | La clé. On peut l'attraper (`XR Grab Interactable`). Son **nom** sert à la reconnaître. |
| `Serrure` | La serrure. Script `KeyLock` (à compléter). Son enfant `Socket` est un `XR Socket Interactor` : un emplacement qui attire et retient l'objet qu'on y pose. |

Sélectionnez `Serrure/Socket` : dans `XR Socket Interactor` → `Interactor Events` → `Select Entered`, vous voyez `KeyLock.OnKeyInserted`. Le socket appelle cette fonction à chaque fois qu'un objet y est posé.

## Installation

1. Glissez `Serrure.prefab` contre une porte ou un coffre, **flèche bleue vers le joueur**.
2. Dans le script `KeyLock` :
   - `Key Name` : le nom de l'objet clé qui ouvre (ex : `Cle_Rouge`).
   - `On Solved` : `+`, glissez la `Grille` de la porte, `GameObject` → `SetActive`, case **décochée**.
3. Glissez `Cle_Rouge.prefab` et cachez-la dans la salle (dans un tiroir, derrière un meuble...).
4. Dans la Hierarchy, le nom de la clé doit être **exactement** `Key Name`. Attention : une clé dupliquée s'appelle `Cle_Rouge (1)`, renommez-la.

Bonus facile : faites plusieurs clés de couleurs différentes (autre matériau, autre nom), une seule est la bonne. Une mauvaise clé reste dans la serrure : le joueur peut la reprendre.

## Le code à écrire

Ouvrez `Scripts/TP/KeyLock.cs`. La fonction `OnKeyInserted(SelectEnterEventArgs args)` est appelée par le socket. La variable `key` (déjà écrite) est l'objet qui vient d'être posé.

| TODO | À faire |
|---|---|
| 1 | Si `key.name` est différent (`!=`) de `keyName` : afficher `Ce n'est pas la bonne clé` (`Debug.Log`) puis `return;` |
| 2 | C'est la bonne clé : `onSolved.Invoke();` |

> **Astuce** : `return;` arrête la fonction tout de suite. C'est pratique pour éliminer les mauvais cas au début.

## Tester

Scène `EscapeGame_Enigmes`, deuxième énigme : la clé rouge est sur la petite table à côté. Approchez-la de la serrure et lâchez-la : elle se place dans la serrure et la grille s'ouvre.
