using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool resolved = true;
    public bool dissolved;

    public void Resolve()
    {
        if (resolved) return;
        
        dissolved = false;
        resolved = true;
        animator.Play("Barrier_Resolve");
        Play_Dissolve_Wall_Activated();      
    }

    public void Dissolve()
    {
        if (dissolved) return;
        
        resolved = false;
        dissolved = true;
        animator.Play("Barrier_Disssolve");
        Play_Dissolve_Wall_Deactivated();       
    }

    public void Play_Dissolve_Wall_Activated()
    {
        resolved = false;
        Level.instance.audioEvents.PlayAudioEvent("DissolveWallActivated", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("DissolveWallIsActiveStart", gameObject);
    }

    public void Play_Dissolve_Wall_Deactivated()
    {
        resolved = true;
        Level.instance.audioEvents.PlayAudioEvent("DissolveWallDeactivated", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("DissolveWallIsActiveStop", gameObject);
    }
}
