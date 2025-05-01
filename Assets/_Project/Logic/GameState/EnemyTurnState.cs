using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyTurnState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private readonly List<Enemy> _enemies;
    private Health _playerHealth;
    private readonly IEnemyMoveSelector _moveSelector;
    private readonly Dictionary<Health, Action> _deathHandlers = new Dictionary<Health, Action>();
    
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
        var player = GameObject.FindObjectOfType<Player>();
        _playerHealth = player.GetComponent<Health>();
        _playerHealth.Dead += HandlePlayerDied;
        _moveSelector = new ScoringMoveSelector();
    }

    public void Enter()
    {
        Debug.Log("Enemies are now available");
        
        foreach (var enemy in _enemies.ToList())
        {
            var health = enemy.GetComponent<Health>();
            void OnDeadHandler() => HandleEnemyDied(enemy);
            health.Dead += OnDeadHandler;
            _deathHandlers[health] = OnDeadHandler;
        }
        
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
        
        mover.MovementStarting  += DOHandleMoveStarting;
        mover.MovementFinished += HandleMoveFinished;

        enemy.Move(chosenTile);
    }

    private void DOHandleMoveStarting()
    {
        TileHighlighter.Instance.ClearHighlights();
        
        var mover = _enemies[_currentEnemyIndex].GetComponent<CharacterMover>();
        mover.MovementStarting -= DOHandleMoveStarting;
    }
    
    private void HandleMoveFinished()
    {
        var mover = _enemies[_currentEnemyIndex].GetComponent<CharacterMover>();
        mover.MovementFinished -= HandleMoveFinished;

        FinishEnemyMove();
    }

    private void FinishEnemyMove()
    {
        _currentEnemyIndex++;
        StartNextEnemyMove();
    }
    
    private void HandleEnemyDied(Enemy deadEnemy)
    {
        var health = deadEnemy.GetComponent<Health>();
        if (_deathHandlers.TryGetValue(health, out var handler))
        {
            health.Dead -= handler;
            _deathHandlers.Remove(health);
        }
        
        var diedIndex = _enemies.IndexOf(deadEnemy);
        _enemies.RemoveAt(diedIndex);
        
        if (diedIndex <= _currentEnemyIndex && _currentEnemyIndex > 0)
            _currentEnemyIndex--;
        
        if (diedIndex == _currentEnemyIndex)
            FinishEnemyMove();
    }
    
    private void HandlePlayerDied()
    {
        TileHighlighter.Instance.ClearHighlights();
        _stateMachine.ChangeState(new GameOverState(_stateMachine));
    }

    public void Exit()
    {
        foreach (var kvp in _deathHandlers)
        {
            kvp.Key.Dead -= kvp.Value;
        }
        _deathHandlers.Clear();
        
        foreach (var enemy in _enemies)
        {
            var mover = enemy.GetComponent<CharacterMover>();
            mover.MovementStarting  -= DOHandleMoveStarting;
            mover.MovementFinished -= HandleMoveFinished;
        }
        
        _playerHealth.Dead -= HandlePlayerDied;
        
        TileHighlighter.Instance.ClearHighlights();
        _currentEnemyIndex = 0;
        
        Debug.Log("Enemies are now not available");
    }
}
