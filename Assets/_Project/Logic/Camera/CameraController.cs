using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Distance and Height")]
    [SerializeField] private float _distance = -10f;
    [Tooltip("Высота камеры над целью по Y")]
    [SerializeField] private float _height = 5f;

    [Header("Angles")]
    [Tooltip("Угол наклона камеры вниз по X")]
    [SerializeField, Range(0f, 90f)] private float _pitchAngle = 45f;

    [Header("Input")]
    [Tooltip("Скорость вращения при перетаскивании мыши")]
    [SerializeField] private float _rotationSpeed = 3f;
    
    private Transform _target;
    private float _yawAngle;

    
    private void Update()
    {
        if (_target == null)
            return;
        
        if (Input.GetMouseButton(1))
        {
            float delta = Input.GetAxis("Mouse X") * _rotationSpeed;
            _yawAngle += delta;
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;
        
        UpdateCamera();
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
        
        if (_target != null)
        {
            InitializeYaw();
            UpdateCamera();
        }
    }

    private void InitializeYaw()
    {
        Vector3 dir = transform.position - (_target.position + Vector3.up * _height);
        dir.y = 0f;
        
        if (dir.sqrMagnitude > 0.001f)
            _yawAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
        else
            _yawAngle = 0f;
    }

    private void UpdateCamera()
    {
        Quaternion yawRot = Quaternion.Euler(0f, _yawAngle, 0f);
        Vector3 localOffset = new Vector3(0f, 0f, _distance);
        Vector3 worldOffset = yawRot * localOffset + Vector3.up * _height;
        transform.position = _target.position + worldOffset;
        transform.rotation = Quaternion.Euler(_pitchAngle, _yawAngle, 0f);
    }
}
