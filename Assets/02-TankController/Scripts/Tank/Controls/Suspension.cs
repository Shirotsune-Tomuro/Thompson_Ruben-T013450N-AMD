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
        Vector3 pos = m_SpringLoc.transform.position;
        Vector3 dir = -m_SpringLoc.transform.up;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(pos, dir, out hit, m_SpringLength);

        //if hit = false then dist = springlength 
        //else dist = hit.distance
        float dist = (!hasHit) ? (m_SpringLength) : (hit.distance);
        Vector3 poss = m_Wheel.position;
        poss.y = pos.y + dir.y * dist;
        m_Wheel.position = poss;

        float SpringLenPercent = Mathf.Clamp01(dist / m_SpringLength);
        float compressPercent = 1 - SpringLenPercent;
        poss = (transform.position - transform.up) * SpringLenPercent;
        poss.y -= m_WheelRadius;
        m_Wheel.position = poss;

        //if the spring is compressed, calculate suspension force
        if (compressPercent > 0.0f)
        {
            float displacement = Vector3.Dot((pos - m_RestPos), dir);
            float susVelocity = Vector3.Dot(m_Rigidbody.GetPointVelocity(m_Wheel.position), dir);

            float x = displacement;
            float k = m_Stiffness;
            float c = m_Damping;

            float force = (-k * x) - (c * susVelocity);
            Vector3 susForce = dir * force;

            Debug.Log("Suspension Force: " + force);

            m_Rigidbody.AddForceAtPosition(-susForce, m_Wheel.position, ForceMode.Acceleration);
        }

        //Vector3 localPos = transform.localPosition;
        //localPos.Scale(Vector3.right);
        //transform.localPosition = localPos;
    }

    private void FixedUpdate()
    {
        if (m_IsGrounded)
            SuspensionPhysics();
    }
}
