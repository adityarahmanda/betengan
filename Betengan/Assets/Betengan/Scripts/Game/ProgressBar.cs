using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{ 
    private float initWidth;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start() 
    {
        initWidth = spriteRenderer.size.x;
    }

    public void SetFillAmount(float value)
    {
        spriteRenderer.size = new Vector2((value / 100) * initWidth, spriteRenderer.size.y);
    }
}