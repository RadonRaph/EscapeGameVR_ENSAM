using UnityEngine;

/// <summary>
/// [AJOUT] Brouillard d'ambiance : active et règle le brouillard.
/// À mettre sur un objet vide de la scène.
/// </summary>
public class FogSetup : MonoBehaviour
{
    [Tooltip("Couleur du brouillard (sombre = ambiance cachot)")]
    public Color fogColor = new Color(0.05f, 0.06f, 0.1f);

    [Tooltip("Épaisseur du brouillard : 0.02 = léger, 0.1 = très épais")]
    [Range(0f, 0.2f)]
    public float density = 0.04f;

    // Update est appelé à chaque image : le brouillard suit les valeurs de l'Inspector
    void Update()
    {
        // RenderSettings = les réglages d'environnement de la scène
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = density;
    }
}