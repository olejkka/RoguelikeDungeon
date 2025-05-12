using System;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterStateMachineFactory
{
    public static CharacterStateMachine CreateCharacterStateMachine(Character character)
    {
        Dictionary<Type, IState> states = new Dictionary<Type, IState>
        {
            [typeof(IdleState)] = new IdleState(CreateTransitionsFromIdle(character), character),
            [typeof(MoveState)] = new MoveState(CreateTransitionsFromMove(character), character),
            [typeof(AttackingState)] = new AttackingState(CreateTransitionsFromAttacking(character), character),
        };

        return new CharacterStateMachine(states);
    }
    
    private static List<ITransition> CreateTransitionsFromIdle(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<MoveState>(() => character.HasTargetTile && character.TargetTile.OccupiedCharacter == null),
            new TransitionTo<AttackingState>(() => character.HasTargetTile && character.TargetTile.OccupiedCharacter != null)
        };
    }

    private static List<ITransition> CreateTransitionsFromMove(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<IdleState>(() => character.HasTargetTile == false),
            new TransitionTo<AttackingState>(() => character.HasTargetTile && character.TargetTile.OccupiedCharacter != null)
        };
    }
    
    private static List<ITransition> CreateTransitionsFromAttacking(Character character)
    {
        return new List<ITransition>
        {
            new TransitionTo<IdleState>(() => !character.HasTargetTile || character.TargetTile.OccupiedCharacter == null),
        };
    }
}