using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrisonGate : MonoBehaviour
{
    public Team teamOwner;
    public float hideDuration;

    private Collider2D[] colliders;
    private SpriteRenderer spriteRenderer;

    public PrisonGateHUD HUD;
    public ProgressBar progressBar;

    private void Awake() 
    {
        colliders = GetComponentsInChildren<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        HUD.SetEnabled(false);
    }

    private void ShowGate(bool value)
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = value;
        }
        spriteRenderer.enabled = value;
    }

    IEnumerator OpenGateForSeconds(float seconds){
        ShowGate(false);
        HUD.SetEnabled(true);

        float waitSeconds = 0;
        while (waitSeconds <= seconds)
        {
            waitSeconds += Time.deltaTime;
            float progress = (waitSeconds / seconds) * 100;
            progressBar.SetFillAmount(progress);
            yield return null; 
        }

        ShowGate(true);
        HUD.SetEnabled(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(teamOwner == Team.RedTeam && other.gameObject.tag == "Blue Team")
        {
            StartCoroutine(OpenGateForSeconds(hideDuration));
        }

        if(teamOwner == Team.BlueTeam && other.gameObject.tag == "Red Team")
        {
            StartCoroutine(OpenGateForSeconds(hideDuration));
        }
    }
}
