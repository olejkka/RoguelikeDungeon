using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{
    private Character _character;
    public EnemyTurn(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("(EnemyTurn) Начало хода противника");
    }

    public override void Exit()
    {
        Debug.Log("(EnemyTurn) Конец хода противника");
    }

    public override void Update()
    {
        
    }
}