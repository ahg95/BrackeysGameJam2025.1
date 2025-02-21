using DG.Tweening;
using UnityEngine;

namespace _1_Code
{
    public class FlightManager : MonoBehaviour
    {
        private static FlightManager Instance { get; set; }
        private static float FlightSpeed => Instance.flightSpeed;

        [Tooltip("Speed at which the plane travels between airports.")] [SerializeField]
        private float flightSpeed = 3f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Multiple instances of FlightManager detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Sends a plane to a destination airport.
        /// </summary>
        /// <param name="plane">The plane to be sent.</param>
        /// <param name="destinationAirport">The destination airport.</param>
        public static void SendPlaneToAirport(Plane plane, Airport destinationAirport)
        {
            if (!plane || !destinationAirport)
            {
                Debug.LogError("Plane or destination airport reference is missing.");
                return;
            }

            if (plane.CurrentAirport == destinationAirport)
            {
                Debug.LogWarning("The plane is already landed at the selected airport.");
                return;
            }

            var currentAirport = plane.CurrentAirport;
            if (currentAirport)
            {
                currentAirport.RemovePlane(plane);
            }

            // Mark that the plane is taking off.
            plane.DepartAirport();

            // Delegate movement to the Plane.
            plane.MoveToAirport(destinationAirport, FlightSpeed);
        }
    }
}