using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public float connectionTimeout = 10f;

    public TMP_InputField addressInput;
    public TMP_InputField portInput;
    public Button connectButton;

    public TextMeshProUGUI statusText;

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
        connectButton.onClick.AddListener(ConnectToServer);
        statusText.text = "";
    }

    public void ConnectToServer()
    {
        string address = null;
        int port;

        IPAddress ipAddress;
        if(!IPAddress.TryParse(addressInput.text, out ipAddress))
        {
            string addressFromURL = GetIPFromURL(addressInput.text);
            if(addressFromURL == null)
            {
                statusText.text = "Address is not valid";
                return;
            }

            address = addressFromURL;
        } else {
            address = addressInput.text;
        }

        if(!int.TryParse(portInput.text, out port))
        {
            statusText.text = "Port is not valid";
            return;
        }
          
        StartCoroutine(ConnectToServer(address, port));
    }

    private IEnumerator ConnectToServer(string _address, int _port)
    {
        connectButton.interactable = false;

        statusText.text = "Connecting..."; 
        Client.instance.ConnectToServer(_address, _port);
        
        float timer = connectionTimeout;
        while(Client.instance.myId == 0)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            } else {
                if(Client.instance.myId == 0)
                {
                    statusText.text = "Connection Timeout";    
                    connectButton.interactable = true; 
                    yield break;  
                }
            }
        }
        
        SceneManager.LoadScene("MainMenu");        
        
    }

    public string GetIPFromURL(string _url) 
    {
        _url = _url.Replace("http://", ""); 
        _url = _url.Replace("https://", ""); 
    
        IPHostEntry hosts = Dns.GetHostEntry(_url);
        if(hosts.AddressList.Length > 0) {
            return hosts.AddressList[0].ToString();
        } else {
            return null;
        }
    }
}
