using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private TileType _type;

    private Vector2Int _position;
    private bool _isHighlighted;
    private Character _occupiedCharacter;
    private TileHighlightVisuals _visuals;

    public Vector2Int Position { get => _position; set => _position = value; }
    public bool IsHighlighted { get => _isHighlighted; set => _isHighlighted = value; }
    public Character OccupiedCharacter { get => _occupiedCharacter; set => _occupiedCharacter = value; }
    public TileHighlightVisuals Visuals => _visuals;
    public TileType Type { get => _type; set => _type = value; }

    private void Awake()
    {
        _visuals = GetComponentInChildren<TileHighlightVisuals>();
        _visuals.highlightEmptyTile?.SetActive(false);
        _visuals.highlightEnemyTile?.SetActive(false);
        _visuals.hoverHighlightTile?.SetActive(false);
    }

    public void Initialize()
    {
        Position = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
    }

    public bool TryGetSpawnPoint(out Transform point)
    {
        point = _spawnPoint != null ? _spawnPoint.transform : null;
        return point != null;
    }

    public List<Tile> GetNeighbors(NeighborTilesSelectionSO settings)
    {
        if (settings == null) return new List<Tile>();

        var offsets = settings.GetOffsets();
        List<Tile> result = new List<Tile>();

        foreach (var offset in offsets)
        {
            Tile neighbor = TilesRepository.Instance.GetTileAt(Position + offset);
            if (neighbor != null)
                result.Add(neighbor);
        }

        return result;
    }

    public void SetHighlighted(bool state)
    {
        IsHighlighted = state;
    }
}