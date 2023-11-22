using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrderScript : MonoBehaviour
{
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).SetDelay(.5f)
            .OnStart(() => transform.rotation = Camera.main.transform.rotation);
    }

    void Update()
    {
        
    }
}
