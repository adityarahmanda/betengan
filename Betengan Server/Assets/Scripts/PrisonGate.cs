using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrisonGate : MonoBehaviour
{
    public Team teamOwner = Team.None;
    public float openDuration = 3f;

    private Collider2D[] colliders;

    private void Awake() 
    {
        colliders = GetComponentsInChildren<Collider2D>();
    }

    private void OpenGate(bool _value)
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = !_value;
        }
    }

    IEnumerator OpenGateForSeconds(float seconds){
        OpenGate(true);
        yield return new WaitForSeconds(seconds);
        OpenGate(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(teamOwner == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            StartCoroutine(OpenGateForSeconds(openDuration));
            ServerSend.OpenPrisonGate(Team.RedTeam, openDuration);
        }

        if(teamOwner == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            StartCoroutine(OpenGateForSeconds(openDuration));
            ServerSend.OpenPrisonGate(Team.BlueTeam, openDuration);
        }
    }
}
