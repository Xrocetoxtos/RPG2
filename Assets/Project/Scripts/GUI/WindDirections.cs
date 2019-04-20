using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDirections : MonoBehaviour
{
    private Quaternion rotation;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    void FixedUpdate()
    {
        transform.rotation = rotation;
    }
}
