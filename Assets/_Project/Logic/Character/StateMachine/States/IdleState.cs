using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private Character _character;
    public IdleState(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("Entering IdleState");
        _character.StartBreathing();
    }

    public override void Exit()
    {
        Debug.Log("Exiting IdleState");
        _character.StopAnimation();
    }

    public override void Update()
    {
        
    }
}