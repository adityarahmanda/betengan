using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Sessions")]
    public bool isLobbySession;
    public bool isGameSession;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void StartLobbySession() 
    {
        isLobbySession = true;
        isGameSession = false;
    }

    public void StartGameSession()
    {
        isLobbySession = false;
        isGameSession = true;
    }

    public void StartGame()
    {
        StartGameSession();
        SceneManager.LoadScene("Game");
    }
}