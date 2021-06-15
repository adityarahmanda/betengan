using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void TcpTestReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();  

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and now client {_fromClient}");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player ID: {_fromClient} has assumed the wrong client ID ({_clientIdCheck})!");
        }
    }

    public static void LoginInput(int _fromClient, Packet _packet)
    {
        string _username = _packet.ReadString();
        string _password = _packet.ReadString();

        AuthManager.instance.LoginInput(_fromClient, _username, _password);
    }

    public static void SignUpInput(int _fromClient, Packet _packet)
    {
        string _username = _packet.ReadString();
        string _password = _packet.ReadString();

        AuthManager.instance.SignUpInput(_fromClient, _username, _password);
    }

    public static void SelectTeam(int _fromClient, Packet _packet)
    {
        Team _team = (Team)_packet.ReadInt();

        PlayerManager.instance.SelectTeam(_fromClient, _team);
    }

    public static void RequestStartGame(int _fromClient, Packet _packet)
    {
        GameManager.instance.StartGame();
    }

    public static void PlayerInput(int _fromClient, Packet _packet)
    {
        Vector2 inputs = _packet.ReadVector2(); 

        PlayerManager.instance.players[_fromClient].controller.MovementInput(inputs);
    }

    public static void GameSceneLoaded(int _fromClient, Packet _packet)
    {
        ServerSend.InitializeGame(_fromClient);
    }
}