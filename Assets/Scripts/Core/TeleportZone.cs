using UnityEngine;

/// <summary>
/// [CORE] Zone où le joueur peut se téléporter (invisible en jeu).
/// Dessine une boîte verte dans la vue Scene pour la placer facilement :
/// la poser sur le sol et l'agrandir avec l'outil Scale.
/// </summary>
public class TeleportZone : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0.2f, 1f, 0.3f, 0.25f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.color = new Color(0.2f, 1f, 0.3f, 1f);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
