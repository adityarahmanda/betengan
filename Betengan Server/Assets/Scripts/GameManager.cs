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
    }

    public void StartGame()
    {
        StartGameSession();
        ServerSend.StartGame();

        Debug.Log("Starting Game...");
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        foreach(Player _player in PlayerManager.instance.players.Values)
        {
            Vector2 centerPos = (_player.team == Team.RedTeam) ? redTeamBase.position : blueTeamBase.position;
            PlayerController playerPrefab = (_player.team == Team.RedTeam) ? redTeamPlayerPrefab : blueTeamPlayerPrefab;

            Vector2 spawnPos = RandomCircle(centerPos, spawnRadius);
            PlayerController _controller = Instantiate(redTeamPlayerPrefab, spawnPos, Quaternion.identity);
            _controller.Initialize(_player.id);

            _player.controller = _controller;
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