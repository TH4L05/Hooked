using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AudioEvent
{
    public string EventName;
    public AK.Wwise.Event wwiseEvent;
}

[System.Serializable]
public class AudioParameter
{
    public string ParameterName;
    public AK.Wwise.RTPC rtpc;

}

public class AudioEvents : MonoBehaviour
{
    public List<AudioEvent> audioEvents = new List<AudioEvent>();
    public List<AudioParameter> audioParameters = new List<AudioParameter>();


    public void PlayAudioEvent(string name)
    {
        foreach (var audioEvent in audioEvents)
        {
            if (audioEvent.EventName == name)
            {
                 //Debug.Log("PlayAudioEvent -> " + audioEvent.EventName);
                 audioEvent.wwiseEvent.Post(gameObject);
                 return;              
            }
        }
    }


    public void PlayAudioEvent(string name, GameObject obj)
    {
        foreach (var audioEvent in audioEvents)
        {           
            if (audioEvent.EventName == name)
            {
                if (obj != null)
                {
                    //Debug.Log("PlayAudioEvent -> " + audioEvent.EventName);
                    audioEvent.wwiseEvent.Post(obj);
                    return;
                }             
            }
        }
    }

    public void SetAudioParameter(string name, float value)
    {
        foreach (var parameter in audioParameters)
        {
            if (parameter.ParameterName == name)
            {
                parameter.rtpc.SetGlobalValue(value);
                return;
            }
        }
    }


}
