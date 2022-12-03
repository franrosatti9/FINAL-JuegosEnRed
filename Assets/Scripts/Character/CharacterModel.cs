using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class CharacterModel : MonoBehaviourPun
{
    Rigidbody2D _rb;
    [SerializeField] Weapon weapon;
    [SerializeField] float attackRate;
    float _nextAttackTime;
    AudioSource _audioSource;

    public float speed;

    public Action onDeath = delegate { };
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }
    public void Move(Vector2 dir)
    {
        dir = dir.normalized;
        dir *= speed;
        _rb.velocity = dir;
    }

    public void Attack()
    {
        if (CanAttack())
        {
            _nextAttackTime = Time.time + attackRate;
            weapon.Attack();
            _audioSource.Play();
        }
    }

    public bool CanAttack()
    {
        return Time.time >= _nextAttackTime;
    }

    public void Die()
    {
        onDeath.Invoke();
    }

    /*[PunRPC]
    public void DieUpdate()
    {
        onDeath.Invoke();
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine && collision.CompareTag("Trap"))
        {
            Die();
        }
    }

}

public enum CharacterType
{
    Warrior,
    Archer,
    Defender
}
