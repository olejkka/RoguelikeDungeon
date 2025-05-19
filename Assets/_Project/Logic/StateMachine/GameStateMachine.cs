using System;
using System.Collections.Generic;

public class GameStateMachine : StateMachine
{
    public GameStateMachine(Dictionary<Type, IState> states) : base(states)
    {
    }
}