using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance { get; private set; }

    [SerializeField] PlayerManager _player;

    [Header("SAVE/LOAD")]
    [SerializeField] bool _saveGame;
    [SerializeField] bool _loadGame;

    [Header("World Scene Index")]
    [SerializeField] int _worldSceneIndex = 1;

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
        LoadAllCharacterProfiles();
        
    }

    void Update()
    {
        if (_saveGame)
        {
            _saveGame = false;
            SaveGame();
        }
        
        if(_loadGame)
        {
            _loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot01:
                currentCharacterData = characterSlot01;
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot02:
                currentCharacterData = characterSlot02;
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot03:
                currentCharacterData = characterSlot03;
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot04:
                currentCharacterData = characterSlot04;
                fileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot05:
                currentCharacterData = characterSlot05;
                fileName = "characterSlot_05";
                break;
            case CharacterSlot.CharacterSlot06:
                currentCharacterData = characterSlot06;
                fileName = "characterSlot_06";
                break;
            case CharacterSlot.CharacterSlot07:
                currentCharacterData = characterSlot07;
                fileName = "characterSlot_07";
                break;
            case CharacterSlot.CharacterSlot08:
                currentCharacterData = characterSlot08;
                fileName = "characterSlot_08";
                break;
            case CharacterSlot.CharacterSlot09:
                currentCharacterData = characterSlot09;
                fileName = "characterSlot_09";
                break;
            case CharacterSlot.CharacterSlot10:
                currentCharacterData = characterSlot10;
                fileName = "characterSlot_10";
                break;
        }
        
        return fileName;
    }

    public void CreateNewGame()
    {
        // CREATE A NEW FILE WITH A FILE NAME DEPENDING ON WHICH CHARACTER SLOT WE ARE USING
        _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        // LOAD A PREVIOUS FILE WITH A FILE NAME DEPENDING ON WHICH CHARACTER SLOT WE ARE USING
        _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
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
        _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
        _saveFileDataWriter = new SaveFileDataWriter();
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        _saveFileDataWriter.saveFileName = _saveFileName;
        
        // PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
        _player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        
        // WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
        _saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }
    
    // LOAD ALL CHARACTER PROFILES ON DEVICE WHEN STARTING THE GAME
    void LoadAllCharacterProfiles()
    {
        _saveFileDataWriter = new SaveFileDataWriter();
        _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot01);
        characterSlot01 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot02);
        characterSlot02 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot03);
        characterSlot03 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot04);
        characterSlot04 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot05);
        characterSlot05 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot06);
        characterSlot06 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot07);
        characterSlot07 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot08);
        characterSlot08 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot09);
        characterSlot09 = _saveFileDataWriter.LoadSaveFile();
        
        _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot10);
        characterSlot10 = _saveFileDataWriter.LoadSaveFile();
        
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSceneIndex);

        yield return null;
    }
    
    public int GetWorldSeedIndex()
    {
        return _worldSceneIndex;
    }
}
