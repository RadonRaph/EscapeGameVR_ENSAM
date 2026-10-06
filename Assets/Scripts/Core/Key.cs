using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE] Clé que le joueur peut prendre en main.
/// Son nom (keyName) doit correspondre à celui attendu par la serrure.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class Key : MonoBehaviour
{
    public string keyName = "Clé rouge";

    // Lâche la clé et la fixe dans la serrure (elle ne peut plus être reprise)
    public void InsertInto(Transform slot)
    {
        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        if (grab.isSelected)
        {
            grab.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grab);
        }
        grab.enabled = false;

        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = true;

        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
