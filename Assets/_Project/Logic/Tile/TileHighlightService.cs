using System.Collections.Generic;
using System.Linq;

public static class TileHighlightService
{
    private static TileHighlighter _tileHighlighter;
    
    
    public static void Init(TileHighlighter tileHighlighter)
    {
        _tileHighlighter = tileHighlighter;
    }
    
    public static void HighlightTiles(Character character, List<Tile> moves)
    {
        List<Tile> enemyTiles = moves
            .Where(tile => tile.OccupiedCharacter != null &&
                           CharacterIdentifier.IsEnemy(character, tile.OccupiedCharacter))
            .ToList();

        List<Tile> emptyTiles = moves
            .Where(tile => tile.OccupiedCharacter == null)
            .ToList();

        _tileHighlighter.HighlightEmptyTiles(emptyTiles);
        _tileHighlighter.HighlightEnemyTiles(enemyTiles);
    }
}