using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager _character;
    
    [Header("Stamina Regeneration")]
    float _staminaRegenerationTimer = 0;
    float _staminaTickTimer = 0;
    [SerializeField] float _staminaRegenerationDelay = 2;
    [SerializeField] float _staminaRegenerationAmount = 2;

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        
        // CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED
        
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }
    
    public virtual void RegenerateStamina()
    {
        // ONLY OWNERS CAN EDIT THEIR NETWORK VARIABLES
        if (!_character.IsOwner)
        {
            return;
        }
        // IF WE ARE SPRINTING, THEN WE DO NOT WANT TO REGENERATE STAMINA
        if (_character._characterNetworkManager._isSprinting.Value)
        {
            return;
        }
        
        if (_character._isPerformingAction)
        {
            return;
        }
        
        _staminaRegenerationTimer += Time.deltaTime;

        if (_staminaRegenerationTimer >= _staminaRegenerationDelay)
        {
            if (_character._characterNetworkManager._currentStamina.Value < _character._characterNetworkManager._maxStamina.Value)
            {
                _staminaTickTimer += Time.deltaTime;
                
                if (_staminaTickTimer >= 0.1f)
                {
                    _staminaTickTimer = 0;
                    _character._characterNetworkManager._currentStamina.Value += _staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        // WE ONLY WANT TO RESET THE REGEN IF THE ACTION USED STAMINA\
        // WE DONT WANT TO RESET THE REGEN IF WE ARE ALREADY REGENERATING
        if (currentStaminaAmount < previousStaminaAmount)
        {
            _staminaRegenerationTimer = 0;
        }
        
    }
}
