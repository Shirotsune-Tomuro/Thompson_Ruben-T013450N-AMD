using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    private float m_currentHealth;
    private int m_Armour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(float maxHealth, int armour)
    {
        m_currentHealth = maxHealth;
        m_Armour = armour;
    }

    public void ChangeHealth(float value)
    {
        if (value < 0)
        {
            float damage = value * (1 - m_Armour);
            m_currentHealth -= damage;
        }
        else
            m_currentHealth += value;
    } 
}
