using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private readonly List<Enemy> _enemies;
    private int _currentEnemyIndex = 0;

    public EnemyTurnState(GameStateMachine stateMachine, List<Enemy> enemies)
    {
        _stateMachine = stateMachine;
        _enemies = enemies;
    }

    public void Enter()
    {
        StartNextEnemyMove();
    }

    public void Tick() { }

    private void StartNextEnemyMove()
    {
        TileHighlighter.Instance.ClearHighlights();

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

        if (availableTiles.Count == 0)
        {
            FinishEnemyMove();
            return;
        }
        
        Tile chosenTile = availableTiles[Random.Range(0, availableTiles.Count)];
        
        // Debug.Log($"chosenTile - {chosenTile.Position}");
        
        Movement movement = enemy.GetComponent<Movement>();
        movement.OnMoveFinished += HandleMoveFinished;
        
        enemy.Move(chosenTile);
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
