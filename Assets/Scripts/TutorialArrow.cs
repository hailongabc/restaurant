using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialArrow : MonoBehaviour
{
   
    void Start()
    {
        transform.GetChild(0).transform.DOMoveY(.5f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

   
}
