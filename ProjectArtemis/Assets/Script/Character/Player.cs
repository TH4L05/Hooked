using UnityEngine;

public class Player : Character, IDamageable
{
    #region Private Fields

    [Header("References")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Transform raySpawn;
    [SerializeField] private HookSystem hookscript;
    private RaycastHit hookCheckhit;

    #endregion

    #region Public Fields
    public Camera CameraMain => cameraMain;
    public bool CanShootHook { get; private set; }
    public HookSystem HookSystem => hookscript;
    public RaycastHit HookCheckRaycastHit => hookCheckhit;
    public Transform RaySpawn => raySpawn;

    #endregion

    #region Unity Functions

    private void Update()
    {
        //DrawInteractableRayCast();
        CheckHookShotDistance();
    }

    #endregion

    #region Setup

    protected override void StartSetup()
    {
        base.StartSetup();
        movement.SetData(data);
    }

    public void SetSensitivity(float value)
    {
        movement.SetSensitivity(value);
    }

    #endregion

    /*public void DrawInteractableRayCast()
    {
        Vector3 rayOrigin = raySpawn.position;
        Vector3 rayDirection = raySpawn.forward;        
        Ray ray = new Ray(rayOrigin, rayDirection);
        float rayDistanceMax = 3f;
        RaycastHit hit;
        Color rayColor = Color.blue;

        Debug.DrawRay(rayOrigin, rayDirection * rayDistanceMax, rayColor);
        if (Physics.Raycast(ray, out hit, rayDistanceMax))
        {
            //Debug.Log($"<color=teal>RaycastPlayer hit = {hit.collider.tag} + {hit.collider.name}</color>");

            ItemOnFocus = hit.collider.GetComponent<Item>();

            if (ItemOnFocus == null)
            {
                InteractableIsOnFocus = false;
                GameEvents.ShowInfoText?.Invoke(false, 0); 
                //GameEvents.ShowWeaponInfo?.Invoke(false, null);
                return;
            }
            else if(!ItemOnFocus.Data.isInteractable)
            {
                InteractableIsOnFocus = false;
                GameEvents.ShowInfoText?.Invoke(false, 0);
                //.instance.gameEvents.ShowWeaponInfo?.Invoke(false, null);
                return;
            }


            switch (ItemOnFocus.Data.ItemType)
            {
                case ItemType.Default:
                    InteractableIsOnFocus = true;
                    GameEvents.ShowInfoText?.Invoke(true, 1);
                    break;

                case ItemType.Button:
                    InteractableIsOnFocus = true;
                    GameEvents.ShowInfoText?.Invoke(true, 3);
                    break;

                case ItemType.Money:
                    InteractableIsOnFocus = true;
                    break;

                case ItemType.Weapon:
                    InteractableIsOnFocus = true;
                    //var weapon = hit.collider.GetComponentInChildren<Weapon>();
                    GameEvents.ShowInfoText?.Invoke(true, 2);                  
                    //GameEvents.ShowWeaponInfo?.Invoke(true, ItemOnFocus.gameObject);
                    break;

                default:
                    InteractableIsOnFocus = false;
                    ItemOnFocus = null;
                    GameEvents.ShowInfoText?.Invoke(false, 0);
                    //GameEvents.ShowWeaponInfo?.Invoke(false, null);
                    break;
            }
        }
        else
        {
            InteractableIsOnFocus = false;
            ItemOnFocus = null;
            //GameEvents.ShowWeaponInfo?.Invoke(false, null);
            GameEvents.ShowInfoText?.Invoke(false, 0);           
        }
    }*/

    public void UpdateInputValues(Vector2 input, Vector2 rotation, bool jumpPressed, bool sprintPressed)
    {
        movement.UpdateValues(input, rotation, jumpPressed, sprintPressed);
    }

    public void CheckHookShotDistance()
    {
        Vector3 rayOrigin = raySpawn.position;
        Vector3 rayDirection = raySpawn.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        Color rayColor;
        float distance = 0;

        rayColor = Color.magenta;
        if (Physics.Raycast(ray, out hookCheckhit))
        {          
            distance = Vector3.Distance(transform.position, hookCheckhit.point);

            GameEvents.EventOnUpdateCrosshair?.Invoke(distance);

            if (distance < hookscript.RayCastRange)
            {
                
                CanShootHook = true;
                var item = hookCheckhit.collider.GetComponent<Item>();
                if (item != null && item.Data.isHookable)
                {                   
                    rayColor = Color.green;
                    GameEvents.EventOnUpdateCrosshair?.Invoke(0);
                    /*switch (hookscript.HookNodeCount)
                    {
                        case 0:
                            GameEvents.EventOnCrosshairReset?.Invoke();
                            break;
                        case 1:
                            GameEvents.EventOnCrosshairSetLastActive?.Invoke();
                            break;
                        case 2:
                            GameEvents.EventOnCrosshairAllHooksActive?.Invoke();
                            break;
                        default:
                            GameEvents.EventOnCrosshairReset?.Invoke();
                            break;
                    }*/

                }
                else
                {
                    rayColor = Color.red;
                }
            }
            else
            {
                CanShootHook = false;
                rayColor = Color.gray;
                GameEvents.EventOnUpdateCrosshair?.Invoke(50);
            }


        }    
        Debug.DrawRay(rayOrigin, rayDirection * 200f, rayColor);
    }

    public void HandleHookShotMovement(Transform hookNode, float forceMultiplier)
    {
        movement.HookShotMove(hookNode, forceMultiplier);
    }

    public void CancelHookShotMovement()
    {
        movement.CancelHookShotMovement();
    }

    public void ResetMovementValues()
    {    
        GameEvents.PlayAnimation("Idle", 0, 0, false);
        movement.ResetValues();
    }

    public void StopMovementValues()
    {
        Level.instance.inputHandler.PauseAll = true;
        GameEvents.PlayAnimation("Idle", 0, 0, false);
        movement.ResetValuesAndStop();
    }
}
