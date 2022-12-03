using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Vector3 offset;
    Transform _target;
    Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void SetName(string nick)
    {
        nameText.text = nick;
    }
    private void Update()
    {
        if (_target != null)
        {
            var pos = _camera.WorldToScreenPoint(_target.position + offset);
            transform.position = pos;
        }
    }
}
