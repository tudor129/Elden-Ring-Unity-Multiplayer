using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager _character;

    float _vertical;
    float _horizontal;
    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        // Option 1:
        _character._animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        _character._animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        
        // Option 2:
        /*float snappedHorizontal = 0;
        float snappedVertical = 0;
        
        // This if chain will round the horizontal movement

        if (horizontalValue > 0 && horizontalValue <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalValue > 0.5f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalValue < 0 && horizontalValue >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalValue < -0.5f)
        {
            snappedHorizontal = -1;
        }
        
        // This if chain will round the vertical movement
        
        if (verticalValue > 0 && verticalValue <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalValue > 0.5f)
        {
            snappedVertical = 1;
        }
        else if (verticalValue < 0 && verticalValue >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalValue < -0.5f)
        {
            snappedVertical = -1;
        }*/
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
