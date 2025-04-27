using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Player _player;
    private readonly GameStateMachine _stateMachine;
    private readonly Movement _movement;
    private readonly HighlighterAwalibleMoves _moveLogic;
    private readonly List<Enemy> _enemies;
    public static event Action OnSteppingOnATransitionTile;

    public PlayerTurnState(
        GameStateMachine stateMachine,
        Player player,
        HighlighterAwalibleMoves moveLogic,
        List<Enemy> enemies)
    {
        _stateMachine = stateMachine;
        _player = player;
        _moveLogic = moveLogic;
        _enemies = enemies;
        _movement = player.GetComponent<Movement>();
    }

    public void Enter()
    {
        _movement.OnMoveFinished += HandleMoveFinished;
        
        _moveLogic.HighlightAvailableToMoveTiles();
    }

    public void Tick()
    {
        
    }

    private void HandleMoveFinished()
    {
        _movement.OnMoveFinished -= HandleMoveFinished;

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
        _movement.OnMoveFinished -= HandleMoveFinished;
    }
}