using System;

public interface IState
{
    public void Enter();

    public void Exit();

    public void Update();

    public bool TryGetNextState(out Type type);
}