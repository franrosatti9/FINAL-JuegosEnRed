using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPun
{
    Rigidbody2D _rb;
    [SerializeField] float speed = 4f;

    private void Awake()
    {
        // TAL VEZ HACER QUE LO INSTANCIE EL MASTER

        if (!photonView.IsMine)
        { 
            if(!PhotonNetwork.IsMasterClient) Destroy(this);
        }
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _rb.velocity = transform.up * speed;
        Invoke("AutoDestroy", 5f);
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (collision.collider.gameObject.CompareTag("Shield"))
        {
            Debug.Log("Shield!");
            var contact = collision.GetContact(0).normal.normalized;
            transform.up = Vector3.Reflect(transform.up, contact);
            _rb.velocity = transform.up * speed;
        }
        else
        {
            photonView.RPC("AutoDestroy", photonView.Owner);
            //PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void AutoDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
