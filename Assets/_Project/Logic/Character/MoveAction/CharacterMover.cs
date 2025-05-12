using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework;

[RequireComponent(typeof(Character))]
public class CharacterMover : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _numJumps = 2;
    [SerializeField] private float _durationOfJump = 1f;
    [SerializeField] private float _durationOfRotate = 0.2f;

    private Character _character;
    public event Action MovementStarting;
    public event Action MovementFinished;
    private bool IsMoving { get; set; }

    private void Awake()
    {
        _character = GetComponent<Character>(); 
    }
    
    public void MoveTo(Tile targetTile)
    {
        if (IsMoving == false)
        {
            if (targetTile == 
                _character.CurrentTile ||
                targetTile.Type == TileType.Wall ||
                targetTile.IsHighlighted == false ||
                targetTile.OccupiedCharacter != null ||
                IsMoving)
            {
                Debug.Log("(CharacterMover) Movement is not possible");
                return;
            }

            MovementStarting?.Invoke();
            IsMoving = true;
            _character.CurrentTile = targetTile;

            RotateTowards(targetTile.transform.position, () =>
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
                        MovementFinished?.Invoke();
                    });
            });
        }
    }
    
    public void RotateTowards(Vector3 targetPosition, Action onComplete = null)
    {
        Vector3 direction = (targetPosition - _character.transform.position).normalized;
        Vector3 lookAtTarget = _character.transform.position + direction;

        _character.transform.DOKill(true);
        _character.transform
            .DOLookAt(lookAtTarget, _durationOfRotate, AxisConstraint.Y)
            .OnComplete(() => onComplete?.Invoke());
    }
}
