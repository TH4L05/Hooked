using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("RotateAround", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
    }

    public void RotateAround()
    {
        
    }
}
