using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;

namespace _1_Code
{
    public class Plane : BasePassengerHolder
    {
        private Dictionary<DestinationColor, int> _passenger = new();
        
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