using System.Collections;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    // suspension variables
    private Rigidbody m_Rigidbody;
    private Vector3 m_RestPos;

    private float m_Stiffness = 1.0f;
    private float m_Damping = 0.5f;

    // move variables
    public bool m_IsGrounded = false;
    Coroutine c_GroundCheck;

    public void Init(float k, float c)
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_RestPos = transform.localPosition;

        m_Stiffness = k;
        m_Damping = c;
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

    private void FixedUpdate()
    {
        if (m_IsGrounded)
        {
            // hookes law F = -kx
            // hookes law with damping F = -kx - bv

            Debug.DrawLine(transform.position, m_RestPos);

            Vector3 displacementVec = transform.position - m_RestPos;
            float displacementRatio = Vector3.Dot(displacementVec, -transform.up);

            float compressionVel = Vector3.Dot(m_Rigidbody.linearVelocity, -transform.up);

            float forceMag = (-m_Stiffness * displacementRatio) - (m_Damping * compressionVel);
            Vector3 appliedForce = -transform.up * forceMag;
            m_Rigidbody.AddForceAtPosition(appliedForce, transform.position, ForceMode.Acceleration);

            Vector3 localPos = transform.localPosition;
            localPos.Scale(Vector3.right);
            transform.localPosition = localPos;
        }
    }

}
