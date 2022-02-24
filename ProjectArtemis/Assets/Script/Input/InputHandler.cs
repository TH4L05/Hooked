using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    #region fields

    [SerializeField] private InputController inputController;
    [SerializeField] private Player player;
    [SerializeField] private IngameMenu ingameMenu;

    //private WeaponController weaponController;
    private HookSystem hookSystem;
    private Vector2 arrowAxisInput;
    private bool paused = true;

    public bool canShootPrimary;
    public bool canShootSecondary;

    public static Action<bool> PullQuitActive;
    public static Action<bool> PullActive;

    public bool PauseAll { get; set; }

    #endregion fields

    #region UnityFunctions

    void Start()
    {
        paused = false;
    }

    private void FixedUpdate()
    {
        if(PauseAll) return;
        if (paused) return;
        UpdatePlayerValues();
    }

    void Update()
    {
        TogglePauseMenuOnInput();

        if (PauseAll) return;
        if (paused) return;
        AddForceHookTargets();
        ResetHooksInput();
        PrimaryFireInput();
        SecondaryFireInput();
        
    }

    #endregion

    #region Setup

    public void SetReferences(Player player, HookSystem hookSystem)
    {
        this.player = player;
        this.hookSystem = hookSystem;
    }

    #endregion

    #region InputHandle

    private void UpdatePlayerValues()
    {
        player.UpdateInputValues(
                            inputController.InputAxis, 
                            inputController.InputMouse, 
                            inputController.JumpInputPressed, 
                            inputController.SprintInputPressed
                            );

        if (inputController.JumpInputPressed)
        {
            inputController.JumpInputPressed = false;
        }
    }

    private void PrimaryFireInput()
    {
        if(inputController.PrimaryFireInputPressed && hookSystem.canShootLeftHook && Level.instance.player.CanShootHook)
        {
            canShootPrimary = true;
            hookSystem.DestroyLeftHook();
            hookSystem.SetArrowVFxMaterial(0);
            GameEvents.PlayAnimation?.Invoke("ShootStart",0,0, false);
            hookSystem.canShootRightHook = false;
            return;
        }
        else if (!inputController.PrimaryFireInputPressed)
        {
            GameEvents.PlayAnimation?.Invoke("shootEnding",0,0,false);
            GameEvents.PlayAnimation?.Invoke("BowEmissionResetOrange", 0, 0, false);
            if (canShootPrimary )
            {
                canShootPrimary = false;
                GameEvents.PlayAnimation?.Invoke("ShootEnd",0,0, false);
                Invoke("Shoot1", 0.1f);                      
            }
        }
    }

    private void SecondaryFireInput()
    {
        if (inputController.SecondaryFireInputPressed && hookSystem.canShootRightHook && Level.instance.player.CanShootHook)
        {
            canShootSecondary = true;
            hookSystem.DestroyRightHook();
            hookSystem.SetArrowVFxMaterial(1);
            GameEvents.PlayAnimation?.Invoke("ShootStart",0,0, false);
            hookSystem.canShootLeftHook = false;
            return;
        }
        else if (!inputController.SecondaryFireInputPressed)
        {
            GameEvents.PlayAnimation?.Invoke("shootEnding", 0, 0, false);
            GameEvents.PlayAnimation?.Invoke("BowEmissionResetBlue", 0, 0, false);
            if (canShootSecondary )
            {
                canShootSecondary = false;
                GameEvents.PlayAnimation?.Invoke("ShootEnd",0,0, false);
                Invoke("Shoot2", 0.1f);              
            }
        }
    }

    private void Shoot1()
    {
        hookSystem.CreateLeftHook();       
    }

    private void Shoot2()
    {
        hookSystem.CreateRightHook();
    }

    public void ChangeCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void AddForceHookTargets()
    {  
        if (inputController.HookAddForceInputPressed)
        {
            hookSystem.SetPull(true);
            hookSystem.AddForceToHookedObjects();            
            PullActive?.Invoke(true);
        }
        else
        {
            hookSystem.SetPull(false);
            hookSystem.CancelPull();
            PullActive?.Invoke(false);
        }
        //inputController.HookAddForceInputPressed = false;
    }

    private void ResetHooksInput()
    {
        
        if (inputController.ResetHookInputPressed)
        {
            if (hookSystem.ConnectionEstablished)
            {
                Level.instance.HookSystem.intendedDisconnection = true;
                Level.instance. HookSystem.PlayLineDestroyAnimation();
                Level.instance. player.CancelHookShotMovement();
                PullQuitActive?.Invoke(true);

                GameEvents.PlayAnimation("PullQuit", 0, 0, false);
            }
            
        }
        else
        {
            PullQuitActive?.Invoke(false);
        }
        //inputController.ResetHookInputPressed = false;
        
    }


    public void EnableInputActions(bool enable)
    {
        if (enable)
        {
            inputController.EnableDisableInputActions("enable", inputController.InputActions);
        }
        else
        {
            inputController.EnableDisableInputActions("disable", inputController.InputActions);
        }
    }

    private void TogglePauseMenuOnInput()
    {
        if (inputController.InGameInputPressed)
        {
            if (paused)
            {
                paused = false;
                ingameMenu.ToggleMenu();
                
            }
            else
            {
                paused = true;
                ingameMenu.ToggleMenu();               
            }           
        }
        inputController.InGameInputPressed = false;
    }

    public void InputHandlerIsPaused(bool pause)
    {
        paused = pause;
    }

    #endregion
}
