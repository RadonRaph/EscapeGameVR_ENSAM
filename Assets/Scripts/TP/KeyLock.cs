using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// [TP] Serrure : s'ouvre quand on y pose la bonne clé.
/// La serrure est un XR Socket Interactor : quand un objet y est posé,
/// son événement Select Entered appelle OnKeyInserted (voir l'Inspector).
/// Voir Prefabs/Enigmes/KeyLock/README_KeyLock.md
/// </summary>
public class KeyLock : MonoBehaviour
{
    [Header("Serrure")]
    [Tooltip("Nom de l'objet clé qui ouvre cette serrure (son nom dans la Hierarchy)")]
    public string keyName = "Cle_Rouge";

    [Header("Quand la bonne clé est posée")]
    public UnityEvent onSolved;

    // args contient l'objet qui vient d'être posé dans la serrure
    public void OnKeyInserted(SelectEnterEventArgs args)
    {
        // L'objet posé dans la serrure
        GameObject key = args.interactableObject.transform.gameObject;
        Debug.Log("Objet posé dans la serrure : " + key.name);

        // TODO 1 : mauvaise clé -> message + on arrête la fonction
        if (key.name != keyName)
        {
            Debug.Log("Ce n'est pas la bonne clé");
            return;
        }

        // TODO 2 : bonne clé -> on déclenche l'événement
        Debug.Log("Bonne clé ! Serrure ouverte");
        onSolved.Invoke();
    }
}
