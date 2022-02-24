using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootTargets : MonoBehaviour
{
    [SerializeField] private List<Target> targetsList = new List<Target>();
    [SerializeField] private UnityEvent OnAllTargetshit;
    Dictionary<Target,bool> targets = new Dictionary<Target,bool>();
    [SerializeField] private float targetHitCheckRepeatTime = 1f;
    private int targethitcount;


    private void Start()
    {
        foreach (var target in targetsList)
        {
            targets.Add(target,false);
        }

        InvokeRepeating("CheckTargetHitStatus", 0f, targetHitCheckRepeatTime);
    }

    private void CheckTargetHitStatus()
    {
        CheckIfTargetsActive();

        foreach (var item in targets)
        {
            if (item.Value == true)
            {
                targethitcount++;
            }
        }

        if (targethitcount == targetsList.Capacity)
        {
            OnAllTargetshit?.Invoke();
            CancelInvoke("CheckTargetHitStatus");
        }

        targethitcount = 0;
    }

    private void CheckIfTargetsActive()
    {
        foreach (Target target in targetsList)
        {
            if (target.IsActive == true)
            {
                targets[target] = true;
            }
        }
    }
}
