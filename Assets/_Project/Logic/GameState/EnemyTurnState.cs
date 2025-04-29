using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyTurnState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private readonly List<Enemy> _enemies;
    private readonly IEnemyMoveSelector _moveSelector;
    
    private int _currentEnemyIndex;
    private float _initialDelaySeconds = 0.5f;
    

    public EnemyTurnState
    (
        GameStateMachine stateMachine, 
        List<Enemy> enemies
        )
    {
        _stateMachine = stateMachine;
        _enemies = enemies;
        _moveSelector = new ScoringMoveSelector();
    }

    public void Enter()
    {
        // Debug.Log("Enemies are now available");
        
        TileHighlighter.Instance.ClearHighlights();
        DOVirtual.DelayedCall(_initialDelaySeconds, StartNextEnemyMove);
    }

    public void Tick() { }

    private void StartNextEnemyMove()
    {
        if (_currentEnemyIndex >= _enemies.Count)
        {
            var player = GameObject.FindObjectOfType<Player>();
            var highlighter = player.GetComponent<AvailableMovesHighlighter>();

            TileHighlighter.Instance.ClearHighlights();
            var nextState = new PlayerTurnState(_stateMachine, player, highlighter, _enemies);
            _stateMachine.ChangeState(nextState);
            return;
        }

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
        
        var chosenTile = _moveSelector.SelectTile(enemy, moves);
        if (chosenTile == null)
        {
            FinishEnemyMove();
            return;
        }

        var mover = enemy.GetComponent<CharacterMover>();
        
        mover.OnMoveStarted  += HandleMoveStarted;
        mover.OnMoveFinished += HandleMoveFinished;

        enemy.Move(chosenTile);
    }

    private void HandleMoveStarted()
    {
        TileHighlighter.Instance.ClearHighlights();
        
        var mover = _enemies[_currentEnemyIndex].GetComponent<CharacterMover>();
        mover.OnMoveStarted -= HandleMoveStarted;
    }
    
    private void HandleMoveFinished()
    {
        var mover = _enemies[_currentEnemyIndex].GetComponent<CharacterMover>();
        mover.OnMoveFinished -= HandleMoveFinished;

        FinishEnemyMove();
    }

    private void FinishEnemyMove()
    {
        _currentEnemyIndex++;
        StartNextEnemyMove();
    }

    public void Exit()
    {
        foreach (var enemy in _enemies)
        {
            var mover = enemy.GetComponent<CharacterMover>();
            mover.OnMoveStarted  -= HandleMoveStarted;
            mover.OnMoveFinished -= HandleMoveFinished;
        }
        
        TileHighlighter.Instance.ClearHighlights();
        _currentEnemyIndex = 0;
        
        // Debug.Log("Enemies are now not available");
    }
}
