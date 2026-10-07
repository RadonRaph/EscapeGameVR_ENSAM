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
        //todo1
        if (key == "C")
        {
            typed = "";
        //todo2
        } 
        else if (key == "OK")
        {
            if (typed == code)
            {
                onSolved.Invoke();
            }
            else
            {
                typed = "";
            }
         } 
         else 
         {
                typed = typed + key;
         }
    
        display.text = typed;

        //todo1
        if (key == "C")
        {
            typed = "";
            //todo2
        }
        else if (key == "OK")
        {
            if (typed == code)
            {
                onSolved.Invoke();
            }
            else
            {
                typed = "";
            }
        }
        else
        {
            typed = typed + key;
        }

        display.text = typed;

        // TODO 1 : si key vaut "C", vider typed.
        //          Astuce : on compare deux textes avec ==   ->   if (key == "C") { ... }
        //          Vider un texte : typed = "";

        // TODO 2 : sinon, si key vaut "OK", comparer typed avec code :
        //          - si c'est le bon code : déclencher l'événement onSolved avec onSolved.Invoke();
        //          - sinon : vider typed (le joueur recommence)
        //          Astuce : pour enchaîner les cas -> if (...) { ... } else if (...) { ... } else { ... }
        //          Un if peut être écrit dans un autre if.

        // TODO 3 : sinon (c'est un chiffre), ajouter key à la fin de typed.
        //          Astuce : on colle deux textes avec +   ->   typed = typed + key;

        // TODO 4 : afficher typed sur l'écran du keypad (APRÈS les if, pour tous les cas).
        //          Astuce : le texte affiché est dans la propriété text   ->   display.text = ...;
    }
}

