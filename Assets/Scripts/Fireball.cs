using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Fireball : MonoBehaviourPun
{
    Rigidbody2D _rb;
    [SerializeField] float speed = 4f;
    public Action onDeath = delegate { };
    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _rb.velocity = transform.up * speed;
    }

    public void HitPlayer(Player client)
    {
        onDeath.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterModel>().Die();
            Debug.Log("Dead!");
        }
        if (collision.gameObject.CompareTag("Sword") || collision.gameObject.CompareTag("Arrow")) return;
        if (photonView.IsMine) PhotonNetwork.Destroy(this.gameObject);
    }
}
