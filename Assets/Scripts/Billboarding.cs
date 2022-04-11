using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private bool isStaticBillboard;
    [SerializeField] SpriteRenderer sr;

    Vector3 currentPos;
    Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        prevPos = transform.position;
        prevPos.y = 0;
        prevPos.Normalize();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        BillBoard();
    }

    void BillBoard()
    {
        if (!isStaticBillboard)
        {
            transform.LookAt(mainCamera.transform);
        }
        else
        {
            transform.rotation = mainCamera.transform.rotation;
        }
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);


        currentPos = gameObject.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        currentPos.y = 0;
        currentPos.Normalize();
        cameraForward.y = 0;
        cameraForward.Normalize();
        //Debug.Log("camera before rotation: " + cameraForward);

        float angleToX = Vector3.Angle(cameraForward, Vector3.right);
        if (cameraForward.z < 0) angleToX = -angleToX;
        //Vector3 camSnapToX = Vector3.RotateTowards(cameraForward, Vector3.right, Mathf.PI, 1);
        Vector3 transSnapToX = Quaternion.AngleAxis(angleToX, Vector3.up) * (currentPos - prevPos).normalized;
        //Debug.Log("camera: " + camSnapToX);
        transSnapToX.Normalize();
       // Debug.Log("trans: " + transSnapToX);

        /*if ((angle > 0 && angle <= 90) || (angle > 270 && angle < 360)) sr.flipX = true;
        else sr.flipX = false;*/
        if (transSnapToX.z < 0) sr.flipX = true;
        //else if (transSnapToX.z > 0 && transSnapToX.x < 0) sr.flipX = true;
        else sr.flipX = false;
        prevPos = currentPos;
    }
}
