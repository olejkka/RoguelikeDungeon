using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
            List<Tile> sortedTiles = entry.Value
                .OrderBy(t => Vector2Int.Distance(currentTile.Position, t.Position))
                .ToList();

            foreach (var tile in sortedTiles)
            {
                if (tile.Type == TileType.Wall)
                    break;

                if (tile.OccupiedCharacter != null)
                {
                    if (CharacterIdentifier.IsEnemy(currentTile.OccupiedCharacter, tile.OccupiedCharacter))
                        moves.Add(tile);

                    break;
                }

                moves.Add(tile);
            }
        }

        return moves;
    }
}