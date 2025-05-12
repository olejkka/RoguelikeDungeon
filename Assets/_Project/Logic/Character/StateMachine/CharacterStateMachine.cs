using System;
using System.Collections.Generic;

public class CharacterStateMachine : StateMachine
{
    public CharacterStateMachine(Dictionary<Type, IState> states) : base(states)
    {
    }
}