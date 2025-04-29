using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AvailableMovesHighlighter : MonoBehaviour
{
    TileHighlighter _tileHighlighter;
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _tileHighlighter = Bootstrapper.Instance.TileHighlighter;
    }

    public void Highlight()
    {
        if (_character.CurrentTile == null)
        {
            Debug.LogWarning($"[AvailableMovesHighlighter] {name} cannot find CurrentTile!");
            return;
        }

        var moves = AvailableMovesCalculator.GetAvailableTiles(_character);

        var enemyTiles = moves
            .Where(t => t.OccupiedCharacter != null && CharacterIdentifier.IsEnemy(_character, t.OccupiedCharacter))
            .ToList();
        var emptyTiles = moves.Except(enemyTiles).ToList();

        _tileHighlighter.HighlightEmptyTiles(emptyTiles);
        _tileHighlighter.HighlightEnemyTiles(enemyTiles);
    }
}