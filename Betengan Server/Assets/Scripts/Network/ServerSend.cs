using System.Text;
using UnityEngine;

public class ServerSend
{
    #region SendDataMethods
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
            if(Server.clients[i].id != _exceptClient)
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
            if(Server.clients[i].id != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    private static void SendTCPDataToAllPlayers(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if(Server.clients[i].player != null)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAllPlayers(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if(Server.clients[i].player != null)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion

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

    public static void LoginFailed(int _id, string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.login))
        {
            _packet.Write(false);
            _packet.Write(_message);

            SendTCPData(_id, _packet);
        }
    }

    public static void LoginSuccess(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.login))
        {
            _packet.Write(true);
            
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

    public static void SendRoomMaster()
    {
        using (Packet _packet = new Packet((int)ServerPackets.roomMaster))
        {
            _packet.Write(LobbyManager.instance.roomMasterId);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void ChangeTeam(int _playerId, Team _team)
    {
        using (Packet _packet = new Packet((int)ServerPackets.changeTeam))
        {
            _packet.Write(_playerId);
            _packet.Write((int)_team);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void StartGame(int _maxScore, int _roundCountdownTime)
    {
        using (Packet _packet = new Packet((int)ServerPackets.startGame))
        {
            _packet.Write(_maxScore);
            _packet.Write(_roundCountdownTime);
            
            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void SpawnPlayers(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayers))
        {   
            _packet.Write(PlayerManager.instance.GetTotalPlayers());
            foreach(Player _player in PlayerManager.instance.players.Values)
            {
                _packet.Write(_player.id);
                _packet.Write(_player.controller.transform.position);
            }

            SendTCPData(_toClient, _packet);
        }
    }

    public static void StartNewRound(int _currentRound)
    {
        using (Packet _packet = new Packet((int)ServerPackets.startNewRound))
        {   
            _packet.Write(_currentRound);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void PlayerPosition(int _playerId, Vector2 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_playerId);
            _packet.Write(_position);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void PlayerScaleX(int _playerId, float scaleX)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerScaleX))
        {
            _packet.Write(_playerId);
            _packet.Write(scaleX);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void PlayerAnimation(int _playerId, string _name, bool _value)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerAnimation))
        {
            _packet.Write(_playerId);
            _packet.Write(_name);
            _packet.Write(_value);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void PlayerPower(int _playerId, float _power)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPower))
        {
            _packet.Write(_playerId);
            _packet.Write(_power);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void OpenPrisonGate(Team _teamChest, float _openDuration)
    {
        using (Packet _packet = new Packet((int)ServerPackets.prisonGate))
        {
            _packet.Write((int)_teamChest);
            _packet.Write(_openDuration);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void TeamScore(Team _team, int _score)
    {
        using (Packet _packet = new Packet((int)ServerPackets.teamScore))
        {
            _packet.Write((int)_team);
            _packet.Write(_score);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void SetRoundWinner(Team _winnerTeam)
    {
        using (Packet _packet = new Packet((int)ServerPackets.roundWinner))
        {
            _packet.Write((int)_winnerTeam);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void SetGameWinner(Team _winnerTeam)
    {
        using (Packet _packet = new Packet((int)ServerPackets.gameWinner))
        {
            _packet.Write((int)_winnerTeam);

            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void EndGame()
    {
        using (Packet _packet = new Packet((int)ServerPackets.endGame))
        {
            SendTCPDataToAllPlayers(_packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_playerId, _packet);
        }
    }
   #endregion
}
