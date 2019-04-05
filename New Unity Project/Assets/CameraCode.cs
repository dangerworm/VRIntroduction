using TMPro;
using UnityEngine;

public class CameraCode : MonoBehaviour
{
    public Transform MainCameraTransform;

    private float _pitch;
    private float _yaw;

    private float _dRotation;

    void Start()
    {
        _pitch = 0f;
        _yaw = 0f;
        _dRotation = 200.0f;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * Time.deltaTime;
        _pitch -= mouseY * Time.deltaTime;

        MainCameraTransform.localEulerAngles = new Vector3(_pitch, _yaw, 90) * _dRotation;
    }
}
