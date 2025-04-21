using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    public static event Action OnPlayerCreated;
    
    [FormerlySerializedAs("_playerPrefab")] [SerializeField] private Character characterPrefab;

    public Character Generate()
    {
        Transform spawnPoint = FindSpawnPointTransform();
        if (spawnPoint == null)
        {
            Debug.LogError("Точка спавна не найдена (Tile.IsSpawnPoint == true, но поле _spawnPoint не задано)");
            return null;
        }
        var player = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
        
        OnPlayerCreated?.Invoke();

        return player;
    }

    private Transform FindSpawnPointTransform()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.IsSpawnPoint && tile.TryGetSpawnPoint(out Transform spawnPoint))
            {
                return spawnPoint;
            }
        }

        return null;
    }
}