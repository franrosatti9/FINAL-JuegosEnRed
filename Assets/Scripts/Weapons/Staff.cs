using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Staff : Weapon
{
    [SerializeField] GameObject spell;
    [SerializeField] Transform spellPos;
    public GameObject selected1, selected2;

    AudioSource _audioSource;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        selected1 = null;
        selected2 = null;
    } 

    public override void Attack()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        Shoot();
    }

    public void Shoot()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        //if (!photonView.IsMine) return;
        var spellInstance = PhotonNetwork.Instantiate("Spell", spellPos.position, Quaternion.identity);
        spellInstance.transform.up = pivot.transform.right;
        spellInstance.GetComponent<Spell>().staff = this;
    }

    public void SwitchPlaces()
    {
        Vector2 aux = selected1.transform.position;
        //selected1.transform.position = selected2.transform.position;
        //selected2.transform.position = aux;
        selected1.GetComponent<SpellSelectable>().ChangePosition(selected2.transform.position);
        selected2.GetComponent<SpellSelectable>().ChangePosition(aux);

        photonView.RPC("PlaySwitchSound", RpcTarget.All);
        //photonView.RPC("UpdateSwitchPlaces", RpcTarget.Others, selected1, selected2);
    }

    [PunRPC]
    public void PlaySwitchSound()
    {
        _audioSource.Play();
    }
}
