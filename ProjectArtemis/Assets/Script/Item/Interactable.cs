using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private List<Transform> specialTransforms = new List<Transform>();
    [SerializeField] UnityEvent OnInteract;

    private bool caninteract = true;
    private int repeatCount = 0;
    private int repeatAmount = 0;

    public void Interact()
    {
        if (caninteract)
        {
            OnInteract?.Invoke();
        }      
    }

    public void TestEvent()
    {
        Debug.Log("TEST EVENT TRIGGERED");
    }

    public void RotateSelfX(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    public void RotateSelfY(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    public void RotateSelfZ(float angle)
    {
        transform.Rotate(0, 0, angle);
    }

    public void RotateOtherX(float angle)
    {
        transform.Rotate(angle, 0, 0);
        specialTransforms[0].rotation = transform.rotation;
    }

    public void RotateOtherY(float angle)
    {
        transform.Rotate(0, angle, 0);
        specialTransforms[0].rotation = transform.rotation;
    }

    public void RotateOtherZ(float angle)
    {
        transform.Rotate(0, 0, angle);
        specialTransforms[0].rotation = transform.rotation;
    }

    public void RotateOverTimePlusZ(float angle)
    {
        repeatAmount = (int)angle *10;
        repeatCount = 0;
        InvokeRepeating("RotatePlusZ", 1, 0.01f);
    }

    public void RotateOverTimeMinusZ(float angle)
    {
        repeatAmount = (int)angle * 10;
        repeatCount = 0;
        InvokeRepeating("RotateMinusZ", 1, 0.01f);
    }

    public void RotateOverTimePlusX(float angle)
    {
        repeatAmount = (int)angle * 10;
        repeatCount = 0;
        InvokeRepeating("RotatePlusX", 1, 0.01f);
    }

    public void RotateOverTimeMinusX(float angle)
    {
        repeatAmount = (int)angle * 10;
        repeatCount = 0;
        InvokeRepeating("RotateMinusX", 1, 0.01f);
    }

    private void RotatePlusZ()
    {
        repeatCount++;
        
        if (repeatCount <= repeatAmount)
        {
            //transform.Rotate(0, 0, 0.1f);
            specialTransforms[0].Rotate(0, 0, 0.1f);
        }
        else
        {
            Debug.Log("InvokeStopped");
            CancelInvoke("RotatePlusZ");
        }
    }

    private void RotateMinusZ()
    {
        repeatCount++;

        if (repeatCount <= repeatAmount)
        {
            //transform.Rotate(0, 0, -0.1f);
            specialTransforms[0].Rotate(0, 0, -0.1f);
        }
        else
        {
            Debug.Log("InvokeStopped");
            CancelInvoke("RotateMinusZ");
        }
    }

    private void RotatePlusX()
    {
        repeatCount++;

        if (repeatCount <= repeatAmount)
        {
            //transform.Rotate(0, 0, 0.1f);
            specialTransforms[0].Rotate(0.1f, 0, 0);
        }
        else
        {
            Debug.Log("InvokeStopped");
            CancelInvoke("RotatePlusX");
        }
    }

    private void RotateMinusX()
    {
        repeatCount++;

        if (repeatCount <= repeatAmount)
        {
            //transform.Rotate(0, 0, -0.1f);
            specialTransforms[0].Rotate(-0.1f, 0, 0);
        }
        else
        {
            Debug.Log("InvokeStopped");
            CancelInvoke("RotateMinusX");
        }
    }
}
