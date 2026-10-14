using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PowerLever : MonoBehaviour
{
    public Transform pivot;
    public GameObject keypad;
    public UnityEvent onPowered = new UnityEvent();

    XRSimpleInteractable[] touches;
    bool ready;
    bool powered;

    void Start()
    {
        if (pivot == null || keypad == null)
        {
            Debug.LogError(
                "PowerLever : renseignez Pivot et Keypad dans l'Inspector.",
                this);
            return;
        }

        touches = keypad.GetComponentsInChildren<XRSimpleInteractable>(true);

        foreach (var touche in touches)
            touche.enabled = false;

        ready = true;
    }

    public void Activer()
    {
        if (!ready || powered)
            return;

        powered = true;
        StartCoroutine(Abaisser());
    }

    IEnumerator Abaisser()
    {
        Quaternion depart = pivot.localRotation;
        Quaternion arrivee = Quaternion.Euler(-9.144f, 0f, 0f);
        float temps = 0f;

        while (temps < 0.5f)
        {
            temps += Time.deltaTime;
            float progression = Mathf.Clamp01(temps / 0.5f);

            pivot.localRotation =
                Quaternion.Slerp(depart, arrivee, progression);

            yield return null;
        }

        pivot.localRotation = arrivee;

        foreach (var touche in touches)
            touche.enabled = true;

        onPowered.Invoke();
    }
}