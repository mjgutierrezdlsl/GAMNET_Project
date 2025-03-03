using System;
using System.Collections;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private UnityEvent _onCreateLobby;
    [SerializeField] private UnityEvent _onJoinLobby;

    private Coroutine _heartbeatCoroutine;
    private Lobby _joinedLobby;

    public Lobby JoinedLobby
    {
        get => _joinedLobby;
        private set
        {
            _joinedLobby = value;
            _onJoinLobby?.Invoke();
        }
    }

    private void OnEnable()
    {
        _onJoinLobby.AddListener(OnJoinLobby);
    }

    private void OnJoinLobby()
    {
        LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId, new()
        {
            Data = new()
            {
                {
                    "playerName",
                    new(
                        PlayerDataObject.VisibilityOptions.Member,
                        value: AuthManager.Instance.PlayerName
                    )
                }
            }
        });
        StartCoroutine(PollLobby(JoinedLobby.Id));
    }

    private IEnumerator PollLobby(string lobbyId)
    {
        var delay = new WaitForSecondsRealtime(1.1f);
        while (true)
        {
            UpdateLobby(lobbyId);
            yield return delay;
        }
    }

    private async void UpdateLobby(string lobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
            var names = "Names:\n";

            foreach (var player in lobby.Players)
            {
                names += $"{player.Data["playerName"].Value}\n";
            }

            print(names);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnDisable()
    {
        _onJoinLobby.RemoveListener(OnJoinLobby);
    }

    public async void CreateLobby(string lobbyName, int maxPlayers, CreateLobbyOptions options)
    {
        try
        {
            JoinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            print($"Lobby created: {JoinedLobby.LobbyCode} | {JoinedLobby.Id}");

            // Start Lobby Heartbeat to prevent inactivity
            _heartbeatCoroutine = StartCoroutine(HeartbeatRoutine(JoinedLobby.Id, 15f));

            _onCreateLobby?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            print($"Joining {lobbyCode}");
            JoinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyCode);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void JoinLobbyById(string lobbyId)
    {
        try
        {
            print($"Joining {lobbyId}");
            JoinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private IEnumerator HeartbeatRoutine(string id, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(id);
            yield return delay;
        }
    }

    private async void OnApplicationQuit()
    {
        if (AuthenticationService.Instance.PlayerId == null)
        {
            return;
        }

        if (_heartbeatCoroutine != null)
        {
            // Stop heartbeat coroutine
            StopCoroutine(_heartbeatCoroutine);
        }

        // Ensures that the lobbies are exited once the player closes the game
        var lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
        foreach (var lobby in lobbies)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(lobby, AuthenticationService.Instance.PlayerId);
                print($"{lobby} exited");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}