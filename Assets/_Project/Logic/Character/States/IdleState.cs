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
        _character.StartIdle();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}