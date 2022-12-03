using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
{
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    public void Shoot()
    {

    }
}
