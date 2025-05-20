
using System.Collections.Generic;
using UnityEngine;

public class GameOver : State
{
    private Character _character;
    public GameOver(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("(GameOver) Конец игры");
    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {
        
    }
}
