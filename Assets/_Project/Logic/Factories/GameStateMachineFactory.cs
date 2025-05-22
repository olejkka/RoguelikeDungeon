using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameStateMachineFactory
{
    public static GameStateMachine CreateGameStateMachine(Character character)
    {
        var enemies = Object.FindObjectsOfType<Enemy>().ToList();
        var scoringSelector = new ScoringMoveSelector();

        var states = new Dictionary<Type, IState>
        {
            [typeof(Pause)] = new Pause(CreateTransitionsFromPause(), character),
            [typeof(PlayerTurn)] = new PlayerTurn(CreateTransitionsFromPlayerTurn(character), character),
            [typeof(EnemyTurn)] = new EnemyTurn(CreateTransitionsFromEnemyTurn(enemies, character), enemies, scoringSelector),
            [typeof(GameOver)] = new GameOver(CreateTransitionsFromGameOver(), character),
        };

        return new GameStateMachine(states);
    }

    private static List<ITransition> CreateTransitionsFromPause()
    {
        return new List<ITransition>();
    }

    private static List<ITransition> CreateTransitionsFromPlayerTurn(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<GameOver>(() => character.Health.CurrentHealth <= 0),
            new TransitionTo<EnemyTurn>(() => character.RemainingActions == 0),
        };
    }

    private static List<ITransition> CreateTransitionsFromEnemyTurn(List<Enemy> enemies, Character player)
    {
        return new List<ITransition>
        {
            new TransitionTo<PlayerTurn>(() => enemies.All(e => e.RemainingActions == 0)),
            new TransitionTo<GameOver>(() => player.Health.CurrentHealth <= 0),
        };
    }

    private static List<ITransition> CreateTransitionsFromGameOver()
    {
        return new List<ITransition>();
    }
}