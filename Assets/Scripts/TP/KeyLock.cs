using UnityEngine;

/// <summary>
/// [TP] Serrure : s'ouvre quand on y met la bonne clé.
/// Le BoxCollider de la serrure est un trigger : OnTriggerEnter est appelé
/// quand un objet (la clé) entre dedans.
/// Voir Prefabs/Enigmes/KeyLock/README_KeyLock.md
/// </summary>
public class KeyLock : Puzzle
{
    [Header("Serrure")]
    [Tooltip("Nom de la clé qui ouvre cette serrure (keyName du script Key)")]
    public string keyName = "Clé rouge";

    [Tooltip("Endroit où la clé se place une fois insérée")]
    public Transform keySlot;

    void OnTriggerEnter(Collider other)
    {
        // TODO 1 : récupérer le script Key de l'objet entré : other.GetComponentInParent<Key>()
        //          si l'objet n'a pas de Key (résultat null), arrêter la fonction (return)

        // TODO 2 : si le keyName de la clé est différent de keyName :
        //          afficher "Ce n'est pas la bonne clé" dans la Console et arrêter la fonction

        // TODO 3 : c'est la bonne clé : la fixer dans la serrure avec InsertInto(keySlot)
        //          puis appeler Solve()
    }
}
