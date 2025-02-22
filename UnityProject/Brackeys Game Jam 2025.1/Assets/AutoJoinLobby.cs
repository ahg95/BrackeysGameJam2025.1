using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class AutoJoinLobby : MonoBehaviour, INetworkRunnerCallbacks
{
    [Tooltip("Maximum number of players in the room.")]
    public int maxPlayers = 4;

    [Tooltip("Should the room be visible in the lobby list?")]
    public bool isRoomVisible = true;

    [SerializeField] private NetworkRunner networkRunner;

    void Awake()
    {
        if (!networkRunner) networkRunner = FindFirstObjectByType<NetworkRunner>();
        if (!networkRunner) networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.AddCallbacks(this);

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1); // next scene in build order
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);

        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            // Start Fusion in client mode and join a lobby
            await networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                PlayerCount = maxPlayers,
                IsVisible = isRoomVisible,
                SceneManager = networkRunner.GetComponent<INetworkSceneManager>(),
            });

            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1); // next scene in build order
            await networkRunner.LoadScene(scene, LoadSceneMode.Single, setActiveOnLoad: true);
            networkRunner.ProvideInput = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // Fusion Callback Methods
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player} joined the lobby.");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player} left the lobby.");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to the Fusion server.");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from the Fusion server.");
        Destroy(gameObject);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnSessionListUpdated(NetworkRunner runner, System.Collections.Generic.List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, System.Collections.Generic.Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }
}
