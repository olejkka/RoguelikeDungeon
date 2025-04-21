using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPointCreator : MonoBehaviour
{
    public static event Action OnSpawnPointCreated;

    public void CreateSpawnPoint()
    {
        Tile[] availableTiles = FindObjectsOfType<Tile>()
            .Where(tile => tile.IsWall == false)
            .ToArray();

        if (availableTiles.Length == 0)
        {
            Debug.LogWarning("Нет доступных тайлов для спавна.");
            return;
        }

        Tile selectedTile = availableTiles[Random.Range(0, availableTiles.Length)];
        selectedTile.IsSpawnPoint = true;
        
        OnSpawnPointCreated?.Invoke();
    }
}