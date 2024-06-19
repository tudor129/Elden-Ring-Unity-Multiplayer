using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager _character;

    int _vertical;
    int _horizontal;
    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
        
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;

        if (isSprinting)
        {
            verticalAmount = 2;
        }
        
        _character._animator.SetFloat(_horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        _character._animator.SetFloat(_vertical, verticalAmount, 0.1f, Time.deltaTime);
        
        
    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false) 
    {
        _character._applyRootMotion = applyRootMotion;
        _character._animator.CrossFade(targetAnimation, 0.2f);
        // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS WHILE PERFORMING AN ACTION
        // FOR EXAMPLE IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING TO PERFORM A NEW ACTION
        _character._isPerformingAction = isPerformingAction;
        _character._canMove = canMove;
        _character._canRotate = canRotate;
        
        // TELL THE SERVER TO UPDATE THE ANIMATION STATE FOR THIS CHARACTER AND PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        _character._characterNetworkManager.NotifyServerOfAnActionAnimationServerRpc(
            NetworkManager.Singleton.LocalClientId, 
            targetAnimation, 
            applyRootMotion);
    }
}
