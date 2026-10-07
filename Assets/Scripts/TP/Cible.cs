using UnityEngine;

public class Serrure : MonoBehaviour
{
    public EscapeManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cle"))
            manager.PorteOuverte();
    }
}