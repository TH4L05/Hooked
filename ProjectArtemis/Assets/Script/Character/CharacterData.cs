using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "ProjectArtemis/Data/CharacterData")]
public class CharacterData : ScriptableObject
{  
    [Range(1f, 99f)] public float gravity = 9.81f;
    [Range(1f, 50f)] public float jump_force = 7f;
    [Range(1f, 20f)] public float move_speed = 5f;
    [Range(1f, 20f)] public float sprint_speed = 7.5f;
    [Range(1f, 20f)] public float crouch_speed = 2.5f;
    [Range(1f, 20f)] public float slide_speed = 2.5f;
    [Range(0.1f, 20f)] public float draghook = 1f;
    [Range(0.1f, 20f)] public float dragdefault = 2.2f;
    [Range(0.1f, 20f)] public float groundfriction = 4.5f;
    [Range(0.01f, 1f)] public float aerial_control = 1f;
    [Range(0f, 2f)] public float jumpBufferTimeframe = 0.2f;
    [Range(0f, 2f)] public float coyoteTimeframe = 0.15f;

    public Vector3 position;
    public Vector3 rotation;
}
