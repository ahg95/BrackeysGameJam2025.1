using UnityEngine;

namespace _1_Code
{
    public class FlightInputHandler : MonoBehaviour
    {
        // Holds the currently selected plane.
        private Plane _selectedPlane;
    
        [Tooltip("Reference to the flight manager that handles plane movement.")]
        [SerializeField] private FlightManager flightManager;
    
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        
            if (flightManager == null)
            {
                Debug.LogError("FlightManager reference not set in FlightInputHandler!");
            }
        }

        private void Update()
        {
            // Check for left mouse click.
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Check if the clicked object has a Plane component.
                    Plane plane = hit.collider.GetComponent<Plane>();
                    // Check if the clicked object has an Airport component.
                    Airport airport = hit.collider.GetComponent<Airport>();

                    if (plane != null)
                    {
                        // Select the plane.
                        _selectedPlane = plane;
                        Debug.Log("Plane selected: " + plane.gameObject.name);
                    }
                    else if (airport != null)
                    {
                        if (_selectedPlane != null)
                        {
                            // Send the selected plane to this airport.
                            flightManager.SendPlaneToAirport(_selectedPlane, airport);
                            Debug.Log("Sending plane " + _selectedPlane.gameObject.name + " to airport " + airport.gameObject.name);
                        
                            // Clear the selection.
                            _selectedPlane = null;
                        }
                        else
                        {
                            Debug.Log("No plane selected; please click on a plane first.");
                        }
                    }
                    else
                    {
                        Debug.Log("Clicked on an object that is not part of the flight system.");
                    }
                }
            }
        }
    }
}