using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyTurn : State
{
    private readonly IEnemyMoveSelector _moveSelector;
    private readonly List<Enemy> _enemies;
    
    private float _delayBeforeNextMove = 0.5f;
    private Tween _delayedCall;
    private int _currentEnemyIndex;

    public EnemyTurn(
        IReadOnlyList<ITransition> transitions,
        List<Enemy> enemies,
        IEnemyMoveSelector moveSelector
    ) : base(transitions)
    {
        _enemies = enemies;
        _moveSelector = moveSelector;
    }

    public override void Enter()
    {
        Debug.Log("(EnemyTurnState) Начало хода противника");
        
        foreach (var enemy in _enemies)
            enemy.ResetActions();
        
        _currentEnemyIndex = 0;
        
        _delayedCall = DOVirtual.DelayedCall(_delayBeforeNextMove, StartNextEnemyMove)
            .SetId(this);
    }

    private void StartNextEnemyMove()
    {
        if (_currentEnemyIndex >= _enemies.Count)
            return;

        var enemy = _enemies[_currentEnemyIndex];

        var moves = AvailableMovesCalculator.GetAvailableTiles(enemy);
        var enemyTiles = moves
            .Where(t => t.OccupiedCharacter != null && CharacterIdentifier.IsEnemy(enemy, t.OccupiedCharacter))
            .ToList();
        var emptyTiles = moves.Except(enemyTiles).ToList();

        TileHighlighter.Instance.ClearHighlights();
        TileHighlighter.Instance.HighlightEmptyTiles(emptyTiles);
        TileHighlighter.Instance.HighlightEnemyTiles(enemyTiles);
        
        if (moves.Count == 0)
        {
            FinishEnemyMove();
            return;
        }

        Tile chosenTile = _moveSelector.SelectTile(enemy, moves);
        
        if (chosenTile == null)
        {
            FinishEnemyMove();
            return;
        }

        Debug.Log($"(EnemyTurn) Enemy #{_currentEnemyIndex} chosen tile: {chosenTile} (Occupied: {chosenTile.OccupiedCharacter})");
        
        void OnActionStarting()
        {
            enemy.Mover.MovementStarting -= OnActionStarting;
            TileHighlighter.Instance.ClearHighlights();
        }
        
        enemy.Mover.MovementStarting += OnActionStarting;
        
        void OnActionComplete()
        {
            enemy.Mover.MovementFinished -= OnActionComplete;
            FinishEnemyMove();
        }
        
        enemy.Mover.MovementFinished += OnActionComplete;
        
        enemy.TargetTile = chosenTile;
    }

    private void FinishEnemyMove()
    {
        _currentEnemyIndex++;
        
        if (_currentEnemyIndex < _enemies.Count)
            DOVirtual.DelayedCall(_delayBeforeNextMove, StartNextEnemyMove)
                .SetId(this);
    }

    public override void Exit()
    {
        _currentEnemyIndex = 0;
        DOTween.Kill(this);
        
        Debug.Log("(EnemyTurn) Конец хода противника");
    }

    public override void Update()
    {
        
    }
}
