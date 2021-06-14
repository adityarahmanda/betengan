using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public int maxTeamPlayers = 4;

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

    public void NewPlayer(int _playerId, string _username)
    {
        int redTeamMembers = CountTeamMembers(Team.RedTeam);
        int blueTeamMembers = CountTeamMembers(Team.BlueTeam);
        
        Team _playerTeam = Team.None;
        if(redTeamMembers <= blueTeamMembers)
            _playerTeam = Team.RedTeam;
        else 
            _playerTeam = Team.BlueTeam;
        
        Player player = new Player(_playerId, _username, _playerTeam);
        players.Add(_playerId, player);

        Server.clients[_playerId].player = player;
    }

    public bool IsPlayerExist(int _playerId)
    {
        return players.ContainsKey(_playerId); 
    }

    public void SendIntoLobby(int _playerId)
    {
        foreach (Player _player in players.Values)
        {
            ServerSend.SendIntoLobby(_playerId, _player);
        }

        foreach (Player _player in players.Values)
        {
            if(_player.id != _playerId)
            { 
                ServerSend.SendIntoLobby(_player.id, players[_playerId]);
            }
        }

        if(LobbyManager.instance.roomMasterId == 0)
        {
            LobbyManager.instance.SetRoomMaster(_playerId);
        } else {
            ServerSend.SendRoomMaster();   
        }
    }

    public int CountTeamMembers(Team _team)
    {
        if(players.Count == 0) return 0;

        int count = 0;
        foreach(Player _player in players.Values) 
        {
            if(_player.team == _team) count++;
        }
        return count;
    }

    public void SelectTeam(int _playerId, Team _team)
    {
        if(CountTeamMembers(_team) < maxTeamPlayers)
        {
            players[_playerId].team = _team;
            ServerSend.ChangeTeam(_playerId, _team);
        }
    }

    public int GetTotalPlayers()
    {
        return players.Count;
    }

    public int GetFirstPlayerId()
    {
        if(players.Count != 0)
        {
            foreach (Player _player in players.Values)
            {
                return _player.id;
            }
        }
        return 0;
    }
}