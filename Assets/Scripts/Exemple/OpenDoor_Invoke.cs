using UnityEngine;

public class OpenDoor_Invoke : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [ContextMenu("Bouger")]    
    
    public void bougerPorte()
    {
        transform.Translate(0f, 5f, 0f,Space.World);
    }

}
