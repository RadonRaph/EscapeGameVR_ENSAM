using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [CORE] Base de toutes les énigmes.
/// Quand l'énigme est réussie, le script appelle Solve() :
///  - la porte liée (doorToOpen) s'ouvre
///  - l'événement onSolved est déclenché (animation, VFX, lumière...)
/// </summary>
public class Puzzle : MonoBehaviour
{
    [Header("Énigme")]
    [Tooltip("Porte ouverte quand l'énigme est résolue (optionnel)")]
    public Door doorToOpen;

    [Tooltip("Actions en plus quand l'énigme est résolue : animation, VFX, lumière...")]
    public UnityEvent onSolved;

    bool isSolved = false;

    public bool IsSolved
    {
        get { return isSolved; }
    }

    // À appeler quand le joueur a trouvé la solution
    protected void Solve()
    {
        // Une énigme ne se résout qu'une seule fois
        if (isSolved)
        {
            return;
        }

        isSolved = true;
        Debug.Log("Énigme résolue : " + gameObject.name);

        if (doorToOpen != null)
        {
            doorToOpen.Open();
        }
        else
        {
            Debug.LogWarning("L'énigme " + gameObject.name + " n'a pas de porte à ouvrir : remplissez le champ Door To Open.");
        }

        onSolved.Invoke();
    }
}
