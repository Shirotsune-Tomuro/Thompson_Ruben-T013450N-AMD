using UnityEngine;

public class DriveSystem : MonoBehaviour
{
    [SerializeField] public bool m_IsLeft;
    DriveWheel m_DriveWheel;
    Suspension[] m_Suspension;


    public void Init(float susK, float susC)
    {
        m_DriveWheel = GetComponentInChildren<DriveWheel>();
        m_Suspension = GetComponentsInChildren<Suspension>();

        m_DriveWheel.Init();

        foreach (var suspension in m_Suspension)
        {
            suspension.Init(susK, susC);
        }
    }
    public void Accellerate(float Acceleration)
    {
        int contacts = 0;

        foreach (var suspension in m_Suspension)
        {
            suspension.Move(true);
            if (suspension.m_IsGrounded)
                contacts++;
        }

        m_DriveWheel.Move(Acceleration, contacts);
    }

    public void Stop()
    {
        foreach (var suspension in m_Suspension)
        {
            suspension.Move(false);
        }

        m_DriveWheel.StopMove();
    }
}
