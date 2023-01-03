using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JoinToRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _nameRoom;
   
    private void Start()
    {
        ConnectToServer();
    }
    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect to Sever...");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server");
        base.OnConnectedToMaster();
        RoomOptions roomOptinons = new RoomOptions();
        roomOptinons.MaxPlayers = 8;
        roomOptinons.IsVisible = true;
        roomOptinons.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(_nameRoom, roomOptinons, TypedLobby.Default);
    }
   
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined to room");
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Game");
    }
   
}
