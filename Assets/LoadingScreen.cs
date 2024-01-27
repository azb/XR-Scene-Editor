
using UnityEngine;
using Photon.Pun;

public class LoadingScreen : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject connectingScreen;

    void Start()
    {
        // Check if we are connected to Photon
        if (PhotonNetwork.IsConnected)
        {
            // Disable connecting screen if there is more than one player
            CheckPlayerCount();
        }
        else
        {
            //Debug.LogError("Not connected to Photon. Make sure to join a room first.");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Called when a new player enters the room
        CheckPlayerCount();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Called when a player leaves the room
        CheckPlayerCount();
    }

    private void CheckPlayerCount()
    {
        // Enable or disable the connecting screen based on player count
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            connectingScreen.SetActive(false);
        }
        else
        {
            connectingScreen.SetActive(true);
        }
    }
}