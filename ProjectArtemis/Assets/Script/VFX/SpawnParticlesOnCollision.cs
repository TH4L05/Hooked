using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnParticlesOnCollision : MonoBehaviour
{

    public GameObject CollisionParticles;
    private Vector3 collpoint;

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            //Instantiate your particle system here.
            Instantiate(CollisionParticles, contact.point, Quaternion.identity);

        }
        GetComponent<InteractCube>().PlayCubeImpactSound();
    }
}