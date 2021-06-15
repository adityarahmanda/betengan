using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Sprite openedTreasure;
    public Sprite closedTreasure;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenTreasure() {
        spriteRenderer.sprite = openedTreasure;
    }
    
    public void CloseTreasure() {
        spriteRenderer.sprite = closedTreasure;
    }
}
