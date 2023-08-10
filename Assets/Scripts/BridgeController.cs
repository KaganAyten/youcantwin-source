using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BridgeController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke(nameof(Fall), .6f);
    }
    private void Fall()
    {
        transform.DOMoveY(transform.position.y - 0.6f, 2);
    }
}
