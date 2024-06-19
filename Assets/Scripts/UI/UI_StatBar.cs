using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    Slider _slider;
    // VARIABLE TO SCALE BAR SIZE DEPENDING ON STAT (HIGHER STAT = LONGER BAR ACROSS SCREEN)
    // SECONDARY BAR BEHIND MAIN BAR FOR POLISH EFFECT (YELLOW BAR THAT SHOWS HOW MUCH AN ACTION/DAMAGE TAKES AWAY FROM CURRENT STAT)
    
    protected virtual void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        
    }

    public virtual void SetStat(int newValue)
    {
        _slider.value = newValue;
    }
   
    
    public virtual void SetMaxStat(int maxValue)
    {
        _slider.maxValue = maxValue;
    }
}
