using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Controls")]
    [SerializeField] 
    public GameObject mainCamera;
    [SerializeField] 
    public GameObject dollyCart;
    [SerializeField]
    public GameObject dollyTrack;

    [SerializeField]
    private float cameraSpeed = 1f;
    [SerializeField]
    private float tiltSpeed = 40f;
    [SerializeField]
    private float zoomSpeed = 20f;

    // Horizontal
    [SerializeField, Range(0f, 1f)]
    private float panValue = 0.5f;

    private float pan1D;

    private float tempPan = 0.5f;

    // Vertical
    [SerializeField, Range(0f, 16f)]
    private float tiltValue;

    private float tilt1D;

    private float tempTilt = 2f;

    [SerializeField, Range(10f, 60f)]
    private float zoomValue;

    private float zoom1D;

    private float tempZoom = 20f;


    [Header("Targets")]
    // Alternate Cameras
    private Transform[] targets;
    [SerializeField]
    private int targetIndex;

    private void Awake()
    {
        mainCamera = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
        dollyCart = FindObjectOfType<CinemachineDollyCart>().gameObject;
        dollyTrack = FindObjectOfType<CinemachineSmoothPath>().gameObject;
    }

    private void FixedUpdate()
    {
        HandleCamera();
    }

    void HandleCamera()
    {
        // Pan
        HandlePan();
        dollyCart.GetComponent<CinemachineDollyCart>().m_Position = panValue;

        // Tilt
        HandleTilt();
        dollyTrack.transform.position = new Vector3(dollyTrack.transform.position.x, tiltValue, dollyTrack.transform.position.z);

        // Zoom
        HandleZoom();
        mainCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = zoomValue;
    }

    #region Pan Methods
    void HandlePan()
    {
        //Debug.Log("Pan1D: " + pan1D);
        //Debug.Log("PanValue: " + panValue);

        if (pan1D > 0)
        {
            //Debug.Log("Incrementing Pan");
            IncrementPan();
        }
        else if (pan1D < 0)
        {
            //Debug.Log("Decrementing Pan");
            DecrementPan();
        }
        //Debug.Log("tempPan: " + tempPan);

    }

    void IncrementPan()
    {
        tempPan += pan1D * Time.deltaTime * cameraSpeed;
        panValue = Mathf.Clamp(tempPan, 0, 1);
        tempPan = Mathf.Clamp(tempPan, -1, 1);
    }
    
    void DecrementPan()
    {
        tempPan += pan1D * Time.deltaTime * cameraSpeed;
        panValue = Mathf.Clamp(tempPan, 0, 1);
        tempPan = Mathf.Clamp(tempPan, -1, 1);
    }
    #endregion

    #region Tilt Methods
    void HandleTilt()
    {
        if (tilt1D > 0)
        {
            //Debug.Log("Incrementing Tilt");
            IncrementTilt();
        }
        else if (tilt1D < 0)
        {
            //Debug.Log("Decrementing Tilt");
            DecrementTilt();
        }
    }

    void IncrementTilt()
    {
        tempTilt += tilt1D * Time.deltaTime * tiltSpeed * cameraSpeed;
        tiltValue = Mathf.Clamp(tempTilt, 0, 16);
        tempTilt = Mathf.Clamp(tempTilt, -16, 16);
    }

    void DecrementTilt()
    {
        tempTilt += tilt1D * Time.deltaTime * tiltSpeed * cameraSpeed;
        tiltValue = Mathf.Clamp(tempTilt, 0, 16);
        tempTilt = Mathf.Clamp(tempTilt, -16, 16);
    }
    #endregion

    #region Zoom Methods
    void HandleZoom()
    {
        if (zoom1D > 0)
        {
            //Debug.Log("Incrementing Zoom");
            IncrementZoom();
        }
        else if (zoom1D < 0)
        {
            //Debug.Log("Decrementing Zoom");
            DecrementZoom();
        }
    }

    void IncrementZoom()
    {
        tempZoom += zoom1D * Time.deltaTime * zoomSpeed * cameraSpeed;
        zoomValue = Mathf.Clamp(tempZoom, 10, 60);
        tempZoom = Mathf.Clamp(tempZoom, -60, 60);
    }

    void DecrementZoom()
    {
        tempZoom += zoom1D * Time.deltaTime * zoomSpeed * cameraSpeed;
        zoomValue = Mathf.Clamp(tempZoom, 10, 60);
        tempZoom = Mathf.Clamp(tempZoom, -60, 60);
    }
    #endregion

    #region Input Methods
    public void OnPan(InputAction.CallbackContext context)
    {
        pan1D = context.ReadValue<float>();
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tilt1D = context.ReadValue<float>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom1D = context.ReadValue<float>();
        //Debug.Log(zoom1D);
    }

    public void ChangeTarget(InputAction.CallbackContext context)
    {
        // Not Set Up
    }
    #endregion
}
