using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spell : MonoBehaviourPun
{
    Rigidbody2D _rb;
    [SerializeField] float speed = 5f;
    public Staff staff;

    //public GameObject selected1, selected2;

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _rb.velocity = transform.up * speed;
        Invoke("AutoDestroy", 5f);
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var selected = collision.collider.GetComponent<SpellSelectable>();
        if (selected != null)
        {
            if (staff.selected1 == null)
            {
                staff.selected1 = selected.gameObject;
                selected.EnableSelected(true);
                Debug.Log("SELECTED");
            }
            else if (staff.selected1 != null && staff.selected2 == null)
            {
                //Switch places
                staff.selected2 = selected.gameObject;
                selected.EnableSelected(true);

                staff.SwitchPlaces();

                // Restart selected
                staff.selected1.GetComponent<SpellSelectable>().EnableSelected(false);
                staff.selected1 = null;
                staff.selected2.GetComponent<SpellSelectable>().EnableSelected(false);
                staff.selected2 = null;
                

            }
            PhotonNetwork.Destroy(gameObject);
        }


        if (collision.collider.gameObject.CompareTag("Shield"))
        {
            Debug.Log("Shield!");
            var contact = collision.GetContact(0).normal.normalized;
            transform.up = Vector3.Reflect(transform.up, contact);
            _rb.velocity = transform.up * speed;
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    

    public void AutoDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
