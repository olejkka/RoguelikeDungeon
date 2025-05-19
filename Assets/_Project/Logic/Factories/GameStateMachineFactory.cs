using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateMachineFactory
{
    public static GameStateMachine CreateGameStateMachine()
    {
        Dictionary<Type, IState> states = new Dictionary<Type, IState>
        {
            
        };

        return new GameStateMachine(states);
    }
    
    private static List<ITransition> CreateTransitionsFromPause()
    {
        return new List<ITransition>
        {
            
        };
    }

    private static List<ITransition> CreateTransitionsFromPlayerTurn()
    {
        return new List<ITransition>
        {
            
        };
    }
    
    private static List<ITransition> CreateTransitionsFromEnemyTurn()
    {
        return new List<ITransition>
        {
            
        };
    }
}