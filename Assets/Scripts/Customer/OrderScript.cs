using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrderScript : MonoBehaviour
{
    public Sprite[] sprites; // Hoặc public string[] spritePaths;
   [HideInInspector] public SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        ChangeSprite();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).SetDelay(.5f)
            .OnStart(() => transform.rotation = Camera.main.transform.rotation);
    }

    void Update()
    {
        
    }

    void ChangeSprite()
    {
        int randomIndex = Random.Range(0, sprites.Length); // Hoặc sử dụng spritesPaths.Length
        spriteRenderer.sprite = sprites[randomIndex]; // Hoặc sử dụng LoadSpriteFromPath(spritePaths[randomIndex]);
    }
}
