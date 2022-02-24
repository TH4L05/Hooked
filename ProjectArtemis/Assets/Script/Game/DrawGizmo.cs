using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    private enum GizmoType
    {
        Cube,
        WireCube,
        Sphere,
        WireSphere,
        Line,
        Icon,
        Mesh
    }

    [SerializeField] private GizmoType gizmoType = GizmoType.Cube;
    [SerializeField] private Color gizmoColor = Color.white;

    [Header("Cube")]
    [SerializeField] private bool useTransformScaleAsSize = false;
    [SerializeField] private Vector3 size = Vector3.one;


    [Header("Sphere")]
    [Range(0.1f,10f)][SerializeField] private float radius = 0.5f;

    [Header("Line")]
    [SerializeField] private Vector3 startPosition = Vector3.one;
    [SerializeField] private Vector3 endPosition = Vector3.one;

    [Header("Icon")]
    [SerializeField] private string iconName = "Icon";

    [Header("Mesh")]
    [SerializeField] private Mesh mesh;
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private Quaternion rotation = Quaternion.Euler(0,0,0);
    [SerializeField] private Vector3 scale = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        switch (gizmoType)
        {
            case GizmoType.Cube:
            default:
                if (useTransformScaleAsSize)
                {

                    Gizmos.DrawCube(transform.position, transform.localScale);
                }
                else
                {
                    Gizmos.DrawCube(transform.position, size);
                }
                break;

            case GizmoType.WireCube:
                if (useTransformScaleAsSize)
                {

                    Gizmos.DrawWireCube(transform.position, transform.localScale);
                }
                else
                {
                    Gizmos.DrawWireCube(transform.position, size);
                }
                break;

            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position, radius);
                break;

            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;

            case GizmoType.Line:
                Gizmos.DrawLine(startPosition, endPosition);
                break;

            case GizmoType.Icon:
                Gizmos.DrawIcon(transform.position, iconName, true);
                break;

            case GizmoType.Mesh:
                if (mesh == null) return;
                Gizmos.DrawMesh(mesh, 0, position, rotation, scale);

                break;
                
        }
    }

}
