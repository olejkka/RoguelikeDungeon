using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    public static PlayerFactory Instance { get; private set; }
    
    [SerializeField] private Player _playerPrefab;
    
    
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

    public Character Generate()
    {
        Transform spawnPoint = FindSpawnPointTransform();
        
        if (spawnPoint == null)
        {
            Debug.LogError("Точка спавна не найдена (Tile.IsSpawnPoint == true, но поле _spawnPoint не задано)");
            return null;
        }
        
        Player player = Instantiate(_playerPrefab, spawnPoint.position, Quaternion.identity);

        return player;
    }

    private Transform FindSpawnPointTransform()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.Type == TileType.Spawn && tile.TryGetSpawnPoint(out Transform spawnPoint))
            {
                return spawnPoint;
            }
        }

        return null;
    }
}