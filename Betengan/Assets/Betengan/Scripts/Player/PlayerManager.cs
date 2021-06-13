using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Dictionary<int, Player> players;

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
        players = new Dictionary<int, Player>();
    }

    public void NewPlayer(int _playerId, string _username, Team _team)
    {
        Player _player = new Player(_playerId, _username, _team);
        players.Add(_playerId, _player);
    }

    public void RemovePlayer(int _playerId)
    {
        players.Remove(_playerId);
    }

     public bool IsPlayerExist(int _playerId)
    {
        return players.ContainsKey(_playerId); 
    }
}
