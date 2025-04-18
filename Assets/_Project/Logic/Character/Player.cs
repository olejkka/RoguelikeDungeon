using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    
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

    public void Move(Tile clickedTile)
    {
        _movement.Move(clickedTile);
    }
}
