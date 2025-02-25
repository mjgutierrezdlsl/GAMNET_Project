using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;

public class ViewLobbyController : MonoBehaviour
{
    [SerializeField] Transform _contentParent;
    [SerializeField] LobbyEntry _lobbyEntryPrefab;
    private List<LobbyEntry> _lobbyEntries = new();

    private void OnEnable()
    {
        RefreshLobbyList();
    }

    private async void RefreshLobbyList()
    {
        // Clear existing entries
        ClearEntries();

        var lobbies = await LobbyService.Instance.QueryLobbiesAsync();
        foreach (var lobby in lobbies.Results)
        {
            var entry = Instantiate(_lobbyEntryPrefab, _contentParent);
            entry.Initialize(lobby.Name, lobby.AvailableSlots, lobby.MaxPlayers);
            _lobbyEntries.Add(entry);
        }
    }

    private void ClearEntries()
    {
        for (int i = _lobbyEntries.Count - 1; i >= 0; i--)
        {
            Destroy(_lobbyEntries[i].gameObject);
        }
        _lobbyEntries.Clear();
    }
}
