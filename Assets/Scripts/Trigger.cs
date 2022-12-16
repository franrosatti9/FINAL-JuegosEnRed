using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviourPun
{
    [SerializeField] string tagToCompare;
    [SerializeField] bool activateOnce;
    bool _activated = false;
    public UnityEvent onActivate;
    void Start()
    {
        _activated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient) return; // TAL VEZ CAMBIAR
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            if (activateOnce) GetComponent<Collider2D>().enabled = false;
            photonView.RPC("UpdateActivation", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdateActivation()
    {
        onActivate.Invoke();
    }
}