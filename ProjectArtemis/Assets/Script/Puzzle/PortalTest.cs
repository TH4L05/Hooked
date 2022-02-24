using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTest : MonoBehaviour
{
    public List<PortalableObject> portalableObjects = new List<PortalableObject>();
    public PortalTest linkedPortal;
    public BoxCollider bcollider;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalableObjects.Add(obj);
            obj.EnterPortal(this, linkedPortal, bcollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if (portalableObjects.Contains(obj))
        {
            portalableObjects.Remove(obj);
            obj.ExitPortal(bcollider);
        }
    }

    void Update()
    {
        for (int i = 0; i < portalableObjects.Count; i++)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalableObjects[i].transform.position);

            if (objPos.z > 0.0f)
            {
                portalableObjects[i].Warp();
            }

        }
    }
}
