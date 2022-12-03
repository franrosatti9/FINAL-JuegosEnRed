using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUI : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI pingText;
    public TextMeshProUGUI time;

    float _currentTime;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        if (!PhotonNetwork.OfflineMode) UpdateUI();
        _currentTime = 0;
    }
    private void Update()
    {
        UpdateTime();
        if(!PhotonNetwork.OfflineMode) UpdatePing();

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateUI();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;

        text.text = roomName;
        string maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        string countPlayer = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        text.text = roomName + ": " + countPlayer + "/" + maxPlayer;
    }
    void UpdatePing()
    {
        pingText.text = PhotonNetwork.GetPing().ToString() + "ms";
    }

    void UpdateTime()
    {
        _currentTime = gameManager.currentTime;
        var roundedTime = Mathf.RoundToInt(_currentTime);
        time.text = roundedTime.ToString() + " s";
    }
}
