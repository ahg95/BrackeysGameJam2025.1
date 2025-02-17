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

        private readonly Queue<DestinationColor> _passengerQueue = new();
        [SerializeField] private List<Plane> planes = new();

        private void Start()
        {
            for (var i = 0; i < maxCapacity; i++)
            {
                // Generate a random passenger color.
                var randomPassengerColor =
                    (DestinationColor)UnityEngine.Random.Range(0, Enum.GetValues(typeof(DestinationColor)).Length);

                // Add the randomly generated passenger to the queue.
                _passengerQueue.Enqueue(randomPassengerColor);
            }

            Debug.Log($"Airport initialized with {_passengerQueue.Count} passengers.");
        }

        protected override bool AddPassengers(DestinationColor passengerColor, int count = 1)
        {
            return false;
        }

        protected override bool RemovePassengers(DestinationColor passengerColor, int count = 1)
        {
            return false;
        }
    }
}