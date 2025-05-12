using System;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterStateMachineFactory
{
    private const float InteractDistance = 2f;

    public static CharacterStateMachine CreateCharacterStateMachine(Character character)
    {
        Dictionary<Type, IState> states = new Dictionary<Type, IState>
        {
            [typeof(IdleState)] = new IdleState(CreateTransitionsFromIdle(character), character),
            [typeof(MoveState)] = new MoveState(CreateTransitionsFromMove(character), character),
        };

        return new CharacterStateMachine(states);
    }

    private static List<ITransition> CreateTransitionsFromMove(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<IdleState>(() => character.HasHasTargetTile == false)
        };
    }

    private static List<ITransition> CreateTransitionsFromIdle(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<MoveState>(() => character.HasHasTargetTile)
        };
    }
}