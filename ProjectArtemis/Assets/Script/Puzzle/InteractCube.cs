using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCube : MonoBehaviour
{
    public CubeSpawner spawner; 
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Barrier"))
        {
            Dissolve();
        }
    }

    public void Dissolve()
    {
        animator.Play("Dissolve");
        Level.instance.audioEvents.PlayAudioEvent("CubeTeleport",gameObject);
        Level.instance.player.HookSystem.DestroyHookNodes(0f);
    }

    public void SpawnNewCube()
    {
        var cube = spawner.Spawn();

        if (cube == null) return;
        cube.GetComponent<InteractCube>().spawner = spawner;
        Level.instance.audioEvents.PlayAudioEvent("CubeSpawn", cube);
    }

    public void DestroyCube()
    {
        Destroy(gameObject);
    }

    public void PlayCubeImpactSound()
    {
        Level.instance.audioEvents.PlayAudioEvent("CubeImpact", gameObject);
    }
}
