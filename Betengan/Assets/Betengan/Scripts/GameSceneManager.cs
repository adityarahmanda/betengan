using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    public CameraController cameraController;

    [Header("Game Settings")]
    public int currentRound = 0;
    public int maxScore;
    public int redTeamScore = 0;
    public int blueTeamScore = 0;

    [Header("Prison Gates")]
    public PrisonGate redTeamPrisonGate;
    public PrisonGate blueTeamPrisonGate;

    [Header("Team Scores UI")]
    public ScoreUI redTeamScoreUI;
    public ScoreUI blueTeamScoreUI;

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
        ClientSend.GameSceneLoaded();
    }

    public void SetMaxScore(int _maxScore)
    {
        maxScore = _maxScore;

        redTeamScoreUI.Initialize(maxScore);
        blueTeamScoreUI.Initialize(maxScore);
    }

    public void SpawnPlayer(int _playerId, Vector2 _spawnPos)
    {
        Player _player = PlayerManager.instance.GetPlayer(_playerId);
        PlayerController playerPrefab = (_player.team == Team.RedTeam) ? redTeamPlayerPrefab : blueTeamPlayerPrefab;

        PlayerController _controller = Instantiate(playerPrefab, _spawnPos, Quaternion.identity);
        _controller.Initialize(_player.id, _player.username);

        if(_player.id == Client.instance.myId)
        {
            cameraController.Follow(_controller.transform);
        }
        
        _player.controller = _controller;
    }

    public void RoundWinner(Team _winnerTeam, int _currentRound, int _redTeamScore, int _blueTeamScore)
    {
        if(_winnerTeam == Team.RedTeam)
        {
            Debug.Log("Red team wins the round");
        } 
        else if(_winnerTeam == Team.BlueTeam)
        {
            Debug.Log("Blue team wins the round");
        }

        currentRound = _currentRound;
        SetScores(_redTeamScore, _blueTeamScore);
    }

    public void GameWinner(Team _winnerTeam, int _redTeamScore, int _blueTeamScore)
    {   
        if(_winnerTeam == Team.RedTeam)
        {
            Debug.Log("Red team wins the game");
        } 
        else if(_winnerTeam == Team.BlueTeam)
        {
            Debug.Log("Blue team wins the game");
        }

        SetScores(_redTeamScore, _blueTeamScore);
    }

    public void SetScores(int _redTeamScore, int _blueTeamScore)
    {
        if(redTeamScore != _redTeamScore)
        {
            redTeamScoreUI.SetScore(_redTeamScore);
            redTeamScore = _redTeamScore;
        }

        if(blueTeamScore != _blueTeamScore)
        {
            blueTeamScoreUI.SetScore(_blueTeamScore);
            blueTeamScore = _blueTeamScore;
        }
    }

    public void OpenPrisonGate(Team _teamChest, float _openDuration)
    {
        PrisonGate teamPrisonGate = (_teamChest == Team.RedTeam) ? redTeamPrisonGate : blueTeamPrisonGate;
        teamPrisonGate.OpenPrisonGate(_openDuration);
    }
}