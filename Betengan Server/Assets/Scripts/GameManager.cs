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
    public int winPauseTime = 3;
    public int roundCountdownTime = 5;
    public int maxScore = 2;
    
    [Header("Game Status")]
    public int currentRound = 1;
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
        Debug.Log("Starting Game...");

        isLobbySession = false;
        isGameSession = true;

        currentRound = 1;
        redTeamScore = 0;
        blueTeamScore = 0;    

        ShowScores();
        SpawnPlayer();

        ServerSend.StartGame(maxScore, roundCountdownTime);
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
    
    public void ShowScores()
    {
        Debug.Log("Red team score : " + redTeamScore + ", Blue Team Score : " + blueTeamScore);
    }

    public void SetWinner(Team _winnerTeam)
    {
        if(_winnerTeam == Team.RedTeam)
        {
            redTeamScore++;
            ServerSend.TeamScore(Team.RedTeam, redTeamScore);
            Debug.Log("Red team wins the round");
        } 
        else if(_winnerTeam == Team.BlueTeam)
        {
            blueTeamScore++;
            ServerSend.TeamScore(Team.BlueTeam, blueTeamScore);
            Debug.Log("Blue team wins the round");
        }
        ShowScores();

        if(IsGameWinner())
        {
            if(redTeamScore >= maxScore)
            {
                ServerSend.SetGameWinner(Team.RedTeam);
                Debug.Log("Red team wins the game");
            } 
            else if(blueTeamScore >= maxScore)
            {
                ServerSend.SetGameWinner(Team.BlueTeam);
                Debug.Log("Blue team wins the game");
            }

            StartCoroutine(EndGameAfterTimeout(winPauseTime));
        }
        else
        {
            ServerSend.SetRoundWinner(_winnerTeam);
            StartCoroutine(StartNewRoundAfterTimeout(winPauseTime));
        }
    }

    public bool IsGameWinner()
    {
        return redTeamScore >= maxScore || blueTeamScore >= maxScore;
    }

    public void StartNewRound()
    {
        ResetPlayer();
     
        currentRound++;
        ServerSend.StartNewRound(currentRound);
    }

    public IEnumerator StartNewRoundAfterTimeout(float _timeout)
    {
        yield return new WaitForSeconds(_timeout);
        StartNewRound();
    }

    public void EndGame()
    {
        PlayerManager.instance.DestroyAllPlayerControllers();
        StartLobbySession();
        ServerSend.EndGame();   
    }

    public IEnumerator EndGameAfterTimeout(float _timeout)
    {
        yield return new WaitForSeconds(_timeout);
        EndGame();
    }

    public void ResetPlayer()
    {
        foreach(Player _player in PlayerManager.instance.players.Values)
        {
            //reset position
            Vector2 centerPos = (_player.team == Team.RedTeam) ? redTeamBase.position : blueTeamBase.position;
            _player.controller.transform.position = RandomCircle(centerPos, spawnRadius);

            //reset power
            _player.controller.power = 100f;
        }
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