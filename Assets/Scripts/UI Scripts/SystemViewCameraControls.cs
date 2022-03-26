using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SystemViewCameraControls : MonoBehaviour
{

    [SerializeField] Collider bounds;

    float UDVal;
    float LRVal;
    float ZoomVal;
    Vector3 translation = new Vector3();
    Vector3 targetPos = new Vector3();

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {

        translation.x = 0;
        translation.y = 0;
        translation.z = 0;

        if (UDVal > 0)
        {
            translation.z = 1;
        }
        else if (UDVal < 0)
        {
            translation.z = -1; ;
        }

        if (LRVal > 0)
        {
            translation.x = 1;
        }
        else if (LRVal < 0)
        {
            translation.x = -1;
        }

        if (ZoomVal > 0)
        {

        }
        else if (ZoomVal < 0)
        {

        }

        targetPos = transform.position + translation;
        targetPos.x = Mathf.Clamp(targetPos.x, bounds.bounds.min.x, bounds.bounds.max.x);
        targetPos.z = Mathf.Clamp(targetPos.z, bounds.bounds.min.z, bounds.bounds.max.z);
        transform.position = targetPos;

    }

    public void OnUD(InputAction.CallbackContext context)
    {
        UDVal = context.ReadValue<float>();
    }

    public void OnLR(InputAction.CallbackContext context)
    {
        LRVal = context.ReadValue<float>();
    }

}
