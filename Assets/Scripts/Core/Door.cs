using UnityEngine;

/// <summary>
/// [CORE] Porte qui bloque le passage vers la salle suivante.
/// Open() fait simplement disparaître la porte.
/// Bonus : remplacer par une animation (Animator) pour l'ouvrir joliment.
/// </summary>
public class Door : MonoBehaviour
{
    public void Open()
    {
        Debug.Log("Porte ouverte : " + gameObject.name);
        gameObject.SetActive(false);
    }
}
