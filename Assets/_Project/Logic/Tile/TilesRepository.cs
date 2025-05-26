using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TilesRepository : MonoBehaviour
{
    public static TilesRepository Instance { get; private set; }
    
    private Dictionary<Vector2Int, Tile> _tiles = new();
    
    
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

    public void RegisterTile(Tile tile, Vector2Int position)
    {
        _tiles[position] = tile;
    }

    public Tile GetTileAt(Vector2Int position)
    {
        _tiles.TryGetValue(position, out Tile tile);
        return tile;
    }

    public Dictionary<Vector2Int, Tile> GetTiles()
    {
        return _tiles;
    }

    public void ClearTiles()
    {
        _tiles.Clear();
    }
}