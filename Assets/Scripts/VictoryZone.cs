using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class VictoryZone : MonoBehaviourPun
{
    public Action onWin = delegate { };
    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onWin.Invoke();
        }
    }
}
