using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Button button;
    public TextMeshProUGUI status;
    public TMP_InputField roomInput;
    public TMP_InputField nicknameInput;
    
    private void Start()
    {
        roomInput.text = "room";
        nicknameInput.text = "nickname";

        if(!PhotonNetwork.IsConnected) PhotonNetwork.OfflineMode = true;
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.OfflineMode) return;
        button.interactable = false;
        PhotonNetwork.JoinLobby();
        status.text = "Connecting To Lobby";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Connection failed";
        PhotonNetwork.OfflineMode = true;
    }
    #region Lobby Callbacks
    public override void OnJoinedLobby()
    {
        button.interactable = true;
        status.text = "Connected to Lobby";
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "JOIN!";
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        status.text = "Lobby failed";
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "CONNECT!";
    }
    #endregion

    #region Room Callbacks
    public override void OnCreatedRoom()
    {
        status.text = "Created Room";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Created Room failed";
        button.interactable = true;
    }
    public override void OnJoinedRoom()
    {
        status.text = "Joined Room";
        if (PhotonNetwork.OfflineMode) PhotonNetwork.LoadLevel("GameplaySingleplayer");
        else PhotonNetwork.LoadLevel("Gameplay");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Joined Room failed";
        button.interactable = true;
    }
    #endregion

    public void Connect()
    {
        //PhotonNetwork.OfflineMode = false;
        if (string.IsNullOrEmpty(roomInput.text) || string.IsNullOrWhiteSpace(roomInput.text)) return;
        if (string.IsNullOrEmpty(nicknameInput.text) || string.IsNullOrWhiteSpace(nicknameInput.text)) return;

        PhotonNetwork.NickName = nicknameInput.text;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsOpen = true;
        options.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomInput.text, options, TypedLobby.Default);
        button.interactable = false;
    }

    public void PlayOffline()
    {
        if (string.IsNullOrEmpty(roomInput.text) || string.IsNullOrWhiteSpace(roomInput.text)) return;
        if (string.IsNullOrEmpty(nicknameInput.text) || string.IsNullOrWhiteSpace(nicknameInput.text)) return;

        PhotonNetwork.NickName = nicknameInput.text;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 1;
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, options, TypedLobby.Default);
    }

    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void DisconnectPhoton()
    {
        PhotonNetwork.Disconnect();
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.ConnectUsingSettings();
        button.interactable = false;
        status.text = "Connecting To Master";
    }

}
