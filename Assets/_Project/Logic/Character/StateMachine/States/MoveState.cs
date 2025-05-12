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
        _character.MoveTo();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}