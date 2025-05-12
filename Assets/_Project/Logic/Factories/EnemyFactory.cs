using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyCount = 1;
    public static EnemyFactory Instance { get; private set; }

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
    
    public List<Enemy> SpawnEnemies()
    {
        var enemies = new List<Enemy>();
        Tile spawnTile = FindSpawnTile();
        if (spawnTile == null)
        {
            Debug.LogWarning("Точка спавна не найдена.");
            return enemies;
        }

        var availableTiles = FindObjectsOfType<Tile>()
            .Where(t => t.Type == TileType.Floor && t != spawnTile && t.OccupiedCharacter == null)
            .ToList();

        for (int i = 0; i < _enemyCount && availableTiles.Count > 0; i++)
        {
            int idx = Random.Range(0, availableTiles.Count);
            Tile tile = availableTiles[idx];
            availableTiles.RemoveAt(idx);
            
            Vector3 spawnPosition;
            if (tile.TryGetSpawnPoint(out Transform spawnPointTransform) && spawnPointTransform != null)
                spawnPosition = spawnPointTransform.position;
            else
                spawnPosition = tile.transform.position;

            var enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            enemies.Add(enemy);
        }

        return enemies;
    }

    private Tile FindSpawnTile()
    {
        return FindObjectsOfType<Tile>().FirstOrDefault(tile => tile.Type == TileType.Spawn);
    }
}