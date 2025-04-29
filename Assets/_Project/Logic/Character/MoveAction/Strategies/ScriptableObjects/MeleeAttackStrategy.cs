using System.Linq;
using UnityEngine;

/// <summary>
/// Стратегия ближней атаки перед движением.
/// Если цель — враг и рядом с ней есть свободный тайл или сам атакующий на соседнем, то наносит урон.
/// </summary>
[CreateAssetMenu(menuName = "MoveAction/Strategies/MeleeAttackStrategy")]
public class MeleeAttackStrategy : ScriptableObject, IMoveStrategy
{
    [SerializeField] private int _damage = 10;
    
    private static readonly Vector2Int[] AdjacentOffsets = new[]
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, 1), new Vector2Int(-1, -1)
    };

    public bool TryExecute(CharacterMover mover, Tile targetTile)
    {
        var attacker = mover.GetComponent<Character>();
        var victim = targetTile.OccupiedCharacter;
        if (victim == null || !CharacterIdentifier.IsEnemy(attacker, victim))
            return false;
        
        var neighbors = AdjacentOffsets
            .Select(o => TilesRepository.Instance.GetTileAt(targetTile.Position + o))
            .Where(t => t != null && TileRules.IsWalkable(t))
            .ToList();

        // Если атакующий уже рядом — сразу бьем
        if (neighbors.Contains(attacker.CurrentTile))
        {
            victim.Health.TakeDamage(_damage);
            
            Debug.Log($"Атака без передвижения - {victim.name} took damage {_damage}. Current HP - {victim.Health.CurrentHealth}");
            return true;
        }

        // Иначе идем на ближайший свободный тайл, а по OnMoveFinished бьем
        var freeTiles = neighbors.Where(t => t.OccupiedCharacter == null).ToList();
        if (freeTiles.Count == 0)
            return false;

        var destination = freeTiles
            .OrderBy(t => Vector3.Distance(attacker.transform.position, t.transform.position))
            .First();

        void OnMoveFinished()
        {
            mover.OnMoveFinished -= OnMoveFinished;
            victim.Health.TakeDamage(_damage);
            
            Debug.Log($"Атака с передвижением - {victim.name} took damage {_damage}. Current HP - {victim.Health.CurrentHealth}");
        }
        mover.OnMoveFinished += OnMoveFinished;

        mover.InternalMove(destination);
        return true;
    }
}

