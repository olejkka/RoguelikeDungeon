using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float fixedXAngle = 45f;

    private Transform _target;
    private Vector3 _offsetFromTarget;

    public void SetTarget(Transform target)
    {
        _target = target;
        if (_target != null)
        {
            // сохраняем начальное смещение от цели
            _offsetFromTarget = cameraTransform.position - _target.position;
        }
    }

    void Update()
    {
        if (_target == null || !Input.GetMouseButton(1)) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        // поворот смещения вокруг Y
        Quaternion rotation = Quaternion.Euler(0f, mouseX, 0f);
        _offsetFromTarget = rotation * _offsetFromTarget;

        // позиция = цель + повернутое смещение
        cameraTransform.position = _target.position + _offsetFromTarget;

        // всегда смотрим на цель
        cameraTransform.rotation = Quaternion.Euler(fixedXAngle, cameraTransform.rotation.eulerAngles.y, 0f);
        cameraTransform.LookAt(_target);
    }
}