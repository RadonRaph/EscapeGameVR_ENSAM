using UnityEngine;

public class BrasDroitAnime : MonoBehaviour
{
    [Header("Mouvement")]
    public float vitesse = 3f;
    public float amplitudeBras = 35f;      // haut du bras
    public float amplitudeAvantBras = 25f; // avant-bras

    Transform bras;       // RigArmRight1
    Transform avantBras;  // RigArmRight2
    Quaternion rotBras;
    Quaternion rotAvantBras;

    void Start()
    {
        bras = TrouverOs(transform, "RigArmRight1");
        avantBras = TrouverOs(transform, "RigArmRight2");

        if (bras == null || avantBras == null)
        {
            Debug.LogWarning("BrasDroitAnime : os du bras droit introuvables.");
            enabled = false;
            return;
        }

        rotBras = bras.localRotation;
        rotAvantBras = avantBras.localRotation;
    }

    void LateUpdate()
    {
        float t = Mathf.Sin(Time.time * vitesse);

        bras.localRotation = rotBras * Quaternion.AngleAxis(t * amplitudeBras, Vector3.forward);
        avantBras.localRotation = rotAvantBras * Quaternion.AngleAxis(t * amplitudeAvantBras, Vector3.forward);
    }

    // Recherche récursive d'un os par son nom
    Transform TrouverOs(Transform parent, string nom)
    {
        foreach (Transform enfant in parent)
        {
            if (enfant.name == nom) return enfant;
            Transform trouve = TrouverOs(enfant, nom);
            if (trouve != null) return trouve;
        }
        return null;
    }
}