/**using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonOpenDoorS2 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Quand l'énigme est résolue")]
    public UnityEvent onSolved;

    [Header("Configuration")]
    public Transform objetCible;
    //public Vector3 positionCible = new Vector3(1.3f, 0f, 2.25f);
    public float distanceMaxAutorisee = 0.5f;

    // 1. Obligatoire : Déplace l'objet pendant qu'on le maintient
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // 2. Remplace ta fonction Press() : Se déclenche quand on lâche le clic
    public void OnEndDrag(PointerEventData eventData)
    {
        // On calcule la distance entre la position actuelle et la cible
        float distance = Vector3.Distance(transform.position, positionCible);

        Debug.Log("Distance actuelle : " + distance);

        // Si l'objet est assez proche
        if (distance < distanceMaxAutorisee)
        {
            Debug.Log("Énigme résolue !");
            onSolved.Invoke();

            // Optionnel : magnétiser l'objet sur la cible exacte
            transform.position = positionCible;
        }
        else
        {
            Debug.Log("Trop loin...");
            // Optionnel : Ici tu pourrais le faire retourner à sa position de départ
        }
    }
}**/