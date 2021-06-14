using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Team teamOwner;

    private void OnCollisionEnter2D(Collision2D other) {
        if(teamOwner == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            ServerSend.SetWinner(Team.BlueTeam);
            Debug.Log("Blue Team Wins");
        }

        if(teamOwner == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            ServerSend.SetWinner(Team.RedTeam);
            Debug.Log("Red Team Wins");
        }
    }
}
