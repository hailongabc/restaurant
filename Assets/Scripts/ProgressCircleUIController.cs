using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCircleUIController : MonoBehaviour
{
    private Material _material;
    public float _timer;
    public float _duration = 3f;
    private int shaderFillAmount;
    void Start()
    {
        shaderFillAmount = Shader.PropertyToID("_FillAmount");
        //Debug.Log(shaderFillAmount);
        _material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        _material.SetFloat(shaderFillAmount, Mathf.Clamp01(_timer / _duration));
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);

        if(_timer >= _duration)
        {
            _timer = 0f;
            gameObject.SetActive(false);
        }
    }

    public void StartAction()
    {
        if (_material == null)
            _material = GetComponent<MeshRenderer>().material;

        //_isInteracting = true;
       _material.SetFloat(shaderFillAmount, 0f);
        _timer = 0f;

        gameObject.SetActive(true);
    }

    public void StopAction()
    {
        if (_material == null)
            _material = GetComponent<MeshRenderer>().material;

        _material.SetFloat(shaderFillAmount, 0f);
       // _isInteracting = false;
        gameObject.SetActive(false);
    }
}
