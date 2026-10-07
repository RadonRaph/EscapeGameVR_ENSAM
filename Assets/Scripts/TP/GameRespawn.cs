using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    public float treshold;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < treshold)
        {
            transform.position = new Vector3(0f, 0f, -2f);
        }
    }
}
