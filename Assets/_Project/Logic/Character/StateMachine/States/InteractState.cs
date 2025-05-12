using System.Collections.Generic;
using UnityEngine;

public class InteractState : State
{
    private Player _player;

    public InteractState(IReadOnlyList<ITransition> transitions, Player player) : base(transitions)
    {
        _player = player;
    }

    public override void Enter()
    {
        Debug.Log("Entering InteractState");
    }

    public override void Exit()
    {
        Debug.Log("Exiting InteractState");
    }

    public override void Update()
    {
        
    }
}