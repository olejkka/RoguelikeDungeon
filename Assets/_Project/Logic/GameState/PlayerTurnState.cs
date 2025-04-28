using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private const float InitialDelaySeconds = 0.5f;
    
    private Player _player;
    private readonly GameStateMachine _stateMachine;
    private readonly Movement _movement;
    private readonly HighlighterAwalibleMoves _moveLogic;
    private readonly List<Enemy> _enemies;
    public static event Action OnSteppingOnATransitionTile;

    
    public PlayerTurnState
    (
        GameStateMachine stateMachine,
        Player player,
        HighlighterAwalibleMoves moveLogic,
        List<Enemy> enemies
        )
    {
        _stateMachine = stateMachine;
        _player = player;
        _moveLogic = moveLogic;
        _enemies = enemies;
        _movement = player.GetComponent<Movement>();
    }

    public void Enter()
    {
        DOVirtual.DelayedCall(InitialDelaySeconds, ShowMoves);
    }

    public void Tick()
    {
    }
    
    private void ShowMoves()
    {
        _movement.OnMoveFinished += HandleMoveFinished;
        _moveLogic.HighlightAvailableToMoveTiles();
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