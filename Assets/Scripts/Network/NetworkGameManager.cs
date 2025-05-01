using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{
    [SerializeField] bool _useRelay;
    [SerializeField] TextMeshProUGUI _networkSystemLabel;

    public void StartHost(int maxConnections = 2)
    {
        StartHostWithRelay();
    }
    public async void StartHostWithRelay(int maxConnections = 2, string connectionType = "udp")
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        NetworkManager.Singleton.StartHost();
        _networkSystemLabel.text = $"Network System: Host\nJoin Code: {joinCode}";
    }

    public void JoinCode(TMP_InputField joinCodeInputField)
    {
        if (string.IsNullOrEmpty(joinCodeInputField.text)) { return; }
        StartClientWithRelay(joinCodeInputField.text);
    }

    private async void StartClientWithRelay(string joinCode, string connectionType = "udp")
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        NetworkManager.Singleton.StartClient();
        _networkSystemLabel.text = $"Network System: Client\nJoin Code: {joinCode}";
    }
}
