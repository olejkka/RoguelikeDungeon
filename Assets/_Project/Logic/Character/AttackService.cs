using UnityEngine;

public static class AttackService
{
    public static bool TryMeleeAttack(Character attacker, Tile targetTile, int damage)
    {
        var target = targetTile.OccupiedCharacter;
        if (target != null && CharacterIdentifier.IsEnemy(attacker, target))
        {
            target.Health.TakeDamage(damage);
            
            Debug.Log($"{target.name} take damage {target.Health.CurrentHP}/{target.Health.MaxHP}");
            
            return true;
        }
        return false;
    }
}