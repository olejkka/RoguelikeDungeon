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

    public Vector2Int Position => _position;
    public bool IsHighlighted => _isHighlighted;
    public Character OccupiedCharacter => _occupiedCharacter;
    public TileHighlightVisuals Visuals => _visuals;
    public TileType Type => _type;

    private void SetPosition(Vector2Int position)
    {
        _position = position;
    }

    public void SetHighlighted(bool state)
    {
        _isHighlighted = state;
    }

    public void SetOccupiedCharacter(Character character)
    {
        _occupiedCharacter = character;
    }

    public void SetType(TileType type)
    {
        _type = type;
    }

    private void Awake()
    {
        _visuals = GetComponentInChildren<TileHighlightVisuals>();
        _visuals.highlightEmptyTile?.SetActive(false);
        _visuals.highlightEnemyTile?.SetActive(false);
        _visuals.hoverHighlightTile?.SetActive(false);
    }

    public void Initialize()
    {
        SetPosition(new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        ));
    }

    public void RegisterInRepository()
    {
        if (TilesRepository.Instance == null)
        {
            Debug.LogError("[Tile] TilesRepository не найден. Убедитесь, что Bootstrapper загружает TilesRepository первым.");
            
            return;
        }

        TilesRepository.Instance.RegisterTile(this, Position);
    }

    public bool TryGetSpawnPoint(out Transform point)
    {
        point = _spawnPoint != null ? _spawnPoint.transform : null;
        return point != null;
    }

    public List<Tile> GetNeighbors(NeighborTilesSelectionSO settings)
    {
        if (settings == null)
            return new List<Tile>();

        var offsets = settings.GetOffsets();
        var result = new List<Tile>();

        foreach (var offset in offsets)
        {
            var neighbor = TilesRepository.Instance.GetTileAt(Position + offset);
            
            result.Add(neighbor);
        }

        return result;
    }
}
