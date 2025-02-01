using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerController _playerPrefab;
    [SerializeField] List<PlayerController> _impostors, _crewmates;
    [SerializeField] int _impostorCount = 1;
    [SerializeField] int _playerCount = 4;
    private void Start()
    {
        // Instantiate the players
        // check impostor count
        // impostorChange = Random.Range(0,1)
        // Check if probable impostor (impostorChange > 0.5)
        // Set PlayerRole
        // _crewmates.Add(player)
        // _impostor.Add(player)
        #region Psuedocode
        // for (int i = 0; i < _playerCount; i++)
        // {
        //     var player = Instantiate(_playerPrefab);

        //     if (_impostors.Count != _impostorCount)
        //     {
        //         var impostorChance = Random.Range(0, 1);
        //         player.SetRole(impostorChance > 0.5f ? PlayerRole.IMPOSTOR : PlayerRole.CREWMATE);
        //     }
        //     else
        //     {
        //         player.SetRole(PlayerRole.CREWMATE);
        //     }

        //     if (player.Role == PlayerRole.CREWMATE)
        //     {
        //         _crewmates.Add(player);
        //     }
        //     else
        //     {
        //         _impostors.Add(player);
        //     }
        // }

        // for (int i = 0; i < _impostorCount; i++)
        // {

        //     var player = _crewmates[Random.Range(0, _crewmates.Count)];
        //     player.SetRole(PlayerRole.IMPOSTOR);
        //     _crewmates.Remove(player);
        //     _impostors.Add(player);
        // }
        #endregion
    }
}