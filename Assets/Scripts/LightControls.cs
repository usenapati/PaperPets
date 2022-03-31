using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightControls : MonoBehaviour
{

    private float value;

    public void increaseRotation()
    {
        //transform.rotation = new Quaternion(transform.rotation.x + .1f * value, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.Rotate(new Vector3(.1f, 0, 0));
    }

    public void decreaseRotation()
    {
        //transform.rotation = new Quaternion(transform.rotation.x - .1f * value, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.Rotate(new Vector3(-.1f, 0, 0));
    }

    void Update()
    {
        if (value > 0)
        {
            //increaseRotation();
            transform.Rotate(new Vector3(.1f, 0, 0));
        }
        else if (value < 0)
        {
            //decreaseRotation();
            transform.Rotate(new Vector3(-.1f, 0, 0));
        }
    }

    public void OnPan(InputAction.CallbackContext context)
    {
        value = context.ReadValue<float>();
    }
}
