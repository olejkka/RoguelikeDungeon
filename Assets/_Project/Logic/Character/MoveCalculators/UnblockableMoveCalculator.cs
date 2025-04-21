using System.Collections.Generic;

public class UnblockableMoveCalculator : MoveCalculator
{
    public override List<Tile> CalculateMoves(Tile currentTile, NeighborTilesSelectionSO settings, bool isPlayer)
    {
        List<Tile> moves = new List<Tile>();
        List<Tile> possibleMoves = currentTile.GetNeighbors(settings);

        foreach (var tile in possibleMoves)
        {
            if (tile == null) continue;
            if (tile.IsWall) continue;

            var target = tile.OccupiedCharacter;
            if (target == null || CharacterIdentifier.IsEnemy(currentTile.OccupiedCharacter, target))
            {
                moves.Add(tile);
            }
        }

        return moves;
    }
}