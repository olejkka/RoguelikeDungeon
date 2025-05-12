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
        _character.MoveTo();
    }

    public override void Exit()
    {
        Debug.Log("Exiting MoveState");
        // _character.Stop();
    }

    public override void Update()
    {
        
    }
}