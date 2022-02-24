using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 1000;
    [SerializeField] private Material laserMaterial;
    [Range(0.1f,1)][SerializeField] private float startWidth = 1f;
    [Range(0.1f,1)][SerializeField] private float endWidth = 1f;

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material = laserMaterial;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

            var reciver = hit.collider.GetComponent<LaserReciver>();
            if (reciver != null && reciver.reciverMaterial == laserMaterial)
            {
                reciver.LaserHit();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position * maxDistance);
        }
    }
}
