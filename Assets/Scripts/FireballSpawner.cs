using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireballSpawner : MonoBehaviourPun
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform pointA, pointB;

    Collider2D[] closePLayers = new Collider2D[4];
    AudioSource _audioSource;
    bool _activated;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        if (!photonView.IsMine) Destroy(this);
        InvokeRepeating("SpawnFireball", 0f, 2f);
        _activated = true;
    }
    void Start()
    {
        //if (!photonView.IsMine) return;
        //if (!photonView.IsMine) Destroy(this);
        //InvokeRepeating("SpawnFireball", 0f, 2f);
    }
    private void Update()
    {
        int playersFound = Physics2D.OverlapAreaNonAlloc(pointA.position, pointB.position, closePLayers, layerMask);

        if (playersFound <= 0)
        {
            CancelInvoke();
            _activated = false;
        }
        else
        {
            StartInvoke();
            _activated = true;
        }
    }
    void SpawnFireball()
    {
        PhotonNetwork.Instantiate("Fireball", transform.position, transform.rotation);
        photonView.RPC("SpawnSound", RpcTarget.All);
    }

    [PunRPC]
    public void SpawnSound()
    {
        _audioSource.Play();
    }
    public void StartInvoke()
    {
        if (_activated) return;
        InvokeRepeating("SpawnFireball", 0f, 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        _activated = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
