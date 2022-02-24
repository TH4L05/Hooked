using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private List<Transform> spawns = new List<Transform>();

    public void SpawnSingle()
    {
        if (spawnObjects[0] == null) return;
        Instantiate(spawnObjects[0], transform.position, Quaternion.identity);
    }

    public void SpawnSingle(Transform spawn)
    {
        if (spawnObjects[0] == null) return;
        Instantiate(spawnObjects[0], spawn.position, Quaternion.identity);
    }

    public void SpawnSingle(int spawnsIndex)
    {
        if (spawnObjects[0] == null) return;
        if (spawnsIndex > spawns.Capacity) return;

        Instantiate(spawnObjects[0], spawns[spawnsIndex].position, Quaternion.identity);
    }

    public void SpawnAll()
    {
        if (spawnObjects[0] == null) return;
        foreach (var obj in spawnObjects)
        {
            Instantiate(obj, transform.position, Quaternion.identity);
        }
    }

    public void SpawnAll(Transform spawn)
    {
        if (spawnObjects[0] == null) return;
        foreach (var obj in spawnObjects)
        {
            Instantiate(obj, spawn.position, Quaternion.identity);
        }
    }

    public void SpawnAllList()
    {
        for (int i = 0; i < spawnObjects.Count; i++)
        {
            Instantiate(spawnObjects[i], spawns[i].position, Quaternion.identity);
        }
    }
}
