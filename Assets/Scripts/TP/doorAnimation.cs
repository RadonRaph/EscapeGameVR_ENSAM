using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
    [Header("Animation de la porte")]
    public float openAngle = 90f;
    public float duration = 1.5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        StartCoroutine(OpenAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localRotation =
                Quaternion.Slerp(closedRotation, openRotation, t);

            yield return null;
        }

        transform.localRotation = openRotation;
    }
}