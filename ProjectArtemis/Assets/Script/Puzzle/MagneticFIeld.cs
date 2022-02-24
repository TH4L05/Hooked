using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFIeld : MonoBehaviour
{
    public float fieldDistance = 10f;
    public float forceMultplier = 2f;
    public new BoxCollider collider;
    public Transform magnet;
    public List<GameObject> objectsInField = new List<GameObject>();
    bool active;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        //collider.size = new Vector3(0, fieldDistance, 0);
        //collider.center = new Vector3(0, fieldDistance /2 , 0);
        transform.localPosition = new Vector3(0, fieldDistance / 2, 0);
        transform.localScale = new Vector3(1, fieldDistance , 1);
        magnet = transform.parent;

        //InvokeRepeating("AddForceToObjects", 0, 0.2f);
    }

    public void Update()
    {
        AddForceToObjects();
    }

    private void AddForceToObjects()
    {
        if (objectsInField.Count > 0 && collider.enabled)
        {
            foreach (var obj in objectsInField)
            {
                var item = obj.GetComponent<Item>();
                var rb = obj.gameObject.GetComponent<Rigidbody>();
                if (rb == null) continue;

                var direction = magnet.transform.position - item.gameObject.transform.position;
                rb.AddForce(direction * forceMultplier * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        var item = collider.GetComponent<Item>();
        if (item == null) return;

        if (item.Data.isMagnetic)
        {
            if (objectsInField.Count == 0)
            {
                objectsInField.Add(collider.gameObject);
                return;
            }

            foreach (var obj in objectsInField)
            {
                if (obj == collider.gameObject) return;
            }

            objectsInField.Add(collider.gameObject);

            var rb = collider.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        foreach (var obj in objectsInField)
        {
            if (collider.gameObject == obj)
            {
                objectsInField.Remove(obj);
            }
        }
    }

    public void ChangeStatus()
    {
        active = !active;
        collider.enabled = active;
        objectsInField.Clear();
    }
}
