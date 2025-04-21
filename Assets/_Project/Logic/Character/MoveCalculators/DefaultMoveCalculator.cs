﻿using UnityEngine;
using System.Collections.Generic;

public class DefaultMoveCalculator : MoveCalculator
{
    public override List<Tile> CalculateMoves(Tile currentTile, NeighborTilesSelectionSO settings, bool isPlayer)
    {
        List<Tile> moves = new List<Tile>();
        List<Tile> possibleMoves = currentTile.GetNeighbors(settings);

        Dictionary<Vector2Int, List<Tile>> directionalMoves = new Dictionary<Vector2Int, List<Tile>>();

        foreach (var offset in settings.GetOffsets())
        {
            directionalMoves[offset] = new List<Tile>();
        }

        foreach (var tile in possibleMoves)
        {
            if (tile == null) continue;

            Vector2Int direction = GetDirection(currentTile.Position, tile.Position);
            if (directionalMoves.ContainsKey(direction))
            {
                directionalMoves[direction].Add(tile);
            }
        }

        foreach (var entry in directionalMoves)
        {
            bool foundObstacle = false;
            foreach (var tile in entry.Value)
            {
                if (foundObstacle) break;
                if (tile.IsWall)
                {
                    foundObstacle = true;
                    break;
                }
                if (tile.OccupiedCharacter != null)
                {
                    moves.Add(tile);
                    foundObstacle = true; 
                }
                else
                {
                    moves.Add(tile);
                }
            }
        }

        return moves;
    }
}