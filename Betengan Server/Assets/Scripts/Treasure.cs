using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Team teamOwner;

    private void OnCollisionEnter2D(Collision2D other) {
        if(teamOwner == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            GameManager.instance.SetRoundWinner(Team.BlueTeam);
        }

        if(teamOwner == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            GameManager.instance.SetRoundWinner(Team.RedTeam);;
        }
    }
}
