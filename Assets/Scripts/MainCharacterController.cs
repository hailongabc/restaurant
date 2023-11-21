using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _rootPosition;
    [SerializeField] private float speed;
    [SerializeField] private Camera camera;
    [SerializeField] private float durationZoomCam = 1f;
    [SerializeField] private float zoomInValue = 6f;
    [SerializeField] private float zoomOutValue = 7.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _edgeLayer;
    [SerializeField] private Transform _slopeDetector;
    public MCCarrierScript mcCarrierScript;

    private bool isMoving = false;
    private bool isStart = false;
    Tween fadeTween;
    void Update()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            _rootPosition = Input.mousePosition;
            camera.DOOrthoSize(zoomOutValue, durationZoomCam);
            if (!isStart)
            {
                fadeTween = Manager.ins.header.transform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
                fadeTween.OnComplete(() =>
                {
                    Manager.ins.cash.transform.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                });
            }

        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _rootPosition;
            if (delta != Vector3.zero)
            {
                Vector3 move = new Vector3(delta.x, transform.position.y, delta.y);
                float factor = Mathf.Clamp01(move.magnitude / 100f);
                if (factor > 0f)
                {
                    var moveFactor = transform.localRotation * Vector3.forward * factor;
                    moveFactor.Normalize();
                    var characterHeight = 0f;

                    if (Physics.Raycast(_slopeDetector.position, Vector3.down, out var hitSlope, 100f, _groundLayer))
                    {
                        moveFactor = Vector3.ProjectOnPlane(moveFactor, hitSlope.normal);
                        characterHeight = hitSlope.point.y;
                    }
                    moveFactor = WallHandler(moveFactor);
                    transform.localPosition += moveFactor * (speed * Time.deltaTime);
                    //Debug.DrawRay(_slopeDetector.position, Vector3.down, Color.blue);
                    if (characterHeight != 0)
                    {
                        var temp = transform.localPosition;
                        temp.y = characterHeight;
                        transform.localPosition = temp;
                    }
                    transform.localRotation = Quaternion.Euler(0f, 40f, 0f) * Quaternion.LookRotation(move, new Vector3(0f, 1f, 0f));
                    if (!isMoving)
                    {
                        isMoving = true;
                        _animator.SetBool("IsMoving", true);
                       
                    }
                }
            }
        }
        else
        {
            camera.DOOrthoSize(zoomInValue, durationZoomCam);
            if (isMoving)
            {
                isMoving = false;
                _animator.SetBool("IsMoving", false);
                Debug.Log("cccccccccccc");
            }
        }

        if (mcCarrierScript.listFood.Count > 0)
        {
            _animator.SetBool("IsCarrying", true);
        }
        else
        {
            _animator.SetBool("IsCarrying", false);
        }

    }
    private Vector3 WallHandler(Vector3 moveFactor)
    {
        var dist = 0.85f;
        var origin = transform.position - transform.forward * 0.5f;
        if (Physics.SphereCast(origin, 0.3f, transform.forward, out var hitEdge, dist, _edgeLayer))
        {
            var projectedMoveFactor = Vector3.ProjectOnPlane(moveFactor, hitEdge.normal);
            Ray cornerRay = new Ray(transform.position, projectedMoveFactor.normalized);

            Debug.DrawRay(origin, cornerRay.direction * 10, Color.red);
            Debug.DrawRay(origin, transform.forward * 10, Color.green);

            if (Physics.SphereCast(cornerRay, 0.15f, 0.45f, _edgeLayer))
            {
                moveFactor = Vector3.zero;
            }
            else
            {
                moveFactor = projectedMoveFactor.normalized / 3;
            }
        }

        return moveFactor;
    }

}
