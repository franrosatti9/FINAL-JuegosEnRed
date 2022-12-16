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

    private void Start()
    {
        //var cam = GameObject.FindGameObjectWithTag("CinemachineCam");
        //cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = this.gameObject.transform;
    }
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        _model.Move(new Vector2(horizontal, vertical));


        if (Input.GetMouseButtonDown(0))
        {
            if (characterType == CharacterType.Archer || characterType == CharacterType.Wizard) 
            {
                MasterManager.Instance.RPCMaster("RequestProjectile", PhotonNetwork.LocalPlayer);
                return;        
            }
            _model.Attack();
        }
    }

    private void OnDisable()
    {
        //_model.Move(Vector2.zero);
    }
}


