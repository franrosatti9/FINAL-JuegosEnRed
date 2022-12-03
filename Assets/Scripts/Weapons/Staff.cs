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

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        selected1 = null;
        selected2 = null;
    } 

    public override void Attack()
    {
        if (!photonView.IsMine) return;
        Shoot();
    }

    public void Shoot()
    {
        if (!photonView.IsMine) return;
        var spellInstance = PhotonNetwork.Instantiate("Spell", spellPos.position, Quaternion.identity);
        spellInstance.transform.up = pivot.transform.right;
        spellInstance.GetComponent<Spell>().staff = this;
    }

    public void SwitchPlaces()
    {
        Vector2 aux = selected1.transform.position;
        selected1.transform.position = selected2.transform.position;
        selected2.transform.position = aux;
    }
}
