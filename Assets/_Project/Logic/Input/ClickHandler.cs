using UnityEngine;
using System;

public class ClickHandler : MonoBehaviour
{
    public static event Action<Tile> TileClicked;
    
    void OnMouseDown()
    {
        Tile tile = GetComponentInParent<Tile>();
        
        if (tile.IsHighlighted)
            TileClicked?.Invoke(tile);
    }
}