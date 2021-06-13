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
        string _message = _packet.ReadString();
        AuthManager.instance.statusText.text = _message;

        if(_success) 
        {
            GameManager.instance.StartLobbySession();
            CanvasManager.instance.SwitchCanvas(CanvasType.Lobby);
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
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Team _team = (Team)_packet.ReadInt();

        PlayerManager.instance.NewPlayer(_id, _username, _team);
        LobbyManager.instance.InstantiatePlayerUsername(_id);
    }

    public static void ChangeTeam(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Team _team = (Team)_packet.ReadInt();

        PlayerManager.instance.players[_id].team = _team;
        LobbyManager.instance.UpdateTeamList(_id);
    }

    public static void StartGame(Packet _packet)
    {
        GameManager.instance.StartGame();
    }

    public static void PlayerPosition(Packet _packet)
    {   
        int _id = _packet.ReadInt();
        Vector2 _pos = _packet.ReadVector2();

        if(PlayerManager.instance.players.ContainsKey(_id))
        {
            //PlayerManager.instance.players[_id].SetPosition(_pos);
        }
    }

    public static void PlayerAnimation(Packet _packet)
    {   
        int _id = _packet.ReadInt();
        string _name = _packet.ReadString();
        bool _value = _packet.ReadBool();

        if(PlayerManager.instance.players.ContainsKey(_id))
        {
            //PlayerManager.instance.players[_id].SetAnimation(_name, _value);
        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        PlayerManager.instance.RemovePlayer(_id);
        if(GameManager.instance.isLobbySession)
        {
            LobbyManager.instance.RemovePlayerUsername(_id);
        }

        Debug.Log("player " + _id + " is disconnected");
    }
}
