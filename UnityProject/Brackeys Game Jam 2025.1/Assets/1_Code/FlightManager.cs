using DG.Tweening;
using UnityEngine;

namespace _1_Code
{
    public class FlightManager : MonoBehaviour
    {
        private static FlightManager Instance { get; set; }
        private static float FlightSpeed => Instance.flightSpeed;
        
        [Tooltip("Speed at which the plane travels between airports.")]
        [SerializeField] private float flightSpeed = 3f;
        
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
        /// <param name="destinationAirport">The airport where the plane is sent.</param>
        public static void SendPlaneToAirport(Plane plane, Airport destinationAirport)
        {
            if (!plane || !destinationAirport)
            {
                Debug.LogError("Plane or destination airport reference is missing.");
                return;
            }

            // Check if the plane is already landed at the destination.
            if (plane.CurrentAirport == destinationAirport)
            {
                Debug.LogWarning("The plane is already landed at the selected airport.");
                return;
            }
            
            var currentAirport = plane.CurrentAirport;
            if (currentAirport)
            {
                currentAirport.RemovePlane(plane); // Remove the plane from the current airport.
            }


            // Mark that the plane is departing.
            plane.DepartAirport();
            MovePlane(plane, destinationAirport);
        }

        /// <summary>
        /// Moves a plane to the specified destination airport over time.
        /// </summary>
        /// <param name="plane">The plane to be moved.</param>
        /// <param name="destinationAirport">The destination airport to which the plane will travel.</param>
        private static void MovePlane(Plane plane, Airport destinationAirport)
        {
            Vector3 targetPosition = destinationAirport.transform.position;
            float travelTime = Vector3.Distance(plane.transform.position, targetPosition) / FlightSpeed;

            plane.transform.DOMove(targetPosition, travelTime).OnComplete(() =>
            {
                // Check capacity and either explode or land
                destinationAirport.ProcessPlaneArrival(plane);

                // If not exploded, mark the plane as landed
                // (You could use a null check if you remove the plane after explosion.)
                plane.LandAtAirport(destinationAirport);
            });
        }

    }
}