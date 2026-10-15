using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Zone invisible : quand le joueur entre dedans, déclenche onEnter.
/// À mettre sur un GameObject vide avec un Box Collider en "Is Trigger".
/// Ex. : On Enter -> SimonPuzzle.StartPuzzle()
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour
{
    [Tooltip("Tag mis sur le XR Origin du joueur")]
    public string playerTag = "Player";

    [Tooltip("Ne se déclenche qu'une seule fois")]
    public bool triggerOnce = true;

    [Header("Quand le joueur entre dans la zone")]
    public UnityEvent onEnter;

    bool done = false;

    // Appelé quand on ajoute le script : coche "Is Trigger" automatiquement
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && done) return;
        if (!IsPlayer(other.transform)) return;

        done = true;
        Debug.Log("Joueur entré dans la zone : " + name);
        onEnter.Invoke();
    }

    // Remonte les parents : marche avec le collider du corps OU des mains
    bool IsPlayer(Transform t)
    {
        while (t != null)
        {
            if (t.CompareTag(playerTag)) return true;
            t = t.parent;
        }
        return false;
    }
}