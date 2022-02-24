using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    [SerializeField] private GameObject[] specialObjects;

    private GameObject objInZone;

    private GameObject teleportPoint;
    private Player player;

    public void TeleportPlayer(GameObject teleportPoint)
    {
        objInZone = transform.GetComponent<Trigger>().objInZone;
        if (!objInZone.CompareTag("Player")) return;
        this.teleportPoint = teleportPoint;
        Level.instance.player.ResetMovementValues();
        Level.instance.inputHandler.PauseAll = true;
        Level.instance.HookSystem.PlayLineDestroyAnimation();
        Level.instance.audioEvents.PlayAudioEvent("PlayerTeleport", Level.instance.player.gameObject);
        StartCoroutine(WaitForPlayerDisolve());

    }

    private IEnumerator WaitForPlayerDisolve()
    {
        var charactercontroller = Level.instance.player.GetComponent<CharacterController>();
        charactercontroller.enabled = false;
        //Time.timeScale = 0.006f;
        objInZone.GetComponent<Animator>().Play("dissolve");
        GameEvents.PlayAnimation("ShootEnd", 0, 0, false);

        yield return new WaitForSeconds(1.6f);


        Level.instance.player.transform.position = teleportPoint.transform.position;
        //Time.timeScale = 1.0f;
        charactercontroller.enabled = true;
        Level.instance.inputHandler.PauseAll = false;
    }


    public void TeleportObjAll(GameObject teleportPoint)
    {
        objInZone = transform.GetComponent<Trigger>().objInZone;


        var charactercontroller = objInZone.GetComponent<CharacterController>();
        if (charactercontroller != null)
        {
            TeleportPlayer(teleportPoint);
        }
        else
        {
            if (objInZone != null)
            {
                objInZone.transform.position = teleportPoint.transform.position;
            }
        }
        objInZone = null;
    }

    public void TeleportObjExecptPlayer(GameObject teleportPoint)
    {
        objInZone = transform.GetComponent<Trigger>().objInZone;

       if (objInZone.CompareTag("Player")) return;

       if (objInZone != null)
       {
            if (objInZone.CompareTag("InteractCube"))
            {
                objInZone.GetComponent<InteractCube>().Dissolve();
            }
            else
            {
                objInZone.transform.position = teleportPoint.transform.position;
            }
       }
        objInZone = null;
    }

    public void UnlockLevel(int indx)
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.gameData.UnlockLevel(indx);
        SaveProfile();
    }

    public void SetLastLevelIndex(int indx)
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.gameData.SetLastPlayedLevel(indx);
    }

    public void SaveDestructionPoints()
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.SaveDestructionPoints();
        
    }

    public void ResetDestructionPoints()
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.ResetDestructionPoints();
    }

    public void SaveProfile()
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.gameData.SaveActiveProfile();
    }

    public void DissolveCube(Collider collider)
    {
        var cubeAnim = collider.GetComponent<Animator>();
        cubeAnim.Play("Dissolve");
        Destroy(collider.gameObject, 3.1f);
    }

    public void SetTotalDestructionsPoints()
    {
        if (Level.instance.gameData.activePlayerProfile == null) return;
        Level.instance.gameData.SetTotalDestructionPoints();
    }

    public void UnlockBonusLevel()
    {
        var points = Level.instance.gameData.activePlayerProfile.GetTotalDestructionPoints();
        Debug.Log(points.ToString());

        if (points > 11100)
        {
            var door = specialObjects[0].GetComponent<Animator>();
            door.Play("Door_Open");
            Level.instance.audioEvents.PlayAudioEvent("DoorOpen", specialObjects[0]);
        }
        else
        {
            var door = specialObjects[1].GetComponent<Animator>();
            door.Play("Door_Open");
            Level.instance.audioEvents.PlayAudioEvent("DoorOpen", specialObjects[1]);
        }


    }


    public void UnlockBonusDoorOver50(GameObject gameObject)
    {
        var points = Level.instance.gameData.GetTotalDestructionPoints();
        var door = gameObject.GetComponent<Animator>();

        if (points > 11100)
        {
            door.Play("Door_Open");
            Level.instance.audioEvents.PlayAudioEvent("DoorOpen", gameObject);
        }
        else
        {
            return;
        }

    }

    public void UnlockBonusDoorUnder50(GameObject gameObject)
    {
        var points = Level.instance.gameData.GetTotalDestructionPoints();
        var door = gameObject.GetComponent<Animator>();

        if (points < 11100)
        {
            door.Play("Door_Open");
            Level.instance.audioEvents.PlayAudioEvent("DoorOpen", gameObject);
        }
        else
        {
            return;
        }

    }

    public void UnlockPlayTextDestructionPoints(TextSystem textsystem)
    {
        var points = Level.instance.gameData.GetTotalDestructionPoints();

        var door = gameObject.GetComponent<Animator>();

        if (points > 11100)
        {
            textsystem.SetTextIndex(0);
            textsystem.TextStartPlay();           
        }
        else
        {
            textsystem.SetTextIndex(1);
            textsystem.TextStartPlay();
        }
    }
}
