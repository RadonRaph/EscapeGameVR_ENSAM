# Énigme : Cible

Le joueur doit toucher une cible plusieurs fois en lançant des objets. Au bout de 3 touches, la porte s'ouvre.

## Les prefabs

| Prefab | Rôle |
|---|---|
| `Cible` | La cible. Script `ThrowTarget` (à compléter). L'enfant `Compteur` affiche le nombre de touches. |
| `Projectile` | Une balle à lancer. Scripts `XR Grab Interactable` et `Throwable` (seuls les objets `Throwable` comptent). |

Pour lancer : attraper la balle, faire le geste du lancer et **lâcher** la gâchette pendant le mouvement.

## Installation

1. Glissez `Cible.prefab` contre un mur, **flèche bleue vers le joueur**, à 2 ou 3 m de l'endroit où le joueur se tient.
2. Dans le script `ThrowTarget` :
   - `Hits Needed` : le nombre de touches (3).
   - `Min Speed` : la vitesse minimum d'un lancer (2). Un objet simplement posé sur la cible ne compte pas.
   - `Counter` : déjà rempli avec le texte `Compteur`.
   - `Door To Open` : glissez la porte à ouvrir.
3. Glissez quelques `Projectile` sur une table. Si une balle tombe hors du niveau, elle revient à sa place (`Reset If Fallen`).

## Le code à écrire

Ouvrez `Scripts/TP/ThrowTarget.cs`. La fonction `OnCollisionEnter(Collision collision)` est appelée par Unity quand un objet percute la cible.

| TODO | À faire |
|---|---|
| 1 | Si l'objet n'a pas de script `Throwable` (`collision.gameObject.GetComponent<Throwable>() == null`) : `return;` |
| 2 | Si la vitesse du choc `collision.relativeVelocity.magnitude` est plus petite que `minSpeed` : `return;` |
| 3 | Ajouter 1 à `hits`, puis afficher le compteur : `counter.text = hits + " / " + hitsNeeded;` |
| 4 | Si `hits` est plus grand ou égal à `hitsNeeded` : `Solve();` |

> **Astuce** : `return;` arrête la fonction tout de suite. On élimine d'abord les mauvais cas, et le code qui reste ne concerne que les bons lancers.

## Tester

Scène `EscapeGame_Enigmes`, énigme de droite : les 3 balles sont sur la table à 3 m de la cible.

## Pour aller plus loin

- Faire clignoter la cible à chaque touche (changer la couleur du matériau quelques instants).
- Une cible qui bouge (animation de gauche à droite).
