using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Lava : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterModel>().Die();
        }
    }
}
