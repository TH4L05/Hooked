using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI time;
    private Timer timer;
    private int seconds;
    private int minutes;
    

    private void Awake()
    {
        timer = new Timer();
        timer.ShowDebugMessages = true;
    }

    private void Start()
    {
        timer.StartTimer(1, false);
    }

    private void FixedUpdate()
    {
        
        if (timer.Finished)
        {
            seconds++;
            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }
            time.text = minutes.ToString("00") + " : " + seconds.ToString("00");
        }
    }

}
