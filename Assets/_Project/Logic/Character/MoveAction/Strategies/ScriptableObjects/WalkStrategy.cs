using UnityEngine;

/// <summary>
/// Стратегия передвижения на пустой тайл
/// </summary>
[CreateAssetMenu(fileName = "WalkStrategy", menuName = "MoveAction/Strategies/WalkStrategy")]
public class WalkStrategy : ScriptableObject, IMoveStrategy
{
    public bool TryExecute(CharacterMover mover, Tile targetTile)
    {
        mover.InternalMove(targetTile);
        return true;
    }
}
