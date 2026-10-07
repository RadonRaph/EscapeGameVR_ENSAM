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

        if (keyName != key.name)
        {
            Debug.Log("Ce n'est pas la bonne clé");
            return;
        } else
        {
            Debug.Log("Vous avez réussi !");
            onSolved.Invoke();
        }

        // TODO 1 : si le nom de l'objet (key.name) est différent de keyName :
        //          - afficher "Ce n'est pas la bonne clé" dans la Console avec Debug.Log("...");
        //          - arrêter la fonction avec return;
        //          Astuce : "différent de" s'écrit !=   ->   if (key.name != keyName) { ... }
        //          return; arrête la fonction tout de suite : le code en dessous n'est pas exécuté.

        // TODO 2 : si on arrive ici, c'est la bonne clé : déclencher l'événement onSolved.
        //          Astuce : onSolved.Invoke();
        //          Bonus : afficher aussi un message de réussite avec Debug.Log
    }
}
