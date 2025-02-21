using System;
using System.Collections.Generic;
using _1_Code.Abstracts;
using _1_Code.Enums;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

namespace _1_Code
{
    public class Plane : BasePassengerHolder
    {
        // Passenger-related fields.
        private readonly Dictionary<DestinationColor, int> _passengers = new();
        [SerializeField] private List<DestinationColor> passengerColors = new();
        private int _passengerCount = 0;

        public static event Action<Plane> OnPlaneExploded;

        public bool IsFull => _passengerCount >= maxCapacity;
        public int RemainingCapacity => maxCapacity - _passengerCount;

        // Tracking the current airport; if null the plane is flying.
        public Airport CurrentAirport { get; private set; }

        // ----- FUEL IMPLEMENTATION -----
        [Header("Fuel Settings")] [Tooltip("Maximum fuel capacity for the plane.")] [SerializeField]
        private float maxFuel = 100f;

        [Tooltip("Current fuel level.")] [SerializeField]
        private float currentFuel;

        [Tooltip("Fuel consumption rate per second while flying.")] [SerializeField]
        private float fuelConsumptionRate = 5f;

        [Tooltip("Refuel rate per second when landed.")] [SerializeField]
        private float refuelRate = 10f;

        [SerializeField] private UnityEngine.UI.Slider fuelSlider;

        [Header("Audio")]
        [SerializeField] private AudioClip explosionClip;
        
        private Tween _moveTween;

        private float CurrentFuel
        {
            get => currentFuel;
            set
            {
                currentFuel = value;
                fuelSlider.SetValueWithoutNotify(FuelPercentage);
            }
        }

        private float FuelPercentage => currentFuel / maxFuel;
        public float FuelConsumptionRate => fuelConsumptionRate;
        public float RefuelRate => refuelRate;
        // ----------------------------------

        private void Awake()
        {
            // Start with full fuel.
            CurrentFuel = maxFuel;
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }

        private void Update()
        {
            if (CurrentAirport != null)
            {
                // When landed, the plane refuels.
                if (CurrentFuel < maxFuel)
                {
                    CurrentFuel += refuelRate * Time.deltaTime;
                    CurrentFuel = Mathf.Min(CurrentFuel, maxFuel);
                }
            }
            else
            {
                // If in flight, consume fuel steadily.
                float fuelUsed = fuelConsumptionRate * Time.deltaTime;
                ConsumeFuel(fuelUsed);
            }
        }

        /// Handles the movement of the plane to the destination airport.
        /// </summary>
        /// <param name="destinationAirport">The airport to which the plane will move.</param>
        /// <param name="speed">The speed factor for movement.</param>
        public void MoveToAirport(Airport destinationAirport, float speed)
        {
            if (destinationAirport == null)
            {
                Debug.LogError("Destination airport is null. Movement canceled.");
                return;
            }

            // Calculate the travel time based on distance and speed.
            Vector3 targetPosition = destinationAirport.transform.position;
            float travelTime = Vector3.Distance(transform.position, targetPosition) / speed;

            // Set up the movement tween with DOTween.
            _moveTween = transform.DOMove(targetPosition, travelTime)
                .OnUpdate(() =>
                {
                    // Consume fuel during movement.
                    ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
                })
                .OnComplete(() =>
                {
                    // Complete the landing process if the plane still exists.
                    if (this != null)
                    {
                        destinationAirport.ProcessPlaneArrival(this);
                        LandAtAirport(destinationAirport);
                    }
                });
        }


        // Called when the plane lands at an airport.
        public void LandAtAirport(Airport landingAirport)
        {
            CurrentAirport = landingAirport;
        }

        // Clears the current airport when taking off.
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

        /// <summary>
        /// Consumes the given amount of fuel and checks if the plane should explode.
        /// </summary>
        /// <param name="amount">Amount of fuel to consume.</param>
        private void ConsumeFuel(float amount)
        {
            CurrentFuel -= amount;

            if (CurrentFuel <= 0f)
            {
                CurrentFuel = 0f;
                ExplodePlane();
            }
        }

        public void ExplodePlane()
        {
            // Stop movement if the plane explodes.
            _moveTween?.Kill();
            Debug.Log($"Plane {name} exploded!");

            OnPlaneExploded?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}