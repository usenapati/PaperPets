using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SystemViewCameraControls : MonoBehaviour
{

    [SerializeField] Collider bounds;
    [SerializeField] Camera cam;
    [SerializeField] float initMoveSpeed;
    [SerializeField] float zoomSpeed;

    float UDVal;
    float LRVal;
    float ZoomVal;
    float camWidth;
    float camHeight;
    float maxZoom;
    float initZoom;
    float moveSpeed;
    Vector3 translation = new Vector3();
    Vector3 targetPos = new Vector3();
    float targetZoom;

    private void Awake()
    {
        camWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        camHeight = Camera.main.orthographicSize;
        maxZoom = (Screen.height * bounds.bounds.extents.x) / Screen.width;
        initZoom = Camera.main.orthographicSize;
        moveSpeed = initMoveSpeed;
        Debug.Log(camWidth + " " + camHeight);
        GameManager.Instance.setUIZoom(100f);
    }

    private void FixedUpdate()
    {

        translation.x = 0;
        translation.y = 0;
        translation.z = 0;

        if (UDVal > 0)
        {
            translation.z = moveSpeed;
        }
        else if (UDVal < 0)
        {
            translation.z = -moveSpeed;
        }

        if (LRVal > 0)
        {
            translation.x = moveSpeed;
        }
        else if (LRVal < 0)
        {
            translation.x = -moveSpeed;
        }

        targetPos = transform.position + translation;
        targetPos.x = Mathf.Clamp(targetPos.x, bounds.bounds.min.x + camWidth, bounds.bounds.max.x - camWidth);
        targetPos.z = Mathf.Clamp(targetPos.z, bounds.bounds.min.z + camHeight, bounds.bounds.max.z - camHeight);
        transform.position = targetPos;

    }

    private void Update()
    {
        if (ZoomVal > 0)
        {
            targetZoom = cam.orthographicSize - zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(targetZoom, 1, maxZoom);
            camWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            camHeight = Camera.main.orthographicSize;
            moveSpeed = initMoveSpeed * cam.orthographicSize / initZoom;
        }
        else if (ZoomVal < 0)
        {
            targetZoom = cam.orthographicSize + zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(targetZoom, 1, maxZoom);
            camWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            camHeight = Camera.main.orthographicSize;
            moveSpeed = initMoveSpeed * cam.orthographicSize / initZoom;
            
        }
        print(targetZoom);
        GameManager.Instance.setUIZoom(targetZoom);
    }

    public void OnUD(InputAction.CallbackContext context)
    {
        UDVal = context.ReadValue<float>();
    }

    public void OnLR(InputAction.CallbackContext context)
    {
        LRVal = context.ReadValue<float>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        ZoomVal = context.ReadValue<Vector2>().y;
    }

}
