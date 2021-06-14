using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUsername : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI usernameText;

    public void SetUsernameText(string _value)
    {
        usernameText.text = _value;
    }

    public void SetLabelText(string _value)
    {
        labelText.text = _value;
    }
}
