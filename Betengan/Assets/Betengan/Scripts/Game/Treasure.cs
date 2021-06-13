using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Team teamOwner;
    public Sprite openTreasure;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(teamOwner == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            spriteRenderer.sprite = openTreasure;
            Debug.Log("Blue Team Wins");
        }

        if(teamOwner == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            spriteRenderer.sprite = openTreasure;
            Debug.Log("Red Team Wins");
        }
    }
}
