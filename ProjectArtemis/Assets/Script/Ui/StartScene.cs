using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StartScene : MonoBehaviour
{

    public UnityEvent OnAnyKeyPressed;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        InvokeRepeating("StartGame", 0, 0.25f);
    }

    public void StartGame()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            OnAnyKeyPressed?.Invoke();
            CancelInvoke("StartGame");
        }
    }
}
