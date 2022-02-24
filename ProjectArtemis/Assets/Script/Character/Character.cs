using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData data;
    [Range(1f, 20f)] [SerializeField] protected float deleteTime = 2.0f;
    protected bool isDead;   
    protected Timer timer;    
    public bool IsDead => isDead;

    private void Start()
    {
        StartSetup();
        ConnectEvents();
    }

    private void ConnectEvents()
    {
        //GameEvents.OnCharacterNoHealthLeft += Death;
    }

    protected virtual void StartSetup()
    {
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log($"{transform.name} get hit and takes damage of {damage}");
        //GameEvents.OnCharacterHealthValueChanged?.Invoke(id, -damage);
    }

    public virtual void Death(string id)
    {
        if (isDead) return;
        isDead = true;
        timer.StartTimer(deleteTime, false);
    }

    public virtual void DeathSetup()
    {
        Destroy();
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
