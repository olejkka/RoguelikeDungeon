using System;
using UnityEngine;

public class TileRegistrator : MonoBehaviour
{
    public static event Action AllTilesRegistered;
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
        TileFactory.RoomGenerated += DORoomGeneratedHandler;
    }

    private void OnDisable()
    {
        TileFactory.RoomGenerated -= DORoomGeneratedHandler;
    }

    void DORoomGeneratedHandler()
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
        
        AllTilesRegistered?.Invoke();
    }
}