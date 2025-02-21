using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;

namespace _1_Code
{
    public class Plane : BasePassengerHolder
    {
        // Uses a dictionary to store how many passengers of each color are onboard.
        private readonly Dictionary<DestinationColor, int> _passengers = new();
        private int _passengerCount = 0;
        
        // Whether the plane is full
        public bool IsFull => _passengerCount >= maxCapacity;
        
        // How many seats are available
        public int RemainingCapacity => maxCapacity - _passengerCount;

        // Optionally, a helper method to check how many passengers of a color are onboard.
        public int GetPassengerCount(DestinationColor color)
        {
            return _passengers.GetValueOrDefault(color, 0);
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
                _passengers.Remove(passengerColor);
            
            _passengerCount -= count;
            return true;
        }
    }
}