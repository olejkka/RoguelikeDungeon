using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
{
    private readonly AvailableMovesHighlighter _availableMovesHighlighter;
    private readonly CharacterMover _mover;
    private Character _character;
    
    public PlayerTurn(
        IReadOnlyList<ITransition> transitions,
        Character character
        ) : base(transitions)
    {
        _character = character;
        _availableMovesHighlighter = character.GetComponent<AvailableMovesHighlighter>();
        _mover = character.GetComponent<CharacterMover>();
        
        if (_availableMovesHighlighter == null)
        {
            Debug.LogWarning("(PlayerTurn) Отсутствует компонент AvailableMovesHighlighter на Character"); 
        }
        
        if (_mover == null)
        {
            Debug.LogWarning("(PlayerTurn) Отсутствует компонент CharacterMover на Character");
        }
    }

    public override void Enter()
    {
        Debug.Log("(PlayerTurn) Начало хода игрока");
        
        _character.ResetActions();
        _availableMovesHighlighter.Highlight();
        
        _mover.MovementStarting += OnActionStarting;
    }
    
    private void OnActionStarting()
    {
        _mover.MovementStarting -= OnActionStarting;
        TileHighlighter.Instance.ClearHighlights();
    }

    public override void Exit()
    {
        _mover.MovementStarting -= OnActionStarting;
        
        Debug.Log("(PlayerTurn) Конец хода игрока");
    }

    public override void Update()
    {
        
    }
}