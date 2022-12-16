using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
public class GameManager : MonoBehaviourPunCallbacks
{
    public bool isStarted = false;
    public GameObject loseScreen;
    public GameObject winScreen;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public float currentTime;
    [SerializeField] GameObject winScreenTimeText;
    [SerializeField] float secondsToStart = 2f;
    [SerializeField] VictoryZone victoryZone;

    public Action onPlayersJoined = delegate { };
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckStartGame();
        }

        currentTime = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
        if (isStarted)
        {
            currentTime += Time.deltaTime;
        }

        //if (!PhotonNetwork.IsMasterClient) return;
        //if (Input.GetKeyDown(KeyCode.P)) CloseRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckStartGame();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CloseRoom();
        }

        // chequear diccionario de players para ver quien se fue
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
        // for players, mandar i como parametro de que numero les toca y guardarlo. rpctarget = player
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
        PhotonNetwork.LoadLevel("LoadingScene");
    }

    [PunRPC]
    public void QuitRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
        PhotonNetwork.LeaveRoom();
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