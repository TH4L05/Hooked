using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Transform linkedPoint;
    public bool isActive = true;
    private Vector3 lastvelocity;

    public Camera portalCam;
    public Material portalCamMat;

    private void Start()
    {
        SetRenderTexture();
    }

    private void SetRenderTexture()
    {
        if (!portalCam) return;
        portalCam.targetTexture.Release();
        portalCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        portalCamMat.mainTexture = portalCam.targetTexture;
    }


    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.name);
        if (collider.gameObject.layer == 6) return;

        if (isActive)
        {
            linkedPortal.Toggle();
            Toggle();

            if (linkedPoint != null)
            {
                if (collider.CompareTag("Player"))
                {
                    //Debug.Log("PLayerEnter");
                    collider.transform.position = linkedPortal.transform.position;
                    return;
                }
                collider.transform.position = linkedPoint.transform.position;
            }
            else
            {
                Teleport(collider);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Toggle();
    }


    private void Teleport(Collider collider)
    {
        Vector3 portalToObject = collider.transform.position - transform.position;
        float dot = Vector3.Dot(transform.up, portalToObject);

        //Debug.Log(dot);
        if (dot < 1f) //org = dot < 0f 
        {
            var charactercontroller = collider.GetComponent<CharacterController>();
            if (charactercontroller != null)
            {
                charactercontroller.enabled = false;
                //Debug.Log("PlayerControllerDisabled");
            }

            //Debug.Log(linkedPortal.transform.rotation);
            float rotationDiff = -Quaternion.Angle(transform.rotation, linkedPortal.transform.rotation);
            rotationDiff += 180f;

            collider.transform.Rotate(linkedPortal.transform.up, rotationDiff);

            Vector3 posOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToObject;

            collider.transform.position = linkedPortal.transform.position + posOffset;


            if (charactercontroller != null)
            {
                charactercontroller.enabled = true;
                //Debug.Log("PlayerControllerEnabled");
            }

            var rb = collider.GetComponent<Rigidbody>();
            if (rb == null) return;
            var oppositeVelocity = -rb.velocity;

            if (oppositeVelocity.magnitude < 4)
            {
                rb.AddForce(oppositeVelocity * 2.5f, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(oppositeVelocity * 2f, ForceMode.Impulse);
            }           
        }
    }

    public void Toggle()
    {
        isActive = !isActive;
    }
}
