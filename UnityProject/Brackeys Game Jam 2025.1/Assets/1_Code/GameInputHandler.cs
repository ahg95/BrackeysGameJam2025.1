using _1_Code.Interaction;
using _1_Code.Selection_Visualization.QuickOutline.Scripts;
using UnityEngine;

namespace _1_Code
{
    public class GameInputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameObject _lastHoveredObject; // Track the last hovered GameObject

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleHover();
            HandleInteraction();
        }

        private void HandleHover()
        {
            // Cast a ray from the mouse position to detect hovered objects
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hoveredObject = hit.collider.gameObject;

                // Check if the hovered object has an Outline component
                Outline outline = hoveredObject.GetComponent<Outline>();

                if (hoveredObject != _lastHoveredObject)
                {
                    // Disable outline on the previously hovered object
                    if (_lastHoveredObject)
                    {
                        Outline previousOutline = _lastHoveredObject.GetComponent<Outline>();
                        if (previousOutline)
                        {
                            previousOutline.enabled = false;
                        }
                    }

                    // Enable outline on the currently hovered object
                    if (outline)
                    {
                        outline.enabled = true;
                    }

                    // Update the last hovered object
                    _lastHoveredObject = hoveredObject;
                }
            }
            else if (_lastHoveredObject)
            {
                // If nothing is hovered, disable outline on the last hovered object
                Outline previousOutline = _lastHoveredObject.GetComponent<Outline>();
                if (previousOutline)
                {
                    previousOutline.enabled = false;
                }
                _lastHoveredObject = null;
            }
        }

        private void HandleInteraction()
        {
            // Detect clicks and interact with objects under the mouse
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Check if the clicked object implements IInteractable
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        // Delegate the interaction behavior to the object
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