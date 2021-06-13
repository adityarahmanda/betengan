using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [Header("UI References")]
    public Transform redTeamPlayersList;
    public Transform blueTeamPlayersList;

    public CustomButton redTeamSelectButton;
    public CustomButton blueTeamSelectButton;

    public Button startGameButton;

    [Header("UI Prefabs")]
    public TextMeshProUGUI usernameTextPrefabs;

    public Dictionary<int, TextMeshProUGUI> playerUsernameTexts;

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
        playerUsernameTexts = new Dictionary<int, TextMeshProUGUI>();

        redTeamSelectButton.button.onClick.AddListener(() => ClientSend.SelectTeam(Team.RedTeam));
        blueTeamSelectButton.button.onClick.AddListener(() => ClientSend.SelectTeam(Team.BlueTeam));

        startGameButton.onClick.AddListener(() => ClientSend.RequestStartGame());
    }

    public void InstantiatePlayerUsername(int _playerId)
    {
        Player _player = PlayerManager.instance.players[_playerId];
        
        TextMeshProUGUI playerUsernameText = Instantiate(usernameTextPrefabs, GetPlayerTeamList(_player.team), true);
        playerUsernameText.text = _player.username;

        playerUsernameTexts.Add(_player.id, playerUsernameText);

        if(Client.instance.myId == _playerId)
        {
            DisableTeamSelectButton(_player.team);
        }
    }

    public void RemovePlayerUsername(int _playerId)
    {
        Debug.Log("Removing Player Username");
        Destroy(playerUsernameTexts[_playerId].gameObject);
        playerUsernameTexts.Remove(_playerId);
    }

    public void UpdateTeamList(int _playerId)
    {
        Player _player = PlayerManager.instance.players[_playerId];

        playerUsernameTexts[_player.id].transform.SetParent(GetPlayerTeamList(_player.team));

        if(Client.instance.myId == _playerId)
        {
            DisableTeamSelectButton(_player.team);
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

    private void DisableTeamSelectButton(Team _team)
    {
        if(_team == Team.RedTeam)
        {
            redTeamSelectButton.button.interactable = false;
            redTeamSelectButton.text = "Selected";

            blueTeamSelectButton.button.interactable = true;
            blueTeamSelectButton.text = "Select";
        } 
        else if(_team == Team.BlueTeam)
        {
            redTeamSelectButton.button.interactable = true;
            redTeamSelectButton.text = "Select";

            blueTeamSelectButton.button.interactable = false;
            blueTeamSelectButton.text = "Selected";
        }
    }
}