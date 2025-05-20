using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Character))]
public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;
    
    public int MaxHealth => maxHealth;
    public int CurrentHealth => _currentHealth;
    
    public Action<int,int> HPChanged;
    

    
    private void Awake()
    {
        _currentHealth = maxHealth;
        HPChanged?.Invoke(_currentHealth, maxHealth);
    }
    
    public void TakeDamage(int damage)
    {
        if (damage <= 0 || _currentHealth <= 0)
            return;

        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        HPChanged?.Invoke(_currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || _currentHealth <= 0)
            return;

        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        HPChanged?.Invoke(_currentHealth, maxHealth);
    }
}