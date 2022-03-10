using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private bool isStaticBillboard;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
    }
}
