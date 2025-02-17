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
        [SerializeField] private List<Plane> planes = new List<Plane>();
        
        [Header("Debug")] 
        [SerializeField] private bool startWithRandomPassengers = false;
        
        
        private Queue<DestinationColor> _passengerQueue = new Queue<DestinationColor>();
        
        private void Awake()
        {
            if (startWithRandomPassengers) PopulateAirport();
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
                if (plane.IsFull) continue; // Is the plane full? next plane.
                if (_passengerQueue.Count == 0) break; // No more passengers in airport? stop.

                var remainingSeatsInPlane = plane.RemainingCapacity;

                // Iterating through remaining seats in plane.
                for (var i = 0; i < remainingSeatsInPlane; i++)
                {
                    if (!_passengerQueue.TryDequeue(out var passengerColor)) break; // No more passengers in airport? stop.
                    plane.AddPassengers(passengerColor);
                }
            }
        }

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