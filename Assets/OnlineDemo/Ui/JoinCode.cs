using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using IngameDebugConsole;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class JoinCode : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    [ConsoleMethod("CreateRealy", "Creates a lobby")]
    public static async void CreateRealy()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Join code: {joinCode}");

            RelayServerData relayServerData = new RelayServerData(allocation,"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData) ;

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    [ConsoleMethod("JoinRelay", "Join a lobby")]
    public static async void JoinAllocation(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with join code: " + joinCode);
            JoinAllocation jA = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(jA,"dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData) ;

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }
}