using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]private Slider _healthSlider;
    public Gradient gradient;
    public Image fill;
   
    public void SetMaxHealth(int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        SetHealth(maxHealth);
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        _healthSlider.value = health;
        fill.color = gradient.Evaluate(_healthSlider.normalizedValue);
    }
}
