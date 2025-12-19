using System.Collections;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    // suspension variables
    private Rigidbody m_Rigidbody;
    private Transform m_Wheel;
    private Vector3 m_RestPos;

    private float m_Stiffness = 1.0f;
    private float m_Damping = 0.5f;
    private float m_SpringLength = 0.5f;
    private float m_WheelRadius;

    // move variables
    public bool m_IsGrounded = false;
    Coroutine c_GroundCheck;

    public void Init(float k, float c, float length, float wRadius)
    {
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_Wheel = transform.GetChild(0);
        m_RestPos = transform.localPosition;

        m_SpringLength = length;
        m_Stiffness = k;
        m_Damping = c;
        m_WheelRadius = wRadius;
    }

    public void Move(bool shouldMove)
    {
        if (c_GroundCheck == null && shouldMove)
            c_GroundCheck = StartCoroutine(GroundCheck());
        else if (c_GroundCheck != null && !shouldMove)
        {
            StopCoroutine(c_GroundCheck);
            c_GroundCheck = null;
        }
    }

    private IEnumerator GroundCheck()
    {
        while (true)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.5f))
                m_IsGrounded = true;
            else
                m_IsGrounded = false;
            Debug.DrawRay(transform.position, -transform.up * 0.5f, m_IsGrounded ? Color.green : Color.red);


            yield return new WaitForFixedUpdate();
        }
    }

    private void SuspensionPhysics()
    {
        // hookes law F = -kx
        // hookes law with damping F = -kx - cv

        Debug.DrawLine(transform.position, m_RestPos);

        Vector3 pos = transform.position;
        Vector3 dir = -transform.up;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(pos, dir, out hit, m_SpringLength);

        float dist = (!hasHit) ? (m_SpringLength) : (hit.distance);
        m_Wheel.position = pos + dir * dist;

        float SpringLenPercent = Mathf.Clamp01(1.0f - (dist / m_SpringLength));
        float compressPercent = 1 - SpringLenPercent;

        Debug.Log("Compress Percent: " + compressPercent);

        float wheelPosOnSpring = m_SpringLength * (1 - compressPercent) * -m_WheelRadius;
        Vector3 newWheelPos = m_Wheel.position - m_Wheel.transform.up * wheelPosOnSpring;
        m_Wheel.localPosition = m_Wheel.InverseTransformPoint(newWheelPos);

        if (compressPercent > 0)
        {
            float displacement = compressPercent * m_SpringLength;
            float susVelocity = Vector3.Dot(-m_Wheel.transform.up, m_Rigidbody.GetPointVelocity(m_Wheel.position));

            float x = displacement;
            float k = m_Stiffness;
            float c = m_Damping;

            float force = (-k * x) - (c * susVelocity);
            Vector3 susForce = -m_Wheel.transform.up * force;
            m_Rigidbody.AddForceAtPosition(susForce, m_Wheel.position, ForceMode.Acceleration);
        }

        Vector3 localPos = transform.localPosition;
        localPos.Scale(Vector3.right);
        transform.localPosition = localPos;
    }

    private void FixedUpdate()
    {
        if (m_IsGrounded)
            SuspensionPhysics();
    }


}
