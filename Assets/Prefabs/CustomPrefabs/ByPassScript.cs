using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [TP] Molettes à aligner : chaque molette (XR Simple Interactable) appelle TurnDial
/// avec son numéro grâce à son événement Select Entered (voir l'Inspector d'une molette).
/// L'énigme est résolue quand chaque molette affiche le bon symbole en haut.
/// Voir Prefabs/Enigmes/RotationPuzzle/README_RotationPuzzle.md
/// </summary>
public class ByPassScript : MonoBehaviour
{
    [SerializeField] private GameObject monObjet1;
    [SerializeField] private GameObject monObjet2;
    [SerializeField] private GameObject monObjet3;
    [Header("Molettes")]
    [Tooltip("Les molettes, de gauche à droite")]
    public Transform[] dials;
    [Tooltip("Le nouveau matériau ")]
    public Material nouveauMateriau;
    public Material Rouge;


    [Tooltip("Nombre de symboles sur chaque molette")]
    public int symbolCount = 4;

    [Tooltip("Symbole attendu pour chaque molette (0 = A, 1 = B, 2 = C, 3 = D)")]
    public int[] solution = { 3, 3, 1 };
    public int[] solution2 = { 3, 1, 1 };


    [Header("Quand les molettes sont bien alignées")]
    public UnityEvent onSolved;

    // Le symbole affiché en haut de chaque molette (0 = A)
    int[] current;

    void Start()
    {
        current = new int[dials.Length];
    }

    // index = numéro de la molette qui a été appuyée (0 = celle de gauche)
    public void TurnDial(int index)
    {
        MeshRenderer[] renderers1 = monObjet1.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] renderers2 = monObjet2.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] renderers3 = monObjet3.GetComponentsInChildren<MeshRenderer>();

        // Tourner la molette d'un cran (déjà écrit)
        dials[index].Rotate(0f, 0f, -360f / symbolCount);
        current[index] = current[index] + 1;
        if (current[index] >= symbolCount)
        {
            current[index] = 0;
        }

        foreach (MeshRenderer r in renderers1)
        {
            if (current[0] == solution[0]) { r.material = nouveauMateriau; }
            else { r.material = Rouge; }
        }

        foreach (MeshRenderer r in renderers2)
        {
            if (current[1] == solution[1]) { r.material = nouveauMateriau; }
            else { r.material = Rouge; }
        }

        foreach (MeshRenderer r in renderers3)
        {
            if (current[2] == solution[2]) { r.material = nouveauMateriau;Debug.Log("BLEU"); }
            else { r.material = Rouge; }
        }


        for (int i = 0; i < dials.Length; i++)
        {

            if (current[i] != solution[i])
            {
                Debug.Log("Molette " + i + " : " + current[i]);
                return;
            }

        }
        onSolved.Invoke();

        // TODO 1 : parcourir toutes les molettes avec une boucle for.
        //          Astuce : for (int i = 0; i < dials.Length; i++) { ... }
        //          i vaut 0, puis 1, puis 2... jusqu'au nombre de molettes (dials.Length) non compris.

        // TODO 2 : dans la boucle, si current[i] est différent de solution[i],
        //          cette molette n'est pas sur le bon symbole : arrêter la fonction.
        //          Astuce : "différent de" s'écrit !=   ->   if (current[i] != solution[i]) { return; }
        //          Pour vérifier, vous pouvez afficher la valeur de chaque molette :
        //          Debug.Log("Molette " + i + " : " + current[i]);

        // TODO 3 : APRÈS la boucle (en dehors des accolades du for) : aucune molette n'a fait return,
        //          elles sont donc toutes bonnes -> déclencher l'événement onSolved.
        //          Astuce : c
    }
}