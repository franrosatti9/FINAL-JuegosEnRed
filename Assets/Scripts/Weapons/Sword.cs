using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    BoxCollider2D _col;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _col = GetComponent<BoxCollider2D>();
    }


    public override void Attack()
    {
        _anim.SetTrigger("Attack");
    }
}
