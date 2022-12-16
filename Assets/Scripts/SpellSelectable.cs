using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpellSelectable : MonoBehaviourPun
{
    public bool selected = false;
    public Staff staff;
    public GameObject selectedSprite;
    void Start()
    {
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableSelected(bool selected)
    {
        this.selected = selected;
        selectedSprite.SetActive(selected);
        photonView.RPC("UpdateSelected", RpcTarget.Others, selected);
    }

    public void ChangePosition(Vector3 newPos)
    {
        transform.position = newPos;
        photonView.RPC("UpdatePosition", RpcTarget.Others, newPos);
    }
    [PunRPC]
    public void UpdateSelected(bool selected)
    {
        this.selected = selected;
        selectedSprite.SetActive(selected);
    }

    [PunRPC]
    public void UpdatePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
