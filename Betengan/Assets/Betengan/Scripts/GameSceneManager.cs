using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    public CameraController cameraController;

    [Header("Prison Gates")]
    public PrisonGate redTeamPrisonGate;
    public PrisonGate blueTeamPrisonGate;

    [Header("Player Prefabs")]
    public PlayerController redTeamPlayerPrefab;
    public PlayerController blueTeamPlayerPrefab;

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

    private void Start() {
        ClientSend.GameSceneLoaded();
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

    public void OpenPrisonGate(Team _teamChest, float _openDuration)
    {
        PrisonGate teamPrisonGate = (_teamChest == Team.RedTeam) ? redTeamPrisonGate : blueTeamPrisonGate;
        teamPrisonGate.OpenPrisonGate(_openDuration);
    }
}