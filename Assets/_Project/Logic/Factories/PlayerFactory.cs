using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private Character characterPrefab;
    public static event Action OnPlayerCreated;
    public static PlayerFactory Instance { get; private set; }

    
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
        var player = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
        
        OnPlayerCreated?.Invoke();

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