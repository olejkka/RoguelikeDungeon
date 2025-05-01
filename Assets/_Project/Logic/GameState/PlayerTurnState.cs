using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Player _player;
    private readonly GameStateMachine _stateMachine;
    private readonly CharacterMover _characterMover;
    private readonly Health _health;
    private readonly AvailableMovesHighlighter _availableMovesHighlighter;
    private readonly List<Enemy> _enemies;
    
    private float _initialDelaySeconds = 0.5f;
    public static event Action PlayerSteppedOnTheTransitionTile;

    
    public PlayerTurnState
    (
        GameStateMachine stateMachine,
        Player player,
        AvailableMovesHighlighter availableMovesHighlighter,
        List<Enemy> enemies
        )
    {
        _stateMachine = stateMachine;
        _player = player;
        _availableMovesHighlighter = availableMovesHighlighter;
        _enemies = enemies;
        _characterMover = player.GetComponent<CharacterMover>();
        _health = player.GetComponent<Health>();
    }

    public void Enter()
    {
        Debug.Log("Player are now available");
        
        _characterMover.MovementStarting += DOHandleMoveStarting;
        _characterMover.MovementFinished += HandleMoveFinished;
        _health.Dead += HandlePlayerDied;
        
        DOVirtual.DelayedCall(_initialDelaySeconds, ShowMoves);
    }

    public void Tick()
    {
    }
    
    private void ShowMoves()
    {
        TileHighlighter.Instance.ClearHighlights();
        _availableMovesHighlighter.Highlight();
    }

    private void DOHandleMoveStarting()
    {
        _characterMover.MovementStarting -= DOHandleMoveStarting;
        
        TileHighlighter.Instance.ClearHighlights();
    }
    
    private void HandleMoveFinished()
    {
        _characterMover.MovementFinished -= HandleMoveFinished;
        
        TileHighlighter.Instance.ClearHighlights();

        if (_player.CurrentTile.Type == TileType.Transition)
        {
            PlayerSteppedOnTheTransitionTile?.Invoke();
        }
        else
        {
            _stateMachine.ChangeState(new EnemyTurnState(_stateMachine, _enemies));
        }
    }
    
    private void HandlePlayerDied()
    {
        TileHighlighter.Instance.ClearHighlights();
        
        Debug.Log($"{_player.name} вмэр");
        // _stateMachine.ChangeState(new GameOverState(...));
    }

    public void Exit()
    {
        _characterMover.MovementStarting -= DOHandleMoveStarting;
        _characterMover.MovementFinished -= HandleMoveFinished;
        _health.Dead -= HandlePlayerDied;
        
        TileHighlighter.Instance.ClearHighlights();
        
        Debug.Log("Player are now not available");
    }
}