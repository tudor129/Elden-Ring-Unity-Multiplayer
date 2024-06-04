using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }
    
    public PlayerManager _player;
    public Camera _cameraObject;

    [SerializeField] Transform _cameraPivotTransform;
    // CHANGE THESE SETTINGS TO TWEAK CAMERA BEHAVIOR
    [Header("Camera Settings")]
    float _cameraSmoothSpeed = 1f; // THE HIGHER THE VALUE, THE LONGER FOR THE CAMERA TO CATCH UP TO THE TARGET
    [SerializeField] float _leftandRightRotationSpeed = 220f;
    [SerializeField] float _upandDownRotationSpeed = 220f;
    [SerializeField] float _minimumPivot = -30f; // lowest point you are able to look down
    [SerializeField] float _maximumPivot = 60f; // highest point you are able to look up
    [SerializeField] float _cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask _collideWithLayers;
    
    // JUST DISPLAYS CAMERA VALUES
    [Header("Camera Values")]
    Vector3 _cameraVelocity;
    Vector3 _cameraObjectPosition; // USED FOR CAMERA COLLISIONS(MOVES THE CAMERA OBJ TO THIS POSITION)
    [SerializeField] float _leftAndRightLookAngle;
    [SerializeField] float _upAndDownLookAngle;
    // VALUES USED FOR CAMERA COLLISION
    float _cameraZPosition;
    float _targetCameraZPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // IF THERE IS ALREADY AN INSTANCE OF THIS OBJECT, DESTROY THIS ONE
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _cameraZPosition = _cameraObject.transform.localPosition.z;
        
    }

    public void HandleAllCameraActions()
    {
        
        if (_player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }
    
    void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(
            transform.position, 
            _player.transform.position, 
            ref _cameraVelocity, 
            _cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }
    
    void HandleRotations()
    {
        // IF LOCKED ON, FORCE ROTATION TOWARDS LOCK ON TARGET
        // ELSE ROTATE NORMALLY
        
        // NORMAL ROTATIONS
        // ROTATE LEFT AND RIGHT BASED ON THE  HORIZONTAL MOVEMENT OF THE CAMERA INPUT(RIGHT STICK)
        _leftAndRightLookAngle += PlayerInputManager.Instance.GetCameraHorizontalInput() * _leftandRightRotationSpeed * Time.deltaTime;
        // ROTATE UP AND DOWN BASED ON THE VERTICAL MOVEMENT OF THE CAMERA INPUT(RIGHT STICK)
        _upAndDownLookAngle -= PlayerInputManager.Instance.GetCameraVerticalInput() * _upandDownRotationSpeed * Time.deltaTime;
        _upAndDownLookAngle = Mathf.Clamp(_upAndDownLookAngle, _minimumPivot, _maximumPivot);
        
        
        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        
        // ROTATE THIS GAMEOBJECT LEFT AND RIGHT
        cameraRotation.y = _leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;
        
        // ROTATE THE PIVOT GAME OBJECT UP AND DOWN
        cameraRotation = Vector3.zero;
        cameraRotation.x = _upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        _cameraPivotTransform.localRotation = targetRotation;
    }

    void HandleCollisions()
    {
        _targetCameraZPosition = _cameraZPosition;
        RaycastHit hit;
        // DIRECTION FOR COLLISION CHECK 
        Vector3 direction = _cameraObject.transform.position - _cameraPivotTransform.position;
        direction.Normalize();

        // WE CHECK IF THERE IS AN OBJ IN FRONT OF OUR DESIRED DIRECTION (SEE ABOVE)
        if (Physics.SphereCast(
                _cameraPivotTransform.position,
                _cameraCollisionRadius,
                direction,
                out hit,
                Mathf.Abs(_targetCameraZPosition),
                _collideWithLayers)) 
        {
            // IF THERE IS, WE GET OUR DISTANCE FROM IT
            float distanceFromHitObject = Vector3.Distance(_cameraPivotTransform.position, hit.point);
            // WE THEN EQUATE OUR TARGET CAMERA Z POSITION TO THE DISTANCE FROM THE HIT OBJ
            _targetCameraZPosition = -(distanceFromHitObject - _cameraCollisionRadius);
        }
        // IF OUR TARGET POSITION IS LESS THAN OUR COLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
        if (Mathf.Abs(_targetCameraZPosition) < _cameraCollisionRadius)
        {
            _targetCameraZPosition = -_cameraCollisionRadius;
        }
        // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2 SECONDS
        _cameraObjectPosition.z = Mathf.Lerp(
            _cameraObject.transform.localPosition.z, 
            _targetCameraZPosition, 
            0.2f); 
        _cameraObject.transform.localPosition = _cameraObjectPosition;
    }
}
