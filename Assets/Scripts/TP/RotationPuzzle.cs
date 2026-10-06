using UnityEngine;

/// <summary>
/// [TP] Molettes à aligner : chaque molette (RotatingDial) tourne d'un cran quand on appuie dessus.
/// L'énigme est résolue quand chaque molette affiche le bon symbole en haut.
/// Voir Prefabs/Enigmes/RotationPuzzle/README_RotationPuzzle.md
/// </summary>
public class RotationPuzzle : Puzzle
{
    [Header("Molettes")]
    public RotatingDial[] dials;

    [Tooltip("Symbole attendu pour chaque molette (0 = A, 1 = B, 2 = C, 3 = D)")]
    public int[] solution = { 1, 3, 2 };

    // Appelée par une molette à chaque fois qu'elle tourne
    public void OnDialTurned()
    {
        // TODO 1 : parcourir toutes les molettes avec une boucle for (i de 0 à dials.Length)

        // TODO 2 : dans la boucle, si dials[i].currentIndex est différent de solution[i],
        //          l'énigme n'est pas résolue : arrêter la fonction (return)

        // TODO 3 : après la boucle, toutes les molettes sont bonnes : appeler Solve()
    }
}
