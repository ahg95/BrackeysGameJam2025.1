using System;
using _1_Code;
using _1_Code.Interaction;
using UnityEngine;
using Plane = _1_Code.Plane;

public class GameManager : MonoBehaviour
{
    [Header("Level settings")] 
    [SerializeField] private int numberOfPlanes = 6;
    
    [Header("Assignables")]
    [SerializeField] private GameInputHandler inputHandler;
    [SerializeField] private FlightManager flightManager;
    [SerializeField] private SelectionManager selectionManager;
    [Space(5)]
    [SerializeField] private Plane planePrefab;
    
    private Airport[] _airports;
    private Transform _planesParent;

    [Header("Debug")] 
    [SerializeField] private bool startGame = false;
    [SerializeField] private bool endGame = false;

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        if (startGame)
        {
            startGame = false;
            StartGame();
        }
        if (endGame)
        {
            endGame = false;
            EndGame();
        }
    }

    private void Awake()
    {
        _airports = FindObjectsByType<Airport>(FindObjectsSortMode.None);
    }

    public void StartGame()
    {
        DistributePlanes();
    }

    public void EndGame()
    {
        Destroy(_planesParent);
    }

    private void DistributePlanes()
    {
        var random = new System.Random();
        int planesLeft = numberOfPlanes;
    
        _planesParent = new GameObject("Planes").transform;
        
        while (planesLeft > 0)
        {
            foreach (var airport in _airports)
            {
                if (planesLeft == 0)
                    break;
    
                if (airport.CurrentPlaneCount < airport.MaxPassengerCapacity)
                {
                    var plane = Instantiate(planePrefab, airport.transform.position, Quaternion.identity, _planesParent);
                    airport.ProcessPlaneArrival(plane);
                    planesLeft--;
                }
            }
        }
    }
}
