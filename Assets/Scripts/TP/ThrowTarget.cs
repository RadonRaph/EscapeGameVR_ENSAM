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
        // TODO 1 : vérifier que l'objet peut être lancé : il doit avoir un Rigidbody.
        //          Le Rigidbody de l'objet qui touche la cible est dans collision.rigidbody.
        //          S'il n'en a pas (il vaut null), arrêter la fonction.
        //          Astuce : if (collision.rigidbody == null) { return; }

        // TODO 2 : vérifier la vitesse du choc : collision.relativeVelocity.magnitude (en m/s).
        //          Si elle est plus petite que minSpeed, arrêter la fonction.
        //          Astuce : "plus petit que" s'écrit <
        //          Pour régler minSpeed, affichez la vitesse de chaque choc :
        //          Debug.Log("Vitesse : " + collision.relativeVelocity.magnitude);

        // TODO 3 : ajouter 1 à hits, puis afficher le compteur sur la cible (exemple : "2 / 3").
        //          Astuce : hits = hits + 1;
        //          Le texte affiché est dans counter.text. On colle textes et nombres avec +
        //          ->   counter.text = hits + " / " + hitsNeeded;

        // TODO 4 : si hits vaut hitsNeeded, déclencher l'événement onSolved.
        //          Astuce : "égal à" s'écrit == (deux signes égal, un seul = sert à donner une valeur)
        //          ->   if (hits == hitsNeeded) { ... }   puis   onSolved.Invoke();
    }
}
