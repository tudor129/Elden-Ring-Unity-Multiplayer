using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }
     
    [Header("NETWORK JOIN")]
    [SerializeField] bool _startGameAsClient;
    
    [HideInInspector] public PlayerUIHudManager _playerUIHudManager;


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
        
        _playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (_startGameAsClient)
        {
            _startGameAsClient = false;
            // WE MUST FIRST SHUT DOWN, BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLE SCREEN
            NetworkManager.Singleton.Shutdown();
            // WE THEN RESTART THE NETWORK, AS THE CLIENT
            NetworkManager.Singleton.StartClient();
        }
    }
}
