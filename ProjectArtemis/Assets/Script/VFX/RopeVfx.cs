using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeVfx : MonoBehaviour
{
    public bool onMove;

    public float speed;

    public Vector3 position;


    public void Update()
    {
        if (onMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, position, step);

            if (transform.position == position)
            {
                Destroy(gameObject);
            }

        }
    }

    public void Move()
    {
            onMove = true;
    }

}
