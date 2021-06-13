using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private Vector3 offset;
    private Transform playerTarget;

    void Start()
    {
        offset = transform.localPosition;

        if(transform.parent != null)
        {
            playerTarget = transform.parent;
            transform.parent = null;
        }
    }

    void Update()
    {
        if(playerTarget != null)
        {
            transform.position = playerTarget.position + offset; 
        }
    }
}
