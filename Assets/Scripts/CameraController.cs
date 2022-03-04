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

    // Horizontal
    [SerializeField, Range(0f, 1f)]
    private float panValue = 0f;

    private float pan1D;

    // Vertical
    [SerializeField, Range(0f, 16f)]
    private float tiltValue;

    private float tilt1D;

    
    [SerializeField, Range(20f, 60f)]
    private float zoomValue;

    private float zoom1D;


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

        dollyCart.GetComponent<CinemachineDollyCart>().m_Position = panValue + pan1D;

        // Tilt

        // Zoom
    }

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
    }

    public void ChangeTarget(InputAction.CallbackContext context)
    {
        // Not Set Up
    }
    #endregion
}
