using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
{
    private readonly AvailableMovesHighlighter _availableMovesHighlighter;
    private Character _character;
    
    public PlayerTurn(IReadOnlyList<ITransition> transitions, Character character) : base(transitions)
    {
        _character = character;
    }

    public override void Enter()
    {
        Debug.Log("(PlayerTurn) Начало хода игрока");
        
        TileHighlighter.Instance.ClearHighlights();
        _availableMovesHighlighter.Highlight();
    }

    public override void Exit()
    {
        TileHighlighter.Instance.ClearHighlights();
        
        Debug.Log("(PlayerTurn) Конец хода игрока");
    }

    public override void Update()
    {
        
    }
}