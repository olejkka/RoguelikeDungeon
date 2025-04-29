using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Выбирает следующий ход для врага, строго в рамках подсвеченных (доступных) тайлов:
/// 1. Если тайл игрока входит в availableTiles:
///    – С вероятностью AttackChance возвращаем именно тайл игрока (инициируем атаку через Movement→AttackService).
///    – Иначе выбираем ближайший к игроку тайл из availableTiles.
/// 2. Если тайл игрока НЕ входит в availableTiles:
///    – Всегда возвращаем ближайший к игроку тайл из availableTiles.
/// </summary>
public class ScoringMoveSelector : IEnemyMoveSelector
{
    // Вероятность атаковать (если игрок в зоне досягаемости)
    private float _attackChance = 0.5f;

    public Tile SelectTile(Enemy enemy, List<Tile> availableTiles)
    {
        if (availableTiles == null || availableTiles.Count == 0)
            return null;

        // Находим игрока и его тайл
        var player = GameObject.FindObjectOfType<Player>();
        if (player == null || player.CurrentTile == null)
        {
            // Если игрок не найден — fallback на случайный ход
            return new RandomMoveSelector().SelectTile(enemy, availableTiles);
        }
        Tile playerTile = player.CurrentTile;

        bool canReachPlayer = availableTiles.Contains(playerTile);

        // Если можем «достать» игрока
        if (canReachPlayer)
        {
            // С вероятностью атакуем
            if (Random.value < _attackChance)
            {
                return playerTile;
            }
            // Иначе будем двигаться
        }

        // 3) Выбираем ближайший к игроку тайл из availableTiles
        //    — считаем по квадратам дистанций, чтобы не тратить на корни
        int bestSqr = availableTiles
            .Min(t => (t.Position - playerTile.Position).sqrMagnitude);

        var closest = availableTiles
            .Where(t => (t.Position - playerTile.Position).sqrMagnitude == bestSqr)
            .ToList();

        // Если несколько «одинаковых» по расстоянию — выбираем случайно между ними
        return closest[Random.Range(0, closest.Count)];
    }
}
