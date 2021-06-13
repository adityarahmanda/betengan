﻿using System.Text;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void TCPTest(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.tcpTest))
        {
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void UDPTest(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.udpTest))
        {
            SendUDPData(_toClient, _packet);
        }
    }

    public static void Login(int _id, bool _value, string _message, string _username = null)
    {
        using (Packet _packet = new Packet((int)ServerPackets.login))
        {
            _packet.Write(_value);
            _packet.Write(_message);

            if(_value) _packet.Write(_username);

            SendTCPData(_id, _packet);
        }
    }

    public static void SignUp(int _id, bool _value, string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.signUp))
        {
            _packet.Write(_value);
            _packet.Write(_message);

            SendTCPData(_id, _packet);
        }
    }

    public static void SendIntoLobby(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.sendIntoLobby))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write((int)_player.team);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void ChangeTeam(int _playerId, Team _team)
    {
        using (Packet _packet = new Packet((int)ServerPackets.changeTeam))
        {
            _packet.Write(_playerId);
            _packet.Write((int)_team);

            SendTCPDataToAll(_packet);
        }
    }

    public static void StartGame()
    {
        using (Packet _packet = new Packet((int)ServerPackets.startGame))
        {
            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_id);

            SendTCPDataToAll(_id, _packet);
        }
    }
   #endregion
}