using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject _titleScreenMainMenu;
    [SerializeField] GameObject _titleScreenLoadMenu;



    [Header("Buttons")]
    [SerializeField] Button _loadMenuReturnButton;
    [SerializeField] Button _mainMenuLoadGameButton;
    
    public void StartNetowrkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.Instance.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
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
}
