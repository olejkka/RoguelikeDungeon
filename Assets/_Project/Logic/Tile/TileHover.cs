using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public class TileHover : MonoBehaviour
{
    [FormerlySerializedAs("meshRenderer")]
    [Header("References")]
    [Tooltip("MeshRenderer у вашего тайла — перетащите сюда компонент MeshRenderer дочернего объекта \"Mesh\"")]
    [SerializeField] private MeshRenderer _meshRenderer;

    [FormerlySerializedAs("hoverMaterial")]
    [Header("Materials")]
    [Tooltip("Материал, который будет применяться при наведении")]
    [SerializeField] private Material _hoverMaterial;

    [FormerlySerializedAs("hoverScaleFactor")]
    [Header("Hover Scale")]
    [Tooltip("Множитель масштаба меша при наведении (1 = без изменения)")]
    [SerializeField] private float _hoverScaleFactor = 1.1f;
    
    private Material originalMaterial;
    private Vector3 originalScale;

    private void Start()
    {
        if (_meshRenderer == null)
        {
            Debug.LogError($"[TileHover] MeshRenderer не найден у {gameObject.name}", this);
            return;
        }
        
        originalMaterial = _meshRenderer.sharedMaterial;
        originalScale    = _meshRenderer.transform.localScale;
    }

    private void OnMouseEnter()
    {
        if (_hoverMaterial != null)
            _meshRenderer.material = _hoverMaterial;
        
        _meshRenderer.transform.localScale = originalScale * _hoverScaleFactor;
    }

    private void OnMouseExit()
    {
        _meshRenderer.material = originalMaterial;
        _meshRenderer.transform.localScale = originalScale;
    }
}