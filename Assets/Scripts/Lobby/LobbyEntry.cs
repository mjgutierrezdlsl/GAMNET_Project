using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _lobbyName, _playerCount;
    Lobby _lobby;

    public void Initialize(Lobby lobby)
    {
        _lobby = lobby;
        _lobbyName.text = $"{lobby.Name} ({lobby.Id})";
        _playerCount.text = $"{lobby.MaxPlayers - lobby.AvailableSlots}/{lobby.MaxPlayers}";
    }

    public async void JoinLobby()
    {
        if (_lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            Debug.LogWarning("Unable to join as we are already the host");
            return;
        }
        print($"Joining {_lobby.Id}");
        _lobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobby.Id);
        _playerCount.text = $"{_lobby.MaxPlayers - _lobby.AvailableSlots}/{_lobby.MaxPlayers}";
    }
}
