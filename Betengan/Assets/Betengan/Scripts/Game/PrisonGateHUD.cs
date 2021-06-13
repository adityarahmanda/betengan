using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrisonGateHUD : MonoBehaviour
{   
    private TextMeshPro statusText;
    private SpriteRenderer[] spriteRenderers;

    private void Awake() 
    {
        statusText = GetComponentInChildren<TextMeshPro>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetEnabled(bool value)
    {
        statusText.enabled = value;
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = value;
        }
    }
}