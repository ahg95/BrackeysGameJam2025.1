using DG.Tweening;
using UnityEngine;

namespace _1_Code
{
    public class FlightManager : MonoBehaviour
    {
        [Tooltip("Speed at which the plane travels between airports.")] [SerializeField]
        private float flightSpeed = 5f;

        /// <summary>
        /// Sends a plane to a destination airport.
        /// </summary>
        /// <param name="plane">The plane to be sent.</param>
        /// <param name="destinationAirport">The airport where the plane is sent.</param>
        public void SendPlaneToAirport(Plane plane, Airport destinationAirport)
        {
            if (plane == null || destinationAirport == null)
            {
                Debug.LogError("Plane or destination airport reference is missing.");
                return;
            }

            // Start moving the plane towards the destination airport.
            MovePlane(plane, destinationAirport);
        }

        /// <summary>
        /// Moves a plane to the specified destination airport over time.
        /// </summary>
        /// <param name="plane">The plane to be moved.</param>
        /// <param name="destinationAirport">The destination airport to which the plane will travel.</param>
        private void MovePlane(Plane plane, Airport destinationAirport)
        {
            Vector3 targetPosition = destinationAirport.transform.position;

            // Use DOTween to move the plane
            plane.transform.DOMove(targetPosition,
                    Vector3.Distance(plane.transform.position, targetPosition) / flightSpeed)
                .OnComplete(() =>
                {
                    // Once arrived, tell the destination airport to process the plane (disembark/board).
                    destinationAirport.ProcessPlane(plane);
                    Debug.Log("Plane has arrived at " + destinationAirport.name + " and is processing passengers.");
                });
        }
    }
}