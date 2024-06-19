using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    public PlayerManager _player;
    
    // 1. FIND A WAY TO READ VALUES OF A JOY STICK
    // 2. MOVE CHARACTER BASED ON THESE VALUES
    
    [Header("Camera Movement Input")]
    [SerializeField] Vector2 _cameraInput;
    [SerializeField] float _cameraVerticalInput;
    [SerializeField] float _cameraHorizontalInput;
    [SerializeField] float _cameraSensitivityModifierX = 1f;
    [SerializeField] float _cameraSensitivityModifierY = 1f;
    
    [Header("Player Movement Input")]
    [SerializeField] Vector2 _movementInput;
    [SerializeField] float _verticalInput;
    [SerializeField] float _horizontalInput;
    [SerializeField] float _moveAmount;

    [Header("Player Action Input")]
    [SerializeField] bool _dodgeInput = false;
    [SerializeField] bool _sprintInput = false;
    
    
   
    
    PlayerControls _playerControls;

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

        Cursor.lockState = CursorLockMode.Confined;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        Instance.enabled = false;
        
        // WHEN THE SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // IF WE ARE LOADING INTO OUR WORLD, ENABLE OUR PLAYER INPUT
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSeedIndex())
        {
            Instance.enabled = true;
        }
        // OTHERWISE DISABLE OUR PLAYER INPUT
        // THIS IS SO OUR PLAYER CAN'T MOVE AROUND IN THE MAIN MENU
        else
        {
            Instance.enabled = false;
        }
    }
    void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.PlayerMovement.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerCamera.Movement.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerActions.Dodge.performed += ctx => _dodgeInput = true;
            
            // HOLDING THE INPUT, SETS THE BOOL TO TRUE
            _playerControls.PlayerActions.Sprint.performed += ctx => _sprintInput = true;
            // RELEASING THE INPUT, SETS THE BOOL TO FALSE
            _playerControls.PlayerActions.Sprint.canceled += ctx => _sprintInput = false;
            
        }
        _playerControls.Enable();
    }
    void OnDestroy()
    {
        // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THE EVENT
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    // IF WE MINIMIZE OR LOWER THE WINDOW, DISABLE OUR PLAYER INPUT
    void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            if (hasFocus)
            {
                _playerControls.Enable();
            }
            else
            {
                _playerControls.Disable();
            }
        }
    }
    void Update()
    {
        HandleAllInputs();
    }
    void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprinting();
    }
    
    // MOVEMENT
    void HandlePlayerMovementInput()
    {
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;
        
        // RETURNS THE ABSOLUTE VALUE OF THE INPUT
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));
        
        // WE CLAMP THE VALUES, SO THEY ARE 0, 0.5, OR 1 (OPTIONAL) -- GIVES THE SOULS CONTROLLER FEEL
        if (_moveAmount <= 0.5 && _moveAmount > 0)
        {
            _moveAmount = 0.5f;
        }
        else if ( _moveAmount > 0.5 && _moveAmount < 1)
        {
            _moveAmount = 1f;
        }
        
        // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ARE NOT LOCKED ON TO THE ENEMY, SO WE ONLY WANT NON-STRAFING MOVEMENT
        // WE USE HORIZONTAL WHEN WE ARE LOCKED ON OR STRAFING

        if (_player == null)
        {
            return;
        }
        
        // IF WE ARE NOT LOCKED ON, WE WANT TO PASS 0 FOR THE HORIZONTAL INPUT
        _player._playerAnimatorManager.UpdateAnimatorMovementParameters(0, _moveAmount, _player._playerNetworkManager._isSprinting.Value);
        
        // IF WE ARE LOCKED ON, WE WANT TO PASS THE HORIZONTAL 
        
    }
    void HandleCameraMovementInput()
    {
        _cameraVerticalInput = _cameraInput.y * _cameraSensitivityModifierY;
        _cameraHorizontalInput = _cameraInput.x * _cameraSensitivityModifierX;
    }
    // ACTIONS
    void HandleDodgeInput()
    {
        if (_dodgeInput)
        {
            _dodgeInput = false;
            // FUTURE NOTE: RETURN(DO NOTHING) IF MENU OR UI WINDOW IS OPEN, DO NOTHING 
            // PERFORM A DODGE
            _player._playerLocomotionManager.AttemptToPerformDodge();
        }
    }
    
    void HandleSprinting()
    {
        if (_sprintInput)
        {
            // HANDLE SPRINTING
            _player._playerLocomotionManager.HandleSprinting();
        }
        else
        {
            _player._playerNetworkManager._isSprinting.Value = false;
        }
    }
    
    public float GetPlayerMoveAmount()
    {
        return _moveAmount;
    }
    public float GetPlayerVerticalInput()
    {
        return _verticalInput;
    }
    public float GetPlayerHorizontalInput()
    {
        return _horizontalInput;
    }
    
    public float GetCameraVerticalInput()
    {
        return _cameraVerticalInput;
    }
    public float GetCameraHorizontalInput()
    {
        return _cameraHorizontalInput;
    }
}
