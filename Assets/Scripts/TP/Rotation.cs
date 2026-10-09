using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float vitesse = 90f; // degrés par seconde

    void Update()
    {
        transform.Rotate(0f, vitesse * Time.deltaTime, 0f);
    }
}