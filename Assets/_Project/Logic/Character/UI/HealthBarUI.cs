using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterHealth))]
public class HealthBarUI : MonoBehaviour, IDeathListener
{
    [Header("UI Elements")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillImage;

    [FormerlySerializedAs("_health")]
    [Header("Settings")]
    [SerializeField] private CharacterHealth characterHealth;
    [SerializeField] private float _fadeDuration = 0.5f;

    private Color _bgOriginalColor;
    private Color _fillOriginalColor;
    private int _lastHealth;

    private void Awake()
    {
        if (characterHealth == null)
            characterHealth = GetComponent<CharacterHealth>();
        
        _bgOriginalColor = _backgroundImage.color;
        _fillOriginalColor = _fillImage.color;
        
        var hiddenBg = _bgOriginalColor;
        hiddenBg.a = 0f;
        _backgroundImage.color = hiddenBg;

        var hiddenFill = _fillOriginalColor;
        hiddenFill.a = 0f;
        _fillImage.color = hiddenFill;
        
        _lastHealth = characterHealth.CurrentHealth;
        _fillImage.fillAmount = Mathf.Clamp01((float)_lastHealth / characterHealth.MaxHealth);
        
        characterHealth.HPChanged += OnHPChanged;
    }

    private void OnDestroy()
    {
        if (characterHealth != null)
        {
            characterHealth.HPChanged -= OnHPChanged;
        }
    }

    private void OnHPChanged(int current, int max)
    {
        float targetFill = Mathf.Clamp01((float)current / max);
        _fillImage.DOFillAmount(targetFill, _fadeDuration);

        if (current < _lastHealth)
        {
            ShowBar();
        }

        _lastHealth = current;
    }

    private void ShowBar()
    {
        _backgroundImage.DOFade(_bgOriginalColor.a, _fadeDuration);
        _fillImage.DOFade(_fillOriginalColor.a, _fadeDuration);
    }

    private void HideBar()
    {
        _backgroundImage.DOFade(0f, _fadeDuration);
        _fillImage.DOFade(0f, _fadeDuration);
    }

    public void OnCharacterDeath(Character character)
    {
        HideBar();
    }
}
