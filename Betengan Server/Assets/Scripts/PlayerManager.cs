using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Dictionary<int, Player> players;

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

    private void Start() {
        players = new Dictionary<int, Player>();
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
    }

    public void RemovePlayer(int _playerId)
    {
        players.Remove(_playerId);
    }

    public bool IsPlayerExist(int _playerId)
    {
        return players.ContainsKey(_playerId); 
    }

    public void SendIntoLobby(int _playerId)
    {
        // Send all players to the new player
        foreach (Player _player in players.Values)
        {
            if (_player.id != _playerId)
            {
                ServerSend.SendIntoLobby(_playerId, _player);
            }
        }

        // Send the new player to all players (including himself)
        foreach (Player _player in players.Values)
        {
            ServerSend.SendIntoLobby(_player.id, players[_playerId]);
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
}