using System;
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
            Vector3 targetPosition = destinationAirport.transform.position + Vector3.up;
            float travelTime = Vector3.Distance(plane.transform.position, targetPosition) / FlightSpeed;

            // Use DOTween to move the plane.
            plane.transform.DOMove(targetPosition, travelTime)
                .OnComplete(() =>
                {
                    // Once arrived, tell the destination airport to process the plane.
                    destinationAirport.ProcessPlane(plane);
                    // Set the plane's current airport.
                    plane.LandAtAirport(destinationAirport);
                    Debug.Log("Plane has arrived at " + destinationAirport.name + " and is processing passengers.");
                });
        }
    }
}