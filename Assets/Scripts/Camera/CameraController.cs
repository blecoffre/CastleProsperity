using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions _cameraActions;
    private InputAction _movement;
    private Transform _cameraTransform;

    //Horizontal Translation
    [SerializeField] private float _maxSpeed = 5f;
    private float _currentSpeed;
    //Horizontal Translation
    [SerializeField] private float _acceleration = 10f;
    //Horizontal Translation
    [SerializeField] private float _damping = 15f;

    //Vertical Translation
    [SerializeField] private float _stepSize = 2f;
    //Vertical Translation
    [SerializeField] private float _zoomDampening = 7.5f;
    //Vertical Translation
    [SerializeField]
    private float _minHeight = 5f;
    //Vertical Translation
    [SerializeField]
    private float _maxHeight = 50f;
    //Vertical Translation
    [SerializeField]
    private float _zoomSpeed = 2f;

    //Rotation
    [SerializeField] private float _maxRotationSpeed = 1f;

    //Edge Movement
    [SerializeField] [Range(0f, 0.1f)] private float _edgeTolerance = 0.05f;
    [SerializeField] private bool _useScreenEdge = false;

    //value set in various functions 
    //used to update the position of the camera base object.
    private Vector3 _targetPosition;

    private float _zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 _horizontalVelocity;
    private Vector3 _lastPosition;

    //tracks where the dragging action started
    private Vector3 _startDrag;

    private Camera _camera = default;
    public Camera Camera => _camera;

    private void Awake()
    {
        _cameraActions = new CameraControlActions();
        _camera = this.GetComponentInChildren<Camera>();
        _cameraTransform = _camera.transform;
    }

    private void OnEnable()
    {
        _zoomHeight = _minHeight;
        _cameraTransform.LookAt(this.transform);

        _lastPosition = this.transform.position;
        _movement = _cameraActions.Camera.Movement;

        _cameraActions.Camera.RotateCamera.performed += RotateCamera;
        _cameraActions.Camera.ZoomCamera.performed += ZoomCamera;

        _cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        _cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        _cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;

        _cameraActions.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        if (_useScreenEdge)
        {
            CheckMouseAtScreenEdge();
        }
        DragCamera();

        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        _horizontalVelocity = (this.transform.position - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0;
        _lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = _movement.ReadValue<Vector2>().x * GetCameraRight() + _movement.ReadValue<Vector2>().y * GetCameraForward();
        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            _targetPosition += inputValue;
        }
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = this._cameraTransform.right;
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = this._cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (_targetPosition.sqrMagnitude > 0.1f)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _maxSpeed, Time.deltaTime * _acceleration);
            transform.position += _targetPosition * _currentSpeed * Time.deltaTime;
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _damping);
            transform.position += _horizontalVelocity * Time.deltaTime;
        }

        _targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
        {
            return;
        }

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0.0f, value * _maxRotationSpeed + transform.rotation.eulerAngles.y, 0.0f);
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100.0f;

        if (Mathf.Abs(value) > 0.1f)
        {
            _zoomHeight = _cameraTransform.localPosition.y + value * _stepSize;
            if (_zoomHeight < _minHeight)
            {
                _zoomHeight = _minHeight;
            }
            else if (_zoomHeight > _maxHeight)
            {
                _zoomHeight = _maxHeight;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, _zoomHeight, _cameraTransform.localPosition.z);
        zoomTarget -= _zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, Time.deltaTime * _zoomDampening);
        _cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < _edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1.0f - _edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        if (mousePosition.y < _edgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1.0f - _edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        _targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed)
        {
            return;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _startDrag = ray.GetPoint(distance);
            }
            else
            {
                _targetPosition += _startDrag - ray.GetPoint(distance);
            }
        }
    }
}
