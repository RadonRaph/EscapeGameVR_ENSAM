using System.Collections;
using UnityEngine;

public class FireworksFinale : MonoBehaviour
{
    [Header("Feux d'artifice")]
    [SerializeField] private GameObject fireworkPrefab;
    [SerializeField] private int fireworkCount = 15;
    [SerializeField] private float delayBetween = 0.4f;

    [Header("Zone d'apparition")]
    [SerializeField] private Vector3 areaSize = new Vector3(10f, 3f, 10f);

    [Header("Audio (optionnel)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionClip;

    private bool hasPlayed = false;

    public void PlayFinale()
    {
        if (hasPlayed) return;
        hasPlayed = true;
        StartCoroutine(FinaleRoutine());
    }

    private IEnumerator FinaleRoutine()
    {
        for (int i = 0; i < fireworkCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(0, areaSize.y),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            GameObject fw = Instantiate(fireworkPrefab, transform.position + offset, Quaternion.identity);
            Destroy(fw, 5f);

            if (audioSource != null && explosionClip != null)
                audioSource.PlayOneShot(explosionClip);

            yield return new WaitForSeconds(delayBetween + Random.Range(-0.15f, 0.15f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.up * areaSize.y / 2, areaSize);
    }
}