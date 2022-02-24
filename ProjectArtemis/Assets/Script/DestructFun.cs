using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructFun : Item
{
    public float size = 0.2f;
    public int cubesInRow = 5;
    private bool onDestruction = false;
    float cubesPivotDistance;
    Vector3 cubesPivot;


    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    public GameObject normal;
    public GameObject broken;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged")) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<CharacterController>();
            var velocity = player.velocity.magnitude;
            Debug.Log(velocity);

            if (velocity < 5f) return;
        }

        if (!onDestruction)
        {
            onDestruction = true;
            //UpdatePoints();
            Destroy();
        }

    }

    public void Destroy()
    {
        normal.SetActive(false);

        var collider = GetComponent<MeshCollider>();
        collider.enabled = false;
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {                   
                    CreatePieces(x, y, z);
                }
            }
        }
        //broken.SetActive(true);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void CreatePieces(int x, int y, int z)
    {
        GameObject piece;
        piece = Instantiate(broken);

        piece.transform.position = transform.position /*+ new Vector3(size * x, size * y, size * z) - cubesPivot*/;
        piece.transform.localScale = new Vector3(size, size, size);

        piece.AddComponent<Rigidbody>();
        //piece.GetComponent<Rigidbody>().mass = cubeSize;
        piece.GetComponent<Rigidbody>().mass = 10f;
    }

    public void UpdatePoints()
    {
        Level.instance.UpdateDestructionPoints((int)data.health);
    }
}
