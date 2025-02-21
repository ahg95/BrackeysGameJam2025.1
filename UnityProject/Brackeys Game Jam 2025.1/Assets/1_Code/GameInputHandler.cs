using _1_Code.Interaction;
using UnityEngine;

namespace _1_Code
{
    public class GameInputHandler : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // See if the clicked object implements IInteractable.
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        // Delegate the final behavior to the clicked object itself.
                        interactable.OnInteract();
                    }
                    else
                    {
                        Debug.Log("Clicked on a non-interactable object.");
                    }
                }
            }
        }
    }
}