using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    private List<PlayerController> _playerList = new();
    public PlayerController[] PlayerList => _playerList.ToArray();
    public void AddPlayer(PlayerController player)
    {
        _playerList.Add(player);
    }
    public void RemovePlayer(PlayerController player)
    {
        _playerList.Add(player);
    }
    public void SetImpostor()
    {
        var impostor = _playerList[Random.Range(0, _playerList.Count)];
        impostor.SetRole(PlayerRole.IMPOSTOR);
        foreach (var player in _playerList)
        {
            if (player != impostor)
            {
                player.SetRole(PlayerRole.CREWMATE);
            }
        }
        print($"Client {impostor.OwnerClientId} set as impostor");
    }
    private void Update()
    {
        if (!IsServer) { return; }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetImpostor();
        }
    }
}