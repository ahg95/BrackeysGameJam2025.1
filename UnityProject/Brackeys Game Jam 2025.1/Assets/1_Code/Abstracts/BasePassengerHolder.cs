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
    }
}