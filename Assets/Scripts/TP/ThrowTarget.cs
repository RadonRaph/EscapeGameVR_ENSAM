using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [TP] Cible : il faut la toucher plusieurs fois en lançant des objets.
/// OnCollisionEnter est appelé par Unity quand un objet percute la cible.
/// Voir Prefabs/Enigmes/ThrowTarget/README_ThrowTarget.md
/// </summary>
public class ThrowTarget : MonoBehaviour
{
    [Header("Cible")]
    [Tooltip("Nombre de touches pour résoudre l'énigme")]
    public int hitsNeeded = 3;

    [Tooltip("Vitesse minimum : un objet simplement posé sur la cible ne compte pas")]
    public float minSpeed = 2f;

    [Tooltip("Texte qui affiche le nombre de touches")]
    public TMP_Text counter;

    [Header("Quand la cible a été assez touchée")]
    public UnityEvent onSolved;

    int hits = 0;

    void OnCollisionEnter(Collision collision)
    {
        // TODO 1 : l'objet doit avoir un Rigidbody (sinon il n'a pas été lancé)
        if (collision.rigidbody == null)
        {
            return;
        }

        // TODO 2 : le choc doit être assez fort
        Debug.Log("Vitesse : " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude < minSpeed)
        {
            return;
        }

        // TODO 3 : on compte la touche et on affiche le compteur
        hits = hits + 1;
        counter.text = hits + " / " + hitsNeeded;

        // TODO 4 : assez de touches -> énigme résolue
        if (hits == hitsNeeded)
        {
            Debug.Log("Cible touchée " + hits + " fois, énigme résolue !");
            onSolved.Invoke();
        }
    }
}