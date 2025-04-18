using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    private Player _player;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void Move(Tile targetTile)
    {
        Vector3 direction = (targetTile.transform.position - _player.transform.position).normalized;
        Vector3 lookAtTarget = _player.transform.position + direction;
        
        _player.transform.DOLookAt(lookAtTarget, 1, AxisConstraint.Y).OnComplete(() =>
        {
            Vector3 finalPos = new Vector3(
                Mathf.Round(targetTile.transform.position.x),
                _player.transform.position.y,
                Mathf.Round(targetTile.transform.position.z)
            );

            _player.transform.DOJump(finalPos, 1, 1, 1)
                .SetEase(Ease.Linear);
        });
    }
}