using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _networkSystemLabel;
    private bool _isInitialized;
    private void Update()
    {
        if (_isInitialized) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            NetworkManager.Singleton.StartServer();
            _networkSystemLabel.text = $"Network System: Server";
            _isInitialized = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NetworkManager.Singleton.StartHost();
            _networkSystemLabel.text = $"Network System: Host";
            _isInitialized = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NetworkManager.Singleton.StartClient();
            _networkSystemLabel.text = $"Network System: Client";
            _isInitialized = true;
        }
    }
}
