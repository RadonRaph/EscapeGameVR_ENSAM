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

### Tester avec le casque

- **Quest Link** (PC Windows) : branchez le casque, activez Link, puis lancez Play dans Unity.
- **Build sur le casque** : casque branché en USB (mode développeur activé), `File` → `Build Profiles` → `Android` → `Build And Run`.

## Organisation du projet

| Dossier | Contenu |
|---|---|
| `Scripts/TP/` | **Les scripts des énigmes à compléter** : `Keypad`, `KeyLock`, `RotationPuzzle`, `ThrowTarget`. |
| `Scripts/Exemple/` | `SimpleButtonPuzzle` : une énigme complète, à lire pour comprendre. |
| `Scripts/Core/` | Deux outils (pas besoin de les modifier) : `ResetIfFallen` (un objet tombé hors du niveau revient à sa place) et `TeleportZone`. |
| `Prefabs/Enigmes/` | Un dossier par énigme : les prefabs et un **README** qui explique comment les installer et quoi coder. |
| `Prefabs/` | Les portes (`Porte_Donjon`, `Porte_Station`) et la `ZoneTeleportation`. |
| `Kenney/` | Les modèles 3D : `FurnitureKit` (meubles), `ModularDungeonKit` (donjon), `SpaceStationKit` (station spatiale). |

Cherchez `TODO` dans le dossier `Scripts/TP` pour trouver le code à écrire.

---

## Comment marche une énigme

Tout passe par des **UnityEvents** : des listes d'actions réglées dans l'Inspector, sans code. Le XR Interaction Toolkit en utilise partout.

1. **Le joueur interagit** : un composant XR déclenche un de ses événements (`Interactable Events` dans l'Inspector).
   - `XR Simple Interactable` (bouton, touche, molette) : `Select Entered` quand on appuie dessus.
   - `XR Grab Interactable` (objet à attraper) : `Select Entered` quand on l'attrape.
   - `XR Socket Interactor` (emplacement où poser un objet) : `Select Entered` quand un objet y est posé.
   - Tous : `Hover Entered` / `Hover Exited` quand une main vise l'objet ou ne le vise plus.
2. **L'événement appelle une fonction publique** du script de l'énigme (ex : `Keypad.PressKey("4")`).
3. **Quand l'énigme est réussie**, le script déclenche son propre événement `On Solved` : `onSolved.Invoke();`
4. **`On Solved` ouvre la porte** : dans l'Inspector, `+`, glissez la `Grille` de la porte, choisissez `GameObject` → `SetActive (bool)` et laissez la case **décochée**. Vous pouvez ajouter d'autres actions : une animation, une lumière, des particules...

Lisez `Scripts/Exemple/SimpleButtonPuzzle.cs` et l'Inspector du prefab `BoutonSimple` : c'est une énigme complète.

| Événement à brancher | Exemple dans le projet |
|---|---|
| `Select Entered` → fonction de l'énigme | Bouton rouge → `SimpleButtonPuzzle.Press()` |
| `On Solved` → `GameObject.SetActive(false)` | `BoutonSimple` → la `Grille` de la porte |
| `Hover Entered` / `Hover Exited` → `URPOutline.enabled` | Contour jaune de toutes les touches et objets |

### Mémo C#

Tout ce qu'il faut pour écrire le code des énigmes :

| Je veux... | J'écris | Exemple |
|---|---|---|
| Écrire un message dans la Console | `Debug.Log(...)` | `Debug.Log("Touche : " + key);` |
| Donner une valeur à une variable | `=` (un seul égal) | `typed = "";` |
| Tester si deux valeurs sont égales | `==` (deux égal) | `if (key == "OK") { ... }` |
| Tester si deux valeurs sont différentes | `!=` | `if (key.name != keyName) { ... }` |
| Comparer deux nombres | `<` `>` `<=` `>=` | `if (speed < minSpeed) { ... }` |
| Coller des textes (et des nombres) | `+` | `counter.text = hits + " / " + hitsNeeded;` |
| Ajouter 1 à un nombre | `x = x + 1;` | `hits = hits + 1;` |
| Enchaîner plusieurs cas | `if` / `else if` / `else` | `if (a) { ... } else if (b) { ... } else { ... }` |
| Répéter pour chaque case d'un tableau | `for` | `for (int i = 0; i < dials.Length; i++) { ... }` |
| Lire la case numéro i d'un tableau | `tableau[i]` | `solution[i]` |
| Arrêter la fonction tout de suite | `return;` | `if (collision.rigidbody == null) { return; }` |
| Tester si quelque chose est vide | `== null` | `if (counter == null) { ... }` |
| Changer un texte affiché | `.text` | `display.text = typed;` |
| Déclencher l'événement de réussite | `onSolved.Invoke();` | (ouvre la porte réglée dans l'Inspector) |

> **Règles de base** : chaque instruction finit par `;`. Chaque `{` a sa `}`. Les majuscules comptent : `Debug.Log` marche, `debug.log` non.
>
> **Le réflexe Debug.Log** : quand quelque chose ne marche pas, affichez les valeurs dans la Console (`Debug.Log("hits = " + hits);`) pour voir ce qui se passe vraiment.

### Le devant des énigmes

Le devant de chaque prefab d'énigme est du côté de la **flèche bleue** (axe Z). Tournez l'objet pour que la flèche bleue pointe vers le joueur.

---

## Étape 1 : Découverte

Dans `EscapeGame_Exemple` :

1. Lancez Play. Déplacez-vous (téléportation), attrapez l'ours en peluche, appuyez 3 fois sur le bouton rouge.
2. Sélectionnez `BoutonSimple` : dans `XR Simple Interactable` → `Interactable Events`, regardez `Select Entered`. Puis regardez `On Solved` dans `Simple Button Puzzle`.
3. Changez `Presses Needed` à 5 et relancez.
4. Approchez une main d'un objet : il s'entoure d'un contour jaune. Trouvez les deux événements qui l'allument et l'éteignent (`Hover Entered` / `Hover Exited`).

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
- **Attraper un objet** : `Add Component` → `XR Grab Interactable` (un Rigidbody est ajouté automatiquement).
  - **Contour** : `Add Component` → `URP Outline`, **décochez** le composant. Dans `XR Grab Interactable` → `Interactable Events` : `Hover Entered` → `URPOutline.enabled` coché, `Hover Exited` → `URPOutline.enabled` décoché.
  - Cochez **Convex** sur ses `Mesh Collider` : ils sont souvent sur les **enfants** du modèle, dépliez-le dans la Hierarchy.
  - Un objet que l'on attrape ne doit **pas** être `Contribute GI` (voir étape 5).

## Étape 4 : Deux énigmes différentes

Votre niveau doit contenir **au moins deux énigmes de mécaniques différentes** (deux keypads ne comptent que pour une). Placez un **indice** dans la salle : le code écrit au dos d'un tableau, la couleur de la clé sur un livre...

Idées d'énigmes (sans prefab fourni). Pour chacune, partez de `SimpleButtonPuzzle` : une fonction publique appelée par un événement XR, et `onSolved.Invoke()` quand c'est réussi.

| Énigme | Principe | Pistes |
|---|---|---|
| Séquence de leviers | Actionner des leviers dans le bon ordre. | Chaque levier est un `XR Simple Interactable` dont `Select Entered` envoie son numéro, comme les molettes. |
| Objet à placer | Poser le bon objet sur un socle (une statue, une gemme...). | Un `XR Socket Interactor` sur le socle, comme la serrure. |
| Combinaison de socles | Plusieurs socles, chacun attend un objet précis. | Plusieurs sockets + un tableau, comme `RotationPuzzle`. |
| Balance | Poser des objets pour atteindre le bon poids. | Un trigger et `OnTriggerEnter` / `OnTriggerExit` : additionner le `mass` des `Rigidbody`. |
| Mémoire (Simon) | Reproduire une séquence de boutons qui s'allument. | Une coroutine pour allumer les boutons, puis comparer comme le keypad. |

## Étape 5 : Lighting baked

En VR sur un casque autonome, l'éclairage doit être **précalculé** (baked) : c'est beaucoup plus léger que des ombres en temps réel.

1. **Décor fixe** : sélectionnez les murs, sols et meubles qui ne bougent pas. En haut de l'Inspector, cliquez sur la **petite flèche à côté de Static** et cochez **uniquement `Contribute GI`**.
   - Ne cochez **pas** la case Static entière : elle active aussi le `Batching Static`, qui casse le tile-based rendering du Quest et fait chuter les performances en VR.
   - Les murs, sols et portes reçoivent leur lumière d'une **lightmap** (une texture d'éclairage précalculée).
   - Les petits objets (meubles, accessoires) la reçoivent des **probes** : c'est réglé automatiquement à l'import (`Receive Global Illumination` = `Light Probes`). Ils sont trop petits pour une lightmap propre. Cochez-leur quand même `Contribute GI`.
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
- **Animation de feedback** : une porte, un tiroir ou un coffre qui s'ouvre avec une animation (`Animator`), lancée depuis `On Solved` (au lieu de `SetActive`).
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

- Le décor est-il bien en **`Contribute GI`** (flèche à côté de Static) ? Les lumières sont-elles en `Mode` **Baked** ?
- Dans `Window` → `Rendering` → `Lighting`, gardez `Lightmapper` = **Progressive CPU** : sur certains ordinateurs (Mac notamment), le mode GPU donne des murs noirs ou des taches.
- Relancez `Generate Lighting` après avoir modifié le décor.

### La porte ne s'ouvre pas

- Votre code appelle-t-il bien `onSolved.Invoke()` ? Ajoutez un `Debug.Log` juste avant pour vérifier.
- Si oui : `On Solved` contient-il bien la `Grille`, avec `GameObject` → `SetActive` et la case **décochée** ?
- Un événement XR (`Select Entered`...) ne fait rien : vérifiez qu'il appelle la bonne fonction, sur le bon objet. Une fonction n'apparaît dans la liste que si elle est `public`.
