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
            Debug.LogWarning("Spawn tile not found.");
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

            var enemy = Instantiate(_enemyPrefab, tile.transform.position, Quaternion.identity);
            
            var initializer = enemy.GetComponent<CharacterInitializer>();
            if (initializer != null)
            {
                initializer.InitializeAtCurrentPosition();
            }
            else
            {
                tile.OccupiedCharacter = enemy;
                enemy.CurrentTile = tile;
            }

            enemies.Add(enemy);
        }

        return enemies;
    }

    private Tile FindSpawnTile()
    {
        return FindObjectsOfType<Tile>().FirstOrDefault(tile => tile.Type == TileType.Spawn);
    }
}