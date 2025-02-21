using _1_Code;
using UnityEngine;

[RequireComponent(typeof(Airport))]
public class AirportInteractable : MonoBehaviour, IInteractable
{
    private Airport _airport;
    
    private void Awake()
    {
        _airport = GetComponent<Airport>();
    }

    public void OnInteract()
    {
        var selectedPlane = SelectionManager.Instance.SelectedPlane;
        if (!selectedPlane)
        {
            // No plane selected, airport click might do nothing or show a message
            return;
        }

        // If the plane is already landed here, do nothing:
        if (selectedPlane.CurrentAirport == _airport)
        {
            // Optionally log or hide the UI selection
            Debug.Log("This plane is already landed at this airport. Doing nothing...");
            // Clear selection if you want
            // selectionManager.ClearSelection();
            return;
        }

        // If not landed here, send it:
        FlightManager.SendPlaneToAirport(selectedPlane, _airport);
        // Clear the selection
        SelectionManager.Instance.ClearSelection();
    }
}
