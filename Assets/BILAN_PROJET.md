# Bilan du projet — Escape Game VR



## Présentation

Le projet a été développé à partir de la scène d’exemple fournie,
adaptée dans `Assets/Scenes/EscapeGame_TP1.unity`.

Le parcours comprend deux ambiances : un donjon puis une station
spatiale. Le joueur doit résoudre des énigmes et réactiver un terminal
pour accéder à la sortie.

## Vidéo de démonstration

Une vidéo du parcours complet en VR est disponible ici :

[Voir la vidéo sur Google Drive](https://drive.google.com/drive/folders/1FHhwr9WhHHh_4-fZdwm1YagHbcCT_uff?usp=sharing)

## Parcours du joueur

### 1. Le donjon : trouver la bonne clé

Trois clés de couleurs différentes peuvent être manipulées.
Le script `KeyLock.cs` compare le nom de la clé insérée avec celui
attendu par la serrure.

Une mauvaise clé laisse le passage fermé. La bonne clé déclenche
l’ouverture des barreaux.

L’ouverture utilise un Animator : la grille pivote sur une charnière,
pendant environ 2,5 secondes, puis reste ouverte. L’arche reste fixe.

### 2. La station : réactiver le terminal

Le keypad est visible au départ, mais ses touches sont bloquées.

Un levier personnalisé, piloté par `PowerLever.cs`, permet de
réactiver le terminal. Lors de son activation :

- la poignée s’incline ;
- le voyant vert perd son émission lumineuse ;
- le voyant rouge devient émissif ;
- une courte gerbe d’étincelles apparaît ;
- les interactions du keypad sont réactivées.

Les deux sphères des voyants restent toujours visibles.

### 3. Trouver le code et sortir

Les panneaux affichent les chiffres à lire de gauche à droite :
4, 0, 7 et 1.

Le joueur saisit `4071`, puis valide avec OK. Le script `Keypad.cs`
vérifie la combinaison et déclenche les actions de réussite :
ouverture de la sortie et affichage du message de victoire.

## Éclairage et effets visuels

Les lumières des salles et de la sortie sont en mode Baked.

Les lightmaps mémorisent l’éclairage du décor fixe. Les Adaptive
Probe Volumes fournissent l’éclairage aux objets mobiles, notamment
aux clés. Leur éclairage change selon la salle traversée.

Une lampe visible et un Spot Light éclairent la plateforme de sortie.

Les voyants utilisent des matériaux URP/Lit émissifs. Les étincelles
utilisent un Particle System déclenché par l’événement On Powered.

# Réalisations selon le barème

Voici le travail réalisé dans ma scène `EscapeGame_TP1`.

| Critère | Points du barème | Ma réalisation |
|---|---:|---|
| Setup VR jouable sur Quest 3 | 2 | Téléportation et manipulation des objets présentes. Validation complète sur Quest 3 à effectuer. |
| Compléter un prefab fourni | 2,5 | J’ai complété KeyLock.cs pour reconnaître la bonne clé et déclencher l’ouverture de la grille. |
| Deux énigmes de mécaniques différentes avec indices | 2,5 | Une énigme clé-serrure et une énigme de code sur keypad. Des indices permettent de choisir la clé et de trouver le code 4071. |
| Deux salles thématiques reliées | 2,5 | Un donjon et une station spatiale. La réussite de la serrure ouvre le passage entre les salles. |
| Lighting baked : lightmaps + APV | 1,5 | J’ai calculé les lightmaps et les Adaptive Probe Volumes. Les clés reçoivent l’éclairage des salles. Quelques réglages du décor restent à finaliser. |
| Vidéo du parcours depuis le Quest 3 | 1 | À réaliser. |
| Bonus : deuxième prefab ou nouveau mécanisme | 2 | J’ai également complété Keypad.cs et créé un levier qui débloque les touches du terminal. |
| Bonus : matériaux / VFX | 2 | Matériaux colorés, voyants émissifs et étincelles déclenchées par le levier. Le vert s’éteint et le rouge s’allume après activation. |
| Bonus : idée pertinente / originalité | 2 | J’ai personnalisé la progression : trouver la clé, réactiver le terminal avec le levier, puis découvrir le code de sortie. |
| Bonus : animation de feedback | 1 | La bonne clé déclenche une animation avec Animator : les barreaux pivotent lentement pendant environ 2,5 secondes et restent ouverts. |
| Bonus : fin de partie | 1 | Le bon code ouvre la sortie et affiche un message de victoire. |
