using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _lobbyName, _playerCount;
    private Lobby _lobby;

    public void Initialize(Lobby lobby)
    {
        _lobby = lobby;
        _lobbyName.text = $"{_lobby.Name}";
        _playerCount.text = $"{_lobby.MaxPlayers - _lobby.AvailableSlots}/{_lobby.MaxPlayers}";
    }

    public void JoinLobby()
    {
        LobbyManager.Instance.JoinLobbyById(_lobby.Id);
    }
}