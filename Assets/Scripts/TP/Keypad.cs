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
    public string code = "4519";

    [Tooltip("Texte qui affiche les chiffres tapés")]
    public TMP_Text display;

    // AJOUT : le message affiché quand le code est faux (modifiable dans l'Inspector)
    [Tooltip("Message affiché quand le code est faux")]
    public string wrongMessage = "CHERCHE ENCORE";

    [Header("Quand le bon code est tapé")]
    public UnityEvent onSolved;

    // Les chiffres tapés par le joueur
    string typed = "";

    // AJOUT : vaut true quand le message d'erreur est à l'écran
    bool showingError = false;

    // key vaut "0" à "9", "C" (effacer) ou "OK" (valider)
    public void PressKey(string key)
    {
        Debug.Log("Touche : " + key);

        // AJOUT : si le message d'erreur est affiché, on repart de zéro
        // (sinon on verrait "CHERCHE ENCORE1" en tapant un chiffre)
        if (showingError)
        {
            showingError = false;
            typed = "";
        }

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

                // AJOUT : on affiche le message d'erreur sur l'écran
                showingError = true;
                display.text = wrongMessage;

                // AJOUT : return arrête la fonction ici, pour que la ligne
                // "display.text = typed;" en bas n'efface pas le message
                return;
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