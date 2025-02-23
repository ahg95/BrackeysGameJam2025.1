using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class BallController : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;

    // Jump settings
    [SerializeField] private float jumpChargeRate = 10f; // How fast the jump force accumulates per second
    [SerializeField] private float maxJumpForce = 20f; // Maximum jump force
    [SerializeField] private float groundCheckDistance = 0.6f; // Distance for ground check
    [SerializeField] private LayerMask groundLayer; // Layers considered as ground

    // Internal jump state
    [Header("Debug (READONLY)")] public float _currentJumpForce = 0f;
    public bool _isChargingJump = false;
    [Networked] public bool N_grounded { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        if (!Object.HasStateAuthority) return;
        
        Runner.AddCallbacks(this);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority) N_grounded = IsGrounded();
        
        // Process movement input
        if (!GetInput(out BallInputData inputData)) return;
        inputData.Movement.Normalize();

        // Get the main camera
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("No main camera found. Movement will default to world axes.");
            return;
        }

        // Get camera's forward and right vectors (ignore vertical)
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 adjustedToCam = (camForward * inputData.Movement.y + camRight * inputData.Movement.x);
        rb.AddForce(adjustedToCam * moveSpeed, ForceMode.Force);

        // Jump functionality
        if (N_grounded)
        {
            // Start charging jump when the player holds down space and not already charging
            if (inputData.JumpHeld && !_isChargingJump)
            {
                _isChargingJump = true;
                _currentJumpForce = 0f;
            }

            // If charging, increment the jump force up to a maximum
            if (_isChargingJump && inputData.JumpHeld)
            {
                _currentJumpForce += jumpChargeRate * Runner.DeltaTime;
                _currentJumpForce = Mathf.Min(_currentJumpForce, maxJumpForce);
            }

            // If the jump key is released and we are charging, perform the jump
            if (_isChargingJump && inputData.JumpReleased)
            {
                rb.AddForce(Vector3.up * _currentJumpForce, ForceMode.Impulse);
                _currentJumpForce = 0f;
                _isChargingJump = false;
            }
        }
        else
        {
            // If not grounded, cancel any jump charging
            _isChargingJump = false;
            _currentJumpForce = 0f;
        }
    }

    // Simple ground check using a raycast downward
    private bool IsGrounded()
    {
        if (!Object.HasStateAuthority) return false;
        bool isHit = Physics.Raycast(rb.position, Vector3.down, groundCheckDistance);

        // Debug visualization
        Debug.DrawLine(rb.position, transform.position + Vector3.down * groundCheckDistance,
            isHit ? Color.green : Color.red, 0.1f);

        /*Debug.Log(
            $"IsGrounded: {isHit}, Origin: {rb.position}, RayLength: {groundCheckDistance}, GroundLayer: {groundLayer}");*/
        return isHit;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputData = new BallInputData
        {
            Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            JumpHeld = Input.GetKey(KeyCode.Space),
            JumpReleased = Input.GetKeyUp(KeyCode.Space)
        };

        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}