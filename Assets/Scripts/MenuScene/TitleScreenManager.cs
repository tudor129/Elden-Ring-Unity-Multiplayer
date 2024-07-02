using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;
    
    [Header("Menus")]
    [SerializeField] GameObject _titleScreenMainMenu;
    [SerializeField] GameObject _titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button _loadMenuReturnButton;
    [SerializeField] Button _mainMenuNewGameButton;
    [SerializeField] Button _mainMenuLoadGameButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject _noCharacterSlotsPopUp;
    [SerializeField] Button _noCharacterSlotsOkayButton;

    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;
    public GameObject deleteCharacterSlotPopUp;

    [Header("Title Screen")]
    [SerializeField] bool _deleteCharacterSlot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNetowrkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.Instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        // CLOSE MAIN MENU
        _titleScreenMainMenu.SetActive(false);
        // OPEN LOAD MENU
        _titleScreenLoadMenu.SetActive(true);
        
        // SELECT THE RETURN BUTTON
        _loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        // CLOSE LOAD MENU
        _titleScreenLoadMenu.SetActive(false);
        // OPEN MAIN MENU
        _titleScreenMainMenu.SetActive(true);
        
        // SELECT THE LOAD BUTTON
        _mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        _noCharacterSlotsPopUp.SetActive(true);
        _noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        _noCharacterSlotsPopUp.SetActive(false);
        _mainMenuNewGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }
}
