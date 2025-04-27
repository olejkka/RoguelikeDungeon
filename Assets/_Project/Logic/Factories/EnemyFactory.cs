using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
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
            Debug.LogWarning("Не найдена точка спавна игрока.");
            return enemies;
        }

        List<Tile> availableTiles = FindObjectsOfType<Tile>()
            .Where(tile =>
                tile.Type == TileType.Floor &&
                tile != spawnTile &&
                tile.OccupiedCharacter == null)
            .ToList();

        for (int i = 0; i < _enemyCount && availableTiles.Count > 0; i++)
        {
            int index = Random.Range(0, availableTiles.Count);
            Tile tile = availableTiles[index];
            availableTiles.RemoveAt(index);

            GameObject enemyObj = Instantiate(_enemyPrefab, tile.transform.position, Quaternion.identity);
            if (enemyObj.TryGetComponent<Enemy>(out var enemy))
            {
                tile.OccupiedCharacter = enemy;
                enemy.CurrentTile = tile;
                enemies.Add(enemy);
            }
            else
            {
                Debug.LogError("Префаб врага не содержит компонент Enemy!");
            }
        }

        return enemies;
    }


    private Tile FindSpawnTile()
    {
        return FindObjectsOfType<Tile>().FirstOrDefault(tile => tile.Type == TileType.Spawn);
    }
}