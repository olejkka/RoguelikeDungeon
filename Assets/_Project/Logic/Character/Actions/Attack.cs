using System.Linq;
using UnityEngine;

public static class Attack
{
    private static readonly Vector2Int[] AdjacentOffsets = new[]
    {
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  0),
        new Vector2Int( 0,  1),
        new Vector2Int( 0, -1),
        
        new Vector2Int( 1,  1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1,  1),
        new Vector2Int(-1, -1),
    };
    
    public static bool TryMeleeAttack(Character attacker, Tile targetTile, int damage)
    {
        var victim = targetTile.OccupiedCharacter;
        if (victim == null || !CharacterIdentifier.IsEnemy(attacker, victim))
            return false;
        
        var neighbors = AdjacentOffsets
            .Select(o => TilesRepository.Instance.GetTileAt(targetTile.Position + o))
            .Where(t => t != null && TileRules.IsWalkable(t))
            .ToList();
        
        if (neighbors.Contains(attacker.CurrentTile))
        {
            victim.Health.TakeDamage(damage);
            Debug.Log($"{victim.name} {victim.Health.CurrentHP}/{victim.Health.MaxHP}");
            return true;
        }
        
        var freeTiles = neighbors.Where(t => t.OccupiedCharacter == null).ToList();
        if (freeTiles.Count == 0) return false;
        
        var destination = freeTiles
            .OrderBy(t => Vector3.Distance(attacker.transform.position, t.transform.position))
            .First();

        var movement = attacker.GetComponent<CharacterMover>();
        void OnMoveFinished()
        {
            movement.OnMoveFinished -= OnMoveFinished;
            victim.Health.TakeDamage(damage);
        }
        movement.OnMoveFinished += OnMoveFinished;

        attacker.Move(destination);
        return true;
    }
}
