using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;
using UnityEngine;

namespace _1_Code
{
    public class Airport : BasePassengerHolder
    {
        [SerializeField] private DestinationColor airportColor = DestinationColor.Blue;
        [SerializeField] private List<Plane> planes = new List<Plane>();

        [Header("Debug")] [SerializeField] private bool startWithRandomPassengers = false;

        private Queue<DestinationColor> _passengerQueue = new Queue<DestinationColor>();
        [SerializeField] private List<DestinationColor> passengerList = new List<DestinationColor>();
        
        private void Awake()
        {
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
                    randomPassengerColor = (DestinationColor)UnityEngine.Random.Range(0,
                        System.Enum.GetValues(typeof(DestinationColor)).Length);
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
                if (plane.IsFull) continue; // If the plane is full, skip it.
                if (_passengerQueue.Count == 0) break; // No more passengers in the airport? Stop.

                var remainingSeatsInPlane = plane.RemainingCapacity;

                // Iterating through the remaining seats in the plane.
                for (var i = 0; i < remainingSeatsInPlane; i++)
                {
                    if (!_passengerQueue.TryDequeue(out var passengerColor)) break; // No more passengers? Stop.
                    passengerList.Remove(passengerColor);
                    Debug.Log($"{passengerColor} boarded {plane.name}.");
                    plane.AddPassengers(passengerColor);
                }
            }
        }

        /// <summary>
        /// Manages the boarding and disembarking process of passengers on a plane at the airport.
        /// </summary>
        /// <param name="plane">The plane object that will undergo the boarding and disembarking process.</param>
        public void ProcessPlane(Plane plane)
        {
            // 1. Disembark passengers on the plane that match the airport's color.
            int disembarkedCount = 0;
            // We call RemovePassengers repeatedly until no more matching passengers remain.
            while (plane.RemovePassengers(airportColor, 1))
            {
                disembarkedCount++;
            }

            Debug.Log($"Plane disembarked {disembarkedCount} passenger(s) of color {airportColor}.");

            // 2. Board the plane with passengers from the airport queue.
            int boardedCount = 0;
            // We need to rebuild the airport queue because some passengers may not be eligible.
            Queue<DestinationColor> newQueue = new Queue<DestinationColor>();

            // Attempt to board until the plane is full or there are no more passengers.
            while (_passengerQueue.Count > 0 && !plane.IsFull)
            {
                var nextPassenger = _passengerQueue.Dequeue();
                passengerList.Remove(nextPassenger);

                if (nextPassenger != airportColor)
                {
                    // Try to add the passenger to the plane.
                    if (plane.AddPassengers(nextPassenger))
                    {
                        boardedCount++;
                    }
                    else
                    {
                        // If for some reason the passenger cannot board, put back in the new queue.
                        newQueue.Enqueue(nextPassenger);
                        passengerList.Add(nextPassenger);
                    }
                }
                else
                {
                    // If the passenger's color is the same as the airport,
                    // they are not boarding the plane here, so keep them waiting.
                    newQueue.Enqueue(nextPassenger);
                    passengerList.Add(nextPassenger);
                }
            }

            // Add any remaining passengers from the original queue.
            while (_passengerQueue.Count > 0)
            {
                var passenger = _passengerQueue.Dequeue();
                newQueue.Enqueue(passenger);
            }

            _passengerQueue = newQueue;

            Debug.Log($"Plane boarded {boardedCount} passenger(s) from the airport.");
        }

        /// <summary>
        /// Adds passengers of a specified color to the current passenger holder
        /// while ensuring that the total capacity is not exceeded.
        /// </summary>
        /// <param name="passengerColor">The color of passengers to be added.</param>
        /// <param name="count">The number of passengers to be added. Defaults to 1 if not specified.</param>
        /// <returns>True if the passengers were successfully added; otherwise, false if the capacity limit is exceeded.</returns>
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
        /// If the exact number of passengers requested is not available, the method does not
        /// remove any passengers and returns false.
        /// </summary>
        /// <param name="passengerColor">The color of the passengers to be removed from the queue.</param>
        /// <param name="count">The number of passengers to remove. Defaults to 1.</param>
        /// <returns>True if the requested number of passengers were successfully removed; otherwise, false.</returns>
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