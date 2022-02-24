using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Sound_Box : MonoBehaviour
{
    public Barrier barrier;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !barrier.resolved)
        {
            Level.instance.audioEvents.PlayAudioEvent("DissolveWallPlayerWobbleStart", gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Level.instance.audioEvents.PlayAudioEvent("DissolveWallPlayerWobbleStop");
        }
    }
}
