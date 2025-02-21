using _1_Code.Enums;
using UnityEngine;

namespace _1_Code.Abstracts
{
    public abstract class BasePassengerHolder : MonoBehaviour
    {
        /// <summary>
        /// Represents the maximum number of passengers a passenger holder can accommodate.
        /// </summary>
        [SerializeField] protected int maxCapacity = 3;

        /// <summary>
        /// Adds passengers of a specified color to the passenger holder,
        /// ensuring the total capacity is not exceeded.
        /// </summary>
        /// <param name="passengerColor">The color of the passengers being added.</param>
        /// <param name="count">The number of passengers to add. Defaults to 1 if not specified.</param>
        /// <returns>True if passengers are successfully added; otherwise, false if the capacity is exceeded.</returns>
        public abstract bool AddPassengers(DestinationColor passengerColor, int count = 1);

        /// <summary>
        /// Removes a specified number of passengers of a given color from the passenger holder.
        /// The operation will only succeed if the required number of passengers of that color
        /// is available in the holder.
        /// </summary>
        /// <param name="passengerColor">The color of the passengers to be removed.</param>
        /// <param name="count">The number of passengers to remove. Defaults to 1 if not specified.</param>
        /// <returns>True if the passengers are successfully removed; otherwise, false.</returns>
        public abstract bool RemovePassengers(DestinationColor passengerColor, int count = 1);

        /// <summary>
        /// Transfers passengers of a specified color from this holder to a destination holder.
        /// If the operation is unsuccessful, the passengers are returned to the source holder.
        /// </summary>
        /// <param name="passengerColor">The color of the passengers being transferred.</param>
        /// <param name="count">The number of passengers to transfer.</param>
        /// <param name="destination">The destination passenger holder to which passengers are being transferred.</param>
        /// <returns>True if the transfer is successful; otherwise, false.</returns>
        public bool TransferPassengers(DestinationColor passengerColor, int count, BasePassengerHolder destination)
        {
            // If this holder has no passengers of that color, cancel.
            if (!RemovePassengers(passengerColor, count))
                return false;
            
            // If the destination can not hold the passengers, cancel.
            if (destination.AddPassengers(passengerColor, count))
                return true;
            
            // If the passenger(s) couldn't be added to the destination, add them back.
            AddPassengers(passengerColor, count);
            
            return false;
        }
    }
}