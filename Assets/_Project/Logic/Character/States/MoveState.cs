using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private Character _character;

    public MoveState(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("Entering MoveState");
        
        _character.StartMoving();
    }

    public override void Exit()
    {
        Debug.Log("Exit MoveState");
    }

    public override void Update()
    {
        
    }
}