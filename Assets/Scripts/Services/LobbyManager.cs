using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.Events;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCreateLobby;
    public async void CreateLobby(string lobbyName, int maxPlayers, CreateLobbyOptions options)
    {
        try
        {
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            print($"Lobby created: {lobby.LobbyCode} | {lobby.Id}");

            // Start Lobby Heartbeat to prevent inactivity
            StartCoroutine(HeartbeatRoutine(lobby.Id, 15f));

            _onCreateLobby?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
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

    private void OnApplicationQuit()
    {
        // Ensures that the lobbies are deleted once the player closes the game
        // var lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
        // foreach (var lobby in lobbies)
        // {
        //     await LobbyService.Instance.DeleteLobbyAsync(lobby);
        //     print($"{lobby} deleted");
        // }
        StopAllCoroutines();
    }
}
