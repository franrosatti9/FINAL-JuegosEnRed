using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelectable : MonoBehaviour
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
    }
}
