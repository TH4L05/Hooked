using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Color activeColor;
    public bool IsActive { get; private set; }
    
    public void SetActive()
    {
        if (IsActive) return;

        IsActive = true;
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = activeColor;           
    }
}
