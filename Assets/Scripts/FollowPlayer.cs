using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + offsetX, offsetY, offsetZ);
        
        if(transform.position.x < -110 && transform.position.x > -120)
        {
            offsetY += 0.05f;
            offsetZ += 0.05f;
        } else if (transform.position.x > -110)
        {
            offsetX = -3.5f;
            offsetY = 4;
            offsetZ = 12;
        }
    }
}
