using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Photon.Voice.PUN;
public class GameManager : MonoBehaviourPunCallbacks
{
    public bool isStarted = false;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public float currentTime;
    [SerializeField] float secondsToStart = 2f;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject winScreenTimeText;
    [SerializeField] VictoryZone victoryZone;
    [SerializeField] GameObject loseScreen;

    public Action onPlayersJoined = delegate { };
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(!isStarted) CheckStartGame();
            PhotonNetwork.Instantiate("VoiceObject", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("VoiceObject", Vector3.zero, Quaternion.identity);
        }

        currentTime = 0f;
    }

    void Update()
    {
        if (isStarted)
        {
            currentTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isStarted)  CheckStartGame();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Restart();
        }
    }

    void CheckStartGame()
    {
        if (isStarted) return;
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            //isStarted = true;
            photonView.RPC("UpdateStart", RpcTarget.All, true);
            StartCoroutine(WaitToSubEvents());
            if(!PhotonNetwork.OfflineMode) SpawnPlayers();
        }
    }
    void SubToEvents()
    {
        victoryZone.onWin += WinGame;
        var currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in currentPlayers)
        {
            player.GetComponent<CharacterModel>().onDeath += LoseGame;
        }
    }

    void SpawnPlayers()
    {
        photonView.RPC("UpdateSpawnPlayers", RpcTarget.All);
        StartTimeSync();
    }

    void LoseGame()
    {
        if (!isStarted) return;
        Invoke("Restart", 3f);
        photonView.RPC("LoseScreen", RpcTarget.All);
    }

    void Restart()
    {
        photonView.RPC("RestartGame", RpcTarget.All);
        photonView.RPC("UpdateStart", RpcTarget.All, false);
    }

    public void CloseRoom()
    {
        photonView.RPC("QuitRoom", RpcTarget.All);
    }

    void WinGame()
    {
        photonView.RPC("WinScreen", RpcTarget.All, currentTime);
        photonView.RPC("UpdateStart", RpcTarget.All, false);
        Invoke("CloseRoom", 5f);
    }

    [PunRPC]
    public void UpdateSpawnPlayers()
    {
        onPlayersJoined.Invoke();
    }

    [PunRPC]
    public void LoseScreen()
    {
        loseScreen.SetActive(true);
        
    }

    [PunRPC]
    public void WinScreen(float timeSpent)
    {
        winScreen.SetActive(true);
        winScreenTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = $"Final Time: {Mathf.RoundToInt(currentTime) / 60}m {Mathf.RoundToInt(currentTime) % 60}s";
    }

    [PunRPC]
    public void RestartGame()
    {
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll(); ;
        PhotonNetwork.LoadLevel("LoadingScene");
    }

    [PunRPC]
    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Menu");
    }

    [PunRPC]
    public void UpdateStart(bool started)
    {
        isStarted = started;
        Debug.Log("IsStarted");
    }

    IEnumerator WaitToSubEvents()
    {
        yield return new WaitForSeconds(secondsToStart);
        SubToEvents();
    }

    void StartTimeSync()
    {
        InvokeRepeating("UpdateTimeSync", 0f, 5f);
    }

    void UpdateTimeSync()
    {
        photonView.RPC("SyncTime", RpcTarget.Others, currentTime);
        Debug.Log("Time Sync!");
    }

    [PunRPC]
    public void SyncTime(float time)
    {
        currentTime = time;
    }
}