using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterMoveService
{
    public static List<Tile> GetAvailableToMoveTiles(Character character)
    {
        if (character.CurrentTile == null)
            return new List<Tile>();

        MoveCalculator calculator = MoveCalculatorFactory.Create(character.NeighborTilesSelectionSO);
        List<Tile> rawMoves = calculator.CalculateMoves(
            character.CurrentTile,
            character.NeighborTilesSelectionSO,
            CharacterIdentifier.IsPlayer(character)
        );

        return rawMoves
            .Where(TileRules.IsWalkable)
            .Where(tile =>
                tile.OccupiedCharacter == null ||
                CharacterIdentifier.IsEnemy(character, tile.OccupiedCharacter))
            .ToList();
    }
}