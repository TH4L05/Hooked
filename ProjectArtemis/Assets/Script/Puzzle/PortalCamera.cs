using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private Portal portal;
    [SerializeField] private Portal linkedPortal;  
    [SerializeField] private Transform camPos;  
    private Player player;

    private void Start()
    {
        player = Level.instance. player;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distance);

        //if (distance > 15f)
        //{
        //    transform.position = camPos.transform.position;
        //}
        //else
        //{
            Vector3 playerOffestFromPortal = player.CameraMain.transform.position - linkedPortal.transform.position;
            transform.position = portal.transform.position + playerOffestFromPortal;

            float angleDiff = Quaternion.Angle(portal.transform.rotation, linkedPortal.transform.rotation);
            angleDiff += 180f;

            Quaternion portalRotationDiff = Quaternion.AngleAxis(angleDiff, Vector3.up);
            Vector3 newCamDir = portalRotationDiff * player.CameraMain.transform.forward;

            transform.rotation = Quaternion.LookRotation(newCamDir, Vector3.up);
        //}      
    }
}
