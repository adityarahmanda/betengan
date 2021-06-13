using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    [Header("UI Reference")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI statusText;

    [Header("UI Buttons")]
    public Button loginButton;
    public Button signUpButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        statusText.text = "";

        loginButton.onClick.AddListener(Login);
        signUpButton.onClick.AddListener(SignUp);
    }

    public void Login()
    {
        if(usernameInput.text == "")
        {
            statusText.text = "Username must be filled";
            return;
        } 
        else if(passwordInput.text == "")
        {
            statusText.text = "Password must be filled";
            return;
        }

        ClientSend.LoginInput(usernameInput.text, passwordInput.text);
    }

    public void SignUp()
    {
        if(usernameInput.text == "")
        {
            statusText.text = "Username must be filled";
            return;
        } 
        else if(passwordInput.text == "")
        {
            statusText.text = "Password must be filled";
            return;
        }

        ClientSend.SignUpInput(usernameInput.text, passwordInput.text);
    }

    public void ResetInput()
    {
        usernameInput.text = "";
        passwordInput.text = "";
    }
}
