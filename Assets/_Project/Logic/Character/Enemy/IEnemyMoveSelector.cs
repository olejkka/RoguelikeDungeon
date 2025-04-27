using System.Collections.Generic;

public interface IEnemyMoveSelector
{
    Tile SelectTile(Enemy enemy, List<Tile> availableTiles);
}