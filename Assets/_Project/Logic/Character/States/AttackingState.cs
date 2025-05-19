using System.Collections.Generic;
using UnityEngine;

public class AttackingState : State
{
    private Character _character;

    public AttackingState(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        _character.StartAttacking();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}