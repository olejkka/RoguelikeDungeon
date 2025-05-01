using System.Linq;
using UnityEngine;

/// <summary>
/// Стратегия ближней атаки перед движением.
/// Перед нанесением урона персонаж разворачивается к цели.
/// </summary>
[CreateAssetMenu(menuName = "MoveAction/Strategies/MeleeAttackStrategy")]
public class MeleeAttackStrategy : ScriptableObject, IMoveStrategy
{
    [SerializeField] private int _damage = 10;

    private static readonly Vector2Int[] AdjacentOffsets = new[]
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1)
    };

    public bool TryExecute(CharacterMover mover, Tile targetTile)
    {
        var attacker = mover.GetComponent<Character>();
        var victim = targetTile.OccupiedCharacter;
        if (victim == null || CharacterIdentifier.IsEnemy(attacker, victim) == false)
            return false;

        // Если атакующий уже рядом с целью — сразу повернуться и нанести урон
        var attackerTile = attacker.CurrentTile;
        var adjacentTiles = AdjacentOffsets
            .Select(o => TilesRepository.Instance.GetTileAt(targetTile.Position + o));
        if (adjacentTiles.Contains(attackerTile))
        {
            mover.RotateTowards(victim.transform.position, () =>
            {
                victim.Health.TakeDamage(_damage);
                
                mover.RaiseOnMoveFinished();
                
                Debug.Log($"Melee attack - {victim.name} took damage {_damage}. Current HP - {victim.Health.CurrentHealth}");
            });
            
            return true;
        }

        // Иначе ищем свободный соседний тайл для подхода
        var freeTiles = adjacentTiles
            .Where(t => t != null && t.Type != TileType.Wall && t.OccupiedCharacter == null)
            .ToList();
        if (freeTiles.Any() == false)
            return false;

        var destination = freeTiles
            .OrderBy(t => Vector3.Distance(mover.transform.position, t.transform.position))
            .First();

        // После завершения движения — поворот и атака
        void OnMoveFinished()
        {
            mover.MovementFinished -= OnMoveFinished;
            mover.RotateTowards(victim.transform.position, () =>
            {
                victim.Health.TakeDamage(_damage);
                
                if (victim.Health.CurrentHealth == 0)
                {
                    mover.ClearMoveEvents();
                }
                
                mover.RaiseOnMoveFinished();
                Debug.Log($"Melee attack after move - {victim.name} took damage {_damage}. Current HP - {victim.Health.CurrentHealth}");
            });
        }

        mover.MovementFinished += OnMoveFinished;
        mover.InternalMove(destination);
        return true;
    }
}
