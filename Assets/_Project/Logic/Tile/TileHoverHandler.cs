using UnityEngine;

public class TileHoverHandler : MonoBehaviour
{
    private Tile _parentTile;
    private TileHighlightVisuals _visuals;
    private bool _wasEnemyHighlighted;

    private void Start()
    {
        _parentTile = GetComponentInParent<Tile>();
        _visuals = GetComponentInParent<TileHighlightVisuals>();
    }

    private void OnMouseEnter()
    {
        if (_parentTile != null && _parentTile.IsHighlighted && _visuals != null)
        {
            _wasEnemyHighlighted = _visuals.highlightEnemyTile != null && _visuals.highlightEnemyTile.activeSelf;
            _visuals.highlightEmptyTile?.SetActive(false);
            _visuals.highlightEnemyTile?.SetActive(false);
            _visuals.hoverHighlightTile?.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        ResetHoverEffect();
    }

    public void ResetHoverEffect()
    {
        if (_visuals == null) return;

        _visuals.hoverHighlightTile?.SetActive(false);

        if (_parentTile != null && _parentTile.IsHighlighted)
        {
            if (_wasEnemyHighlighted)
                _visuals.highlightEnemyTile?.SetActive(true);
            else
                _visuals.highlightEmptyTile?.SetActive(true);
        }
    }
}