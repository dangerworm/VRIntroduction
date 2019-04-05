using System.Collections.Generic;
using UnityEngine;

public class ShipCode : MonoBehaviour
{
    public Rigidbody ShipBody;
    public Transform ShipTransform;

    private Dictionary<KeyCode, Vector3> _keyRotations;
    private Vector3 _rotation;

    private float _dt;
    private float _dSpeed;
    private float _dBrake;
    private float _dRotation;

    private bool _cursorLocked;
    private bool _useContinuousRotation;

    // Use this for initialization
    void Start()
    {
        _cursorLocked = true;
        _useContinuousRotation = false;

        _dSpeed = 60.0f;
        _dBrake = 1 + (1 / _dSpeed);

        if (_useContinuousRotation)
        {
            _rotation = new Vector3(0, 0, 0);
            _dRotation = 0.5f;
        }
        else
        {
            _rotation = ShipTransform.localEulerAngles;
            _dRotation = 30.0f;
        }

        _keyRotations = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.A, new Vector3 (0, -1, 0) },
            { KeyCode.D, new Vector3 (0, 1, 0) }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _cursorLocked = !_cursorLocked;
        }

        if (Input.GetKey(KeyCode.W))
        {
            ShipBody.AddForce(ShipTransform.forward * Time.deltaTime * _dSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            ShipBody.AddForce(-ShipTransform.forward * Time.deltaTime * _dSpeed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ShipBody.velocity = ShipBody.velocity / _dBrake;
        }

        foreach (var key in _keyRotations.Keys)
        {
            if (Input.GetKey(key))
            {
                _rotation += _keyRotations[key] * Time.deltaTime * _dRotation;
            }
        }

        if (_useContinuousRotation)
        {
            ShipTransform.localEulerAngles += _rotation;
        }
        else
        {
            ShipTransform.localEulerAngles = _rotation;
        }

        // Slow down sideways movement
        var localVelocity = ShipTransform.InverseTransformVector(ShipBody.velocity);
        localVelocity.x /= _dBrake;
        ShipBody.velocity = ShipTransform.TransformDirection(localVelocity);

        Cursor.lockState = _cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
