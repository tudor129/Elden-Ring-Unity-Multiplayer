using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager _player;
    // THESE VALUES ARE TAKEN FROM THE INPUT MANAGER
    [HideInInspector] public float _verticalMovement;
    [HideInInspector] public float _horizontalMovement;
    [HideInInspector] public float _moveAmount;
    
    [Header("Movement Settings")]
    Vector3 _moveDirection;
    Vector3 _targetRotationDirection;
    [SerializeField] float _walkingSpeed = 2.5f;
    [SerializeField] float _runningSpeed = 5f;
    [SerializeField] float _sprintingSpeed = 6.5f;
    [SerializeField] float _rotationSpeed = 15f;
    [SerializeField] int _sprintingStaminaCost = 2;



    [Header("Dodge Settings")]
    Vector3 _rollDirection;
    [SerializeField] float _dodgeStaminaCost = 25;


    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (_player.IsOwner)
        {
            _player._characterNetworkManager._animatorVerticalValue.Value = _verticalMovement;
            _player._characterNetworkManager._animatorHorizontalValue.Value = _horizontalMovement;
            _player._characterNetworkManager._networkMoveAmount.Value = _moveAmount;
        }
        else
        {
            _moveAmount = _player._characterNetworkManager._networkMoveAmount.Value;
            _verticalMovement = _player._characterNetworkManager._animatorVerticalValue.Value;
            _horizontalMovement = _player._characterNetworkManager._animatorHorizontalValue.Value;
            
            // IF NOT LOCKED ON, PASS MOVE AMOUNT
            _player._playerAnimatorManager.UpdateAnimatorMovementParameters(0, _moveAmount, _player._playerNetworkManager._isSprinting.Value);
            
            // IF LOCKED ON, PASS VERTICAL and HORIZONTAL DIRECTION
        }
        
    }
    public void HandleAllMovement() 
    {
        if (_player._isPerformingAction)
        {
            return;
        }
        // GROUNDED MOVEMENT
        HandleGroundedMovement();
        HandleRotation();
        // AERIAL MOVEMENT
    }

    void GetMovementValues()
    {
        _verticalMovement = PlayerInputManager.Instance.GetPlayerVerticalInput();
        _horizontalMovement = PlayerInputManager.Instance.GetPlayerHorizontalInput();
        _moveAmount = PlayerInputManager.Instance.GetPlayerMoveAmount();

        // CLAMP THE MOVEMENTS
    }

    void HandleGroundedMovement()
    {
        if (!_player._canMove)
        {
            return;
        }
        GetMovementValues();
        // OUR MOVEMENT DIR IS BASED ON CAMERA FACING PERSPECTIVE AND OUR INPUTS
        _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
        _moveDirection += PlayerCamera.Instance.transform.right * _horizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        if (_player._playerNetworkManager._isSprinting.Value)
        {
            _player._characterController.Move(_moveDirection * _sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.Instance.GetPlayerMoveAmount() > 0.5f)
            {
                // MOVE AT RUNNING SPEED
                _player._characterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.GetPlayerMoveAmount() <= 0.5f)
            {
                // MOVE AT WALKING SPEED
                _player._characterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);
            }
        }

       
    }

    void HandleRotation()
    {
        if (!_player._canRotate)
        {
            return;
        }
        _targetRotationDirection = Vector3.zero;
        _targetRotationDirection = PlayerCamera.Instance._cameraObject.transform.forward * _verticalMovement;
        _targetRotationDirection += PlayerCamera.Instance._cameraObject.transform.right * _horizontalMovement;
        _targetRotationDirection.Normalize();
        _targetRotationDirection.y = 0;
        
        if (_targetRotationDirection == Vector3.zero)
        {
            _targetRotationDirection = transform.forward;
        }
        
        Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(
            transform.rotation, 
            newRotation, 
            _rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformDodge()
    {
        if (_player._isPerformingAction)
        {
            return;
        }
        if (_player._playerNetworkManager._currentStamina.Value <= 0)
        {
            return;
        }
        // IF WE ARE MOVING WHEN WE ATTEMPT TO PERFORM A DODGE, WE PERFORM A ROLL
        if (_moveAmount > 0)
        {
            _rollDirection = PlayerCamera.Instance._cameraObject.transform.forward * _verticalMovement;
            _rollDirection += PlayerCamera.Instance._cameraObject.transform.right * _horizontalMovement;

            _rollDirection.y = 0;
            _rollDirection.Normalize();
        
            Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
            _player.transform.rotation = playerRotation;
            
            // PERFORM A ROLL ANIMATION
            _player._playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
        }
        // IF WE ARE STATIONARY WE ARE GOING TO PERFORM A BACKSTEP
        else
        {
            // PERFORM A BACKSTEP ANIMATION
            _player._playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
        }
        
        _player._playerNetworkManager._currentStamina.Value -= _dodgeStaminaCost;
        
        
    }
    public void HandleSprinting()
    {
        if (_player._isPerformingAction)
        {
            // SET SPRINTING TO FALSE
            _player._playerNetworkManager._isSprinting.Value = false;
        }
        
        // IF WE ARE OUT OF STAMINA, SET SPRINTING TO FALSE
        if (_player._playerNetworkManager._currentStamina.Value <= 0)
        {
            _player._playerNetworkManager._isSprinting.Value = false;
            return;
        }
        
        // IF WE ARE MOVING SET SPRINTING TO TRUE
        if (_moveAmount >= 0.5f)
        {
            _player._playerNetworkManager._isSprinting.Value = true;
        }
        // IF WE ARE STATIONARY/MOVING SLOWLY SET SPRINTING TO FALSE
        else
        {
            _player._playerNetworkManager._isSprinting.Value = false;
        }
        // IF STATIONARY SET SPRINTING TO FALSE

        if (_player._playerNetworkManager._isSprinting.Value)
        {
            _player._playerNetworkManager._currentStamina.Value -= _sprintingStaminaCost * Time.deltaTime;
        }
    }
}
