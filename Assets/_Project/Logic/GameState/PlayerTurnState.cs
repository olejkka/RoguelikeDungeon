using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Player _player;
    private readonly GameStateMachine _stateMachine;
    private readonly CharacterMover _characterMover;
    private readonly AvailableMovesHighlighter _moveLogic;
    private readonly List<Enemy> _enemies;
    public static event Action OnSteppingOnATransitionTile;

    
    public PlayerTurnState
    (
        GameStateMachine stateMachine,
        Player player,
        AvailableMovesHighlighter moveLogic,
        List<Enemy> enemies
        )
    {
        _stateMachine = stateMachine;
        _player = player;
        _moveLogic = moveLogic;
        _enemies = enemies;
        _characterMover = player.GetComponent<CharacterMover>();
    }

    public void Enter()
    {
        Debug.Log("Player are now available");
        ShowMoves();
    }

    public void Tick()
    {
    }
    
    private void ShowMoves()
    {
        _characterMover.OnMoveStarted += HandleMoveStarted;
        _characterMover.OnMoveFinished += HandleMoveFinished;
        TileHighlighter.Instance.ClearHighlights();
        _moveLogic.Highlight();
    }

    private void HandleMoveStarted()
    {
        _characterMover.OnMoveStarted -= HandleMoveStarted;
        
        TileHighlighter.Instance.ClearHighlights();
    }
    
    private void HandleMoveFinished()
    {
        _characterMover.OnMoveFinished -= HandleMoveFinished;
        
        TileHighlighter.Instance.ClearHighlights();

        if (_player.CurrentTile.Type == TileType.Transition)
        {
            OnSteppingOnATransitionTile?.Invoke();
        }
        else
        {
            _stateMachine.ChangeState(new EnemyTurnState(_stateMachine, _enemies));
        }
    }

    public void Exit()
    {
        _characterMover.OnMoveStarted -= HandleMoveStarted;
        _characterMover.OnMoveFinished -= HandleMoveFinished;
        
        TileHighlighter.Instance.ClearHighlights();
    }
}