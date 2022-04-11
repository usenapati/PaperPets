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
    public GameObject target;

    [SerializeField]
    private float cameraSpeed = 1f;
    [SerializeField]
    private float tiltSpeed = 40f;
    [SerializeField]
    private float zoomSpeed = 20f;

    // Horizontal
    [Header("Pan Values")]
    [SerializeField]
    private float panMinValue;

    [SerializeField]
    private float panMaxValue = 1f;

    [SerializeField, Range(0f, 1f)]
    private float panValue = 0.5f;

    private float pan1D;

    private float tempPan = 0.5f;



    // Vertical
    [Header("Tilt Values")]
    [SerializeField]
    private float tiltMinValue = -10f;

    [SerializeField]
    private float tiltMaxValue = 30f;

    [SerializeField, Range(-2f, 30f)]
    private float tiltValue;

    private float tilt1D;

    private float tempTilt = 2f;

    // Zoom
    [Header("Zoom Values")]
    [SerializeField]
    private float zoomMinValue = 10f;

    [SerializeField]
    private float zoomMaxValue = 60f;
        
    [SerializeField, Range(10f, 60f)]
    private float zoomValue;

    private float zoom1D;

    private float tempZoom = 20f;

    // Camera Movement
    [Header("Camera Values")]
    [SerializeField]
    private float cameraMinYValue = 0f;

    [SerializeField]
    private float cameraMaxYValue = 3f;

    [SerializeField, Range(0f, 3f)]
    private float cameraYValue;

    private float cameraY1D;

    private float tempCameraY = 1.78f;

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
        target = GameObject.FindGameObjectWithTag("target");
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
        var dollyPosition = dollyTrack.transform.position;
        dollyPosition = new Vector3(dollyPosition.x, tiltValue, dollyPosition.z);
        dollyTrack.transform.position = dollyPosition;

        // Zoom
        HandleZoom();
        mainCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = zoomValue;

        HandleVerticalMovement();
        var targetPosition = target.transform.position;
        targetPosition = new Vector3(targetPosition.x, cameraYValue, targetPosition.z);
        target.transform.position = targetPosition;
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
        panValue = Mathf.Clamp(tempPan, panMinValue, panMaxValue);
        tempPan = Mathf.Clamp(tempPan, -1, 1);
    }
    
    void DecrementPan()
    {
        tempPan += pan1D * Time.deltaTime * cameraSpeed;
        panValue = Mathf.Clamp(tempPan, panMinValue, panMaxValue);
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
        tiltValue = Mathf.Clamp(tempTilt, tiltMinValue, tiltMaxValue);
        tempTilt = Mathf.Clamp(tempTilt, -2, 16);
    }

    void DecrementTilt()
    {
        tempTilt += tilt1D * Time.deltaTime * tiltSpeed * cameraSpeed;
        tiltValue = Mathf.Clamp(tempTilt, tiltMinValue, tiltMaxValue);
        tempTilt = Mathf.Clamp(tempTilt, -2, 16);
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
        //tempZoom += zoom1D * Time.deltaTime * zoomSpeed * cameraSpeed;
        tempZoom += 1 * Time.deltaTime * zoomSpeed * cameraSpeed;
        zoomValue = Mathf.Clamp(tempZoom, zoomMinValue, zoomMaxValue);
        tempZoom = Mathf.Clamp(tempZoom, -60, 60);
    }

    void DecrementZoom()
    {
        tempZoom += -1 * Time.deltaTime * zoomSpeed * cameraSpeed;
        zoomValue = Mathf.Clamp(tempZoom, zoomMinValue, zoomMaxValue);
        tempZoom = Mathf.Clamp(tempZoom, -60, 60);
    }
    #endregion
    
    #region Vertical Movement Methods
    void HandleVerticalMovement()
    {
        if (cameraY1D > 0)
        {
            //Debug.Log("Incrementing Zoom");
            IncrementYPosition();
        }
        else if (cameraY1D < 0)
        {
            //Debug.Log("Decrementing Zoom");
            DecrementYPosition();
        }
    }

    void IncrementYPosition()
    {
        tempCameraY += cameraY1D * Time.deltaTime * cameraSpeed;
        cameraYValue = Mathf.Clamp(tempCameraY, cameraMinYValue, cameraMaxYValue);
        tempCameraY = Mathf.Clamp(tempCameraY, 0, 3);
    }

    void DecrementYPosition()
    {
        tempCameraY += cameraY1D * Time.deltaTime  * cameraSpeed;
        cameraYValue = Mathf.Clamp(tempCameraY, cameraMinYValue, cameraMaxYValue);
        tempCameraY = Mathf.Clamp(tempCameraY, 0, 3);
    }
    #endregion

    #region Input Methods
    public void OnPan(InputAction.CallbackContext context)
    {
        pan1D = context.ReadValue<float>();
        //Debug.Log("Pan: " + pan1D);
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tilt1D = context.ReadValue<float>();
        //Debug.Log("Tilt: " + tilt1D);
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoom1D = context.ReadValue<float>();
        //Debug.Log("Zoom: " + zoom1D);
    }

    public void OnCameraMoveVertical(InputAction.CallbackContext context)
    {
        
        cameraY1D = context.ReadValue<float>();
        //Debug.Log("Camera Y: " + cameraY1D);
    }
    
    public void ChangeTarget(InputAction.CallbackContext context)
    {
        // Not Set Up
    }
    #endregion
}
