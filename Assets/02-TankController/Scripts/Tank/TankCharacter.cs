using UnityEngine;

public class TankCharacter : MonoBehaviour
{
    HealthComponent m_HealthComponent;
    [SerializeField] TankData m_TankData;
    MovementComponent m_MovementComponent;
    Turret m_Turret;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        if (m_HealthComponent == null)
            m_HealthComponent = GetComponent<HealthComponent>();
        m_HealthComponent.Init(m_TankData.MaxHealth, m_TankData.Armour);

        if (m_MovementComponent == null)
            m_MovementComponent = GetComponent<MovementComponent>();
        m_MovementComponent.Init(m_TankData.Acceleration, m_TankData.SuspnsionStiffeness, m_TankData.SuspensionDamping, 
            m_TankData.SpringLength, m_TankData.WheelRadius);

        if (m_Turret == null)
            m_Turret = GetComponentInChildren<Turret>();
        m_Turret.Init(m_TankData.TurretRotationSpeed);
    }
}
