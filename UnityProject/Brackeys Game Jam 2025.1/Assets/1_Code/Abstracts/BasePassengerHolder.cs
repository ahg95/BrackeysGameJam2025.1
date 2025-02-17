using _1_Code.Enums;
using UnityEngine;

namespace _1_Code.Abstracts
{
    public abstract class BasePassengerHolder : MonoBehaviour
    {
        [SerializeField] protected int maxCapacity = 3;

        protected abstract bool AddPassengers(DestinationColor passengerColor, int count = 1);
        protected abstract bool RemovePassengers(DestinationColor passengerColor, int count = 1);
        
        public bool TransferPassengers(DestinationColor passengerColor, int count, BasePassengerHolder destination) {
            // If this holder has no passengers, cancel.
            if (!RemovePassengers(passengerColor, count)) return false;
            
            // If the destination can not hold passengers, cancel.
            if (destination.AddPassengers(passengerColor, count)) return true;
            
            // If the passenger(s) couldn't be added to the destination, add them back to this holder.
            AddPassengers(passengerColor, count);
            
            return false;
        }
    }
}