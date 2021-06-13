using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Sessions")]
    public bool isLobbySession;
    public bool isGameSession;
    
    [Header("Player Power Settings")]
    public float powerDecreaseSpeed = 8;
    public float powerRegenerateSpeed = 50;

    /*
    [Header("Red Team Settings")]
    public Transform redTeamBase;
    public Transform redTeamPrison;

    [Header("Blue Team Settings")]
    public Transform blueTeamBase;
    public Transform blueTeamPrison;

    [Header("Player Prefabs")]
    public Player redTeamLocalPlayerPrefab;
    public Player redTeamPlayerPrefab;
    public Player blueTeamLocalPlayerPrefab;
    public Player blueTeamPlayerPrefab;
    */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start() 
    {
        StartLobbySession();
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
        ServerSend.StartGame();
    }
}