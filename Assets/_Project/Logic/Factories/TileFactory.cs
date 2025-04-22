using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileFactory : MonoBehaviour
{
    public static event Action OnRoomGenerated;
    public static TileFactory Instance { get; private set; }

    
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

        // Генерация логической карты
        var layoutGenerator = new RandomRoomLayoutGenerator(rows, columns, minRoomSize, maxRoomSize, fillProbability);
        TileType[,] layout = layoutGenerator.Generate();

        // Создаём контейнер комнаты
        GameObject roomObj = new GameObject("Room");
        roomObj.transform.parent = this.transform;

        // Сбор координат пола для вычисления центра
        List<Vector3> floorPositions = new List<Vector3>();
        int width = layout.GetLength(0);
        int depth = layout.GetLength(1);
        for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                if (layout[x, z] != TileType.Empty)
                    floorPositions.Add(new Vector3(x * spacing, 0f, z * spacing));

        if (floorPositions.Count == 0)
        {
            Debug.LogWarning("Нет тайлов для комнаты!");
            OnRoomGenerated?.Invoke();
            return roomObj.transform;
        }

        // Вычисление центра комнаты
        Vector3 center = Vector3.zero;
        foreach (var pos in floorPositions) center += pos;
        center /= floorPositions.Count;
        roomObj.transform.position = center;

        // Инстанцируем тайлы вокруг центра
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                TileType type = layout[x, z];
                if (type == TileType.Empty) continue;

                GameObject prefab = type switch
                {
                    TileType.Floor => _tilePrefab,
                    TileType.Wall  => _wallTilePrefab,
                    _ => null
                };
                if (prefab == null) continue;

                Vector3 worldPos = new Vector3(x * spacing, 0f, z * spacing);
                Vector3 localPos = worldPos - center;

                GameObject tileObj = Instantiate(prefab, roomObj.transform);
                tileObj.transform.localPosition = localPos;
                tileObj.name = $"{type}_{x}_{z}";

                var tile = tileObj.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.Type = type;
                    tile.Initialize();
                }
            }
        }

        OnRoomGenerated?.Invoke();
        return roomObj.transform;
    }
}
