using UnityEngine;
using UnityEngine.UI;

public class HealthManagementSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] HealthBar healthBar;
    [SerializeField] ScoreUI scoreUI;

    private void Start()
    {
        _currentHealth = _maxHealth;
        if(healthBar != null )
        {
          healthBar.SetMaxHealth(_maxHealth);
        }
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
            scoreUI.AddScore(1);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (healthBar!= null)
        {
          healthBar.SetHealth(_currentHealth);  
        }
    }
}
