using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _numJumps = 2;
    [SerializeField] private float _durationOfJump = 1f;
    [SerializeField] private float _durationOfRotate = 0.2f;
    private Character _character;
    public event Action OnMoveStarted;
    public event Action OnMoveFinished;
    public bool IsMoving { get; set; }
    
    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Move(Tile targetTile)
    {
        if (targetTile == _character.CurrentTile)
            return;
        
        if (targetTile.Type == TileType.Wall)
            return;
        
        if(targetTile.IsHighlighted == false)
            return;
        
        
        if (Attack.TryMeleeAttack(_character, targetTile, attackDamage))
        {
            OnMoveFinished?.Invoke();
            return;
        }
        
        if (IsMoving == false)
        {
            OnMoveStarted?.Invoke();
            
            Vector3 direction = (targetTile.transform.position - _character.transform.position).normalized;
            Vector3 lookAtTarget = _character.transform.position + direction;
            
            IsMoving = true;
            _character.CurrentTile = targetTile;
            
            _character.transform.DOKill(true);
            _character.transform.DOLookAt(lookAtTarget, _durationOfRotate, AxisConstraint.Y).OnComplete(() =>
            {
                Vector3 finalPos = new Vector3(
                    Mathf.Round(targetTile.transform.position.x),
                    _character.transform.position.y,
                    Mathf.Round(targetTile.transform.position.z)
                );

                _character.transform.DOJump(finalPos, _jumpPower, _numJumps, _durationOfJump)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        IsMoving = false;
                        OnMoveFinished?.Invoke();
                    });
            });
        }
    }
}

