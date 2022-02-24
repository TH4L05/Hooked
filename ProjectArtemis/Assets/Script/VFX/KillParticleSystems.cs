using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticleSystems : MonoBehaviour
{
    [SerializeField] 
    private float timeToKill;
    private float timePassed = 0f;
    public bool lookAtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (lookAtPlayer)
        {
            transform.LookAt(Level.instance.player.transform); 
        }

        timePassed += 1 * Time.deltaTime;
        if(timePassed >= timeToKill) {
            Destroy(gameObject);
        }
    }
}
