using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    float yaw;//x
    float pitch;//y
    public float mouseSensitivity = 5;
    public Transform cameraT;
    public Transform playerPiontT;
    public Transform focusPointT;
    private Vector3 currentFocusPos;
    private Vector3 currentCamPos;



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
        if (Input.GetMouseButton(1))
        {

            FocusState();
        }
        else
        {
            NormalState();
        }
        transform.position = currentFocusPos- transform.forward * dstFromPlayer;
     

    }

    void NormalState()
    {
        float speed = 3f;
        currentFocusPos = SmoothMove(playerPiontT.position,focusPointT.position,speed);
        ChangeFov(42f, 2f);

    }
    void FocusState()
    {
        float speed = 2f;
        currentFocusPos = SmoothMove(focusPointT.position,playerPiontT.position , speed);
        
        ChangeFov(25f, 2f);
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

    //線性移動鏡頭
    private Vector3 SmoothMove(Vector3 from, Vector3 to, float speed)
    {
        return Vector3.Lerp(from, to, speed * Time.deltaTime);
    }

    private void ChangeFov(float value, float speed)
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, value, speed * Time.deltaTime);
    }
}
