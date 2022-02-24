using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region private fields

    private Controls playerControls;
    private List<InputAction> inputActions;
    private List<InputAction> inputActionsUI;
    private List<InputAction> inputActionsOther;

    private Vector2 inputAxis;
    private Vector2 inputMouse;
    private Vector2 inputAxisArrow;

    #endregion private fields

    #region fields

    
    public List<InputAction> InputActions => inputActions;
    public List<InputAction> InputActionsOther => inputActionsOther;

    public Vector2 InputAxis => inputAxis;
    public Vector2 InputMouse => inputMouse;
    public Vector2 InputAxisArrow => inputAxisArrow;

    public bool PrimaryFireInputPressed { get; set; }
    public bool SecondaryFireInputPressed { get; set; }
    public bool SprintInputPressed { get; private set; }
    public bool CrouchInputPressed { get; private set; }
    public bool JumpInputPressed { get; set; }
    public bool InteractInputPressed { get; set; }
    public bool HookAddForceInputPressed { get; set; }
    public bool ResetHookInputPressed { get; set; }
    public bool InGameInputPressed { get; set; }

    #endregion fields

    #region UnityFunctions

    private void Awake()
    {
        playerControls = new Controls();
        inputActions = new List<InputAction>();
        inputActionsUI = new List<InputAction>();
        inputActionsOther = new List<InputAction>();
    }

    private void Start()
    {
        SetInputActions();
    }

    private void OnDestroy()
    {
        EnableDisableInputActions("disable", inputActions);
        EnableDisableInputActions("disable", inputActionsOther);
        //EnableDisableInput("disable", inputActionsUI);   
    }

    #endregion UnityFunctions

    #region Setup

    /// <summary>
    /// sets all avialble inputactions up and acivate's them
    /// </summary>
    private void SetInputActions()
    {
        var input = playerControls.Movement;
        var inputOther = playerControls.Other;
        var inputUI = playerControls.UI;

        input.MovementAxis.performed += AxisMovementInput;
        inputActions.Add(input.MovementAxis);

        input.MouseAxis.performed += MouseAxisInput;
        inputActions.Add(input.MouseAxis);

        input.Jump.performed += JumpInput;
        inputActions.Add(input.Jump);

        input.PrimaryFire.performed += PrimaryFireInput;
        input.PrimaryFire.canceled += PrimaryFireInputCanceled;
        inputActions.Add(input.PrimaryFire);

        input.SecondaryFire.performed += SecondaryFireInput;
        input.SecondaryFire.canceled += SecondaryFireInputCanceled;
        inputActions.Add(input.SecondaryFire);

        input.Sprint.performed += SprintInput;
        input.Sprint.canceled += SprintInput;
        inputActions.Add(input.Crouch);

        input.Crouch.performed += CrouchInput;
        input.Crouch.canceled += CrouchInput;
        inputActions.Add(input.Sprint);

        inputOther.Interact.performed += InteractInput;
        //inputOther.Interact.canceled += InteractInputIsPressed;
        inputActionsOther.Add(inputOther.Interact);

        inputOther.AddForceToHookedObjects.performed += AddForceToHookedObjecsInput;
        inputOther.AddForceToHookedObjects.canceled += AddForceToHookedObjecsInput;
        inputActionsOther.Add(inputOther.AddForceToHookedObjects);

        inputOther.ResetHookNodes.performed += ResetHookNodesInput;
        inputOther.ResetHookNodes.canceled += ResetHookNodesInput;
        inputActionsOther.Add(inputOther.ResetHookNodes);

        inputOther.ToggleInGameMenu.performed += ToggleInGameMenuInput;
        inputActionsOther.Add(inputOther.ToggleInGameMenu);

        EnableDisableInputActions("enable", inputActions);
        EnableDisableInputActions("enable", inputActionsOther);
        EnableDisableInputActions("enable", inputActionsUI);
    }

    #endregion Setup

    #region EnableDisable

    public void EnableMovmentInput(int i)
    {
        
        inputActions[i].Enable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void DisableMovementInput(int i)
    {
        inputActions[i].Disable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void EnableOtherInput(int i)
    {

        inputActionsOther[i].Enable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void DisableOtherInput(int i)
    {
        inputActionsOther[i].Disable();
        //Debug.Log(inputActions[i].enabled);
    }

    /// <summary>
    /// Enable or disable an inputActionList
    /// </summary>
    /// <param name="command">Possible commands - enable, disable</param>
    /// <param name="inputList">inputActionList</param>
    /// 
    public void EnableDisableInputActions(string command, List<InputAction> inputList)
    {
        if (command == "enable")
        {
            foreach (var action in inputList)
            {
                action.Enable();
            }
        }
        else if (command == "disable")
        {
            foreach (var action in inputList)
            {
                action.Disable();
            }
        }
    }

    /// <summary>
    /// Enable or disable a single inputAction an inputActionList
    /// </summary>
    /// <param name="command">Possible commands - enable, disable</param>
    /// <param name="action">inputAction</param>
    /// <param name="inputList">inputActionList</param>
    public void EnableDisableSingleInputAction(string command, InputAction action, List<InputAction> inputList)
    {
        foreach (var inputAction in inputList)
        {
            if (inputAction == action)
            {
                if (command == "enable")
                {
                    inputAction.Enable();
                }
                else if (command == "disable")
                {
                    inputAction.Disable();
                }
                return;
            }
        }
    }

    public void EnableAllInputActions()
    {
        EnableDisableInputActions("enable", inputActions);
        EnableDisableInputActions("enable", inputActionsOther);
        //EnableDisableInputActions("enable", inputActionsUI);
    }

    #endregion EnableDisable

    #region Input

    private void MouseAxisInput(InputAction.CallbackContext inputcontext)
    {
        inputMouse = inputcontext.ReadValue<Vector2>();
    }

    private void AxisMovementInput(InputAction.CallbackContext inputcontext)
    {
        inputAxis = inputcontext.ReadValue<Vector2>();
    }

    private void JumpInput(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>JumpInputIsPressed</color>");
        JumpInputPressed = true;
    }

    private void CrouchInput(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>CrouchInputIsPressed</color>");
        CrouchInputPressed = !CrouchInputPressed;
    }

    private void SprintInput(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>SprintInputIsPressed</color>");
        SprintInputPressed = !SprintInputPressed;
    }

    private void InteractInput(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>InteractInputIsPressed</color>");
        InteractInputPressed = !InteractInputPressed;
    }

    private void AddForceToHookedObjecsInput(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>AddForceToHookedObjecsInputIsPressed</color>");
        //HookAddForceInputPressed = true;
        HookAddForceInputPressed = !HookAddForceInputPressed;
        
    }

    private void ResetHookNodesInput(InputAction.CallbackContext obj)
    {
        ResetHookInputPressed = !ResetHookInputPressed;
    }

    private void SecondaryFireInput(InputAction.CallbackContext obj)
    {
        SecondaryFireInputPressed = true;
    }

    private void SecondaryFireInputCanceled(InputAction.CallbackContext obj)
    {
        SecondaryFireInputPressed = false;
    }

    private void PrimaryFireInput(InputAction.CallbackContext obj)
    {
        PrimaryFireInputPressed = true;
    }

    private void PrimaryFireInputCanceled(InputAction.CallbackContext obj)
    {
        PrimaryFireInputPressed = false;
    }

    private void ToggleInGameMenuInput(InputAction.CallbackContext context)
    {
        InGameInputPressed = !InGameInputPressed;
    }


    #endregion Input
}
