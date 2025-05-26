using UnityEngine;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    private List<Tile> _highlightedTiles = new List<Tile>();
    public static TileHighlighter Instance { get; private set; }

    
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

    public void HighlightEmptyTiles(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var visuals = tile.Visuals;
            
            if (visuals == null) 
                continue;

            visuals.highlightEmptyTile?.SetActive(true);
            visuals.highlightEnemyTile?.SetActive(false);

            tile.SetHighlighted(true);
            _highlightedTiles.Add(tile);
        }
    }

    public void HighlightEnemyTiles(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var visuals = tile.Visuals;
            
            if (visuals == null) 
                continue;

            visuals.highlightEmptyTile?.SetActive(false);
            visuals.highlightEnemyTile?.SetActive(true);

            tile.SetHighlighted(true);
            _highlightedTiles.Add(tile);
        }
    }

    public void ClearHighlights()
    {
        foreach (var tile in _highlightedTiles)
        {
            tile.SetHighlighted(false);

            var visuals = tile.Visuals;
            
            if (visuals == null) 
                continue;

            visuals.highlightEmptyTile?.SetActive(false);
            visuals.highlightEnemyTile?.SetActive(false);

            var hover = tile.GetComponentInChildren<TileHoverHandler>();
            hover?.ResetHoverEffect();
        }

        _highlightedTiles.Clear();
    }
}