using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterMover : MonoBehaviour
{
    private Character _character;

    public event Action MovementStarting;
    public event Action MovementFinished;

    public bool IsMoving { get; set; }

    private void Awake()
    {
        _character = GetComponent<Character>(); 
    }

    public void MoveTo(Tile targetTile)
    {
        if (IsMoving) return;
        if (targetTile == _character.CurrentTile ||
            targetTile.Type == TileType.Wall ||
            !targetTile.IsHighlighted ||
            targetTile.OccupiedCharacter != null)
        {
            Debug.Log("(CharacterMover) Movement is not possible");
            return;
        }

        MovementStarting?.Invoke();
        IsMoving = true;

        Vector3 finalPos = new Vector3(
            Mathf.Round(targetTile.transform.position.x),
            _character.transform.position.y,
            Mathf.Round(targetTile.transform.position.z)
        );
        
        _character.Animation.PlayRotate(
            targetTile.transform.position,
            () => {
                _character.Animation.PlayMove(finalPos, () => {
                    _character.CurrentTile = targetTile;
                    _character.TargetTile = null;
                    IsMoving = false;
                    MovementFinished?.Invoke();
                });
            }
        );
    }
    
    public void MoveToNearestFloor(Tile targetTile)
    {
        if (IsMoving || targetTile == null) return;
        
        var targetPos = targetTile.transform.position;
        
        var currentTile = _character.CurrentTile;
        if (currentTile != null)
        {
            var currentPos = currentTile.transform.position;
            var dx = Mathf.Abs(currentPos.x - targetPos.x);
            var dz = Mathf.Abs(currentPos.z - targetPos.z);
            if ((dx <= 1.01f && dz <= 1.01f))
            {
                MovementStarting?.Invoke();
                _character.Animation.PlayRotate(targetTile.transform.position);
                _character.TargetTile = null;
                MovementFinished?.Invoke();
                return;
            }
        }
        
        var candidates = new System.Collections.Generic.List<Tile>();
        if (targetTile.Type == TileType.Floor &&
            targetTile.OccupiedCharacter == null &&
            targetTile.IsHighlighted)
        {
            candidates.Add(targetTile);
        }
        else
        {
            candidates.AddRange(
                targetTile.GetNeighbors(_character.NeighborTilesSelectionSO)
                    .Where(t => t.Type == TileType.Floor &&
                                t.OccupiedCharacter == null &&
                                t.IsHighlighted)
            );
        }

        if (candidates.Count == 0)
        {
            Debug.Log("(CharacterMover) Нет доступных Floor-тайлов для перемещения к цели");
            return;
        }
        
        var bestTile = candidates
            .OrderBy(t => Vector3.Distance(targetPos, t.transform.position))
            .First();

        MoveTo(bestTile);
    }
}