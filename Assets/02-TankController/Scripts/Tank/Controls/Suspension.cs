using System.Collections;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public bool m_IsGrounded = false;
    Coroutine c_GroundCheck;

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


            yield return new WaitForFixedUpdate();
        }
    }

}
