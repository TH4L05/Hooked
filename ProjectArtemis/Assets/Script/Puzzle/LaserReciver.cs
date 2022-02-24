using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReciver : MonoBehaviour
{
    public MeshRenderer reciverLight;
    public Material greenMaterial;
    public Material redMaterial;
    public Material reciverMaterial;

    public bool Laserhit { get; set; }

    private void Start()
    {
        InvokeRepeating("CheckLaserHit", 0, 1);
    }

    public void CheckLaserHit()
    {
        reciverLight.material = redMaterial;
        
    }

    public void LaserHit()
    {
        reciverLight.material = greenMaterial;
    }


}
