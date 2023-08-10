using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    private void FixedUpdate()
    {
        Vector3 follow = target.position;
        follow.z = -10;
        follow.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, follow, 0.125f);
    }
}
