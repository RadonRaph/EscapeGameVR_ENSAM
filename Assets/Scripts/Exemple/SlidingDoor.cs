using UnityEngine;


public class SlidingDoor : MonoBehaviour
{
    public float openHeight = 3f;
    public float duration = 2f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
  

    Vector3 closedPos;
    bool opening;
    float t;

    void Awake() => closedPos = transform.position;

    public void Open()
    {
        if (opening) return;
        opening = true;
    }

    void Update()
    {
        if (!opening || t >= 1f) return;
        t = Mathf.Min(1f, t + Time.deltaTime / duration);
        transform.position = closedPos + Vector3.up * openHeight * ease.Evaluate(t);
    }
}