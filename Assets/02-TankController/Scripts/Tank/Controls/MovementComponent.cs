using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour, IMove
{
    DriveSystem m_LDrive;
    DriveSystem m_RDrive;
    DriveSystem[] m_DriveSystems;

    float m_Acceleration;

    public void Init(float acceleration, float susK, float susC, float length, float wRadius)
    {
        m_Acceleration = acceleration;

        foreach (var driveSystem in GetComponentsInChildren<DriveSystem>())
        {
            if (driveSystem.m_IsLeft)
                m_LDrive = driveSystem;
            else
                m_RDrive = driveSystem;
        }

        m_DriveSystems = new DriveSystem[] { m_LDrive, m_RDrive };

        foreach (var driveSystem in m_DriveSystems)
        {
            driveSystem.Init(susK, susC, length, wRadius);
        }
    }

    public void Accellerate(float value)
    {
        if (value < 0)
            value *= 0.65f;

        foreach (var driveSystem in m_DriveSystems)
        {
            driveSystem.Accellerate(value * m_Acceleration);
        }
    }

    public void Steer(float value)
    {
        float turnStrength = 1.45f;

        m_LDrive.Accellerate(value * (m_Acceleration * turnStrength));
        m_RDrive.Accellerate(-value * (m_Acceleration * turnStrength));
    }

    public void Stop()
    {
        foreach (var driveSystem in m_DriveSystems)
        {
            driveSystem.Stop();
        }
    }
}
