using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowTransform : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;
    [SerializeField] private bool _isUpdate;

    private Transform _targetTransform;
    void Update()
    {
        if (!_isUpdate) return;
        if (_targetTransform == null) return;
        transform.position = _targetTransform.position;
    }

    public void SetTargetToFollow(Transform target)
    {
        _targetTransform = target;
        transform.position = _targetTransform.position;
    }

    public void SetTargetIndex(int index)
    {
        if (index >= 0 && index < _targets.Length)
        {
            SetTargetToFollow(_targets[index]);
        }
    }
}
