using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void TCPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.tcpTestReceived))
        {
            _packet.Write(Client.instance.myId);
            
            SendTCPData(_packet);
        }
    }

    public static void LoginInput(string _username, string _password)
    {
        using (Packet _packet = new Packet((int)ClientPackets.loginInput))
        {
            _packet.Write(_username);
            _packet.Write(_password);

            SendTCPData(_packet);
        }
    }

    public static void SignUpInput(string _username, string _password)
    {
        using (Packet _packet = new Packet((int)ClientPackets.signUpInput))
        {
            _packet.Write(_username);
            _packet.Write(_password);

            SendTCPData(_packet);
        }
    }

    public static void LobbySceneLoaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobbySceneLoaded))
        {
            SendTCPData(_packet);
        }
    }

    public static void SelectTeam(Team _team)
    {
        using (Packet _packet = new Packet((int)ClientPackets.selectTeam))
        {
            _packet.Write((int)_team);

            SendTCPData(_packet);
        }
    }

    public static void RequestStartGame()
    {
        using (Packet _packet = new Packet((int)ClientPackets.requestStartGame))
        {
            SendTCPData(_packet);
        }
    }

    public static void GameSceneLoaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.gameSceneLoaded))
        {
            SendTCPData(_packet);
        }
    }

    public static void PlayerInput(Vector2 _movementInput)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerInput))
        {
            _packet.Write(_movementInput);

            SendTCPData(_packet);
        }
    }
    #endregion
}
