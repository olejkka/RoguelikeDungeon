using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDestroy = 0f;
    private Health _health;
    private CharacterMover _characterMover;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.Dead += HandleDeath;
    }

    private void OnDestroy()
    {
        _health.Dead -= HandleDeath;
    }

    private void HandleDeath()
    {
        var mover = GetComponent<CharacterMover>();
        if (mover != null)
            mover.ClearMoveEvents();
        
        transform
            .DOScale(Vector3.zero, _delayBeforeDestroy)
            .SetEase(Ease.InBack);
        
        DOVirtual.DelayedCall(_delayBeforeDestroy, () =>
        {
            Destroy(gameObject);
        });
    }
}