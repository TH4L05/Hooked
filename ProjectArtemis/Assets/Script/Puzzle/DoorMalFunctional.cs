using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMalFunctional : MonoBehaviour
{
    public Door doorFinal;
    bool malfunction;

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("InteractCube") && !malfunction)
        {
            malfunction = true;
            doorFinal.animator.SetBool("malfunction", true);
            doorFinal.animator.Play("Malfunction");          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InteractCube") && malfunction)
        {
            malfunction=false;
            doorFinal.animator.SetTrigger("reset");
            doorFinal.animator.SetBool("malfunction", false);
        }      
    }

}
