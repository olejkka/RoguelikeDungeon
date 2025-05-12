using System;

public interface ITransition
{
    public Type NextState { get;}

    public bool CanTransit();
}