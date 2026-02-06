using System.Collections;
using UnityEngine;

public class DriveWheel : MonoBehaviour
{
    Coroutine c_Move;
    Rigidbody m_rb;

    float m_Accel;
    int m_Contacts;


    public void Init()
    {
        m_rb = GetComponentInParent<Rigidbody>();
    }

    public void Move(float acceleration, int contacts)
    {
        m_Accel = acceleration;
        m_Contacts = contacts;

        if (c_Move == null)
            c_Move = StartCoroutine(Accelerate());

    }

    public void StopMove()
    {
        m_Accel = 0;

        if (c_Move != null)
        {
            StopCoroutine(c_Move);
            c_Move = null;
        }
    }

    public IEnumerator Accelerate()
    {
        while (true)
        {
            Debug.Log($"Tank: {m_rb} Accel: {m_Accel * (3 / m_Contacts)}");

            m_rb.AddForceAtPosition(transform.forward * (m_Accel * (3 / m_Contacts)), transform.position, ForceMode.Acceleration);

            yield return new WaitForFixedUpdate();
        }
    }
}
