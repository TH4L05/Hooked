using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookNode : MonoBehaviour
{
    #region Fields

    private int hookNodeIndex;
    private bool hitBarrier = false; 

    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject ropeConnectionPoint;
    [SerializeField] private GameObject destroyVfx;

    public GameObject RopeConnectionPoint => ropeConnectionPoint;
    public GameObject VFXobj => vfx;
    public GameObject DestroyVFX => destroyVfx;


    #endregion

    #region UnityFunctions

    private void Start()
    {
        CheckHookNodeTarget();
        ResetShoot();
        Level.instance.audioEvents.PlayAudioEvent("ActiveConnection", gameObject);
        Debug.Log("ActiveConnection");
    }

    private void OnDestroy()
    {
        Level.instance.audioEvents.PlayAudioEvent("ArrowGetDestroyed", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("StopConnection", gameObject);
    }

    #endregion

    #region NodeBehaviour

    public void SetHookNodeIndex(int idx)
    {
        hookNodeIndex = idx;
    }

    public void InitiateNodeDestroy()
    {
        Level.instance.HookSystem.ConnectionEstablished = false;
        Level.instance.HookSystem.PlayLineDestroyAnimation();
    }

    public void DestroyTheNode()
    {
        if (!hitBarrier)
        {
            var destroyVFX = Instantiate(destroyVfx, vfx.transform.position, Quaternion.identity);
        }
        //destroyVFX.transform.rotation = Quaternion.Euler(0, 500, 0);
        Destroy(gameObject);
    }


    public void ResetShoot()
    {
        Level.instance.HookSystem.ResetShoot(0);
        Level.instance.HookSystem.ResetShoot(1);       
    }

    public void LookAt(Transform tf)
    {
        vfx.transform.LookAt(tf);
    }

    public void CheckHookNodeTarget()
    {
        //Debug.Log(transform.parent.gameObject.tag);
        if (transform.parent.gameObject.CompareTag("HookNode"))
        {
            Debug.Log("is Arrow");
            InitiateNodeDestroy();
        }


        var item = GetComponentInParent<Item>();     
        if (item != null)
        {
            Level.instance.HookSystem.ConnectionEstablished = true;
            switch (item.Data.ItemType)
            {
                case ItemType.Default:
                default:
                    if (item.Data.isHookable)
                    {
                        Level.instance.HookSystem.canPull = true;
                        PlayAudioEventsIfHoockable();
                        ResetShoot();
                    }
                    else
                    {
                        InitiateNodeDestroy();
                    }
                    break;

                case ItemType.Button:
                    break;

                case ItemType.Money:
                    break;

                case ItemType.Weapon:
                    break;

                case ItemType.Static:
                    if (item.Data.isHookable)
                    {
                        Level.instance.HookSystem.canPull = true;
                        PlayAudioEventsIfHoockable();
                        ResetShoot();
                    }
                    else
                    {
                        InitiateNodeDestroy();
                    }
                    break;

                case ItemType.Target:
                    var target = GetComponentInParent<Target>();
                    target.SetActive();
                    InitiateNodeDestroy();
                    break;
                case ItemType.Destructable:

                    InitiateNodeDestroy();                
                    var destructable = GetComponentInParent<DestructableItem>();
                    if (destructable)
                    {
                        destructable.Destroy();
                        //destructable.UpdatePoints();
                    }
                                    
                    var destructf = GetComponentInParent<DestructFun>();
                    if (!destructf) return;
                    destructf.Destroy();
                    //destructf.UpdatePoints();

                    break;
                case ItemType.Dissolving:
                    hitBarrier = true;
                    vfx.SetActive(false);
                    ropeConnectionPoint.SetActive(false);
                    InitiateNodeDestroy();
                    Level.instance.audioEvents.PlayAudioEvent("DissolveWallArrowRepell" , gameObject);
                    break;
            }
        }
        else
        {
            Debug.LogError("DESTROY");
            InitiateNodeDestroy();
        }
    }

    #endregion

    #region Audio

    public void PlayAudioEventsIfHoockable()
    {
        Level.instance.audioEvents.PlayAudioEvent("ArrowHitsHookable", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("HookConnectionGetEstablished", gameObject);

    }

    #endregion

}

