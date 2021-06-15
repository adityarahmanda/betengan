using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public int maxTeamPlayers = 4;
    public string roomMasterLabel = "(RM)";

    [Header("UI References")]
    public Transform redTeamPlayersList;
    public Transform blueTeamPlayersList;

    public CustomButton redTeamSelectButton;
    public CustomButton blueTeamSelectButton;

    public Button startGameButton;
    public TextMeshProUGUI startGameStatusText;

    [Header("UI Prefabs")]
    public PlayerUsername usernameTextPrefabs;

    public Dictionary<int, PlayerUsername> playerUsernameTexts = new Dictionary<int, PlayerUsername>();

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
        redTeamSelectButton.button.onClick.AddListener(() => ClientSend.SelectTeam(Team.RedTeam));
        blueTeamSelectButton.button.onClick.AddListener(() => ClientSend.SelectTeam(Team.BlueTeam));

        startGameButton.onClick.AddListener(() => ClientSend.RequestStartGame());

        ClientSend.LobbySceneLoaded();
    }

    public void InstantiatePlayerUsername(int _playerId, string _username, Team _team)
    {
        PlayerUsername playerUsernameText = Instantiate(usernameTextPrefabs, GetPlayerTeamList(_team), false);
        playerUsernameText.SetUsernameText(_username);
        playerUsernameTexts.Add(_playerId, playerUsernameText);
        
        CheckSelectionButton();
    }

    public void RemovePlayerUsername(int _playerId)
    {
        Debug.Log("Removing Player Username");
        Destroy(playerUsernameTexts[_playerId].gameObject);
        playerUsernameTexts.Remove(_playerId);
    }

    public void ChangeTeam(int _playerId, Team _team)
    {
        playerUsernameTexts[_playerId].transform.SetParent(GetPlayerTeamList(_team), false);
        
        CheckSelectionButton();
    }

    public void CheckSelectionButton()
    {
        int _clientId = Client.instance.myId;

        if(PlayerManager.instance.IsPlayerExist(_clientId))
        {
            Player _localPlayer = PlayerManager.instance.GetPlayer(_clientId);
            SetSelectButton(_localPlayer.team, false, "Selected");
            
            if(_localPlayer.team == Team.RedTeam) {
                if(blueTeamPlayersList.childCount == maxTeamPlayers) {
                    SetSelectButton(Team.BlueTeam, false, "Full");
                } else {
                    SetSelectButton(Team.BlueTeam, true, "Select");
                }
            } else if(_localPlayer.team == Team.BlueTeam) {
                if(redTeamPlayersList.childCount == maxTeamPlayers) {
                    SetSelectButton(Team.RedTeam, false, "Full");
                } else {
                    SetSelectButton(Team.RedTeam, true, "Select");
                }
            }
        }
    }

    public void SetSelectButton(Team _team, bool _interactable, string _text)
    {
        if(_team == Team.RedTeam)
        {
            redTeamSelectButton.button.interactable = _interactable;
            redTeamSelectButton.text = _text;
        } else if(_team == Team.BlueTeam)
        {  
            blueTeamSelectButton.button.interactable = _interactable;
            blueTeamSelectButton.text = _text;
        }
    }

    private Transform GetPlayerTeamList(Team _team)
    {
        if(_team == Team.RedTeam)
            return redTeamPlayersList;
        else if(_team == Team.BlueTeam)
            return blueTeamPlayersList;
            
        return null;
    }

    public void SetRoomMaster(int _roomMasterId)
    {
        playerUsernameTexts[_roomMasterId].SetLabelText(roomMasterLabel);

        if(Client.instance.myId == _roomMasterId)
        {
            startGameStatusText.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(true);
        }
    }
}