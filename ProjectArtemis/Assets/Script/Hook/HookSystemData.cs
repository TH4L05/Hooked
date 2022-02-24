using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New GameData", menuName = "ProjectArtemis/Data/HookSystemData")]
public class HookSystemData : ScriptableObject
{
    [Tooltip("A list of all HooknodeTemplates (listindex defines appearance order) ")]
    public List<GameObject> hookNodeTemplates = new List<GameObject>();
    [HideInInspector] public int maxHookNodeCount = 2;
    public float hookNodeSpeed = 200f;
    [Tooltip("Calculate the forceMultiplier by hookObjectsMass (fm = objMass *2) ")]
    public bool ForceMultiplierByObjMass = true;
    [Tooltip("Changing this value take only an effect if SetForceMultiplierbyObjectMass = false")]
    [Range(1f, 999f)] public float forceMultiplier = 2f;
    [Range(1f, 999f)] public float forceMultiplierMax = 900f;
    [Range(1f, 999f)] public float extraforceMultiplier = 2f;
    [Tooltip("Set the maximum shot range")]
    [Range(1f, 500f)] public float rayCastRange = 50f;
    [Range(0.1f, 50f)] public float dragOnHook = 0.5f;
    public bool deactivateGravityOnHook = false;
    [Range(0.5f, 5f)] public float minDistanceBeforeCut = 1.5f;
}
