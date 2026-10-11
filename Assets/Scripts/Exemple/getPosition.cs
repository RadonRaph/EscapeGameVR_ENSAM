using UnityEngine;
using UnityEngine.Events;

public class getPosition : MonoBehaviour
{
    [SerializeField] private GameObject monObjet;
    public UnityEvent onSolved;
    public Vector3 positionCible = new Vector3(1.3f, 0f, 2.25f);
    public void Press()
    {
        Debug.Log("ible  :"+ positionCible);
        Debug.Log("Objet  :"+ monObjet.transform.localPosition);


        float distance = Vector3.Distance(monObjet.transform.localPosition, positionCible);
        if (distance < 0.5f)
        {
            Debug.Log("Énigme résolue !");
            onSolved.Invoke();
            //transform.Translate(0f, 1.8f, 0f, Space.World);
        }
    }
}
