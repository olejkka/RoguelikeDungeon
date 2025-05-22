using System;
using UnityEngine;

[RequireComponent(typeof(AvailableMovesHighlighter))]
public class Player : Character
{
    public event Action OnTransitionTileStepped;
    
    
    private void OnEnable()
    {
        ClickHandler.TileClicked += OnTileClicked;
        Mover.MovementFinished += OnMovementFinished;
    }

    private void OnDisable()
    {
        ClickHandler.TileClicked -= OnTileClicked;
        Mover.MovementFinished -= OnMovementFinished;
    }
    
    private void OnTileClicked(Tile clickedTile)
    {
        if (Mover.IsMoving) return;
        
        TargetTile = clickedTile;
    }
    
    private void OnMovementFinished()
    {
        if (CurrentTile.Type == TileType.Transition)
            OnTransitionTileStepped?.Invoke();
    }
}
