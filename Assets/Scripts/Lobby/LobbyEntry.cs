using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _lobbyName, _playerCount;
    public void Initialize(string lobbyName, int activeCount, int maxCount)
    {
        _lobbyName.text = lobbyName;
        _playerCount.text = $"{activeCount}/{maxCount}";
    }
    public void UpdatePlayerCount(int activeCount, int maxCount)
    {
        _playerCount.text = $"{activeCount}/{maxCount}";
    }
}
