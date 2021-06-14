using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public int roomMasterId = 0;
    public int maxTeamPlayers = 4;

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

    public void SetRoomMaster(int _playerId)
    {
        roomMasterId =_playerId;
        ServerSend.SendRoomMaster();
        
        Debug.Log("Set player with client id " + _playerId + " as room master");
    }

    public void ResetRoomMaster()
    {
        if(PlayerManager.instance.GetTotalPlayers() == 0)
        {
            roomMasterId = 0;
        }
        else
        {
            SetRoomMaster(PlayerManager.instance.GetFirstPlayerId());        
        }
    }

    public bool IsLobbyFull()
    {
        int totalPlayers = PlayerManager.instance.GetTotalPlayers();
        int maxPlayersInLobby = 2 * maxTeamPlayers;

        return totalPlayers == maxPlayersInLobby;
    }
}
