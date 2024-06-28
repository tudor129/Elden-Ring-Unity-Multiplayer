using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWriter _saveFileWriter;
    
    [Header("Game Slot")]
    public CharacterSlot characterSlot;
    
    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    void OnEnable()
    {
        LoadSaveSlots();
    }
    
    void LoadSaveSlots()
    {
        _saveFileWriter = new SaveFileDataWriter();
        _saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        // SAVE SLOT 01
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot01:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                }
                // IF THE FILE DOESNT EXIST, SET THE GAME OBJECT TO INACTIVE
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot02:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot03:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot04:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot05:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot06:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot06.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot07:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot07.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot08:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot08.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot09:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot09.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot10:
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (_saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot10.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void LoadGameFromCharacterSlot()
    {
        WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.Instance.LoadGame();
    }
}
