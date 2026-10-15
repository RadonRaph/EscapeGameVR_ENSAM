using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// À mettre sur un GameObject vide "SimonPuzzle".
public class SimonPuzzle : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Les 6 cubes. Leur position dans la liste = leur index (0 à 5)")]
    public SimonCube[] cubes = new SimonCube[6];

    // Comme dans le TP : actions réglées dans l'Inspector (ex. SlidingDoor.Open)
    [Header("Quand l'énigme est résolue")]
    public UnityEvent onSolved;

    [Header("Séquence")]
    [Tooltip("Ordre d'illumination, en index de cubes (0 à 5)")]
    public int[] sequence = { 0, 3, 1, 5, 2, 4 };
    [Tooltip("Ignore la séquence ci-dessus et en génère une au hasard")]
    public bool randomSequence = false;

    [Header("Démarrage")]
    [Tooltip("Coché : la séquence se joue au lancement. Décoché : il faut appeler StartPuzzle() (ex. zone trigger)")]
    public bool playOnStart = false;

    [Header("Timings (secondes)")]
    public float startDelay = 1f;
    public float flashTime = 0.6f;
    public float gapTime = 0.25f;
    public float replayDelay = 1.5f;

    [Header("Feedback")]
    public Color errorColor = Color.red;

    int step;
    bool acceptingInput;
    bool solved;
    bool started;
    readonly HashSet<int> validated = new HashSet<int>();

    void Start()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].puzzle = this;
            cubes[i].index = i;
        }

        if (randomSequence) sequence = Shuffle(cubes.Length);

        foreach (int i in sequence)
        {
            if (i < 0 || i >= cubes.Length)
            {
                Debug.LogError($"SimonPuzzle : index {i} hors limites dans la séquence.");
                enabled = false;
                return;
            }
        }

        if (playOnStart) StartPuzzle();
    }

    // "public" : à brancher sur On Enter d'une TriggerZone
    public void StartPuzzle()
    {
        if (started || solved || !enabled) return; // ne démarre qu'une fois
        started = true;
        StartCoroutine(PlaySequence(startDelay));
    }

    // Montre la séquence au joueur
    IEnumerator PlaySequence(float delay)
    {
        acceptingInput = false;
        step = 0;
        validated.Clear();
        AllOff();
        yield return new WaitForSeconds(delay);

        foreach (int i in sequence)
        {
            cubes[i].SetLit(true);
            yield return new WaitForSeconds(flashTime);
            cubes[i].SetLit(false);
            yield return new WaitForSeconds(gapTime);
        }

        acceptingInput = true;
    }

    // Appelé par SimonCube quand le joueur touche un cube
    public void OnCubeTouched(SimonCube cube)
    {
        if (!acceptingInput || solved) return;

        // Re-toucher un cube déjà validé par erreur = ignoré
        if (cube.index != sequence[step] && validated.Contains(cube.index)) return;

        if (cube.index == sequence[step])
        {
            cube.SetLit(true); // reste allumé
            validated.Add(cube.index);
            step++;
            if (step >= sequence.Length) StartCoroutine(Success());
        }
        else
        {
            StartCoroutine(Fail());
        }
    }

    IEnumerator Success()
    {
        solved = true;
        acceptingInput = false;
        yield return new WaitForSeconds(0.3f);

        foreach (var c in cubes) c.SetLit(true);
        Debug.Log("Énigme Simon résolue !");
        onSolved.Invoke();
    }

    IEnumerator Fail()
    {
        acceptingInput = false;

        // Clignotement rouge x2
        for (int n = 0; n < 2; n++)
        {
            foreach (var c in cubes) c.SetColor(errorColor);
            yield return new WaitForSeconds(0.25f);
            AllOff();
            yield return new WaitForSeconds(0.25f);
        }

        // On rejoue la séquence
        yield return PlaySequence(replayDelay);
    }

    void AllOff()
    {
        foreach (var c in cubes) c.SetLit(false);
    }

    static int[] Shuffle(int n)
    {
        int[] a = new int[n];
        for (int i = 0; i < n; i++) a[i] = i;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (a[i], a[j]) = (a[j], a[i]);
        }
        return a;
    }
}