using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDestroy = 0f;
    
    private Health _health;
    private Character _character;
    private List<IDeathListener> _deathListeners;

    private void Awake()
    {
        if (_health == null)
        {
            _health = GetComponent<Health>();
            _character = GetComponent<Character>();
            _deathListeners = GetComponentsInChildren<MonoBehaviour>()
                .OfType<IDeathListener>()
                .ToList();
        }
        
        _health.Dead += HandleDeath;
    }

    private void OnDestroy()
    {
        _health.Dead -= HandleDeath;
    }

    private void HandleDeath()
    {
        foreach (var listener in _deathListeners)
        {
            listener.OnCharacterDeath(_character);
        }
        
        if (_character is Player)
        {
            TileHighlighter.Instance.ClearHighlights();
            GameStateMachine_.Instance.ChangeState(
                new GameOverState(GameStateMachine_.Instance)
            );
        }
        
        transform
            .DOScale(Vector3.zero, _delayBeforeDestroy)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
        
        Debug.Log($"(DeathHandler) Персонаж {_character.name} умер");
    }
}