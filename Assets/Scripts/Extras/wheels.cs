using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheels : MonoBehaviour
{
    [SerializeField]
    private bool spin = false;
    private float speed = 500f;

    void Update()
    {
        if (spin)
            transform.Rotate(speed * Time.deltaTime, 0, 0);
    }
}
