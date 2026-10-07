using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [EXEMPLE] Une énigme simple : appuyer 3 fois sur le bouton ouvre la porte.
/// Toutes les énigmes du TP marchent comme ça :
///  1. un composant XR (ici XR Simple Interactable) appelle une fonction publique
///     grâce à un événement réglé dans l'Inspector (Interactable Events > Select Entered)
///  2. la fonction vérifie si l'énigme est réussie
///  3. si oui, elle déclenche onSolved : dans l'Inspector, onSolved cache la grille (SetActive)
/// </summary>
public class SimpleButtonPuzzle : MonoBehaviour
{
    // "public" : la variable apparaît dans l'Inspector, on peut la régler sans toucher au code.
    // [Tooltip] : le texte d'aide affiché quand la souris passe sur le champ.
    [Tooltip("Nombre d'appuis pour résoudre l'énigme")]
    public int pressesNeeded = 3;

    // Un UnityEvent : une liste d'actions réglée dans l'Inspector (bouton +).
    [Header("Quand l'énigme est résolue")]
    public UnityEvent onSolved;

    // Pas "public" : une variable interne au script, invisible dans l'Inspector.
    int presses = 0;

    // "public" est obligatoire : sinon la fonction n'apparaît pas dans la liste
    // de l'événement Select Entered.
    public void Press()
    {
        // Ajouter 1 au nombre d'appuis
        presses = presses + 1;

        // Afficher un message dans la Console (on colle texte et nombre avec +)
        Debug.Log("Bouton appuyé " + presses + " fois");

        // == compare deux valeurs (un seul = sert à donner une valeur)
        if (presses == pressesNeeded)
        {
            Debug.Log("Énigme résolue !");

            // Déclenche toutes les actions réglées dans On Solved (ici : cacher la grille)
            onSolved.Invoke();
        }
    }
}
