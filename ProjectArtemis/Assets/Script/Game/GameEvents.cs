using System;
using UnityEngine;

public class GameEvents
{
    public static Action<bool,int> ShowInfoText;
    public static Action<bool> OnPlayerPullUiUpdate;
    public static Action<bool> OnPlayerPullQuitUiUpdate;

    public static Action<bool> EventOnLeftHookChanged;
    public static Action<bool> EventOnRightHookChanged;

    public static Action IconsUsable;
    public static Action IconsReset;


    public static Action<float> EventOnUpdateCrosshair;

    public static Action<int> OnUpdateDestructionPoints;
    public static Action<string, float, int, bool> PlayAnimation;
}
