using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPortableObject
{

}

public class PortalableObject : MonoBehaviour
{
    public PortalTest inPortal;
    public PortalTest outPortal;
    public int portalCount = 0;

    public new Rigidbody rigidbody;
    public new Collider collider;

    public static readonly Quaternion halfTurn = Quaternion.Euler(0f, 180f, 0f);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void EnterPortal(PortalTest inPortal, PortalTest linkedPortal, Collider portalCollider)
    {
        Debug.Log("ENTER PORTAL");
        this.inPortal = inPortal;
        outPortal = linkedPortal;
        ++portalCount;
        Physics.IgnoreCollision(collider, portalCollider);
        
    }

    public void ExitPortal(Collider portalCollider)
    {
        Debug.Log("EXIT PORTAL");
        Physics.IgnoreCollision(collider, portalCollider, false);
        --portalCount;
    }

    public void Warp()
    {
        Debug.Log("WARP");
        var inPortalTransform = inPortal.transform;
        var outPortalTransform = outPortal.transform;

        Vector3 pos = inPortalTransform.InverseTransformPoint(transform.position);
        pos = halfTurn * pos;
        transform.position = outPortalTransform.TransformPoint(pos);

        Vector3 velocity = inPortalTransform.InverseTransformDirection(rigidbody.velocity);
        velocity = halfTurn * velocity;       
        rigidbody.velocity = outPortalTransform.TransformDirection(velocity);

        var tmpPortal = inPortal;
        inPortal = outPortal;
        outPortal = tmpPortal;
    }
}
