using System.Collections;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    private AM_02Tank m_ActionMap; //input
    private TankCharacter m_TankCharacter; //tank character

    [SerializeField] private Transform m_SpringArm;
    [SerializeField] private Transform m_Camera;
    Plane m_Plane = new Plane(Vector3.up, Vector3.zero);

    private void OnEnable()
    {
        m_TankCharacter = gameObject.GetComponent<TankCharacter>();
        m_ActionMap = new AM_02Tank();

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
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector3 newlookPos = new Vector3();
        Vector3 currenLookPos = new Vector3();

        //current look position
        Ray oldLookRay = new Ray(transform.position, transform.forward);
        if (m_Plane.Raycast(oldLookRay, out float oldDistance))
            currenLookPos = oldLookRay.GetPoint(oldDistance);

        //new look position from screen point
        Ray newLookRay = Camera.main.ScreenPointToRay(screenPos);
        if (m_Plane.Raycast(newLookRay, out float distance))
            newlookPos = newLookRay.GetPoint(distance);

        // directions from the current look to the new look direction
        Vector3 oldLookDir = currenLookPos - m_SpringArm.position;
        Vector3 newLookDir = newlookPos - m_SpringArm.position;

        //angle between the two directions
        float deltaYaw = Vector3.SignedAngle(oldLookDir, newLookDir, Vector3.up);

        //rotate the spring arm by that angle
        Vector3 euler = m_SpringArm.eulerAngles;
        euler.y += deltaYaw;
        m_SpringArm.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);

        // m_TankCharacter.GetComponent<IWeapon>().Aim();
    }

    private void Handle_ZoomPerformed(InputAction.CallbackContext context)
    {
        // m_TankCharacter.GetComponent<IWeapon>().Zoom();
    }
}