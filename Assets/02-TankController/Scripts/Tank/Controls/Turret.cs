using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform m_Camera;
    float m_RotationSpeed;

    public void Init(float rotSpeed)
    {
        m_RotationSpeed = rotSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 cameraProjection = Vector3.ProjectOnPlane(m_Camera.forward, Vector3.up).normalized;
        Quaternion newRot = Quaternion.LookRotation(cameraProjection, Vector3.up);

        var step = m_RotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, step);
    }
}
