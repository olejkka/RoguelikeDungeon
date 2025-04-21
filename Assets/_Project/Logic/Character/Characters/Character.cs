using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Character : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private NeighborTilesSelectionSO _neighborTilesSelectionSO;
    private Tile _currentTile;
    public Tile CurrentTile { get => _currentTile; set => _currentTile = value; }
    public NeighborTilesSelectionSO NeighborTilesSelectionSO { get => _neighborTilesSelectionSO; set => _neighborTilesSelectionSO = value; }
    
    
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
