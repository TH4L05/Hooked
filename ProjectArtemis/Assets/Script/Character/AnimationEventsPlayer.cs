using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsPlayer : MonoBehaviour
{
    public void Play_Player_Draw_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerDrawsBow", gameObject);
    }

    public void Play_Player_Hold_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerHoldsBowCharged", gameObject);
    }

    public void Stop_Player_Hold_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerHoldsBowChargedStop", gameObject);
    }

    public void Play_Player_Release_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerShoot", gameObject);
    }

    public void Play_Bow_Grab_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerGrabsBow", gameObject);
    }

    public void Play_Pull_Is_Active_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerPullActive", gameObject);
    }

    public void Stop_Pull_Is_Active_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PlayerPullStop", gameObject);
    }
}
