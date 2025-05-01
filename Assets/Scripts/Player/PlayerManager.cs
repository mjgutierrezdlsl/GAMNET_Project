using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    [SerializeField] PlayerController _playerPrefab;
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

    private void SpawnPlayer(ulong id)
    {
        var player = Instantiate(_playerPrefab, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
    }

    private void Update()
    {
        if (!IsServer) { return; }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     foreach (var id in NetworkManager.ConnectedClientsIds)
        //     {
        //         SpawnPlayer(id);
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetImpostor();
        }
    }
}