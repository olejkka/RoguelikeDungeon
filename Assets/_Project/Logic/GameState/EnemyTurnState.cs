using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyTurnState : IGameState
{
    private const float InitialDelaySeconds = 0.5f;
    
    private readonly GameStateMachine _stateMachine;
    private readonly List<Enemy> _enemies;
    private readonly IEnemyMoveSelector _moveSelector;
    private int _currentEnemyIndex = 0;
    

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
        DOVirtual.DelayedCall(InitialDelaySeconds, StartNextEnemyMove);
    }

    public void Tick() { }

    private void StartNextEnemyMove()
    {
        if (_currentEnemyIndex >= _enemies.Count)
        {
            var player = GameObject.FindObjectOfType<Player>();
            var moveLogic = GameObject.FindObjectOfType<HighlighterAwalibleMoves>();
            var nextState = new PlayerTurnState(_stateMachine, player, moveLogic, _enemies);
            _stateMachine.ChangeState(nextState);
            return;
        }

        Enemy enemy = _enemies[_currentEnemyIndex];
        
        List<Tile> availableTiles = CharacterMoveService.GetAvailableToMoveTiles(enemy);
        
        TileHighlightService.HighlightTiles(enemy, availableTiles);

        if (availableTiles == null || availableTiles.Count == 0)
        {
            FinishEnemyMove();
            return;
        }

        // Выбираем тайл через ScoringMoveSelector
        Tile chosenTile = _moveSelector.SelectTile(enemy, availableTiles);
        Debug.Log($"{enemy.name} chose {chosenTile.Position}");
        if (chosenTile == null)
        {
            FinishEnemyMove();
            return;
        }

        // Подписываемся на окончание анимации движения
        var movement = enemy.GetComponent<Movement>();
        movement.OnMoveFinished += HandleMoveFinished;

        // Запускаем движение (или атаку через AttackService внутри Movement.Move)
        enemy.Move(chosenTile);
        Debug.Log($"{enemy.name} moved to {chosenTile.Position}");
    }

    private void HandleMoveFinished()
    {
        Enemy currentEnemy = _enemies[_currentEnemyIndex];
        Movement movement = currentEnemy.GetComponent<Movement>();
        
        movement.OnMoveFinished -= HandleMoveFinished;

        FinishEnemyMove();
    }

    private void FinishEnemyMove()
    {
        _currentEnemyIndex++;
        StartNextEnemyMove();
    }

    public void Exit()
    {
        _currentEnemyIndex = 0;
    }
}
