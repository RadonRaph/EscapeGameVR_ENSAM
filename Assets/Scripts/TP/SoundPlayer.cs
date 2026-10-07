using UnityEngine;

/// <summary>
/// [AJOUT] Joue un son quand on appelle Play(), et l'arrête après une durée choisie.
/// Se branche dans l'Inspector, par exemple dans On Solved ().
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [Tooltip("Le son à jouer")]
    public AudioClip clip;

    [Tooltip("Volume du son (0 = muet, 1 = maximum)")]
    [Range(0f, 1f)]
    public float volume = 1f;

    [Tooltip("Durée maximum du son en secondes (0 = jusqu'à la fin du son)")]
    public float duration = 10f;

    AudioSource source;

    void Awake()
    {
        // On récupère l'Audio Source de cet objet et on le règle en 2D
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 0 = entendu partout, avec le même volume
    }

    // "public" obligatoire : sinon Play n'apparaît pas dans la liste de l'événement
    public void Play()
    {
        Debug.Log("SoundPlayer : Play appelé, clip = " + clip);

        // Si aucun son n'est choisi, on ne fait rien (évite l'erreur)
        if (clip == null) return;

        source.clip = clip;
        source.volume = volume;
        source.Play();

        // Si une durée est choisie, on arrête le son après ce délai
        if (duration > 0f)
        {
            Invoke("StopSound", duration);
        }
    }

    void StopSound()
    {
        source.Stop();
    }
}