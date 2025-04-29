using UnityEngine;

public interface IMoveStrategy
{
    bool TryExecute(CharacterMover mover, Tile targetTile);
}