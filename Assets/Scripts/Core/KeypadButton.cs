using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE] Une touche du keypad. Quand on l'appuie (doigt ou rayon),
/// elle envoie sa valeur au Keypad parent.
/// </summary>
[RequireComponent(typeof(XRSimpleInteractable))]
public class KeypadButton : MonoBehaviour
{
    [Tooltip("Valeur envoyée au keypad : \"0\" à \"9\", \"C\" ou \"OK\"")]
    public string key = "1";

    XRSimpleInteractable interactable;
    Keypad keypad;
    Vector3 startPosition;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        keypad = GetComponentInParent<Keypad>();
        startPosition = transform.localPosition;

        if (keypad == null)
        {
            Debug.LogError("KeypadButton : la touche " + gameObject.name + " doit être un enfant d'un objet avec le script Keypad.");
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        if (keypad != null)
        {
            keypad.PressKey(key);
        }
        StopAllCoroutines();
        StartCoroutine(PressAnimation());
    }

    // La touche s'enfonce un court instant
    IEnumerator PressAnimation()
    {
        transform.localPosition = startPosition - new Vector3(0f, 0f, 0.01f);
        yield return new WaitForSeconds(0.15f);
        transform.localPosition = startPosition;
    }
}
