using System;
using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _1_Code
{
    public class Airport : BasePassengerHolder
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private DestinationColor airportColor = DestinationColor.Blue;
        [SerializeField] private List<Plane> planes = new List<Plane>();
        [SerializeField] private int maxPlanes = 3; // Maximum airport plane capacity (default is 3).

        [Tooltip(
            "Designated landing spots to prevent overlapping planes. The size of this array should be at least equal to Max Planes.")]
        [SerializeField]
        private Transform[] landingSpots;

        [Header("Debug")] [SerializeField] private bool startWithRandomPassengers = false;

        private Queue<DestinationColor> _passengerQueue = new Queue<DestinationColor>();
        [SerializeField] private List<DestinationColor> passengerList = new List<DestinationColor>();

        private void Awake()
        {
            UpdateAirportMaterialColor();
            if (startWithRandomPassengers)
                PopulateAirport();
        }

        // Populates the airport queue with random passengers.
        private void PopulateAirport()
        {
            for (var i = 0; i < maxCapacity; i++)
            {
                DestinationColor randomPassengerColor;
                do
                {
                    randomPassengerColor = (DestinationColor)Random.Range(0,
                        Enum.GetValues(typeof(DestinationColor)).Length);
                } while (randomPassengerColor == airportColor);

                _passengerQueue.Enqueue(randomPassengerColor);
                passengerList.Add(randomPassengerColor);
            }

            Debug.Log($"Airport populated with {_passengerQueue.Count} passengers.");
        }

        private void Start()
        {
            PopulatePlanes();
        }

        /// <summary>
        /// Populates the planes at the airport with passengers from the airport queue,
        /// ensuring that each plane is filled to its capacity or until the passenger
        /// queue is empty.
        /// </summary>
        private void PopulatePlanes()
        {
            foreach (var plane in planes)
            {
                plane.LandAtAirport(this);
                if (plane.IsFull)
                    continue; // If the plane is full, skip it.
                if (_passengerQueue.Count == 0)
                    break; // No more passengers? Stop.

                var remainingSeatsInPlane = plane.RemainingCapacity;

                // Iterating through the remaining seats in the plane.
                for (var i = 0; i < remainingSeatsInPlane; i++)
                {
                    if (!_passengerQueue.TryDequeue(out var passengerColor))
                        break; // No more passengers? Stop.
                    passengerList.Remove(passengerColor);
                    Debug.Log($"{passengerColor} boarded {plane.name}.");
                    plane.AddPassengers(passengerColor);
                }
            }

            // Update landing positions in case any planes have landed already.
            UpdatePlaneLandingPositions();
        }

        /// <summary>
        /// Manages the boarding and disembarking process of passengers on a plane at the airport.
        /// </summary>
        /// <param name="plane">The plane object that will undergo the boarding and disembarking process.</param>
        public void ProcessPlane(Plane plane)
        {
            // Disembark passengers on the plane that match the airport's color.
            int disembarkedCount = 0;
            while (plane.RemovePassengers(airportColor, 1))
            {
                disembarkedCount++;
            }

            Debug.Log($"Plane disembarked {disembarkedCount} passenger(s) of color {airportColor}.");

            // Board the plane with passengers from the airport queue.
            int boardedCount = 0;
            var remainingSeatsInPlane = plane.RemainingCapacity;

            for (var i = 0; i < remainingSeatsInPlane; i++)
            {
                if (_passengerQueue.Count == 0)
                    break; // No more passengers? Stop.

                if (!_passengerQueue.TryDequeue(out var passengerColor))
                    break;
                passengerList.Remove(passengerColor);
                plane.AddPassengers(passengerColor);
                boardedCount++;
            }

            Debug.Log($"Plane boarded {boardedCount} passenger(s).");
        }

        /// <summary>
        /// Processes the arrival of a plane. If the airport is full, triggers the plane explosion.
        /// Otherwise, the plane is assigned a designated landing spot.
        /// </summary>
        /// <param name="plane">The arriving plane.</param>
        public void ProcessPlaneArrival(Plane plane)
        {
            // If there's no space left for the plane, explode the plane.
            if (!HasSpace())
            {
                plane.ExplodePlane(); // Explode the plane
                Debug.Log($"Plane {plane.name} exploded upon arrival to {name}, as the airport is at capacity.");
                return;
            }

            // If there is space, allow the plane to land and process it.
            AddPlane(plane);
            ProcessPlane(plane);

            // Update landing spots so that the plane snaps to its designated transform.
            UpdatePlaneLandingPositions();
        }

        /// <summary>
        /// Adds a plane to the airport's list.
        /// </summary>
        /// <param name="plane">The plane to add.</param>
        private void AddPlane(Plane plane)
        {
            if (planes.Contains(plane))
                return;
            planes.Add(plane);
        }

        /// <summary>
        /// Removes a plane from the airport's list (e.g., when it departs or is destroyed) and updates landing assignments.
        /// </summary>
        /// <param name="plane">The plane to remove.</param>
        public void RemovePlane(Plane plane)
        {
            if (planes.Contains(plane))
            {
                planes.Remove(plane);
                UpdatePlaneLandingPositions();
            }
        }

        /// <summary>
        /// Checks if the airport has space for more planes.
        /// </summary>
        /// <returns>True if the airport can hold more planes, false otherwise.</returns>
        private bool HasSpace() => planes.Count < maxPlanes;

        /// <summary>
        /// Updates the positions of planes to snap them to their designated landing spots.
        /// It assigns landing spots based on the order in the planes list. If a leftover plane does not 
        /// have an assigned landing spot (because there are fewer landing spots than planes), it will remain at its current position.
        /// </summary>
        private void UpdatePlaneLandingPositions()
        {
            if (landingSpots == null || landingSpots.Length == 0)
                return; // No landing spots assigned. Do nothing.

            for (int i = 0; i < planes.Count; i++)
            {
                if (i < landingSpots.Length && planes[i] != null)
                {
                    planes[i].transform.position = landingSpots[i].position;
                }
            }
        }

        private void UpdateAirportMaterialColor()
        {
            // Updates the visual indication of the airport's assigned color using MaterialPropertyBlock
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                var materialPropertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetColor(BaseColor, airportColor.GetColor());
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        private void OnValidate()
        {
            // Ensures that maxPlanes is at least 1 (sensible minimum).
            /*if (maxPlanes < 1)
            {
                maxPlanes = 1;
                Debug.LogWarning("Max planes for an airport cannot be less than 1. Setting to 1.");
            }*/

            // Optionally, warn if there are not enough landing spots for the maximum planes.
            if (landingSpots != null && landingSpots.Length < maxPlanes)
            {
                Debug.LogWarning(
                    "The number of landing spots is less than the maximum planes allowed. Some planes may overlap.");
            }

            UpdateAirportMaterialColor();
        }

        /// <summary>
        /// Adds passengers of a specified color to the current passenger holder
        /// while ensuring that the total capacity is not exceeded.
        /// </summary>
        public override bool AddPassengers(DestinationColor passengerColor, int count = 1)
        {
            if (_passengerQueue.Count + count > maxCapacity)
                return false;

            for (var i = 0; i < count; i++)
            {
                _passengerQueue.Enqueue(passengerColor);
                passengerList.Add(passengerColor);
            }

            return true;
        }

        /// <summary>
        /// Removes a specified number of passengers of a given color from the passenger queue.
        /// </summary>
        public override bool RemovePassengers(DestinationColor passengerColor, int count = 1)
        {
            int removedCount = 0;
            Queue<DestinationColor> newQueue = new Queue<DestinationColor>();
            while (_passengerQueue.Count > 0)
            {
                var passenger = _passengerQueue.Dequeue();
                if (passenger == passengerColor && removedCount < count)
                {
                    removedCount++;
                    passengerList.Remove(passenger);
                }
                else
                {
                    newQueue.Enqueue(passenger);
                }
            }

            _passengerQueue = newQueue;
            return removedCount == count;
        }
    }
}