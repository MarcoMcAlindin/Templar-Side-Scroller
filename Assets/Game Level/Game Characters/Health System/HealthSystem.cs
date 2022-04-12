using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HealthSystem 
{
    [SerializeField] private int _currentHealth;
    public int GetCurrentHealth() { return _currentHealth; }
    private int _maxHealth;


    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public HealthSystem(int maxHealth, int currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    public void TakeDamage(int amount)
    { if ((_currentHealth - amount) < 0) { _currentHealth = 0; } else { _currentHealth -= amount; } }

    public void AddHealthPoints(int amount) { if ((_currentHealth + amount < _maxHealth )) { _currentHealth = _maxHealth; } }

    public bool HasNoHealthLeft() { if (_currentHealth == 0) return true; else return false; }

    public void AddOneHealth()
    {
        AddHealthPoints(1);
    }

    public void TakeOneDamage()
    {
        TakeDamage(1);
    }
}
