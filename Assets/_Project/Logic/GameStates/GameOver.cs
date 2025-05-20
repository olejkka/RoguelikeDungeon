
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        var deathListeners = _character.GetComponentsInChildren<MonoBehaviour>()
            .OfType<IDeathListener>()
            .ToList();
        foreach (var listener in deathListeners)
            listener.OnCharacterDeath(_character);
        
        TileHighlighter.Instance.ClearHighlights();
        
        _character.transform
            .DOScale(Vector3.zero, 1f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Object.Destroy(_character.gameObject));
        
        Debug.Log("(GameOver) Конец игры");
    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {
        
    }
}
