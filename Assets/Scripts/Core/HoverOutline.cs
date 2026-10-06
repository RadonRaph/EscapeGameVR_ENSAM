using SilhouetteOutline;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE] Entoure l'objet d'un contour quand une main le vise ou le touche.
/// À mettre sur un objet qui a un composant XR (Grab Interactable, Simple Interactable...).
/// </summary>
[RequireComponent(typeof(XRBaseInteractable))]
public class HoverOutline : MonoBehaviour
{
    public Color color = new Color(1f, 0.8f, 0.2f);

    [Tooltip("Largeur en pixels (plus grande en casque)")]
    public float width = 8f;

    XRBaseInteractable interactable;
    URPOutline outline;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        outline = GetComponent<URPOutline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<URPOutline>();
        }
        outline.Color = color;
        outline.Width = width;
        outline.enabled = false;
    }

    void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
        outline.enabled = false;
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        outline.enabled = true;
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        // Les deux mains peuvent viser l'objet : on attend que plus aucune ne le vise
        if (!interactable.isHovered)
        {
            outline.enabled = false;
        }
    }
}
