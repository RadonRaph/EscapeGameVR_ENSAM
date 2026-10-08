using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjetTapisRoulant : MonoBehaviour
{
    [Tooltip("Position x, y, z où l'objet doit aller")]
    public Vector3 destination;

    [Tooltip("Vitesse de déplacement, plus élevée, plus rapide")]
    public float vitesse;

    private bool enMouvement = false;
    private bool estArrive = false;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable != null) grabInteractable.enabled = false;
        if (rb != null) rb.isKinematic = true;
    }
    void Update()
    {
        if (enMouvement == true && estArrive == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, vitesse * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                transform.position = destination;
                enMouvement = false;
                estArrive = true;

                // L'objet devient attrapable et soumis à la gravité
                if (grabInteractable != null) grabInteractable.enabled = true;
                if (rb != null) rb.isKinematic = false;
            }
        }
    }

    public void BasculerMouvement()
    {
        if (estArrive == false)
        {
            enMouvement = !enMouvement;
        }
    }
}