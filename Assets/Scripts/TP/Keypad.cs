using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// [TP] Keypad : le joueur tape un code puis appuie sur OK.
/// Chaque touche (XR Simple Interactable) appelle PressKey avec sa valeur
/// grâce à son événement Select Entered (voir l'Inspector d'une touche).
/// Voir Prefabs/Enigmes/Keypad/README_Keypad.md
/// </summary>
public class Keypad : MonoBehaviour
{
    [Header("Keypad")]
    [Tooltip("Le code à trouver")]
    public string code = "1234";

    [Tooltip("Texte qui affiche les chiffres tapés")]
    public TMP_Text display;

    [Header("Quand le bon code est tapé")]
    public UnityEvent onSolved;

    // Les chiffres tapés par le joueur
    string typed = "";

    // key vaut "0" à "9", "C" (effacer) ou "OK" (valider)
    public void PressKey(string key)
    {
        Debug.Log("Touche : " + key);

        // TODO 1 : "C" -> on efface
        if (key == "C")
        {
            typed = "";
        }
        // TODO 2 : "OK" -> on vérifie le code
        else if (key == "OK")
        {
            if (typed == code)
            {
                Debug.Log("Bon code ! Énigme résolue");
                onSolved.Invoke();
            }
            else
            {
                Debug.Log("Mauvais code");
                typed = "";
            }
        }
        // TODO 3 : sinon c'est un chiffre -> on l'ajoute
        else
        {
            typed = typed + key;
        }

        // TODO 4 : on affiche le résultat dans tous les cas
        display.text = typed;
    }
}