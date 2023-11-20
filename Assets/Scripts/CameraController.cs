using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed;
    private Vector3 _velocity;
    private Transform _camera;

    private Vector3 _offset;

    private void Awake()
    {
        _camera = this.transform;

        _offset = _camera.position - _player.position;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _player.localPosition + _offset, ref _velocity, .1f);
    }
 
}
