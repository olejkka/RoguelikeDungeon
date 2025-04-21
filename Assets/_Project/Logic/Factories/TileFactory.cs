using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TileFactory : MonoBehaviour
{
    public static event Action OnBoardGenerated;

    [FormerlySerializedAs("tilePrefab")]
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _wallTilePrefab;

    [Header("Board Settings")]
    [SerializeField] private int columns = 10;
    [SerializeField] private int rows = 10;
    [SerializeField] private float spacing = 1f;

    public void ClearTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void GenerateBoard()
    {
        int totalColumns = columns + 2;
        int totalRows = rows + 2;

        for (int x = 0; x < totalColumns; x++)
        {
            for (int z = 0; z < totalRows; z++)
            {
                bool isWall = x == 0 || x == totalColumns - 1 || z == 0 || z == totalRows - 1;
                GameObject prefabToUse = isWall ? _wallTilePrefab : _tilePrefab;

                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                GameObject tileObj = Instantiate(prefabToUse, pos, Quaternion.identity, transform);
                tileObj.name = isWall ? $"Wall_{x}_{z}" : $"Tile_{x}_{z}";

                Tile tile = tileObj.GetComponent<Tile>();
                if (tile == null)
                {
                    Debug.LogError("Prefab не содержит компонент Tile!");
                    continue;
                }

                tile.IsWall = isWall;
                tile.Initialize();
            }
        }

        OnBoardGenerated?.Invoke();
    }
}