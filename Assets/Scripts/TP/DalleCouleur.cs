using UnityEngine;

public class DalleCouleur : MonoBehaviour
{
    [Tooltip("Glissez ici le nouveau matériau que doit prendre l'objet")]
    public Material newMaterial;

    [Tooltip("Glissez ici les morceaux de la clé (Anneau, Tige, Dent1, Dent2)")]
    public Renderer[] partiesCle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Detecter dans la zone");
            for (int i = 0; i < partiesCle.Length; i++)
            {
                partiesCle[i].material = newMaterial;
            }
        }
    }
    
}
