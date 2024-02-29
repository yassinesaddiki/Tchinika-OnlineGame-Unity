using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverBtn;
    [SerializeField] private Button _clientBtn;
    [SerializeField] private Button _hostBtn;

    private void Awake()
    {
        _serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });

        _clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        _hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }
}
