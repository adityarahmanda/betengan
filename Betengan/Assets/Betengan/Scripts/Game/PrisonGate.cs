using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrisonGate : MonoBehaviour
{
    public Team teamOwner;

    private SpriteRenderer spriteRenderer;

    public PrisonGateHUD HUD;
    public ProgressBar progressBar;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() 
    {
        HUD.SetEnabled(false);
    }

    public void OpenPrisonGate(float _duration)
    {
        StartCoroutine(OpenGateForSeconds(_duration));
    }

    IEnumerator OpenGateForSeconds(float seconds)
    {
        spriteRenderer.enabled = false;
        HUD.SetEnabled(true);

        float waitSeconds = 0;
        while (waitSeconds <= seconds)
        {
            waitSeconds += Time.deltaTime;
            float progress = (waitSeconds / seconds) * 100;
            progressBar.SetFillAmount(progress);
            yield return null; 
        }
        
        spriteRenderer.enabled = true;
        HUD.SetEnabled(false);
    }
}
