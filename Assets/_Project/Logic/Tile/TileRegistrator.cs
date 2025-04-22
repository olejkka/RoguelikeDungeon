using System;
using UnityEngine;

public class TileRegistrator : MonoBehaviour
{
    public static event Action OnTilesRegistered;
    public static TileRegistrator Instance { get; private set; }
    
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
    
    private void OnEnable()
    {
        TileFactory.OnRoomGenerated += OnRoomGeneratedHandler;
    }

    private void OnDisable()
    {
        TileFactory.OnRoomGenerated -= OnRoomGeneratedHandler;
    }

    void OnRoomGeneratedHandler()
    {
        RegisterAllTiles();
    }

    private void RegisterAllTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (var tile in tiles)
        {
            tile.Initialize();

            TilesRepository.Instance.RegisterTile(tile, tile.Position);
        }
        
        OnTilesRegistered?.Invoke();
    }
}