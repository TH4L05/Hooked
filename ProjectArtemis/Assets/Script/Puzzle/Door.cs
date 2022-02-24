using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    bool open;

    public void Open()
    {
        if (open) return;
        open = true;
        animator.SetTrigger("open");    
    }

    public void Close()
    {
        if(!open) return;
        open = false;
        animator.SetTrigger("close");            
    }

    public void Malfunction()
    {
        Open();
        StartCoroutine("W");
    }


    IEnumerator W()
    {
        yield return new WaitForSeconds(1f);
        Close();
    }

    public void Play_Door_Open_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("DoorOpen", gameObject);
    }

    public void Play_Door_Close_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("DoorClose", gameObject);
    }

    public void Play_Door_Malfunction_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("DoorMalfunction", gameObject);
    }
}
