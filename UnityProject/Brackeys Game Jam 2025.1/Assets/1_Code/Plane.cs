using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;

namespace _1_Code
{
    public class Plane : BasePassengerHolder
    {
        private readonly Dictionary<DestinationColor, int> _passenger = new();
        private int _passengerCount = 0;
        public bool IsFull => _passengerCount == maxCapacity;
        public int RemainingCapacity => maxCapacity - _passengerCount;

        public override bool AddPassengers(DestinationColor passengerColor, int count = 1)
        {
            if (_passenger.ContainsKey(passengerColor))
            {
                if (_passenger[passengerColor] + count > maxCapacity) return false;
                _passenger[passengerColor] += count;
            }
            else
            {
                if (count > maxCapacity) return false;
                _passenger[passengerColor] = count;
            }
            
            _passengerCount += count;
            return true;
        }

        public override bool RemovePassengers(DestinationColor passengerColor, int count = 1)
        {
            if (!_passenger.ContainsKey(passengerColor) || _passenger[passengerColor] < count) return false;

            _passenger[passengerColor] -= count;

            if (_passenger[passengerColor] == 0) _passenger.Remove(passengerColor);
            

            _passengerCount -= count;
            return true;
        }
    }
}