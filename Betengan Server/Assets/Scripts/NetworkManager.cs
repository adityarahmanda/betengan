using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [Header("Server Settings")]
    public int maxPlayers = 12;
    public int port = 26950;

    [Header("Prefabs")]
    public GameObject playerPrefabs;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //Limit the application to reduce memory usage
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(maxPlayers, port);
    }

    private void OnApplicationQuit()
    {
        Server.CloseSocket();
    }

    public Player InstantiatePlayer(Vector3 _position, Quaternion _rotation)
    {
        return Instantiate(playerPrefabs, _position, _rotation).GetComponent<Player>();
    }
}
