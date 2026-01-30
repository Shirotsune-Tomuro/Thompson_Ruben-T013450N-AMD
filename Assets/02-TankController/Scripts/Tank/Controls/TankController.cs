using System.Collections;
using System.Threading;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    private AM_02Tank m_ActionMap; //input
    private TankCharacter m_TankCharacter; //tank character

    private Vector2 m_camAngles;
    [SerializeField] private float m_minXAngleDeg = 10f;
    [SerializeField] private float m_maxXAngleDeg = 60f;
    [SerializeField] private Transform m_SpringArm;
    [SerializeField] private Transform m_Camera;

    private void OnEnable()
    {
        m_TankCharacter = gameObject.GetComponent<TankCharacter>();
        m_ActionMap = new AM_02Tank();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        m_ActionMap.Enable();

        m_ActionMap.Default.Accelerate.performed += Handle_AcceleratePerformed;
        m_ActionMap.Default.Accelerate.canceled += Handle_AccelerateCanceled;
        m_ActionMap.Default.Steer.performed += Handle_SteerPerformed;
        m_ActionMap.Default.Steer.canceled += Handle_SteerCanceled;
        m_ActionMap.Default.Fire.performed += Handle_FirePerformed;
        m_ActionMap.Default.Fire.canceled += Handle_FireCanceled;
        m_ActionMap.Default.Aim.performed += Handle_AimPerformed;
        m_ActionMap.Default.Zoom.performed += Handle_ZoomPerformed;
    }
    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.Accelerate.performed -= Handle_AcceleratePerformed;
        m_ActionMap.Default.Accelerate.canceled -= Handle_AccelerateCanceled;
        m_ActionMap.Default.Steer.performed -= Handle_SteerPerformed;
        m_ActionMap.Default.Steer.canceled -= Handle_SteerCanceled;
        m_ActionMap.Default.Fire.performed -= Handle_FirePerformed;
        m_ActionMap.Default.Fire.canceled -= Handle_FireCanceled;
        m_ActionMap.Default.Aim.performed -= Handle_AimPerformed;
        m_ActionMap.Default.Zoom.performed -= Handle_ZoomPerformed;
    }

    private void Handle_AcceleratePerformed(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IMove>().Accellerate(context.ReadValue<float>());
    }

    private void Handle_AccelerateCanceled(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IMove>().Stop();
    }

    private void Handle_SteerPerformed(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IMove>().Steer(context.ReadValue<float>());
    }

    private void Handle_SteerCanceled(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IMove>().Stop();
    }

    private void Handle_FirePerformed(InputAction.CallbackContext context)
    {
        // m_TankCharacter.GetComponent<IWeapon>().Fire();
    }

    private void Handle_FireCanceled(InputAction.CallbackContext context)
    {

    }

    private void Handle_AimPerformed(InputAction.CallbackContext context)
    {
        Vector2 deltaPos = context.ReadValue<Vector2>();

        // Invert Y axis
        deltaPos.y *= -1;

        // Rotate spring arm based on input
        m_camAngles.x = Mathf.Clamp(m_camAngles.x + deltaPos.y, m_minXAngleDeg, m_minXAngleDeg);
        m_camAngles.y = Mathf.Repeat(m_camAngles.y + deltaPos.x, 360f);

        m_SpringArm.rotation = Quaternion.Euler(m_camAngles);
        // m_TankCharacter.GetComponent<IWeapon>().Aim();
    }

    private void Handle_ZoomPerformed(InputAction.CallbackContext context)
    {
        // m_TankCharacter.GetComponent<IWeapon>().Zoom();
    }
}