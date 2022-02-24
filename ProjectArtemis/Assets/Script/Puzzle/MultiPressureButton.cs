using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiPressureButton : MonoBehaviour
{
    public List<PressureButton> pressureButtons = new List<PressureButton>();
    [SerializeField] private UnityEvent OnAllButtonsPressed;
    [SerializeField] private UnityEvent OnNotAllButtonsPressed;

    Dictionary<PressureButton, bool> buttons = new Dictionary<PressureButton, bool>();
    private int pressedButtonCount;

    private void Start()
    {
        foreach (var button in pressureButtons)
        {
            buttons.Add(button, false);
        }
        InvokeRepeating("CheckIfButtonPressed", 0, 0.25f);
    }

    private void Update()
    {
        CheckAllButtonPressed();
    }

    private void CheckAllButtonPressed()
    {

        foreach (var item in buttons)
        {
            if (item.Value == true)
            {
                pressedButtonCount++;
            }
        }

        if (pressedButtonCount == pressureButtons.Capacity)
        {
            OnAllButtonsPressed?.Invoke();
        }
        else
        {
            OnNotAllButtonsPressed?.Invoke();
        }

        pressedButtonCount = 0;
    }

    private void CheckIfButtonPressed()
    {
        foreach (var button in pressureButtons)
        {
            if (button.ButtonIsPressed)
            {
                buttons[button] = true;
            }
            else
            {
                buttons[button] = false;
            }
        }
    }
}
