using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : IState
{
    private IReadOnlyList<ITransition> _transitions;

    public State(IReadOnlyList<ITransition> transitions)
    {
        _transitions = transitions;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();

    public bool TryGetNextState(out Type type)
    {
        foreach (var transition in _transitions)
        {
            if (transition.CanTransit())
            {
                type = transition.NextState;
                return true;
            }
        }

        type = null;
        return false;
    }
}