using UnityEngine;
using TMPro;

public class EscapeManager : MonoBehaviour
{
    public int ciblesNecessaires = 3;
    public GameObject grille;        // grille de l'étape 1
    public Transform tiroir;         // tiroir de la sideTable
    public GameObject grilleSortie;  // grille de la porte de sortie
    public float tempsTotal = 300f;
    public TMP_Text texteChrono;
    public TMP_Text texteMessage;

    int compteur = 0;
    bool fini = false;

    void Update()
    {
        if (fini) return;
        tempsTotal -= Time.deltaTime;
        if (texteChrono != null)
            texteChrono.text = Mathf.CeilToInt(tempsTotal).ToString();
        if (tempsTotal <= 0) { fini = true; Message("Temps écoulé !"); }
    }

    public void CibleTouchee()
    {
        compteur++;
        if (compteur >= ciblesNecessaires)
        {
            grille.SetActive(false);
            Message("Grille levée : cherche le code !");
        }
    }

    public void CodeCorrect()
    {
        tiroir.localPosition += new Vector3(0, 0, 0.3f);
        Message("Le tiroir s'ouvre !");
    }

    public void PorteOuverte()
    {
        grilleSortie.SetActive(false);
        fini = true;
        Message("Bravo, tu es libre !");
    }

    void Message(string m)
    {
        if (texteMessage != null) texteMessage.text = m;
    }
}