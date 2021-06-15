using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool paused = false;
    public int maxScore;
    public int roundCountdownTime;

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

        SceneManager.LoadScene("Lobby");
    }

    public void StartGameSession(int _maxScore, int _roundCountdownTime)
    {
        isLobbySession = false;
        isGameSession = true;
        
        maxScore = _maxScore;
        roundCountdownTime = _roundCountdownTime;
        
        SceneManager.LoadScene("Game");
    }

    public void EndGameSession()
    {
        maxScore = 0;
        roundCountdownTime = 0;

        StartLobbySession();
    }
}