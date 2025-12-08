using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    private AM_02Tank m_ActionMap; //input
    private TankCharacter m_TankCharacter; //tank character

    public void Init(TankCharacter tank)
    {
        if (m_ActionMap == null)
            m_ActionMap = new AM_02Tank();

        if (m_TankCharacter == null)
            m_TankCharacter = tank;
    }

    private void OnEnable()
    {
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
        m_TankCharacter.GetComponent<IMove>().Steer(0f);
    }

    private void Handle_FirePerformed(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IWeapon>().Fire();
    }

    private void Handle_FireCanceled(InputAction.CallbackContext context)
    {

    }

    private void Handle_AimPerformed(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IWeapon>().Aim();
    }

    private void Handle_ZoomPerformed(InputAction.CallbackContext context)
    {
        m_TankCharacter.GetComponent<IWeapon>().Zoom();
    }
}