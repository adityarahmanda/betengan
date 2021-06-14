using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    private PlayerController player;

    /*
    private void OnTriggerEnter2D(Collider2D other) {
        if(player.team == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            Player enemy = other.gameObject.GetComponent<Player>();
            if(player.power > enemy.power)
            {
                enemy.transform.position = GameManager.instance.redTeamPrison.position;
            }
        }

        if(player.team == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            Player enemy = other.gameObject.GetComponent<Player>();
            if(player.power > enemy.power)
            {
                enemy.transform.position = GameManager.instance.blueTeamPrison.position;
            } 
        }
    }
    */
}