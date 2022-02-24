using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTIme : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestroy = 1f;

    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }
}
