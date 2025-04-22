using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _enemyCount = 3;

    private void OnEnable()
    {
        SpawnPointCreator.OnSpawnPointCreated += SpawnEnemies;
    }

    private void OnDisable()
    {
        SpawnPointCreator.OnSpawnPointCreated -= SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        Tile spawnTile = FindSpawnTile();
        if (spawnTile == null)
        {
            Debug.LogWarning("Не найдена точка спавна игрока.");
            return;
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
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy == null)
            {
                Debug.LogError("Префаб врага не содержит компонент Enemy!");
                continue;
            }

            tile.OccupiedCharacter = enemy;
            enemy.CurrentTile = tile;
        }
    }

    private Tile FindSpawnTile()
    {
        return FindObjectsOfType<Tile>().FirstOrDefault(tile => tile.Type == TileType.Spawn);
    }
}