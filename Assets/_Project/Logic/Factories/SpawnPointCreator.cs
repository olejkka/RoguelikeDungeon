using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPointCreator : MonoBehaviour
{
    public static event Action OnSpawnPointCreated;

    private void OnEnable()
    {
        TileFactory.OnBoardGenerated += CreateSpawnPoint;
    }

    private void OnDisable()
    {
        TileFactory.OnBoardGenerated -= CreateSpawnPoint;
    }

    public void CreateSpawnPoint()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();

        if (allTiles.Length == 0)
        {
            Debug.LogWarning("Нет доступных тайлов для спавна.");
            return;
        }

        Tile selectedTile = allTiles[Random.Range(0, allTiles.Length)];
        selectedTile.IsSpawnPoint = true;
        
        OnSpawnPointCreated?.Invoke();
    }
}