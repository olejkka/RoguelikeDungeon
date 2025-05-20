using System;
using System.Collections.Generic;

public static class GameStateMachineFactory
{
    public static GameStateMachine CreateGameStateMachine(Character character)
    {
        Dictionary<Type, IState> states = new Dictionary<Type, IState>
        {
            [typeof(Pause)] = new Pause(CreateTransitionsFromPause(character), character),
            [typeof(PlayerTurnState)] = new PlayerTurn(CreateTransitionsFromPlayerTurn(character), character),
            [typeof(EnemyTurn)] = new EnemyTurn(CreateTransitionsFromEnemyTurn(character), character),
            [typeof(GameOver)] = new GameOver(CreateTransitionsFromGameOver(character), character),
        };

        return new GameStateMachine(states);
    }
    
    private static List<ITransition> CreateTransitionsFromPause(Character character)
    {
        return new List<ITransition>
        {
            
        };
    }

    private static List<ITransition> CreateTransitionsFromPlayerTurn(Character character)
    {
        return new List<ITransition>
        {
            
        };
    }
    
    private static List<ITransition> CreateTransitionsFromEnemyTurn(Character character)
    {
        return new List<ITransition>
        {
            
        };
    }
    
    private static List<ITransition> CreateTransitionsFromGameOver(Character character)
    {
        return new List<ITransition>
        {
            
        };
    }
}