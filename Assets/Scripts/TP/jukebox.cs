using UnityEngine;

public class JukeboxPuzzle : MonoBehaviour
{
    [Header("Musique")]
    public AudioSource music;

    [Header("Nouvelle porte")]
    public DoorAnimation door;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Coin"))
        {
            activated = true;

            // Lancer la musique
            if (music != null)
            {
                music.Play();
            }

            // Ouvrir la nouvelle porte
            if (door != null)
            {
                door.OpenDoor();
            }

            // Faire disparaître la pièce
            Destroy(other.gameObject);

            Debug.Log("Pièce insérée ! Musique lancée, porte ouverte.");
        }
    }
}
