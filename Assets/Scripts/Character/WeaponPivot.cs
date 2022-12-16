using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponPivot : MonoBehaviourPun
{
    public Vector2 shootDir;
    [SerializeField] SpriteRenderer weaponRenderer;
    [SerializeField] SpriteRenderer playerRenderer;

    void Update()
    {
        if (!photonView.IsMine) return;
        shootDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.right = shootDir.normalized;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
    }
}
