using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [EXEMPLE] L'énigme la plus simple : appuyer sur un bouton ouvre la porte.
/// Montre comment écrire une énigme :
///  1. la classe hérite de Puzzle (au lieu de MonoBehaviour)
///  2. on écoute une interaction XR (ici : le bouton est sélectionné)
///  3. quand c'est réussi, on appelle Solve()
/// </summary>
[RequireComponent(typeof(XRSimpleInteractable))]
public class SimpleButtonPuzzle : Puzzle
{
    XRSimpleInteractable button;

    void Awake()
    {
        button = GetComponent<XRSimpleInteractable>();
    }

    // On s'abonne à l'événement "le bouton est appuyé"
    void OnEnable()
    {
        button.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        button.selectEntered.RemoveListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        Debug.Log("Bouton appuyé !");
        Solve();
    }
}
