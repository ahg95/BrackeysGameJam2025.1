using System;
using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;
using UnityEngine;

namespace _1_Code
{
    public class Plane : BasePassengerHolder
    {
        // Uses a dictionary to store how many passengers of each color are onboard.
        private readonly Dictionary<DestinationColor, int> _passengers = new();
        
        [SerializeField] private List<DestinationColor> passengerColors = new();
        
        private int _passengerCount = 0;
        
        public static event Action<Plane> OnPlaneExploded;
        
        // Whether the plane is full.
        public bool IsFull => _passengerCount >= maxCapacity;
        
        // How many seats are available.
        public int RemainingCapacity => maxCapacity - _passengerCount;

        // Tracks the current airport at which the plane is landed (null if in flight).
        public Airport CurrentAirport { get; private set; }
        
        // Call when the plane lands at an airport.
        public void LandAtAirport(Airport landingAirport)
        {
            CurrentAirport = landingAirport;
        }

        // Clears the current airport when the plane is sent away.
        public void DepartAirport()
        {
            CurrentAirport = null;
        }

        // Adds passengers if there is enough capacity.
        public override bool AddPassengers(DestinationColor passengerColor, int count = 1)
        {
            if (_passengerCount + count > maxCapacity)
                return false;
            
            if (!_passengers.TryAdd(passengerColor, count))
            {
                _passengers[passengerColor] += count;
            }

            if (!passengerColors.Contains(passengerColor))
            {
                passengerColors.Add(passengerColor);
            }

            _passengerCount += count;
            return true;
        }

        // Removes passengers if enough of the requested color are onboard.
        public override bool RemovePassengers(DestinationColor passengerColor, int count = 1)
        {
            if (!_passengers.ContainsKey(passengerColor) || _passengers[passengerColor] < count)
                return false;
            
            _passengers[passengerColor] -= count;
            if (_passengers[passengerColor] == 0)
            {
                _passengers.Remove(passengerColor);
                passengerColors.Remove(passengerColor);
            }
            
            _passengerCount -= count;
            return true;
        }
        
        public void ExplodePlane()
        {
            // Trigger the explosion event
            OnPlaneExploded?.Invoke(this);

            // (Optional) Destroy the plane GameObject
            Destroy(gameObject);
        }

    }
}