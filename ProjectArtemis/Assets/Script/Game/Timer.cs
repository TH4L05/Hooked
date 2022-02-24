using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float run_time;
    private double time_left;
    private bool repeat;
    private bool isRunning;

    public bool Paused { get; set; }
    public bool ShowDebugMessages { get; set; }
    public bool Finished { get; private set; }
    public double Time_Left => time_left;
    public bool Repeat => repeat;
    public bool IsRunning => isRunning;

    public static Action OnTimerFinished;

    public Timer()
    {
    }

    public void StartTimer(float time ,bool repeat)
    {
        this.repeat = repeat;
        run_time = time;     
        time_left = run_time;

        if (run_time >0)
        {
            Run();
            ShowDebugMessage("<color=#8b0000ff>Timer started</color>");
        }       
    }

    public void Run()
    {
        isRunning = true;
        Finished = false;

        InvokeRepeating("CalculateTime", 0f, 1f);
    }

    private void CalculateTime()
    {
        if (time_left >=0)
        {
            time_left -= 1f;
        }
        else
        {
            CancelInvoke("CalculateTime");
            Restart();
        }
    }

    private void Restart()
    {
        if (repeat)
        {
            StartTimer(run_time, true);
            ShowDebugMessage("<color=#8b0000ff>Timer restarted</color>");
        }
        else
        {
            isRunning = false;
            Finished = true;
            OnTimerFinished?.Invoke();
            ShowDebugMessage("<color=#8b0000ff>Timer finished</color>");            
        }
    }


    public void Stop()
    {
        time_left = 0;
        repeat = false;
        OnTimerFinished?.Invoke();
        ShowDebugMessage("<color=#8b0000ff>Timer stopped</color>");
    }

    private void ShowDebugMessage(string message)
    {
        if (ShowDebugMessages)
        {
            Debug.Log(message);
        }
    }
}
