using TMPro;
using UnityEngine;

/// <summary>
/// [TP] Keypad : le joueur tape un code puis appuie sur OK.
/// Chaque touche (KeypadButton) appelle PressKey avec sa valeur.
/// Voir Prefabs/Enigmes/Keypad/README_Keypad.md
/// </summary>
public class Keypad : Puzzle
{
    [Header("Keypad")]
    [Tooltip("Le code à trouver")]
    public string code = "1234";

    [Tooltip("Texte qui affiche les chiffres tapés")]
    public TMP_Text display;

    // Les chiffres tapés par le joueur
    string typed = "";

    // key vaut "0" à "9", "C" (effacer) ou "OK" (valider)
    public void PressKey(string key)
    {
        Debug.Log("Touche : " + key);

        // TODO 1 : si key vaut "C", vider typed

        // TODO 2 : si key vaut "OK", comparer typed avec code
        //          - si c'est le bon code : appeler Solve()
        //          - sinon : vider typed

        // TODO 3 : sinon (c'est un chiffre), ajouter key à la fin de typed

        // TODO 4 : afficher typed dans display (propriété .text)
    }
}
