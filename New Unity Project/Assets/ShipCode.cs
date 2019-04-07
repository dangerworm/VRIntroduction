using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ShipCode : MonoBehaviour
{
    public Rigidbody ShipBody;
    public Transform ShipTransform;
    public float EnginePower;
    public float VelocityDampeningFactor;
    public float RotationalDampeningFactor;
    public bool UseContinuousRotation;
    public bool ThrustControlActive;
    public bool StabilisersActive;

    private Dictionary<KeyCode, Vector3> _rotationKeys;
    private Vector3 _rotation;

    private float _dt;
    private float _dRotation;

    private bool _brakeActive;
    private bool _cursorLocked;

    // Use this for initialization
    void Start()
    {
        _brakeActive = false;
        _cursorLocked = true;
        _dRotation = 20.0f;

        _rotation = UseContinuousRotation 
            ? new Vector3(0.0f, 0.0f, 0.0f) 
            : ShipTransform.localEulerAngles;

        _rotationKeys = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, new Vector3( 1,  0,  0) },
            { KeyCode.S, new Vector3(-1,  0,  0) },
            { KeyCode.A, new Vector3( 0, -1,  0) },
            { KeyCode.D, new Vector3( 0,  1,  0) },
            { KeyCode.Q, new Vector3( 0,  0,  1) },
            { KeyCode.E, new Vector3( 0,  0, -1) }
        };
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        ApplyRotation();

        if (ThrustControlActive)
        {
            ApplyInertialDampeners();
        }

        Cursor.lockState = _cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void ProcessInputs()
    {
        ProcessMouse();
        ProcessKeyboard();
    }

    private void ProcessMouse()
    {
        if (Input.GetMouseButton((int)MouseButton.LeftMouse))
        {
            ShipBody.AddForce(ShipTransform.forward * Time.deltaTime * EnginePower);
        }

        if (Input.GetMouseButton((int)MouseButton.RightMouse))
        {
            ShipBody.AddForce(-ShipTransform.forward * Time.deltaTime * EnginePower);
        }
    }

    private void ProcessKeyboard()
    {
        foreach (var key in _rotationKeys.Keys)
        {
            if (Input.GetKey(key))
            {
                _rotation += _rotationKeys[key] * Time.deltaTime * _dRotation * (StabilisersActive ? RotationalDampeningFactor : 1);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _brakeActive = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ThrustControlActive = !ThrustControlActive;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StabilisersActive = !StabilisersActive;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UseContinuousRotation = !UseContinuousRotation;
        }
    }

    private void ApplyRotation()
    {
        if (!(Math.Abs(_rotation.x + _rotation.y + _rotation.z) > Time.deltaTime * 0.1))
            return;

        ShipTransform.Rotate(_rotation);

        if (!UseContinuousRotation)
        {
            _rotation = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if (StabilisersActive)
        {
            ApplyStabilisers();
        }
    }

    private void ApplyInertialDampeners()
    {
        var localVelocity = ShipTransform.InverseTransformVector(ShipBody.velocity);

        localVelocity.x /= VelocityDampeningFactor;
        localVelocity.y /= VelocityDampeningFactor;

        if (_brakeActive)
        {
            localVelocity.z /= VelocityDampeningFactor;
            _brakeActive = false;
        }

        ShipBody.velocity = ShipTransform.TransformDirection(localVelocity);
    }

    private void ApplyStabilisers()
    {
        _rotation /= RotationalDampeningFactor;
    }
}
