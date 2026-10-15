using UnityEngine;

/// <summary>
/// À mettre sur la zone trigger (Box Collider) au centre de chaque sas de sortie.
/// Quand le joueur entre dedans, InfiniteLoop décide : tour suivant, retour au début, ou sortie.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LoopExit : MonoBehaviour
{
    [Tooltip("L'objet qui porte InfiniteLoop (depuis la Hierarchy)")]
    public InfiniteLoop loop;

    [Tooltip("Numéro de ce chemin (0 = gauche, 1 = droite...)")]
    public int pathId = 0;

    [Tooltip("Repère de CE trigger (là où le joueur entre). Laisse VIDE en général = ce trigger. " +
             "Ce n'est PAS le point d'arrivée !")]
    public Transform exitPoint;

    [Tooltip("POINT D'ARRIVÉE (ex. Loop_exit). Vide = 'Loop Start' de InfiniteLoop")]
    public Transform customDestination;

    [Tooltip("Coché : ne se déclenche que si le joueur avance dans le sens de la flèche bleue. " +
             "Indispensable pour un couloir à 2 entrées")]
    public bool oneWay = false;

    // Coche "Is Trigger" automatiquement quand on ajoute le script
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (loop == null || !loop.IsPlayer(other.transform)) return;
        if (oneWay && !EnteredFromBehind(other)) return; // le joueur vient dans l'autre sens : on ignore

        loop.OnPathTaken(this);
    }

    // Vrai si le joueur arrive par l'arrière de la boîte,
    // donc s'il marche dans le sens de la flèche bleue
    bool EnteredFromBehind(Collider other)
    {
        Vector3 playerLocal = transform.InverseTransformPoint(other.bounds.center);
        Vector3 boxLocal = transform.InverseTransformPoint(GetComponent<Collider>().bounds.center);
        return playerLocal.z < boxLocal.z;
    }
}