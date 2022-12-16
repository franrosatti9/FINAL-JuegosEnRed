using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireballSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject fireball;
    AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (!photonView.IsMine) return;
        InvokeRepeating("SpawnFireball", 0f, 2f);
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

    private void OnDisable()
    {
        CancelInvoke();
    }
}
