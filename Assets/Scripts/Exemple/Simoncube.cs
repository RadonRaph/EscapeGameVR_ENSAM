using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class SimonCube : MonoBehaviour
{
    [Header("Couleurs")]
    public Color offColor = new Color(0.2f, 0.2f, 0.2f);
    public Color onColor = Color.cyan;
    public float emissionIntensity = 3f;

    [Header("Enfoncement dans le mur")]
    [Tooltip("Direction vers le mur, dans les axes LOCAUX du cube (Z = flèche bleue)")]
    public Vector3 pushDirection = Vector3.forward;
    [Tooltip("Profondeur en mètres (0.03 = 3 cm)")]
    public float pushDepth = 0.03f;
    [Tooltip("Durée pour s'enfoncer (secondes)")]
    public float pushTime = 0.08f;
    [Tooltip("Temps où le cube reste enfoncé")]
    public float holdTime = 0.1f;

    [Header("Toucher")]
    [Tooltip("Évite les doubles appuis trop rapprochés")]
    public float touchCooldown = 0.4f;
    [Tooltip("Optionnel : toucher physique avec un collider trigger tagué sur la main")]
    public string handTag = "Hand";

    [HideInInspector] public SimonPuzzle puzzle;
    [HideInInspector] public int index;

    Material mat;
    float lastTouch = -10f;
    Vector3 restPos;
    Coroutine pushRoutine;

    void Awake()
    {
        mat = GetComponent<Renderer>().material; 
        mat.EnableKeyword("_EMISSION");
        restPos = transform.position;
        SetLit(false);
    }


    public void Touch()
    {
        if (Time.time - lastTouch < touchCooldown) return;
        lastTouch = Time.time;

        Push();
        if (puzzle != null) puzzle.OnCubeTouched(this);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag)) Touch();
    }

    public void SetLit(bool lit) => SetColor(lit ? onColor : offColor, lit);

    public void SetColor(Color c, bool glow = true)
    {
        mat.color = c;
        mat.SetColor("_EmissionColor", glow ? c * emissionIntensity : Color.black);
    }

    public void Push()
    {
        if (pushRoutine != null) StopCoroutine(pushRoutine);
        pushRoutine = StartCoroutine(PushAnim());
    }

    IEnumerator PushAnim()
    {
        Vector3 pressedPos = restPos + transform.TransformDirection(pushDirection.normalized) * pushDepth;

        yield return MoveTo(pressedPos, pushTime);
        yield return new WaitForSeconds(holdTime);
        yield return MoveTo(restPos, pushTime * 2f); // retour un peu plus lent
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.position = target;
    }
}