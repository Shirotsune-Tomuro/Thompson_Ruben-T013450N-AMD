using System.Collections;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    // suspension variables
    private Rigidbody m_Rigidbody;
    private Transform m_Wheel;
    private Transform m_SpringLoc;
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
        m_SpringLoc = transform.GetChild(1);
        m_RestPos = m_Wheel.transform.localPosition;

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
            Debug.DrawRay(m_SpringLoc.transform.position, -transform.up * m_SpringLength, m_IsGrounded ? Color.green : Color.red);

            yield return new WaitForFixedUpdate();
        }
    }

    private void SuspensionPhysics()
    {
        // hookes law F = -kx
        // hookes law with damping F = -kx - cv
        //F is the total force applied by the spring
        //k is the spring constant (stiffness)
        //x is the displacement from the rest position
        //c is the damping coefficient
        //v is the velocity of the mass relative to the spring

        //readability
        Vector3 springWorldPos = m_SpringLoc.transform.position;
        Vector3 springDown = -m_SpringLoc.transform.up;
        Vector3 springUp = m_SpringLoc.transform.up;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(springWorldPos, springDown, out hit, m_SpringLength);

        //if hit = false then dist = springlength 
        //else dist = hit.distance
        float dist = (!hasHit) ? (m_SpringLength) : (hit.distance);

        //calculate how much the spring is compressed
        float SpringLenPercent = Mathf.Clamp01(dist / m_SpringLength);
        float compressPercent = 1.0f - SpringLenPercent;

        //move the wheel down with the spring
        Vector3 wheelWorldPos = m_Wheel.position;
        wheelWorldPos = springWorldPos + (springDown * dist) + (springUp * m_WheelRadius);
        m_Wheel.position = wheelWorldPos;

        //if the spring is compressed, calculate suspension force
        if (compressPercent > 0.0f)
        {
            float displacement = m_SpringLength - dist;
            float susVelocity = Vector3.Dot(m_Rigidbody.GetPointVelocity(m_Wheel.position), springUp);

            float x = displacement;
            float k = m_Stiffness;
            float c = m_Damping;

            float force = (k * x) - (c * susVelocity);
            Vector3 susForce = springUp * force;

            Debug.Log("Suspension Force: " + force);

            m_Rigidbody.AddForceAtPosition(susForce, m_Wheel.position, ForceMode.Acceleration);
        }
    }

    private void FixedUpdate()
    {
        SuspensionPhysics();
    }
}
