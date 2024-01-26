using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectionStatus : MonoBehaviourPunCallbacks
{
    public Text connectionStatusText;
    public Text roomStatusText;

    void Start()
    {
        // Set the initial connection and room status text
        UpdateConnectionStatusText();
        UpdateRoomStatusText();
    }

    void UpdateConnectionStatusText()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectionStatusText.text = "Connected to Photon";
        }
        else
        {
            connectionStatusText.text = "Not connected to Photon";
        }
    }

    void UpdateRoomStatusText()
    {
        if (PhotonNetwork.InRoom)
        {
            roomStatusText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}\nPlayers: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        else
        {
            roomStatusText.text = "Not in a room";
        }
    }

    public override void OnConnectedToMaster()
    {
        // Called when successfully connected to the Photon Cloud or your own Photon Server
        UpdateConnectionStatusText();
        UpdateRoomStatusText();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Called when disconnected from the Photon server
        UpdateConnectionStatusText();
        UpdateRoomStatusText();
    }

    public override void OnJoinedRoom()
    {
        // Called when successfully joined a room
        UpdateRoomStatusText();
    }

    public override void OnLeftRoom()
    {
        // Called when left a room
        UpdateRoomStatusText();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Called when a new player enters the room
        UpdateRoomStatusText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Called when a player leaves the room
        UpdateRoomStatusText();
    }
}
