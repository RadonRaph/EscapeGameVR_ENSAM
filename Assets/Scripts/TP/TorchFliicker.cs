using UnityEngine;

/// <summary>
/// [AJOUT] Fait vaciller une lumière comme la flamme d'une torche.
/// À mettre sur un objet qui a un composant Light.
/// </summary>
[RequireComponent(typeof(Light))]
public class TorchFlicker : MonoBehaviour
{
    [Tooltip("Intensité minimum de la flamme")]
    public float minIntensity = 0.8f;

    [Tooltip("Intensité maximum de la flamme")]
    public float maxIntensity = 1.6f;

    [Tooltip("Vitesse du vacillement (1 = lent, 10 = nerveux)")]
    public float speed = 4f;

    [Tooltip("Bouge un peu la lumière pour faire danser les ombres (0 = fixe)")]
    public float moveAmount = 0.03f;

    Light torchLight;
    Vector3 startPosition;

    // Décalage aléatoire : chaque torche vacille différemment
    float seed;

    void Start()
    {
        torchLight = GetComponent<Light>();
        startPosition = transform.localPosition;
        seed = Random.value * 100f;
    }

    void Update()
    {
        // Mathf.PerlinNoise donne une valeur douce entre 0 et 1 qui change petit à petit
        float noise = Mathf.PerlinNoise(seed, Time.time * speed);

        // On transforme ce 0-1 en intensité entre min et max
        torchLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        // Petit déplacement pour faire bouger les ombres
        if (moveAmount > 0f)
        {
            float x = Mathf.PerlinNoise(seed + 10f, Time.time * speed) - 0.5f;
            float y = Mathf.PerlinNoise(seed + 20f, Time.time * speed) - 0.5f;
            float z = Mathf.PerlinNoise(seed + 30f, Time.time * speed) - 0.5f;
            transform.localPosition = startPosition + new Vector3(x, y, z) * moveAmount * 2f;
        }
    }
}