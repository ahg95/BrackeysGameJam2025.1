using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BallController : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;

    public override void Spawned()
    {
        base.Spawned();
        Runner.AddCallbacks(this);
        Object.AssignInputAuthority(Runner.LocalPlayer);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out BallInputData inputData))
        {
            // Get the main camera
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("No main camera found. Movement will default to world axes.");
                return;
            }

            // Get the camera's forward and right vectors, but ignore vertical components
            Vector3 camForward = cam.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cam.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            // Combine the camera vectors with the input axes.
            // Typically, vertical input maps to forward/back, and horizontal maps to right/left.
            Vector3 moveDirection = (camForward * inputData.vertical + camRight * inputData.horizontal).normalized;
            Debug.Log($"Move Direction: {moveDirection}");

            rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputData = new BallInputData
        {
            horizontal = Input.GetAxis("Horizontal"),
            vertical = Input.GetAxis("Vertical")
        };
        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
