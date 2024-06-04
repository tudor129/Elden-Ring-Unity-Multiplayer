using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance { get; private set; }

    [SerializeField] int _worldSeedIndex = 1;

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

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSeedIndex);

        yield return null;
    }
    
    public int GetWorldSeedIndex()
    {
        return _worldSeedIndex;
    }
}
