using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other) {
        if(playerController.team == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            PlayerController enemyController = other.gameObject.GetComponent<PlayerController>();
            if(playerController.power > enemyController.power)
            {
                enemyController.transform.position = GameManager.instance.redTeamPrison.position;
            }
        }

        if(playerController.team == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            PlayerController enemyController = other.gameObject.GetComponent<PlayerController>();
            if(playerController.power > enemyController.power)
            {
                enemyController.transform.position = GameManager.instance.blueTeamPrison.position;
            } 
        }
    }
}