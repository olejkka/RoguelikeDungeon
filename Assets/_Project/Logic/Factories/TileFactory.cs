using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TileFactory : MonoBehaviour
{
    public static event Action AllTilesInitialized;
    public static TileFactory Instance { get; private set; }
    
    [FormerlySerializedAs("tilePrefab")]
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _wallTilePrefab;

    [Header("Board Settings")]
    [SerializeField] private int columns = 100;
    [SerializeField] private int rows = 100;
    [SerializeField] private float spacing = 1f;

    [Header("Room Size Settings")]
    [SerializeField] private int minRoomSize = 50;
    [SerializeField] private int maxRoomSize = 100;

    [Header("Room Shape Settings")]
    [SerializeField, Range(0f, 1f)] private float fillProbability = 0.7f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate TileFactory instance detected.");
            Destroy(gameObject);
            
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ValidatePrefabs();
    }

    private void ValidatePrefabs()
    {
        if (_tilePrefab == null || _wallTilePrefab == null)
        {
            Debug.LogError("Tile prefabs are not assigned in the inspector.");
        }
    }

    public void ClearTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public Transform GenerateRoom()
    {
        ClearTiles();
        
        minRoomSize = Mathf.Clamp(minRoomSize, 1, columns * rows);
        maxRoomSize = Mathf.Clamp(maxRoomSize, minRoomSize, columns * rows);

        var layoutGenerator = new RandomRoomLayoutGenerator(rows, columns, minRoomSize, maxRoomSize, fillProbability);
        TileType[,] layout = layoutGenerator.Generate();
        
        GameObject roomObj = new GameObject("Room");
        roomObj.transform.parent = this.transform;

        List<Vector3> floorPositions = GetFloorPositions(layout);
        
        if (floorPositions.Count == 0)
        {
            Debug.LogWarning("No tiles for the room!");
            return roomObj.transform;
        }

        Vector3 center = CalculateRoomCenter(floorPositions);
        roomObj.transform.position = center;

        List<Tile> allTiles = CreateTiles(layout, roomObj.transform, center);
        InitializeAllTiles(allTiles);

        return roomObj.transform;
    }

    private List<Vector3> GetFloorPositions(TileType[,] layout)
    {
        List<Vector3> floorPositions = new List<Vector3>();
        int width = layout.GetLength(0);
        int depth = layout.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (layout[x, z] != TileType.Empty)
                    floorPositions.Add(new Vector3(x * spacing, 0f, z * spacing));
            }
        }

        return floorPositions;
    }

    private Vector3 CalculateRoomCenter(List<Vector3> floorPositions)
    {
        float avgX = floorPositions.Average(p => p.x);
        float avgZ = floorPositions.Average(p => p.z);
        
        return new Vector3(avgX, 0f, avgZ);
    }

    private List<Tile> CreateTiles(TileType[,] layout, Transform parent, Vector3 center)
    {
        List<Tile> allTiles = new List<Tile>();
        
        int width = layout.GetLength(0);
        int depth = layout.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                TileType type = layout[x, z];
                if (type == TileType.Empty)
                    continue;

                GameObject prefab = GetPrefabForTileType(type);
                
                if (prefab == null)
                    continue;

                GameObject tileObj = Instantiate(prefab, parent);
                tileObj.transform.localPosition = new Vector3(x * spacing, 0f, z * spacing) - center;
                tileObj.name = $"{type}_{x}_{z}";

                var tile = tileObj.GetComponent<Tile>();
                
                if (tile != null)
                {
                    tile.Type = type;
                    allTiles.Add(tile);
                }
            }
        }

        return allTiles;
    }

    private GameObject GetPrefabForTileType(TileType type)
    {
        return type switch
        {
            TileType.Floor => _tilePrefab,
            TileType.Wall => _wallTilePrefab,
            _ => null
        };
    }

    private void InitializeAllTiles(List<Tile> tiles)
    {
        if (tiles.Count == 0)
        {
            Debug.LogWarning("No tiles to initialize.");
            return;
        }

        foreach (Tile tile in tiles)
        {
            tile.Initialize();
            tile.RegisterInRepository();
        }

        AllTilesInitialized?.Invoke();
    }
}
