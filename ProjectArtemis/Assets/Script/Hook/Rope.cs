using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Options")]
    [Range(0.1f, 4f)] public float divider = 1f;
    public Transform start;
    public Transform end;    
    public GameObject ropePointTemplate;       
    public Material lineMaterial;

    [Header("Debug")]
    public float d = 0f;
    public float distance;
    public int amount;
    public LineRenderer lineRenderer;
    public GameObject lastpoint;
    public List<GameObject> points = new List<GameObject>();

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        SetRope();
    }

    public void SetRope()
    {
        distance = Vector3.Distance(start.transform.position, end.transform.position);
        points.Add(start.gameObject);

        amount = (int)(distance / divider);

        for (int i = 0; i < amount; i++)
        {          
            Vector3 pos = Vector3.zero;
            pos = new Vector3(start.transform.position.x + d, start.transform.position.y, start.transform.position.z);
                       
            var point = Instantiate(ropePointTemplate);
            point.transform.position = pos;
            point.transform.parent = gameObject.transform;
            points.Add(point);

            var joint = point.GetComponent<ConfigurableJoint>();

            if (i == 0)
            {
                var startRB = point.GetComponent<Rigidbody>();
                startRB.isKinematic = true;
            }
            else
            {
                var rbl = lastpoint.gameObject.GetComponent<Rigidbody>();
                joint.connectedBody = rbl;
            }
            
            d += divider;
            lastpoint = point;
        }

        var endRB = lastpoint.gameObject.GetComponent<Rigidbody>();
        endRB.isKinematic = true;
    }

    public void DrawLine()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].transform.position);
        }
    }

    void Update()
    {
        DrawLine();
    }
}
