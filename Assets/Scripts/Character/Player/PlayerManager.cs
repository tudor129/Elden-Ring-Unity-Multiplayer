  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    
    [HideInInspector] public PlayerAnimatorManager _playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager _playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        // DO MORE STUFF, ONLY FOR THE PLAYER
        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }
    
    protected override void Update()
    {
        base.Update();
        // IF WE DO NOT OWN THIS GAME OBJECT, THEN WE DO NOT WANT TO HANDLE ANY OF THE MOVEMENT
        if (!IsOwner)
        {
            return;
        }
        // HANDLE ALL OF OUR CHARACTER MOVEMENT
        _playerLocomotionManager.HandleAllMovement();
    }
    
    protected override void LateUpdate()
    {
        // IF WE DO NOT OWN THIS GAME OBJECT, THEN WE DO NOT WANT TO HANDLE ANY OF THE CAMERA ACTIONS
        if (!IsOwner)
        {
            return;
        }
        base.LateUpdate();
       
        // HANDLE ALL OF OUR CAMERA ACTIONS
        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // IF THIS IS THE PLAYER OBJ OWNED BY THIS CLIENT
        if (IsOwner)
        {
            PlayerCamera.Instance._player = this;
            PlayerInputManager.Instance._player = this;
        }
        
        
    }
}
