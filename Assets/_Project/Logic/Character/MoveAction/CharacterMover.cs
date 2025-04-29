using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Character))]
public class CharacterMover : MonoBehaviour
{
    [Header("Move Strategies")]
    [SerializeField] private List<ScriptableObject> _moveStrategiesSO;
    private List<IMoveStrategy> _moveStrategies;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _numJumps = 2;
    [SerializeField] private float _durationOfJump = 1f;
    [SerializeField] private float _durationOfRotate = 0.2f;

    private Character _character;
    public event Action OnMoveStarted;
    public event Action OnMoveFinished;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _character = GetComponent<Character>();
        _moveStrategies = _moveStrategiesSO
            .OfType<IMoveStrategy>()
            .ToList();
    }

    /// <summary>
    /// Последовательный перебор стратегий.
    /// </summary>
    public void Move(Tile targetTile)
    {
        foreach (var strat in _moveStrategies)
        {
            if (strat.TryExecute(this, targetTile))
            {
                // Debug.Log($"{name} Выбрана стратегия {strat.GetType().Name}");
                
                if (!IsMoving)
                    OnMoveFinished?.Invoke();

                return;
            }
        }
    }

    /// <summary>
    /// Фактическое движение: поворот, прыжок, смена CurrentTile и события.
    /// </summary>
    public void InternalMove(Tile targetTile)
    {
        if (targetTile == _character.CurrentTile ||
            targetTile.Type == TileType.Wall ||
            targetTile.IsHighlighted == false ||
            targetTile.OccupiedCharacter != null ||
            IsMoving)
            return;

        OnMoveStarted?.Invoke();
        IsMoving = true;
        _character.CurrentTile = targetTile;

        Vector3 direction = (targetTile.transform.position - _character.transform.position).normalized;
        Vector3 lookAtTarget = _character.transform.position + direction;

        _character.transform.DOKill(true);
        _character.transform
            .DOLookAt(lookAtTarget, _durationOfRotate, AxisConstraint.Y)
            .OnComplete(() =>
            {
                Vector3 finalPos = new Vector3(
                    Mathf.Round(targetTile.transform.position.x),
                    _character.transform.position.y,
                    Mathf.Round(targetTile.transform.position.z)
                );

                _character.transform
                    .DOJump(finalPos, _jumpPower, _numJumps, _durationOfJump)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        IsMoving = false;
                        OnMoveFinished?.Invoke();
                    });
            });
    }
}
