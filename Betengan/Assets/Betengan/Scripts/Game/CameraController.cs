using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Boundaries")]
    public float leftBorder = 14.7f;
    public float rightBorder = 14.7f;
    public float topBorder = 1f;
    public float bottomBorder = 1f;

    [Header("Camera Follow Settings")]
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 1f;    
    public Transform followTarget;

    // Update is called once per frame
    void Update()
    {
        if(followTarget != null)
        {
            Vector3 nextPos =  new Vector3(Mathf.Clamp(followTarget.position.x, -leftBorder, rightBorder),
                                          Mathf.Clamp(followTarget.position.y, -bottomBorder, topBorder),
                                          -10f); 
            transform.position = Vector3.Slerp(transform.position, nextPos, SmoothFactor);
        }
    }

    public void Follow(Transform _target)
    {
        followTarget = _target;
    }
}
