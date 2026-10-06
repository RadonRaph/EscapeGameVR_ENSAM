# TP EscapeGameVR – Escape game en réalité virtuelle

## Le projet

Vous allez créer un **escape game en VR** pour le **Meta Quest 3** : le joueur est enfermé, il doit résoudre des énigmes pour ouvrir les portes et passer d'une salle à l'autre.

```
Salle 1 (thème A) → énigme → porte → Salle 2 (thème B) → énigme → sortie
```

## Avant de commencer

- Version de Unity : **6000.3.24f1**, avec le module **Android Build Support**.
- Le projet est déjà configuré pour le Quest 3 (OpenXR, manettes Touch, Android). Vous n'avez rien à régler.
- Ouvrez la scène `Assets/Scenes/EscapeGame_Exemple.unity` et lancez Play : appuyez sur le bouton rouge, la grille s'ouvre et donne sur la station spatiale.
- La scène `EscapeGame_Enigmes` est un **banc d'essai** : les 4 énigmes à compléter, chacune devant une grille. Testez-y votre code.
- Votre scène de travail est `Assets/Scenes/EscapeGame_TP.unity`. Elle contient seulement un sol, une lumière et le joueur.

### Tester sans casque

Dans l'éditeur, quand aucun casque n'est branché, le **XR Interaction Simulator** apparaît automatiquement en Play : il simule le casque et les manettes au clavier et à la souris. Les touches sont rappelées dans la vue Game.

### Tester avec le casque

- **Quest Link** (PC Windows) : branchez le casque, activez Link, puis lancez Play dans Unity.
- **Build sur le casque** : casque branché en USB (mode développeur activé), `File` → `Build Profiles` → `Android` → `Build And Run`.

## Organisation du projet

| Dossier | Contenu |
|---|---|
| `Scripts/TP/` | **Les scripts des énigmes à compléter** : `Keypad`, `KeyLock`, `RotationPuzzle`, `ThrowTarget`. |
| `Scripts/Exemple/` | `SimpleButtonPuzzle` : une énigme complète, à lire pour comprendre. |
| `Scripts/Core/` | Le moteur du TP (pas besoin de le modifier) : `Puzzle`, `Door`, `Key`, `HoverOutline`... |
| `Prefabs/Enigmes/` | Un dossier par énigme : les prefabs et un **README** qui explique comment les installer et quoi coder. |
| `Prefabs/` | Les portes (`Porte_Donjon`, `Porte_Station`) et la `ZoneTeleportation`. |
| `Kenney/` | Les modèles 3D : `FurnitureKit` (meubles), `ModularDungeonKit` (donjon), `SpaceStationKit` (station spatiale). |

Cherchez `TODO` dans le dossier `Scripts/TP` pour trouver le code à écrire.

---

## Comment marche une énigme

Toutes les énigmes héritent de la classe `Puzzle`. Quand le joueur trouve la solution, le script appelle `Solve()` :

- la porte du champ `Door To Open` s'ouvre (elle disparaît) ;
- les actions de `On Solved` sont lancées (une animation, un effet, une lumière...).

| Champ (Inspector) | Rôle |
|---|---|
| `Door To Open` | La porte à ouvrir : glissez l'objet qui a le script `Door` (pour `Porte_Donjon`, c'est l'enfant `Grille`). |
| `On Solved` | Optionnel : `+` puis glissez un objet et choisissez une fonction à appeler. |

Lisez `Scripts/Exemple/SimpleButtonPuzzle.cs` : c'est une énigme complète en 10 lignes.

### Le devant des énigmes

Le devant de chaque prefab d'énigme est du côté de la **flèche bleue** (axe Z). Tournez l'objet pour que la flèche bleue pointe vers le joueur.

---

## Étape 1 : Découverte

Dans `EscapeGame_Exemple` :

1. Lancez Play. Déplacez-vous (téléportation), attrapez l'ours en peluche, appuyez sur le bouton rouge.
2. Sélectionnez `BoutonSimple` et regardez son champ `Door To Open`.
3. Approchez une main d'un objet : il s'entoure d'un contour jaune. C'est le script `HoverOutline`.

## Étape 2 : Compléter au moins une énigme

Choisissez **au moins une** des 4 énigmes fournies. Pour chacune, lisez le README de son dossier :

| Énigme | Dossier | Principe |
|---|---|---|
| Keypad | `Prefabs/Enigmes/Keypad` | Taper le bon code, caché quelque part dans la salle. |
| Clé et serrure | `Prefabs/Enigmes/KeyLock` | Trouver la bonne clé et la mettre dans la serrure. |
| Molettes | `Prefabs/Enigmes/RotationPuzzle` | Tourner des molettes pour aligner les bons symboles. |
| Cible | `Prefabs/Enigmes/ThrowTarget` | Toucher une cible plusieurs fois en lançant des objets. |

Testez votre code dans la scène `EscapeGame_Enigmes` : la grille derrière l'énigme doit s'ouvrir.

## Étape 3 : Vos deux salles

Construisez dans `EscapeGame_TP` **deux salles thématiques** (exemple : un donjon puis une station spatiale, une bibliothèque puis un laboratoire...).

- Chaque salle a une ambiance qu'on reconnaît : modèles, couleurs, lumières.
- Une énigme résolue ouvre la porte vers la salle suivante.
- Pas d'objets qui flottent ou qui se traversent. Testez l'échelle en VR : une porte fait environ 2 m, une table 75 cm.

### Mettre en place la VR dans votre salle

- **Se déplacer** : glissez `Prefabs/ZoneTeleportation` sur le sol, puis agrandissez-la avec l'outil Scale (boîte verte dans la vue Scene).
- **Murs et sols** : les modèles Kenney ont déjà des colliders. Le joueur ne peut pas les traverser.
- **Attraper un objet** : `Add Component` → `XR Grab Interactable` (un Rigidbody est ajouté automatiquement). Ajoutez aussi `Hover Outline` pour le contour.
  - Cochez **Convex** sur ses `Mesh Collider` : ils sont souvent sur les **enfants** du modèle, dépliez-le dans la Hierarchy.
  - Un objet que l'on attrape ne doit **pas** être Static.

## Étape 4 : Deux énigmes différentes

Votre niveau doit contenir **au moins deux énigmes de mécaniques différentes** (deux keypads ne comptent que pour une). Placez un **indice** dans la salle : le code écrit au dos d'un tableau, la couleur de la clé sur un livre...

Idées d'énigmes (sans prefab fourni). Pour chacune, partez de `SimpleButtonPuzzle` : une classe qui hérite de `Puzzle` et appelle `Solve()`.

| Énigme | Principe | Pistes |
|---|---|---|
| Séquence de leviers | Actionner des leviers dans le bon ordre. | Chaque levier est un bouton (`XR Simple Interactable`) qui envoie son numéro, comme `KeypadButton`. |
| Objet à placer | Poser le bon objet sur un socle (une statue, une gemme...). | Un trigger sur le socle et `OnTriggerEnter`, comme `KeyLock`. |
| Combinaison de socles | Plusieurs socles, chacun attend un objet précis. | Plusieurs triggers + un tableau, comme `RotationPuzzle`. |
| Balance | Poser des objets pour atteindre le bon poids. | Additionner le `mass` des `Rigidbody` posés dans un trigger. |
| Mémoire (Simon) | Reproduire une séquence de boutons qui s'allument. | Une coroutine pour allumer les boutons, puis comparer comme le keypad. |

## Étape 5 : Lighting baked

En VR sur un casque autonome, l'éclairage doit être **précalculé** (baked) : c'est beaucoup plus léger que des ombres en temps réel.

1. **Décor fixe** : sélectionnez les murs, sols et meubles qui ne bougent pas, et cochez **Static** en haut de l'Inspector.
   - Les murs, sols et portes reçoivent leur lumière d'une **lightmap** (une texture d'éclairage précalculée).
   - Les petits objets (meubles, accessoires) la reçoivent des **probes** : c'est réglé automatiquement à l'import (`Receive Global Illumination` = `Light Probes`). Ils sont trop petits pour une lightmap propre.
2. **Lumières** : sur chaque lumière, `Mode` = **Baked**.
3. **Probes** : la scène contient déjà un `Adaptive Probe Volume` (mode `Scene`). Il éclaire les objets qui bougent (clé, objets attrapés).
4. **Calcul** : `Window` → `Rendering` → `Lighting` → `Generate Lighting`.

Les modèles Kenney ont déjà leurs UV de lightmap (`Generate Lightmap UVs`). Après chaque grosse modification du décor, relancez `Generate Lighting`.

## Étape 6 : La vidéo

Enregistrez une vidéo de votre niveau du début à la fin, **depuis le casque**, avec l'enregistrement vidéo du menu `Appareil photo` du Quest. Récupérez-la en branchant le casque à l'ordinateur (dossier `Oculus/VideoShots`).

---

## Bonus

- **2e énigme complétée, ou nouveau mécanisme** : une autre énigme fournie, ou une énigme de votre invention.
- **Matériaux / VFX** : un shader, des particules quand une énigme est résolue, des objets émissifs...
- **Animation de feedback** : une porte, un tiroir ou un coffre qui s'ouvre avec une animation (`Animator`), lancée depuis `On Solved`.
- **Fin de partie** : une zone de victoire, un chrono affiché à la sortie, un écran de fin.
- **Idée pertinente / originalité** : un scénario, une mise en scène, une énigme surprenante...

## Barème (/20)

| Critère | Points |
|---|---|
| Setup VR jouable sur Quest 3 (déplacement, saisie d'objets, échelle cohérente) | 2 |
| Compléter 1 prefab fourni (keypad, clé-serrure, molettes ou cible) | 2,5 |
| 2 énigmes de mécaniques différentes, avec un indice dans le niveau | 2,5 |
| 2 salles thématiques reliées : une énigme ouvre la salle suivante | 2,5 |
| Lighting baked (lightmaps + Adaptive Probe Volumes) | 1,5 |
| Vidéo du run complet, enregistrée depuis le Quest 3 | 1 |
| **Bonus** : 2e prefab complété ou nouveau mécanisme | 2 |
| **Bonus** : matériaux / VFX | 2 |
| **Bonus** : idée pertinente / originalité | 2 |
| **Bonus** : animation de feedback | 1 |
| **Bonus** : fin de partie | 1 |

---

## Ça ne marche pas ?

### Une erreur rouge dans la Console

- **On ne peut pas lancer Play tant qu'il reste une erreur rouge.** Double-cliquez sur l'erreur : elle vous emmène à la ligne du problème.
- `error CS1513: } expected` ou `CS1022` : il manque une accolade `}` (ou il y en a une de trop). Chaque `{` doit avoir sa `}`.
- `error CS1002: ; expected` : il manque un `;` à la fin d'une ligne.

### `NullReferenceException`

Ça veut dire : **« tu utilises quelque chose qui est vide »**. Dans 9 cas sur 10, c'est un champ de l'Inspector resté à `None`.

1. Double-cliquez sur l'erreur pour trouver la ligne.
2. Regardez quelle variable est utilisée sur cette ligne.
3. Vérifiez dans l'Inspector que ce champ est bien rempli (glisser-déposer l'objet).

### Les avertissements jaunes

`warning CS0414: The field ... is assigned but its value is never used` : normal tant que les TODO ne sont pas faits. Ils disparaissent quand votre code utilise la variable.

### Je ne peux pas attraper / appuyer sur un objet

- L'objet a-t-il un **collider** ? Pas de collider = pas d'interaction.
- Pour attraper : `XR Grab Interactable` + Rigidbody, et le Mesh Collider en **Convex**.
- Pour une touche ou une molette : approchez le doigt (index tendu) ou visez avec le rayon et appuyez sur la gâchette.

### Je ne peux pas me téléporter

- Il faut une `ZoneTeleportation` sous les pieds, et rien entre elle et le rayon.
- Posez-la au niveau du sol (même Y que le sol) : elle dépasse d'1 cm, juste assez pour ne pas être cachée par le sol.

### L'éclairage calculé est bizarre (murs noirs, taches)

- Le décor est-il bien **Static** ? Les lumières sont-elles en `Mode` **Baked** ?
- Dans `Window` → `Rendering` → `Lighting`, gardez `Lightmapper` = **Progressive CPU** : sur certains ordinateurs (Mac notamment), le mode GPU donne des murs noirs ou des taches.
- Relancez `Generate Lighting` après avoir modifié le décor.

### La porte ne s'ouvre pas

- La Console affiche-t-elle `Énigme résolue` ? Si non, c'est votre code : relisez les TODO.
- Si oui : le champ `Door To Open` de l'énigme est-il rempli ?
