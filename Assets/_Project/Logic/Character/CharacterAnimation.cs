using UnityEngine;
using DG.Tweening;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Jump Animation Settings")]
    [SerializeField] private float _jumpPower = 0.5f;
    [SerializeField] private int _numJumps = 1;
    [SerializeField] private float _durationOfJump = 0.5f;
    [SerializeField] private float _durationOfRotate = 0.2f;
    
    private Tween _idleTween;
    private Tween _rotateTween;

    
    public void PlayIdle()
    {
        if (_idleTween == null || _idleTween.IsActive() == false)
        {
            _idleTween = transform.DOScale(new Vector3(1.1f, 1.05f, 1.1f), 2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
    
    public void PlayMove(Vector3 targetPosition, TweenCallback onComplete)
    {
        transform.DOJump(targetPosition, _jumpPower, _numJumps, _durationOfJump)
            .SetEase(Ease.Linear)
            .OnComplete(onComplete);
    }
    
    public Tween PlayRotate(Vector3 targetPosition, TweenCallback onComplete = null)
    {
        Vector3 direction    = (targetPosition - transform.position).normalized;
        Vector3 lookAtTarget = transform.position + direction;

        _rotateTween?.Kill();
        _rotateTween = transform
            .DOLookAt(lookAtTarget, _durationOfRotate, AxisConstraint.Y)
            .SetEase(Ease.InOutSine);

        if (onComplete != null)
            _rotateTween.OnComplete(onComplete);

        return _rotateTween;
    }
    
    public void PlayAttack()
    {
        Sequence attackSequence = DOTween.Sequence();
        attackSequence
            .Append(transform.DOLocalRotate(new Vector3(15f, 0f, 0f), _durationOfRotate).
                SetEase(Ease.OutQuad))
            .Append(transform.DOLocalRotate(Vector3.zero, _durationOfRotate).
                SetEase(Ease.InQuad));
    }
    
    public void StopAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
}