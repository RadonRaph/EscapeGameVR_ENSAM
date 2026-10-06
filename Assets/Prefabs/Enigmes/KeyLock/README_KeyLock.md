# Énigme : Clé et serrure

Le joueur trouve une clé, la prend en main et l'approche de la serrure. Si c'est la bonne clé, elle se fixe dans la serrure et la porte s'ouvre.

## Les prefabs

| Prefab | Rôle |
|---|---|
| `Cle` | La clé. On peut l'attraper (`XR Grab Interactable`). Script `Key` : son nom est dans `Key Name`. |
| `Serrure` | La serrure. Script `KeyLock` (à compléter). Son Box Collider est un **trigger** : il détecte la clé. |

L'enfant `EmplacementCle` de la serrure est l'endroit où la clé se place une fois insérée.

## Installation

1. Glissez `Serrure.prefab` contre une porte ou un coffre, **flèche bleue vers le joueur**.
2. Dans le script `KeyLock` :
   - `Key Name` : le nom de la clé qui ouvre (ex : `Clé rouge`).
   - `Key Slot` : déjà rempli avec `EmplacementCle`.
   - `Door To Open` : glissez la porte à ouvrir.
3. Glissez `Cle.prefab` et cachez-la dans la salle (dans un tiroir, derrière un meuble...).
4. Sur la clé, vérifiez que `Key Name` est **exactement** le même texte que dans la serrure.

Bonus facile : faites plusieurs clés de couleurs différentes (changez le matériau et le `Key Name`), une seule est la bonne.

## Le code à écrire

Ouvrez `Scripts/TP/KeyLock.cs`. La fonction `OnTriggerEnter(Collider other)` est appelée par Unity quand un objet entre dans le trigger de la serrure. `other` est le collider de cet objet.

| TODO | À faire |
|---|---|
| 1 | Récupérer le script `Key` de l'objet : `Key key = other.GetComponentInParent<Key>();` Si `key` vaut `null`, ce n'est pas une clé : `return;` |
| 2 | Si `key.keyName` est différent (`!=`) de `keyName` : afficher `Ce n'est pas la bonne clé` (`Debug.Log`) puis `return;` |
| 3 | C'est la bonne clé : `key.InsertInto(keySlot);` puis `Solve();` |

> **Astuce** : `return;` arrête la fonction tout de suite. C'est pratique pour éliminer les mauvais cas au début.

## Tester

Scène `EscapeGame_Enigmes`, deuxième énigme : la clé rouge est sur la petite table à côté. Approchez-la de la serrure : elle se fixe et la grille s'ouvre.
