using UnityEngine;

public class Haut : MonoBehaviour
{
    public void haut()
    {
        transform.Translate(0f, 5f, 0f, Space.World);
        Debug.Log(transform.position);

    }
}
