using System.Collections.Generic;
using UnityEngine;

public class Pause : State
{
    private Character _character;
    public Pause(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("(Pause) Начало паузы");
    }

    public override void Exit()
    {
        Debug.Log("(Pause) Конец паузы");
    }

    public override void Update()
    {
        
    }
}