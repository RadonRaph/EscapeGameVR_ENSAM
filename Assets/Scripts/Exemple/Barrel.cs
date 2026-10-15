using UnityEngine;

public class Barrel : MonoBehaviour
{
    public ParticleSystem Fume;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void gauche()
    {
        transform.Translate(0f, 0f, 1f, Space.World);
        Debug.Log(transform.position);
        if (Fume != null)
        {
            Fume.transform.position = transform.position;
            Fume.Play();

        }
    }
    public void droite()
    {
        transform.Translate(0f, 0f, -0.25f, Space.World);
        Debug.Log(transform.position);
        if (Fume != null)
        {
            Fume.transform.position = transform.position;
            Fume.Play();

        }

    }
    
}
