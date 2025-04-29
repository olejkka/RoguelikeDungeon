using UnityEngine;

[RequireComponent(typeof(AvailableMovesHighlighter))]
public class Player : Character
{
    private void OnEnable()
    {
        ClickHandler.TileClicked += OnTileClicked;
    }

    private void OnDisable()
    {
        ClickHandler.TileClicked -= OnTileClicked;
    }
    
    private void OnTileClicked(Tile clickedTile)
    {
        Move(clickedTile);
    }
}
