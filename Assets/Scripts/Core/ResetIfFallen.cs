using UnityEngine;

/// <summary>
/// [CORE] Remet l'objet à sa place de départ s'il tombe sous le sol
/// (objet lancé à travers un mur, clé tombée hors du niveau...).
/// </summary>
public class ResetIfFallen : MonoBehaviour
{
    [Tooltip("En dessous de cette hauteur, l'objet revient à sa place")]
    public float minHeight = -5f;

    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody body;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y > minHeight)
        {
            return;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;
        if (body != null && !body.isKinematic)
        {
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
    }
}
