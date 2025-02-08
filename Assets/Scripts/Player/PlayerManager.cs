using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerController _playerPrefab;
    [SerializeField] List<PlayerController> _allPlayers = new(), _impostors = new(), _crewmates = new();
    [SerializeField] int _impostorCount = 1;
    [SerializeField] int _playerCount = 4;
    [SerializeField] float _spawnRadius = 2f;

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
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        // Spawn all players
        for (int i = 0; i < _playerCount; i++)
        {
            var player = Instantiate(_playerPrefab, transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius, Quaternion.identity);
            _allPlayers.Add(player);

            // TODO: Remove when properly implementing multiplayer
            player.GetComponent<InputHandler>().enabled = false;

            player.name = $"Goblin_Torch [{i}]";

            // Subscribe to Player Corpse Found Event
            player.PlayerCorpseFound += OnPlayerCorpseFound;
        }

        // Sets the impostor
        if (_impostors.Count < _impostorCount)
        {
            var randomPlayer = _allPlayers[Random.Range(0, _allPlayers.Count)];
            randomPlayer.SetRole(PlayerRole.IMPOSTOR);
            _impostors.Add(randomPlayer);
            randomPlayer.GetComponent<InputHandler>().enabled = true;
        }

        // Lists the crewmates
        foreach (var player in _allPlayers)
        {
            if (player.Role != PlayerRole.CREWMATE) { continue; }
            player._detectRadius = 0f;
            _crewmates.Add(player);
        }
    }

    public void DespawnPlayers()
    {
        foreach (var player in _allPlayers)
        {
            player.PlayerCorpseFound -= OnPlayerCorpseFound;
        }
    }

    private void OnPlayerCorpseFound(PlayerController reporter, PlayerController reported)
    {
        print($"{reported.name} found dead by: {reporter.name}");
        Destroy(reported.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}