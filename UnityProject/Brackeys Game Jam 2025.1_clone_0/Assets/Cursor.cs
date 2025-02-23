using UnityEngine;

public class Cursor : MonoBehaviour
{
    void Start()
    {
        // Hide and lock the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}
