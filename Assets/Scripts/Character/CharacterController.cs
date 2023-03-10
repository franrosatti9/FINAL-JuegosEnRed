using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CharacterController : MonoBehaviourPun
{
    CharacterModel _model;
    [SerializeField] WeaponPivot weaponPivot;

    public KeyCode jumpKey = KeyCode.Space;
    public Vector3 _mousePos;
    public CharacterType characterType;

    public bool flipX = false;
    public bool flipY = false;


    private void OnEnable()
    {
        var cam = GameObject.FindGameObjectWithTag("CinemachineCam");
        cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = this.gameObject.transform;
    }
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            GetComponent<AudioListener>().enabled = false;
            Destroy(this);
            if(!PhotonNetwork.IsMasterClient) Destroy(weaponPivot);
        }
        _model = GetComponent<CharacterModel>();  
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal") * (flipX ? -1 : 1);
        var vertical = Input.GetAxisRaw("Vertical") * (flipY ? -1 : 1);
        _model.Move(new Vector2(horizontal, vertical));

        if (Input.GetMouseButtonDown(0))
        {
            if ((characterType == CharacterType.Archer || characterType == CharacterType.Wizard) && !PhotonNetwork.OfflineMode) 
            {
                MasterManager.Instance.RPCMaster("RequestProjectile", PhotonNetwork.LocalPlayer);
                return;        
            }
            _model.Attack();
        }
    }

    public void FlipInput()
    {
        flipX = !flipX;
        flipY = !flipY;
    }

    public void StopMovement()
    {
        _model.Move(Vector2.zero);
    }
}


