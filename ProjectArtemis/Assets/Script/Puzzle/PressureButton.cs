using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public UnityEvent OnTriggerEvent;
    public UnityEvent OnTiggerExitEvent;
    private bool pressed;
    public GameObject enteredObj;
    public bool ButtonIsPressed => pressed;
    public Animator buttonAnim;

    private void OnTriggerStay(Collider collider)
    {
        if (!pressed)
        {
            enteredObj = collider.gameObject;
            pressed = true;
            //Debug.Log("ButtonEntered");
            OnTriggerEvent?.Invoke();
            buttonAnim.SetBool("pressed", true);
            buttonAnim.SetBool("released", false);

            var item = collider.gameObject.GetComponent<Item>();
            if (item == null) return;

            if (item.Data.isMagnetic)
            {
                var rb = collider.GetComponent<Rigidbody>();
                rb.velocity /= 2;
                rb.angularVelocity /= 2;               
            }
        }     
    }
    private void OnTriggerExit(Collider collider)
    {
        enteredObj = null;
        pressed = false;
        buttonAnim.SetBool("pressed", false);
        buttonAnim.SetBool("released", true);
        //Debug.Log("ButtonReleased");
        OnTiggerExitEvent?.Invoke();
    }

    public void Play_Pressure_Plate_Activated_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PressurePlateActivated", gameObject);
    }

    public void Play_Pressure_Plate_Deactivated_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("PressurePlateDeactivated", gameObject);
    }
}
