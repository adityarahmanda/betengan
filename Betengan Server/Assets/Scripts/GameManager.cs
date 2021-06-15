using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float spawnRadius = 2f;

    [Header("Sessions")]
    public bool isLobbySession;
    public bool isGameSession;

    [Header("Game Settings")]
    public int currentRound = 0;
    public int maxScore = 2;
    public int redTeamScore = 0;
    public int blueTeamScore = 0;
    
    [Header("Player Power Settings")]
    public float powerDecreaseSpeed = 8;
    public float powerRegenerateSpeed = 50;

    [Header("Red Team Settings")]
    public Transform redTeamBase;
    public Transform redTeamPrison;

    [Header("Blue Team Settings")]
    public Transform blueTeamBase;
    public Transform blueTeamPrison;

    [Header("Player Prefabs")]
    public PlayerController redTeamPlayerPrefab;
    public PlayerController blueTeamPlayerPrefab;

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

        currentRound = 0;
        redTeamScore = 0;
        blueTeamScore = 0;
    }

    public void StartGame()
    {
        StartGameSession();
        ServerSend.StartGame();

        Debug.Log("Starting Game...");
        ShowScores();
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        foreach(Player _player in PlayerManager.instance.players.Values)
        {
            Vector2 centerPos = (_player.team == Team.RedTeam) ? redTeamBase.position : blueTeamBase.position;
            PlayerController playerPrefab = (_player.team == Team.RedTeam) ? redTeamPlayerPrefab : blueTeamPlayerPrefab;

            Vector2 spawnPos = RandomCircle(centerPos, spawnRadius);
            PlayerController _controller = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            _controller.Initialize(_player.id);

            _player.controller = _controller;
        }
    }

    public void SetRoundWinner(Team _winnerTeam)
    {
        if(_winnerTeam == Team.RedTeam)
        {
            redTeamScore++;
            Debug.Log("Red team wins the round");
        } 
        else if(_winnerTeam == Team.BlueTeam)
        {
            blueTeamScore++;
            Debug.Log("Blue team wins the round");
        }
        ShowScores();

        if(IsGameWinner())
        {
            if(redTeamScore >= maxScore)
            {
                ServerSend.GameWinner(Team.RedTeam, redTeamScore, blueTeamScore);
                Debug.Log("Red team wins the game");
            } 
            else if(blueTeamScore >= maxScore)
            {
                ServerSend.GameWinner(Team.BlueTeam, redTeamScore, blueTeamScore);
                Debug.Log("Blue team wins the game");
            }
        }
        else
        {
            currentRound++;
            ServerSend.RoundWinner(_winnerTeam, currentRound, redTeamScore, blueTeamScore);
            StartNewRound();
        }
    }

    public bool IsGameWinner()
    {
        return redTeamScore >= maxScore || blueTeamScore >= maxScore;
    }

    public void StartNewRound()
    {
        foreach(Player _player in PlayerManager.instance.players.Values)
        {
            Vector2 centerPos = (_player.team == Team.RedTeam) ? redTeamBase.position : blueTeamBase.position;
            _player.controller.transform.position = RandomCircle(centerPos, spawnRadius);
        }
    }

    public void ShowScores()
    {
        Debug.Log("Red team score : " + redTeamScore + ", Blue Team Score : " + blueTeamScore);
    }

    private Vector2 RandomCircle(Vector2 center, float radius)
    {
        float angle = Random.value * 360;

        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        return pos;
    }
}