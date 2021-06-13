using System.Collections;
using System.Collections.Generic;

public class Player
{
    public int id;
    public string username;

    public Team team = Team.None;

    public Player(int _id, string _username, Team _team)
    {
        id = _id;
        username = _username;
        team = _team;
    }
}