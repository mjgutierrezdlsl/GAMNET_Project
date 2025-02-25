using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyController : MonoBehaviour
{
    [SerializeField] private LobbyManager _lobbyManager;

    [Header("Settings")]
    [SerializeField] private int _playerLimit = 4;

    [Header("UI")]
    [SerializeField] private TMP_InputField _lobbyNameInputField;
    [SerializeField] private TextMeshProUGUI _maxPlayerCountLabel;
    [SerializeField] private Toggle _isPrivateToggle;

    private int maxPlayers;

    public string LobbyName { get; set; }
    public int MaxPlayers
    {
        get => maxPlayers;
        set
        {
            if (value > _playerLimit)
            {
                value = 1;
            }
            else if (value < 1)
            {
                value = _playerLimit;
            }
            maxPlayers = value;
            _maxPlayerCountLabel.text = maxPlayers.ToString();
        }
    }
    public bool IsPrivate { get; set; }

    private void OnEnable()
    {
        // Ensures that the MaxPlayers is always set to 1
        MaxPlayers = 1;
    }

    public void IncrementMaxPlayers()
    {
        MaxPlayers++;
    }
    public void DecrementMaxPlayers()
    {
        MaxPlayers--;
    }

    public void CreateLobby()
    {
        if (string.IsNullOrEmpty(LobbyName))
        {
            Debug.LogWarning("Lobby must have a name.");
            return;
        }
        print($"LobbyName: {LobbyName}, MaxPlayers: {MaxPlayers}, IsPrivate: {IsPrivate}");
        _lobbyManager.CreateLobby(LobbyName, MaxPlayers, new()
        {
            IsPrivate = IsPrivate,
        });
        CloseWindow();
    }

    private void OnDisable()
    {
        _lobbyNameInputField.text = string.Empty;
        _isPrivateToggle.isOn = false;

        LobbyName = string.Empty;
        MaxPlayers = 1;
        IsPrivate = false;
    }
    public void CloseWindow()
    {
        gameObject.SetActive(false);

    }
}
