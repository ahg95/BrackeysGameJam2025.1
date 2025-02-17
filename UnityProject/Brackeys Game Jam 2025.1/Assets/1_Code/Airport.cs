using System;
using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;
using UnityEngine;

namespace _1_Code
{
    public class Airport : BasePassengerHolder
    {
        [SerializeField] private DestinationColor airportColor = DestinationColor.Blue;

        // Using a non-readonly queue so we can rebuild it as needed.
        private Queue<DestinationColor> _passengerQueue = new Queue<DestinationColor>();

        [SerializeField] private List<Plane> planes = new List<Plane>();

        [Header("Debug")] [SerializeField] private bool startWithRandomPassengers = false;

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
                var randomPassengerColor =
                    (DestinationColor)UnityEngine.Random.Range(0, Enum.GetValues(typeof(DestinationColor)).Length);
                _passengerQueue.Enqueue(randomPassengerColor);
            }

            Debug.Log($"Airport populated with {_passengerQueue.Count} passengers.");
        }

        // At Start, transfer passengers from the airport to each plane.
        private void Start()
        {
            PopulatePlanes();
        }

        // Goes through each plane and transfers passengers in order until the plane is full.
        private void PopulatePlanes()
        {
            foreach (var plane in planes)
            {
                while (_passengerQueue.Count > 0 && !plane.IsFull)
                {
                    // Peek at the next passenger in order.
                    var nextPassenger = _passengerQueue.Peek();

                    // Use a helper to transfer a single passenger.
                    var transferred = TransferPassengerToPlane(plane, nextPassenger);
                    if (transferred)
                    {
                        Debug.Log($"Transferred passenger {nextPassenger} to plane {plane.name}");
                    }
                    else
                    {
                        // If transfer fails (perhaps due to capacity), stop filling this plane.
                        break;
                    }
                }
            }
        }

        // Transfers a single passenger from the airport's queue to the given plane.
        private bool TransferPassengerToPlane(Plane plane, DestinationColor passengerColor)
        {
            // Remove one instance from the queue.
            var removed = RemovePassengerFromQueue(passengerColor);
            if (!removed) return false;

            // Use the base TransferPassengers method or directly add to the plane.
            return plane.AddPassengers(passengerColor, 1);
        }

        // Helper to remove one matching passenger from the queue while preserving order.
        private bool RemovePassengerFromQueue(DestinationColor passengerColor)
        {
            var removed = false;
            var newQueue = new Queue<DestinationColor>();
            while (_passengerQueue.Count > 0)
            {
                var p = _passengerQueue.Dequeue();
                if (!removed && p == passengerColor)
                {
                    removed = true;
                    continue;
                }

                newQueue.Enqueue(p);
            }

            _passengerQueue = newQueue;
            return removed;
        }

        // Override base AddPassengers to update the queue.
        public override bool AddPassengers(DestinationColor passengerColor, int count = 1)
        {
            if (_passengerQueue.Count + count > maxCapacity)
                return false;

            for (var i = 0; i < count; i++)
            {
                _passengerQueue.Enqueue(passengerColor);
            }

            return true;
        }

        // Override base RemovePassengers to remove passengers in order.
        public override bool RemovePassengers(DestinationColor passengerColor, int count = 1)
        {
            var removedCount = 0;
            var newQueue = new Queue<DestinationColor>();
            while (_passengerQueue.Count > 0)
            {
                var p = _passengerQueue.Dequeue();
                if (p == passengerColor && removedCount < count)
                {
                    removedCount++;
                }
                else
                {
                    newQueue.Enqueue(p);
                }
            }

            _passengerQueue = newQueue;
            return removedCount == count;
        }
    }
}