using TMPro;
using UnityEngine;

/// <summary>
/// [TP] Cible : il faut la toucher plusieurs fois en lançant des projectiles (Throwable).
/// OnCollisionEnter est appelé quand un objet percute la cible.
/// Voir Prefabs/Enigmes/ThrowTarget/README_ThrowTarget.md
/// </summary>
public class ThrowTarget : Puzzle
{
    [Header("Cible")]
    [Tooltip("Nombre de touches pour résoudre l'énigme")]
    public int hitsNeeded = 3;

    [Tooltip("Vitesse minimum : un objet simplement posé sur la cible ne compte pas")]
    public float minSpeed = 2f;

    [Tooltip("Texte qui affiche le nombre de touches")]
    public TMP_Text counter;

    int hits = 0;

    void OnCollisionEnter(Collision collision)
    {
        // TODO 1 : vérifier que l'objet est un projectile : collision.gameObject.GetComponent<Throwable>()
        //          si ce n'est pas un projectile (null), arrêter la fonction (return)

        // TODO 2 : vérifier la vitesse du choc : collision.relativeVelocity.magnitude
        //          si elle est plus petite que minSpeed, arrêter la fonction

        // TODO 3 : ajouter 1 à hits et l'afficher dans counter (exemple : "2 / 3")

        // TODO 4 : si hits a atteint hitsNeeded, appeler Solve()
    }
}
