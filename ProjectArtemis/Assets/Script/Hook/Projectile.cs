using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    public float lifeTime = 2.5f;
    public float Speed { private get; set; }
    [SerializeField] protected float speed = 20f;
    public Rigidbody rbody;

    private void OnEnable()
    {
        rbody = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
      MoveProjectile();       
    }

    public virtual void MoveProjectile()
    {

        if (Speed != 0)
        {
            speed = Speed;
        }

        rbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (rbody != null)
        {
            transform.forward = Vector3.Lerp(transform.forward, rbody.velocity, Time.deltaTime);
        }
    }

}
