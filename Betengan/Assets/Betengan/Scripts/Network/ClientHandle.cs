using System;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void TcpTest(Packet _packet)
    {
        int _myId = _packet.ReadInt(); 
        Client.instance.myId = _myId;
        Debug.Log("TCP Connected");

        ClientSend.TCPTestReceived();
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UdpTest(Packet _packet)
    {
        Debug.Log("UDP Connected");
    }

    public static void Login(Packet _packet)
    {
        bool _success = _packet.ReadBool();

        if(_success) 
        {
            GameManager.instance.StartLobbySession();
        }
        else
        {
            string _message = _packet.ReadString();
            AuthManager.instance.statusText.text = _message;
        }
    }

    public static void SignUp(Packet _packet)
    {
        bool _success = _packet.ReadBool();
        string _message = _packet.ReadString();

        if(_success) AuthManager.instance.ResetInput();

        AuthManager.instance.statusText.text = _message;
    }

    public static void SendIntoLobby(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        string _username = _packet.ReadString();
        Team _team = (Team)_packet.ReadInt();

        if(!PlayerManager.instance.IsPlayerExist(_playerId))
        {
            PlayerManager.instance.NewPlayer(_playerId, _username, _team);
        }
        LobbyManager.instance.InstantiatePlayerUsername(_playerId, _username, _team);
    }

    public static void RoomMaster(Packet _packet)
    {
        int _playerId = _packet.ReadInt();

        LobbyManager.instance.SetRoomMaster(_playerId);
    }

    public static void ChangeTeam(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        Team _team = (Team)_packet.ReadInt();

        PlayerManager.instance.players[_playerId].team = _team;
        LobbyManager.instance.ChangeTeam(_playerId, _team);
    }

    public static void StartGame(Packet _packet)
    {
        int _maxScore = _packet.ReadInt();
        int _roundCountdownTime = _packet.ReadInt();

        GameManager.instance.StartGameSession(_maxScore, _roundCountdownTime);
    }

    public static void SpawnPlayers(Packet _packet)
    {
        int _playersLength = _packet.ReadInt();
        for(int i = 0; i < _playersLength; i++)
        {
            int _playerId = _packet.ReadInt();
            Vector2 _spawnPos = _packet.ReadVector2();

            GameSceneManager.instance.SpawnPlayer(_playerId, _spawnPos);
        }
    }

    public static void StartNewRound(Packet _packet)
    {
        int _currentRound = _packet.ReadInt();
        GameSceneManager.instance.StartNewRound(_currentRound);
    }

    public static void PlayerPosition(Packet _packet)
    {   
        int _playerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        if(PlayerManager.instance.IsPlayerExist(_playerId))
        {
            PlayerManager.instance.players[_playerId].controller.SetPosition(_position);
        }
    }

    public static void PlayerScaleX(Packet _packet)
    {   
        int _playerId = _packet.ReadInt();
        float _scaleX = _packet.ReadFloat();

        if(PlayerManager.instance.IsPlayerExist(_playerId))
        {
            PlayerManager.instance.players[_playerId].controller.SetScaleX(_scaleX);
        }
    }

    public static void PlayerAnimation(Packet _packet)
    {   
        int _playerId = _packet.ReadInt();
        string _name = _packet.ReadString();
        bool _value = _packet.ReadBool();

        if(PlayerManager.instance.IsPlayerExist(_playerId))
        {
            PlayerManager.instance.players[_playerId].controller.SetAnimation(_name, _value);
        }
    }

    public static void PlayerPower(Packet _packet)
    {   
        int _playerId = _packet.ReadInt();
        float _power = _packet.ReadFloat();

        if(PlayerManager.instance.IsPlayerExist(_playerId))
        {
            PlayerManager.instance.players[_playerId].controller.SetPower(_power);
        }
    }

    public static void PrisonGate(Packet _packet)
    {   
        Team _teamChest = (Team)_packet.ReadInt();
        float _openDuration = _packet.ReadFloat();

        GameSceneManager.instance.OpenPrisonGate(_teamChest, _openDuration);
    }
    
    public static void TeamScore(Packet _packet)
    {   
        Team _team = (Team)_packet.ReadInt();
        int _score = _packet.ReadInt();

        GameSceneManager.instance.SetScore(_team, _score);
    }

    public static void SetRoundWinner(Packet _packet)
    {   
        Team _winnerTeam = (Team)_packet.ReadInt();

        GameSceneManager.instance.SetRoundWinner(_winnerTeam);
    }

    public static void SetGameWinner(Packet _packet)
    {   
        Team _winnerTeam = (Team)_packet.ReadInt();

        GameSceneManager.instance.SetGameWinner(_winnerTeam);
    }

    public static void EndGame(Packet _packet)
    {   
        GameManager.instance.EndGameSession();
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _playerId = _packet.ReadInt();

        PlayerManager.instance.RemovePlayer(_playerId);
        if(GameManager.instance.isLobbySession)
        {
            LobbyManager.instance.RemovePlayerUsername(_playerId);
        }

        Debug.Log("player " + _playerId + " is disconnected");
    }
}
