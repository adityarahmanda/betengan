using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public CanvasType type;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void EnabledCanvas(bool _value)
    {
        canvas.enabled = _value;
    }
}