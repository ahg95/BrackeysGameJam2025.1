using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab; // Assign in the Inspector
    [SerializeField] private Transform[] spawnPoints; // Optional: Set predefined spawn locations

    private Vector3 GetSpawnPosition(PlayerRef player)
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            return spawnPoints[player.RawEncoded % spawnPoints.Length].position;
        }
        return Vector3.zero; // Default spawn position
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (!Runner.IsServer) return;
        
        // Determine spawn position
        var spawnPosition = GetSpawnPosition(player);
        var spawnRotation = Quaternion.identity;

        // Spawn player with ownership
        Runner.Spawn(playerPrefab, spawnPosition, spawnRotation, player);
    }
}