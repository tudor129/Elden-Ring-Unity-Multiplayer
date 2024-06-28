using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Character";
    
    [Header("Time Played")]
    public float secondsPlayed;
    // WHY NOT USE VECTOR3?
    // WE CAN ONLY USE PRIMITIVE DATA TYPES IN SERIALIZABLE CLASSES(float, int, string, bool)
    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
    
}
