using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        float deltaX = target.position.x - transform.position.x;
        if (transform.position.x < target.position.x)
        {
            delta.x = deltaX;
        }
        else
        {
            delta.x = deltaX;
        }

        float deltaY = target.position.y - transform.position.y;
        if (transform.position.y < target.position.y)
        {
            delta.y = deltaY;
        }
        else
        {
            delta.y = deltaY;
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
