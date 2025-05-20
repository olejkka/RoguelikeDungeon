using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPointCreator : MonoBehaviour
{
    public static event Action SpawnPointCreated;
    public static SpawnPointCreator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CreateSpawnPoint()
    {
        Tile[] availableTiles = FindObjectsOfType<Tile>()
            .Where(TileRules.IsWalkable)
            .ToArray();

        if (availableTiles.Length == 0)
        {
            Debug.LogWarning("Нет доступных тайлов для спавна.");
            return;
        }

        Tile selectedTile = availableTiles[Random.Range(0, availableTiles.Length)];
        selectedTile.Type = TileType.Spawn;
        
        SpawnPointCreated?.Invoke();
    }
}