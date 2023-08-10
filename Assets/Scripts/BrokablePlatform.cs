using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BrokablePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DORotate(GetRandomZ(-1), .5f).OnComplete(() => { 
            transform.DORotate(GetRandomZ(1), .5f).OnComplete(() => { 
                transform.DOScale(0, .2f).OnComplete(() => { 
                    Destroy(this.gameObject); 
                }); 
            }); 
        });

    }
    private Vector3 GetRandomZ(float negative)
    {
        float randValue = Random.Range(4, 10f);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = negative * randValue;
        return rot;
    }
}
