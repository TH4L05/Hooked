using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Sound : MonoBehaviour
{
    void Start()
    {
        Level.instance.audioEvents.PlayAudioEvent("TeleporterActive", gameObject);
    }

}
