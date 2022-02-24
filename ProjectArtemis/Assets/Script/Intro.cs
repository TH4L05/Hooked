using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
    public LoadScene loadscene;
    public PlayableDirector intro;
    public PlayableDirector fadeOutLoad;
    bool load;

    void Update()
    {
        if (!load)
        {
            if (Keyboard.current.eKey.isPressed)
            {             
                load = true;
                intro.Stop();
                fadeOutLoad.Play();        
            }
        }
    }
}
