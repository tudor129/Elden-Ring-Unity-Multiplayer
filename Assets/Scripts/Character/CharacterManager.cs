using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController _characterController;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public CharacterNetworkManager _characterNetworkManager;
    
    [Header("Flags")]
    public bool _isPerformingAction = false;
    public bool _applyRootMotion = true;
    public bool _canRotate = true;
    public bool _canMove = true;
    
    
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        // IF THIS CHARACTER IS BEING CONTROLLER BY OUR SIDE, THEN ASSIGN IT"S NETWORK POSITION 
        // TO THE POSITION OF OUR TRANSFORM
        if (IsOwner)
        {
            _characterNetworkManager._networkPosition.Value = transform.position;
            _characterNetworkManager._networkRotation.Value = transform.rotation;
            
        }
        // IF THIS CHAR IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN IT'S POSITION HERE LOCALLY 
        // BY THE POSITION OF IT'S NETWORK TRANSFORM
        else
        {
            // Position
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                _characterNetworkManager._networkPosition.Value, 
                ref _characterNetworkManager._networkPositionVelocity, 
                _characterNetworkManager._networkPositionSmoothTime);
            // Rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                _characterNetworkManager._networkRotation.Value, 
                _characterNetworkManager._networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        
    }
}
