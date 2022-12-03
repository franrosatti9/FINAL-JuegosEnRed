using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bow : Weapon
{
    [SerializeField] GameObject arrow;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    } 

    public override void Attack()
    {
        if (!photonView.IsMine) return;
        _anim.SetTrigger("Attack");
    }

    public void Shoot()
    {
        if (!photonView.IsMine) return;
        var arrowInstance = PhotonNetwork.Instantiate("Arrow", transform.position, Quaternion.identity);
        arrowInstance.transform.up = pivot.transform.right;
    }
}
