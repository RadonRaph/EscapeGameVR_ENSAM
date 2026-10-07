using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [TP] Molettes à aligner : chaque molette (XR Simple Interactable) appelle TurnDial
/// avec son numéro grâce à son événement Select Entered (voir l'Inspector d'une molette).
/// L'énigme est résolue quand chaque molette affiche le bon symbole en haut.
/// Voir Prefabs/Enigmes/RotationPuzzle/README_RotationPuzzle.md
/// </summary>
public class RotationPuzzle : MonoBehaviour
{
    [Header("Molettes")]
    [Tooltip("Les molettes, de gauche à droite")]
    public Transform[] dials;

    [Tooltip("Nombre de symboles sur chaque molette")]
    public int symbolCount = 4;

    [Tooltip("Symbole attendu pour chaque molette (0 = A, 1 = B, 2 = C, 3 = D)")]
    public int[] solution = { 1, 3, 2 };

    [Header("Quand les molettes sont bien alignées")]
    public UnityEvent onSolved;

    // Le symbole affiché en haut de chaque molette (0 = A)
    int[] current;

    void Start()
    {
        current = new int[dials.Length];
    }

    // index = numéro de la molette qui a été appuyée (0 = celle de gauche)
    public void TurnDial(int index)
    {
        // Tourner la molette d'un cran (déjà écrit)
        dials[index].Rotate(0f, 0f, -360f / symbolCount);
        current[index] = current[index] + 1;
        if (current[index] >= symbolCount)
        {
            current[index] = 0;
        }

        for (int i = 0; i < dials.Length; i++)
        {
            if (current[i] != solution[i])
            {
                Debug.Log("Molette " + i + " : " + current[i]);
                return;
            }
        }
        onSolved.Invoke();

        // TODO 1 : parcourir toutes les molettes avec une boucle for.
        //          Astuce : for (int i = 0; i < dials.Length; i++) { ... }
        //          i vaut 0, puis 1, puis 2... jusqu'au nombre de molettes (dials.Length) non compris.

        // TODO 2 : dans la boucle, si current[i] est différent de solution[i],
        //          cette molette n'est pas sur le bon symbole : arrêter la fonction.
        //          Astuce : "différent de" s'écrit !=   ->   if (current[i] != solution[i]) { return; }
        //          Pour vérifier, vous pouvez afficher la valeur de chaque molette :
        //          Debug.Log("Molette " + i + " : " + current[i]);

        // TODO 3 : APRÈS la boucle (en dehors des accolades du for) : aucune molette n'a fait return,
        //          elles sont donc toutes bonnes -> déclencher l'événement onSolved.
        //          Astuce : c
    }
}
