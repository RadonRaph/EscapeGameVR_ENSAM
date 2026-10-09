using UnityEngine;

public class ButtonOpenDoor : MonoBehaviour
{
    public Vector3 positionCible = new Vector3(1.3f, 0f, 2.25f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void verification()
    {
        float distance = Vector3.Distance(transform.position, positionCible);
        if (distance < 0.1f)
        {

            transform.Translate(0f, 1.8f, 0f, Space.World);
        }
    }
}
