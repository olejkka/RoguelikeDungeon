using System;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public static event Action OnBoardGenerated;

    [Header("Tile Settings")]
    [SerializeField] private GameObject tilePrefab;
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
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0f, z * spacing);
                var tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{z}";
            }
        }

        OnBoardGenerated?.Invoke();
    }
}