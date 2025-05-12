using UnityEngine;
using DG.Tweening;

public class CharacterAnimation : MonoBehaviour
{
    private Tween _breathingTween;

    
    public void PlayBreathing()
    {
        if (_breathingTween == null || _breathingTween.IsActive() == false)
        {
            _breathingTween = transform.DOScale(new Vector3(1.1f, 1.05f, 1.1f), 2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
    
    public void StopAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
    
    public void PlayMove(Vector3 targetPosition, float duration)
    {
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear);
    }
    
    public void PlayAttack()
    {
        transform.DOShakePosition(0.5f, strength: 0.2f);
    }
}