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
    Transform spawn4;
    private void OnEnable()
    {
        gameManager.onPlayersJoined += InstantiatePlayers;
    }
    void Start()
    {
        spawn1 = gameManager.spawnPoint1;
        spawn2 = gameManager.spawnPoint2;
        spawn3 = gameManager.spawnPoint3;
        spawn4 = gameManager.spawnPoint4;
    }

    public void InstantiatePlayers()
    {
        var players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsLocal)
            {
                Debug.Log("Spawn?");
                if (i == 0) return;
                if (i == 1)
                {
                    /*var obj = PhotonNetwork.Instantiate("PlayerWarrior", spawn1.position, Quaternion.identity);
                    var character = obj.GetComponent<CharacterModel>();
                    var client = PhotonNetwork.LocalPlayer;
                    MasterManager.Instance.RPC("UpdatePlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, character.photonView.ViewID);*/
                    MasterManager.Instance.RPC("RequestConnectPlayer", PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer, "PlayerWarrior", spawn1);

                }
                if (i == 2)
                {
                    /*var obj = PhotonNetwork.Instantiate("PlayerArcher", spawn2.position, Quaternion.identity);
                    var character = obj.GetComponent<CharacterModel>();
                    var client = PhotonNetwork.LocalPlayer;
                    MasterManager.Instance.RPC("UpdatePlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, character.photonView.ViewID);*/
                    MasterManager.Instance.RPC("RequestConnectPlayer", PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer, "PlayerArcher", spawn2);
                }
                if (i == 3)
                {
                    /*var obj = PhotonNetwork.Instantiate("PlayerShield", spawn3.position, Quaternion.identity);
                    var character = obj.GetComponent<CharacterModel>();
                    var client = PhotonNetwork.LocalPlayer;
                    MasterManager.Instance.RPC("UpdatePlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, character.photonView.ViewID);*/
                    MasterManager.Instance.RPC("RequestConnectPlayer", PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer, "PlayerShield", spawn3);
                }
                if (i == 4)
                {
                    /*var obj = PhotonNetwork.Instantiate("PlayerWizard", spawn4.position, Quaternion.identity);
                    var character = obj.GetComponent<CharacterModel>();
                    var client = PhotonNetwork.LocalPlayer;
                    MasterManager.Instance.RPC("UpdatePlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, character.photonView.ViewID);*/
                    MasterManager.Instance.RPC("RequestConnectPlayer", PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer, "PlayerWizard", spawn4);
                }
            }
        }
        
    }

    private void OnDisable()
    {
        gameManager.onPlayersJoined -= InstantiatePlayers;
    }
}