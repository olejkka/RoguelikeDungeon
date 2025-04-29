using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Выбирает следующий ход для врага, строго в рамках подсвеченных тайлов:
/// </summary>
public class ScoringMoveSelector : IEnemyMoveSelector
{
    private float _attackChance = 0.5f;

    public Tile SelectTile(Enemy enemy, List<Tile> availableTiles)
    {
        if (availableTiles == null || availableTiles.Count == 0)
            return null;
        
        var player = GameObject.FindObjectOfType<Player>();
        if (player == null || player.CurrentTile == null)
        {
            return new RandomMoveSelector().SelectTile(enemy, availableTiles);
        }
        Tile playerTile = player.CurrentTile;

        bool canReachPlayer = availableTiles.Contains(playerTile);
        
        if (canReachPlayer)
        {
            if (Random.value < _attackChance)
            {
                return playerTile;
            }
        }

        // Выбираем ближайший к игроку тайл из availableTiles
        int bestSqr = availableTiles
            .Min(t => (t.Position - playerTile.Position).sqrMagnitude);

        var closest = availableTiles
            .Where(t => (t.Position - playerTile.Position).sqrMagnitude == bestSqr)
            .ToList();

        // Если несколько одинаковых по расстоянию — выбираем случайно между ними
        return closest[Random.Range(0, closest.Count)];
    }
}
