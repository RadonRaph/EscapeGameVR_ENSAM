using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Serrure : s'ouvre quand on y pose la clé de la bonne couleur.
/// La serrure est un XR Socket Interactor : quand un objet y est posé,
/// son événement Select Entered appelle OnKeyInserted (voir l'Inspector).
/// </summary>
public class KeyLockV2 : MonoBehaviour
{
    [Header("Serrure")]
    [Tooltip("Nom du matériau qu'on attend que l'objet ait.")]
    public Material materiauAttendu;

    [Header("Quand la clé de la bonne couleur est posée")]
    public UnityEvent onSolved;

    public Transform porte;

    public float vitesseDescente;

    public Vector3 depart;
    public Vector3 destination;
    // args contient l'objet qui vient d'être posé dans la serrure
    public void OnKeyInserted(SelectEnterEventArgs args)
    {
        // L'objet posé dans la serrure
        GameObject key = args.interactableObject.transform.gameObject;
        Debug.Log("Objet posé dans la serrure : " + key.name);

        Renderer couleurCle = key.GetComponentInChildren<Renderer>();

        if (couleurCle.sharedMaterial != materiauAttendu)
        {
            Debug.Log("Ce n'est pas la clé n'est pas de la bonne couleur");
            return;
        } else
        {
            Debug.Log("Vous avez réussi !");
            StartCoroutine(AnimerMur());
            onSolved.Invoke();
        }

    }

    public IEnumerator AnimerMur()
    {
        while (porte.position != destination)
        {
            porte.position = Vector3.MoveTowards(porte.position, destination, vitesseDescente * Time.deltaTime);
            yield return null;
        }
    }
}
