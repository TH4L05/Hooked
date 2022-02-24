using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubeTemplate;

    public GameObject Spawn()
    {
        if (cubeTemplate == null) return null;

        var cube = Instantiate(cubeTemplate, transform.position, Quaternion.identity);
        return cube;
    }
}
