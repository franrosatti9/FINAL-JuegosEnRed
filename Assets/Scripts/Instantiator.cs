using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instantiator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    Transform spawn1;
    Transform spawn2;
    Transform spawn3;
    private void OnEnable()
    {
        gameManager.onPlayersJoined += InstantiatePlayers;
    }
    void Start()
    {
        spawn1 = gameManager.spawnPoint1;
        spawn2 = gameManager.spawnPoint2;
        spawn3 = gameManager.spawnPoint3;
    }

    public void InstantiatePlayers()
    {
        var players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsLocal)
            {
                Debug.Log("Spawn?");
                if (i == 0) PhotonNetwork.Instantiate("PlayerWarrior", spawn1.position, Quaternion.identity);
                if (i == 1) PhotonNetwork.Instantiate("PlayerArcher", spawn2.position, Quaternion.identity);
                if (i == 2) PhotonNetwork.Instantiate("PlayerShield", spawn3.position, Quaternion.identity);
            }
        }
    }

    private void OnDisable()
    {
        gameManager.onPlayersJoined -= InstantiatePlayers;
    }
}