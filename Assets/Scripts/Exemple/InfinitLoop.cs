using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Boucles / labyrinthe infini : chaque trigger (LoopExit) téléporte le joueur
/// sans qu'il s'en rende compte (décor identique ou zone sombre).
/// Option : il faut prendre le bon chemin plusieurs tours de suite pour s'échapper.
/// À mettre sur un GameObject vide (ex. "puzzle_infinite_maze").
/// </summary>
public class InfiniteLoop : MonoBehaviour
{
    [Header("Joueur")]
    [Tooltip("Tag du XR Origin")]
    public string playerTag = "Player";

    [Header("Boucle")]
    [Tooltip("Point d'arrivée par défaut (utilisé par les LoopExit dont 'Custom Destination' est vide)")]
    public Transform loopStart;

    [Tooltip("Bon chemin à chaque tour (Path Id des LoopExit). Ex : 0, 1, 0. Mets 9 pour une boucle sans fin")]
    public int[] correctPaths = { 9 };

    [Tooltip("Indice visible à chaque tour (Element 0 = 1er tour). Optionnel")]
    public GameObject[] stepHints;

    [Header("Sortie")]
    [Tooltip("Où aller quand l'énigme est résolue. Vide = pas de téléportation")]
    public Transform exitDestination;

    [Header("Quand le joueur se trompe (son, lumière qui vacille...)")]
    public UnityEvent onWrongPath;

    [Header("Quand le joueur s'échappe")]
    public UnityEvent onSolved;

    int step = 0;
    bool solved = false;
    float lastTeleport = -10f;
    Transform player;
    CharacterController controller;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
        {
            player = p.transform;
            controller = p.GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogError("InfiniteLoop : aucun objet avec le tag " + playerTag);
        }

        if (correctPaths.Length == 0) correctPaths = new int[] { 9 };

        ShowHint();
    }

    // Appelé par LoopExit quand le joueur entre dans un trigger
    public void OnPathTaken(LoopExit exit)
    {
        if (solved || player == null) return;

        // Le joueur a plusieurs colliders (corps, tête, mains) : on ne réagit qu'une fois
        if (Time.time - lastTeleport < 0.5f) return;

        Transform from = exit.exitPoint != null ? exit.exitPoint : exit.transform;
        Transform destination = exit.customDestination != null ? exit.customDestination : loopStart;

        if (exit.pathId == correctPaths[step])
        {
            step = step + 1;
            Debug.Log("Bon chemin ! Tour " + step + " / " + correctPaths.Length);

            if (step >= correctPaths.Length)
            {
                solved = true;
                lastTeleport = Time.time;
                ShowHint();
                Debug.Log("Boucle résolue !");
                if (exitDestination != null) Teleport(from, exitDestination);
                onSolved.Invoke();
                return;
            }
        }
        else
        {
            Debug.Log("Mauvais chemin (" + exit.name + ") : retour au tour 1");
            step = 0;
            onWrongPath.Invoke();
        }

        ShowHint();

        if (destination == null)
        {
            Debug.LogError("InfiniteLoop : " + exit.name + " n'a pas de 'Custom Destination' et 'Loop Start' est vide.");
            return;
        }

        lastTeleport = Time.time;
        Teleport(from, destination);
    }

    // Affiche uniquement l'indice du tour en cours
    void ShowHint()
    {
        if (stepHints == null) return;
        for (int i = 0; i < stepHints.Length; i++)
        {
            if (stepHints[i] != null) stepHints[i].SetActive(!solved && i == step);
        }
    }

    // Déplace le joueur en gardant sa position et son orientation
    // par rapport aux repères : il ne voit aucune différence
    void Teleport(Transform from, Transform to)
    {
        float yaw = to.eulerAngles.y - from.eulerAngles.y;
        Quaternion turn = Quaternion.Euler(0f, yaw, 0f);
        Vector3 offset = player.position - from.position;

        bool ccOn = controller != null && controller.enabled;
        if (ccOn) controller.enabled = false; // sinon le Character Controller annule le déplacement

        player.SetPositionAndRotation(to.position + turn * offset, turn * player.rotation);

        if (ccOn) controller.enabled = true;
        Physics.SyncTransforms();
    }

    // Remonte les parents : marche avec le collider du corps, de la tête ou des mains
    public bool IsPlayer(Transform t)
    {
        while (t != null)
        {
            if (t.CompareTag(playerTag)) return true;
            t = t.parent;
        }
        return false;
    }
}