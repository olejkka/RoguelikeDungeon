using UnityEngine;

public class TileHoverHandler : MonoBehaviour
{
    private Tile parentTile;
    private TileHighlightVisuals visuals;
    private bool wasEnemyHighlighted;

    private void Start()
    {
        parentTile = GetComponentInParent<Tile>();
        visuals = GetComponentInParent<TileHighlightVisuals>();
    }

    private void OnMouseEnter()
    {
        if (parentTile != null && parentTile.IsHighlighted && visuals != null)
        {
            wasEnemyHighlighted = visuals.highlightEnemyTile != null && visuals.highlightEnemyTile.activeSelf;
            visuals.hoverHighlightTile?.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        ResetHoverEffect();
    }

    public void ResetHoverEffect()
    {
        if (visuals == null) return;

        visuals.hoverHighlightTile?.SetActive(false);

        if (parentTile != null && parentTile.IsHighlighted)
        {
            if (wasEnemyHighlighted)
                visuals.highlightEnemyTile?.SetActive(true);
            else
                visuals.highlightEmptyTile?.SetActive(true);
        }
    }
}