using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE] Molette qui tourne d'un cran à chaque appui (doigt ou rayon).
/// currentIndex = numéro du symbole affiché en haut (0 = premier symbole).
/// Prévient le RotationPuzzle parent à chaque cran.
/// </summary>
[RequireComponent(typeof(XRSimpleInteractable))]
public class RotatingDial : MonoBehaviour
{
    [Tooltip("Nombre de symboles sur la molette")]
    public int symbolCount = 4;

    [Tooltip("Symbole affiché en haut au départ")]
    public int currentIndex = 0;

    XRSimpleInteractable interactable;
    RotationPuzzle puzzle;
    Quaternion startRotation;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        puzzle = GetComponentInParent<RotationPuzzle>();
        startRotation = transform.localRotation;
        transform.localRotation = GetRotation(currentIndex);

        if (puzzle == null)
        {
            Debug.LogError("RotatingDial : la molette " + gameObject.name + " doit être un enfant d'un objet avec le script RotationPuzzle.");
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        currentIndex = currentIndex + 1;
        if (currentIndex >= symbolCount)
        {
            currentIndex = 0;
        }

        StopAllCoroutines();
        StartCoroutine(TurnAnimation(GetRotation(currentIndex)));

        if (puzzle != null)
        {
            puzzle.OnDialTurned();
        }
    }

    // Les symboles sont placés dans le sens des aiguilles d'une montre :
    // on tourne la molette dans l'autre sens pour amener le symbole en haut.
    Quaternion GetRotation(int index)
    {
        float angle = 360f / symbolCount * index;
        return startRotation * Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    IEnumerator TurnAnimation(Quaternion target)
    {
        Quaternion from = transform.localRotation;
        float time = 0f;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(from, target, time / 0.2f);
            yield return null;
        }
        transform.localRotation = target;
    }
}
