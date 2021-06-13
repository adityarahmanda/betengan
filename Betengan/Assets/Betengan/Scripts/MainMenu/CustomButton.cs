using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI textMesh;
    
    public string text
    {
        get { return textMesh.text; }
        set { textMesh.text = value; }
    }
}
