using UnityEngine;
using System.Collections;

public class PorteAnimee : MonoBehaviour
{

    public float vitesseDescente;

    public Vector3 depart;
    public Vector3 destination;

    public void Ouvrir()
    {
        StartCoroutine(AnimerMur());
    }

    public IEnumerator AnimerMur()
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, vitesseDescente * Time.deltaTime);
            yield return null;
        }
    }
}
