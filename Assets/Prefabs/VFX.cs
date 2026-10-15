using UnityEngine;

public class VFX : MonoBehaviour
{
    public ParticleSystem Fume;
    public void PopFume()
    {
        if (Fume != null)
        {
            Fume.transform.position = transform.position;
            Fume.Play();

        }
    }

}
