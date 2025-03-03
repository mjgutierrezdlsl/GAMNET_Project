using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

public class AuthManager : Singleton<AuthManager>
{
    [SerializeField] private bool _autoSignIn;
    [SerializeField] private GameObject _authCanvas;
    [SerializeField] private TextMeshProUGUI _errorLogDisplay;
    [SerializeField] private UnityEvent _onSignIn;

    public string PlayerName { get; private set; }

    private async void Start()
    {
        // Using the Lobby service requires the player
        // to be authenticated using Unity Authentication
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.SessionTokenExists && _autoSignIn)
        {
            try
            {
                SetupAuthenticationEvents();

                // Signs in an anonymous player
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                _onSignIn?.Invoke();
            }
            catch (RequestFailedException err)
            {
                Debug.LogError(err);
                _errorLogDisplay.text = err.Message;
                _errorLogDisplay.gameObject.SetActive(true);
            }
        }
        else
        {
            _authCanvas.SetActive(true);
        }
    }

    public async void ConnectToServer(TMP_InputField nameInput)
    {
        try
        {
            SetupAuthenticationEvents();

            // Signs in an anonymous player
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            SetPlayerName(nameInput.text);
        }
        catch (RequestFailedException err)
        {
            Debug.LogError(err);
            _errorLogDisplay.text = err.Message;
            _errorLogDisplay.gameObject.SetActive(true);
        }
    }

    private void SetupAuthenticationEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
            _errorLogDisplay.text = err.Message;
            _errorLogDisplay.gameObject.SetActive(true);
        };

        AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    public async void SetPlayerName(string nameInput)
    {
        try
        {
            PlayerName = await AuthenticationService.Instance.UpdatePlayerNameAsync(nameInput);
            print($"Player {AuthenticationService.Instance.PlayerId}'s name updated to {PlayerName}");
            _errorLogDisplay.gameObject.SetActive(false);
            _onSignIn?.Invoke();
        }
        catch (RequestFailedException err)
        {
            Debug.LogError(err);
            _errorLogDisplay.text = err.Message;
            _errorLogDisplay.gameObject.SetActive(true);
        }
    }
}