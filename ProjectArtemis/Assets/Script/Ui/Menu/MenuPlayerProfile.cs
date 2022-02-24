using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MenuPlayerProfile : MonoBehaviour
{
    public List<PlayerProfileSlot> slots = new List<PlayerProfileSlot> ();

    public void OnEnable()
    {
        LoadSlotProfiles();
    }

    public void LoadSlotProfiles()
    {
        foreach (var slot in slots)
        {
            slot.LoadProfile();
        }
    }

    

}
