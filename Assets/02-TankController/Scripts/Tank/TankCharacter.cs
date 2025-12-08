using UnityEngine;

public class TankCharacter : MonoBehaviour
{
    MovementComponent m_MovementComponent;
    HealthComponent m_HealthComponent;
    [SerializeField] TankData m_TankData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        if (m_MovementComponent == null)        
            m_MovementComponent = GetComponent<MovementComponent>();
        m_MovementComponent.Init(m_TankData.Acceleration, m_TankData.MaxHealth);

        if (m_HealthComponent == null)
            m_HealthComponent = GetComponent<HealthComponent>();
        m_HealthComponent.Init(m_TankData.MaxHealth, m_TankData.Armour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
