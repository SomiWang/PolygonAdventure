using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ThirdPersonCamera : MonoBehaviour {
    float yaw;//x
    float pitch;//y
    public float mouseSensitivity = 5;
    public Transform cameraT;
    public Transform playerPiontT;
 



    public float dstFromPlayer = 5;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public bool lockCursor;


    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }



    }


    void LateUpdate()
    {
        RotateCamera();
       
        transform.position = playerPiontT.position - transform.forward * dstFromPlayer;


    }



    void RotateCamera()

    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        //天地仰角限制
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
    }

}
