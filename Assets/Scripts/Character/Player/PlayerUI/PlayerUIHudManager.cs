using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar _staminaBar;
    
    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        _staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }
    
    public void SetMaxStaminaValue(int maxValue)
    {
        _staminaBar.SetMaxStat(maxValue);
    }
}
