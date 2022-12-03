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
        if (!photonView.IsMine) Destroy(this);
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

        // COMPORTAMIENTO DEL SPELL

        if (collision.collider.gameObject.CompareTag("Shield"))
        {
            Debug.Log("Shield!");
            var contact = collision.GetContact(0).normal.normalized;
            transform.up = Vector3.Reflect(transform.up, contact);
            _rb.velocity = transform.up * speed;
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void AutoDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
