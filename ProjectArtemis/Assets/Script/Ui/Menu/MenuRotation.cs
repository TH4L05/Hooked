using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotation : MonoBehaviour
{
    [Range(1f,10f)] [SerializeField] private float rotationSpeed = 5f;
    private bool caninteract = true;
    private int repeatCount = 0;
    private int repeatAmount = 0;
    private bool isPositiv;
    

    public void RotateYAxis(float angle)
    {
        if (angle > 0)
        {
            repeatAmount = (int)angle * (int)rotationSpeed;
            isPositiv = true;
        }
        else
        {
            repeatAmount = (int)angle * (int)rotationSpeed * -1;
            isPositiv = false;
        }

        repeatCount = 0;
        InvokeRepeating("RotateY", 0.2f, 0.01f);
    }

    private void RotateY()
    {
        repeatCount++;

        if (repeatCount <= repeatAmount)
        {
            var rotationAngle = 1 / rotationSpeed;

            if (isPositiv)
            {
                transform.Rotate(0, rotationAngle, 0);
            }
            else
            {
                transform.Rotate(0, -rotationAngle, 0);
               
            }           
        }
        else
        {
            CancelInvoke("RotateY");
        }
    }

}
