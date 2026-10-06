using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE] Marque un objet comme projectile : la cible (ThrowTarget) ne compte que ces objets.
/// Pour lancer : attraper l'objet, faire le geste et lâcher la gâchette.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class Throwable : MonoBehaviour
{
}
