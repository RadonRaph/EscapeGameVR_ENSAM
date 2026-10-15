using UnityEngine;
using UnityEngine.Events;

public class PretendHasard : MonoBehaviour
{
    public GameObject objetAttendu;
    public UnityEvent onSolved;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ALERTE : Quelque chose est entré dans la zone ! C'est" + other.gameObject.name);

        if (other.gameObject == objetAttendu)
        {
            Debug.Log("VICTOIRE : Bon objet posé, énigme validée !");
            onSolved.Invoke();
        }
        else
        {
            Debug.Log("ÉCHEC : L'objet posé est :" + other.gameObject.name);
        }
    }
}