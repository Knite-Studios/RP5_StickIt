using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveBy : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f;
    void Update()
    {
        float movement = speed * Time.deltaTime;
        transform.Translate(0, 0, movement);
        if (transform.position.z > 200f)
            transform.Translate(0,0,-300f);
    }
}
