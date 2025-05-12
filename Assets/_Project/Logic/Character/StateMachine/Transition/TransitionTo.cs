using System;

public class TransitionTo<T> : ITransition where T : IState
{
    private Func<bool> _condition;

    public Type NextState => typeof(T);

    public TransitionTo(Func<bool> condition)
    {
        _condition = condition;
    }

    public bool CanTransit()
    {
        return _condition();
    }
}