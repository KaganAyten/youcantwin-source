using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float targetX;
    [SerializeField] private float duration;
    void Start()
    {
        transform.DOMoveX(targetX, duration).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

}
