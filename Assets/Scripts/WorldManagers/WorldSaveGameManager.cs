using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance { get; private set; }

    PlayerManager _player;

    [Header("World Scene Index")]
    [SerializeField] int _worldSeedIndex = 1;

    [Header("Save Data Writer")]
    SaveFileDataWriter _saveFileDataWriter;
    
    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    string _saveFileName;
    
    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;
    

    void Awake()
    {
        // THERE CAN ONLY BE ONE INSTANCE OF THIS CLASS
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
    {
        switch (currentCharacterSlotBeingUsed)
        {
            case CharacterSlot.CharacterSlot01:
                currentCharacterData = characterSlot01;
                _saveFileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot02:
                currentCharacterData = characterSlot02;
                _saveFileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot03:
                currentCharacterData = characterSlot03;
                _saveFileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot04:
                currentCharacterData = characterSlot04;
                _saveFileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot05:
                currentCharacterData = characterSlot05;
                _saveFileName = "characterSlot_05";
                break;
            case CharacterSlot.CharacterSlot06:
                currentCharacterData = characterSlot06;
                _saveFileName = "characterSlot_06";
                break;
            case CharacterSlot.CharacterSlot07:
                currentCharacterData = characterSlot07;
                _saveFileName = "characterSlot_07";
                break;
            case CharacterSlot.CharacterSlot08:
                currentCharacterData = characterSlot08;
                _saveFileName = "characterSlot_08";
                break;
            case CharacterSlot.CharacterSlot09:
                currentCharacterData = characterSlot09;
                _saveFileName = "characterSlot_09";
                break;
            case CharacterSlot.CharacterSlot10:
                currentCharacterData = characterSlot10;
                _saveFileName = "characterSlot_10";
                break;
        }
    }

    public void CreateNewGame()
    {
        // CREATE A NEW FILE WITH A FILE NAME DEPENDING ON WHICH CHARACTER SLOT WE ARE USING
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
        
        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        // LOAD A PREVIOUS FILE WITH A FILE NAME DEPENDING ON WHICH CHARACTER SLOT WE ARE USING
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
        
        _saveFileDataWriter = new SaveFileDataWriter();
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        _saveFileDataWriter.saveFileName = _saveFileName;
        currentCharacterData = _saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }
    
    public void SaveGame()
    {
        // SAVE THE CURRENT FILE WITH A FILE NAME DEPENDING ON WHICH CHARACTER SLOT WE ARE USING
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
        
        _saveFileDataWriter = new SaveFileDataWriter();
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        _saveFileDataWriter.saveFileName = _saveFileName;
        
        // PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
        _player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        
        // WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
        _saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSeedIndex);

        yield return null;
    }
    
    public int GetWorldSeedIndex()
    {
        return _worldSeedIndex;
    }
}
