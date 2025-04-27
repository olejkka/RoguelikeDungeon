using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Character))]
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHP = 100;
    private int _currentHP;
    
    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;
    
    public Action<int,int> OnHPChanged;
    public Action OnDead;

    private void Awake()
    {
        _currentHP = _maxHP;
        OnHPChanged?.Invoke(_currentHP, _maxHP);
    }
    
    public void TakeDamage(int damage)
    {
        if (damage <= 0 || _currentHP <= 0) return;

        _currentHP = Mathf.Max(_currentHP - damage, 0);
        OnHPChanged?.Invoke(_currentHP, _maxHP);

        if (_currentHP == 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || _currentHP <= 0) return;

        _currentHP = Mathf.Min(_currentHP + amount, _maxHP);
        OnHPChanged?.Invoke(_currentHP, _maxHP);
    }

    private void Die()
    {
        OnDead?.Invoke();
    }
}