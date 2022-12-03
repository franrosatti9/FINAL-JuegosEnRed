using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReloadScene : MonoBehaviourPun
{
    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.LoadLevel("GameplaySingleplayer");
            return;
        }
        if (PhotonNetwork.LocalPlayer.IsLocal) PhotonNetwork.LoadLevel("Gameplay");
        
    }
}
