using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    public int currentRound;
    public CameraController cameraController;

    [Header("Prison Gates")]
    public PrisonGate redTeamPrisonGate;
    public PrisonGate blueTeamPrisonGate;

    [Header("Treasures")]
    public Treasure redTeamTreasure;
    public Treasure blueTeamTreasure;

    [Header("Team Score UI")]
    public ScoreUI redTeamScoreUI;
    public ScoreUI blueTeamScoreUI;

    [Header("Text UI")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI winnerText;

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

        redTeamScoreUI.Initialize(GameManager.instance.maxScore);
        blueTeamScoreUI.Initialize(GameManager.instance.maxScore);
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

    public void SetScore(Team _team, int _score)
    {
        if(_team == Team.RedTeam)
            redTeamScoreUI.SetScore(_score);
        else if(_team == Team.BlueTeam)
            blueTeamScoreUI.SetScore(_score);
    }

    public void OpenPrisonGate(Team _teamPrisonGate, float _openDuration)
    {
        if(_teamPrisonGate == Team.RedTeam)
            redTeamPrisonGate.OpenPrisonGate(_openDuration);
        if(_teamPrisonGate == Team.BlueTeam)
            blueTeamPrisonGate.OpenPrisonGate(_openDuration);
    }

    public void SetRoundWinner(Team _winnerTeam)
    {
        GameManager.instance.paused = true;
        
        if(_winnerTeam == Team.RedTeam)
        {
            winnerText.text = "Red Team Stealed The Treasure";
            blueTeamTreasure.OpenTreasure();
        } 
        else if(_winnerTeam == Team.BlueTeam) 
        {
            winnerText.text = "Blue Team Stealed The Treasure";
            redTeamTreasure.OpenTreasure();
        }
        winnerText.gameObject.SetActive(true);
    }

    public void SetGameWinner(Team _winnerTeam)
    {
        GameManager.instance.paused = true;

        if(_winnerTeam == Team.RedTeam)
        {
            winnerText.text = "Red Team Wins!";
            blueTeamTreasure.OpenTreasure();
        } 
        else if(_winnerTeam == Team.BlueTeam) 
        {
            winnerText.text = "Blue Team Wins!";
            redTeamTreasure.OpenTreasure();
        }
        winnerText.gameObject.SetActive(true);
    }

    public void StartNewRound(int _currentRound)
    {
        currentRound = _currentRound;
        
        //Reset UI
        winnerText.gameObject.SetActive(false);
        
        //Reset environment
        redTeamTreasure.CloseTreasure();
        blueTeamTreasure.CloseTreasure();

        StartCoroutine(StartNewRound());
    }

    public IEnumerator StartNewRound()
    {
        GameManager.instance.paused = true;
        int countdownTime = GameManager.instance.roundCountdownTime;

        roundText.text = "Round " + currentRound;
        countdownText.text = "Start after " + countdownTime + " second(s)";

        roundText.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

        while(countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime--;
            countdownText.text = "Start after " + countdownTime + " second(s)";
        }
        
        roundText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        
        GameManager.instance.paused = false;
    }
}