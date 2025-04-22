using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TileFactory : MonoBehaviour
{
    public static event Action OnRoomGenerated;

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

    public void GenerateRoom()
    {
        ClearTiles();

        var layoutGenerator = new RandomRoomLayoutGenerator(rows, columns, minRoomSize, maxRoomSize, fillProbability);
        TileType[,] layout = layoutGenerator.Generate();

        for (int x = 0; x < layout.GetLength(0); x++)
        {
            for (int z = 0; z < layout.GetLength(1); z++)
            {
                TileType type = layout[x, z];

                if (type == TileType.Empty) continue;

                GameObject prefab = type switch
                {
                    TileType.Floor => _tilePrefab,
                    TileType.Wall => _wallTilePrefab,
                    _ => null
                };

                if (prefab == null) continue;

                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                GameObject tileObj = Instantiate(prefab, pos, Quaternion.identity, transform);
                tileObj.name = $"{type}_{x}_{z}";

                Tile tile = tileObj.GetComponent<Tile>();
                if (tile == null)
                {
                    Debug.LogError("Prefab не содержит компонент Tile!");
                    continue;
                }

                tile.Type = type;
                tile.Initialize();
            }
        }

        OnRoomGenerated?.Invoke();
    }
}
