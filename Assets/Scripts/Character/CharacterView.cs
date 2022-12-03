using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviourPun
{
    Rigidbody2D _rb;
    SpriteRenderer _renderer;
    Animator _anim;
    WeaponPivot _pivot;

    public NicknameUI nicknamePrefab;
    NicknameUI _nickname;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _pivot = GetComponentInChildren<WeaponPivot>();

        if (PhotonNetwork.OfflineMode) return;
        
        var canvas = GameObject.Find("Canvas");
        _nickname = GameObject.Instantiate<NicknameUI>(nicknamePrefab, canvas.transform);
        _nickname.SetTarget(transform);
        
        //_nickname.SetName(photonView.Owner.NickName);

        if (photonView.IsMine)
        {
            var name = photonView.Owner.NickName;
            UpdateName(name);
        }

        
        if (!photonView.IsMine)
        {
            photonView.RPC("RequestFlipValue", photonView.Owner, PhotonNetwork.LocalPlayer);
            photonView.RPC("RequestName", photonView.Owner, PhotonNetwork.LocalPlayer);
        }

       
    }

    void Update()
    {
        if (!photonView.IsMine && !PhotonNetwork.OfflineMode) return;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _anim.SetBool("Walking", true);
        }
        else _anim.SetBool("Walking", false);

        if (_pivot.shootDir.x < 0)
        {
            if (!_renderer.flipX)
            {
                _renderer.flipX = true;
                photonView.RPC("UpdateSetFlip", RpcTarget.Others, true);
            }
        }
        else if (_pivot.shootDir.x > 0)
        {
            if (_renderer.flipX)
            {
                _renderer.flipX = false;
                photonView.RPC("UpdateSetFlip", RpcTarget.Others, false);
            }
        }
    }

    [PunRPC]
    public void UpdateSetFlip(bool v)
    {
        _renderer.flipX = v;
    }

    [PunRPC]
    public void RequestFlipValue(Player client)
    {
        photonView.RPC("UpdateSetFlip", client, _renderer.flipX);
    }

    [PunRPC]
    public void RequestName(Player client)
    {
        photonView.RPC("UpdateName", client, photonView.Owner.NickName);
    }
    [PunRPC]
    public void UpdateName(string name)
    {
        if (_nickname != null)
            _nickname.SetName(name);
    }
    private void OnDestroy()
    {
        if(_nickname != null) Destroy(_nickname.gameObject);
    }

    private void OnDisable()
    {
        _anim.SetBool("Walking", false);
    }
}
