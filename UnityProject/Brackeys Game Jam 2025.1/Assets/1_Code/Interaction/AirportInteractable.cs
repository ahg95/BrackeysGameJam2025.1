using UnityEngine;

namespace _1_Code.Interaction
{
    [RequireComponent(typeof(Airport))]
    public class AirportInteractable : MonoBehaviour, IInteractable
    {
        private Airport _airport;
    
        private void Awake() => _airport = GetComponent<Airport>();

        public void OnInteract()
        {
            var selectedPlane = SelectionManager.Instance.SelectedPlane;
            if (!selectedPlane)
            {
                // No plane is selected; airport click might do nothing or show a message.
                return;
            }

            // If the plane is already landed here, do nothing.
            if (selectedPlane.CurrentAirport == _airport)
            {
                Debug.Log("This plane is already landed at this airport. Doing nothing...");
                return;
            }

            // If not landed here, send it.
            FlightManager.SendPlaneToAirport(selectedPlane, _airport);
            // Clear the selection after the plane has been sent.
            UnInteract();
        }

        /// <summary>
        /// UnInteract clears any selected plane.
        /// </summary>
        public void UnInteract()
        {
            
        }
    }
}