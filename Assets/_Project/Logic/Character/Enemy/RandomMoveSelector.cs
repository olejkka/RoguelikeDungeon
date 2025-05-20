using System.Collections.Generic;
using UnityEngine;

public class RandomMoveSelector : IEnemyMoveSelector
{
    public Tile SelectTile(Enemy enemy, List<Tile> availableTiles)
    {
        if (availableTiles == null || availableTiles.Count == 0)
            return null;
        
        int idx = Random.Range(0, availableTiles.Count);
        
        return availableTiles[idx];
    }
}