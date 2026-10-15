using UnityEngine;
using UnityEngine.Events;

public class PretendHasard : MonoBehaviour
{
    public GameObject objetAttendu;
    public UnityEvent onSolved;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == objetAttendu)
        {
            Debug.Log("Bon objet posé, énigme vakidée !");
            onSolved.Invoke();
        }
    }
}
