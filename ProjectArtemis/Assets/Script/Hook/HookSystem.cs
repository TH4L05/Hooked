using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    #region Serialized Fields

    [Header("Parameters")]

    [SerializeField] private HookSystemData hookData;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform playerHookpoint;
    [SerializeField] private Transform raycastStart;

    [Header("Info")]
    [SerializeField] private List<GameObject> activeHookNodes = new List<GameObject>();
    public bool canPull;
    public bool canShootLeftHook;
    public bool canShootRightHook;

    [Header("VFX")]
    [SerializeField] private List<RopeLine> lineVFX = new List<RopeLine>();
    [SerializeField] private List<GameObject> effects = new List<GameObject>();
    [SerializeField] private GameObject arrowVFX;
    [SerializeField] private GameObject arrowVFXParticles;
    [SerializeField] private GameObject ropeDestroyVfx;
    [SerializeField] private List<Material> vfxMaterialsArrowLeft = new List<Material>();
    [SerializeField] private List<Material> vfxMaterialsArrowRight = new List<Material>();  
    [SerializeField] private float LineForm = 1.0f;
    [SerializeField] public float Tiling = 0.1f;
    [SerializeField] public float ColorStrength = 0f;
    [SerializeField] public float DistortionAmount = 0.05f;
    [SerializeField] private Animator lineAnimator;
    [SerializeField] private Animator EmissionAnimator;
    public bool ConnectionEstablished { get; set; }

    #endregion

    #region Fields

    private int hookNodeCount = 0;
    private LineRenderer lineRenderer1;
    private LineRenderer lineRenderer2;
    private GameObject hooknode1;
    private GameObject hooknode2;
    private float distance;
    private bool swapped;
    private Gradient default1;
    private Gradient default2;
    private bool pulltoObject;
    private bool pulltoPlayer;
    private bool onDestroy;

    public float RayCastRange => hookData.rayCastRange; 
    public bool OnHookObjectsTogether { get; set; }
    public int HookNodeCount => hookNodeCount;

    public bool intendedDisconnection = false;

    //public bool CanPull { get; set; }

    #endregion Fields

    #region UnityFunctions

    private void Start()
    {
        canShootLeftHook = true;
        canShootRightHook = true;
        default1 = lineVFX[1].lineGradient;
        default2 = lineVFX[0].lineGradient;
    }

    private void Update()
    {
        if (pulltoObject)
        {
            HookObjectsTogether();
        }
        else if (pulltoPlayer)
        {
            PullObjectToCharakter();
        }      

        if (lineRenderer1)
        {
            lineRenderer1.material.SetFloat("_CutoutStrength", LineForm);
            lineRenderer1.material.SetFloat("_Tiling", Tiling);
            lineRenderer1.material.SetFloat("_ColorStrength", ColorStrength);
            lineRenderer1.material.SetFloat("_DistortionAmount", DistortionAmount);
        }
        else if (lineRenderer2)
        {
            lineRenderer2.material.SetFloat("_CutoutStrength", LineForm);
            lineRenderer2.material.SetFloat("_Tiling", Tiling);
            lineRenderer2.material.SetFloat("_ColorStrength", ColorStrength);
            lineRenderer2.material.SetFloat("_DistortionAmount", DistortionAmount);
        }
    }

    private void LateUpdate()
    {
        if(hookNodeCount == 0) return;
        if (!ConnectionEstablished) return;
        DrawLine();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, hookData.rayCastRange);
    }

    #endregion

    #region CreateHookNodes

    /*public void CreateHookNode(string pos)
    {
        if (pos == "left")
        {
            hookAudio.clip = sounds[0];
            CreateLeftHook();
        }
        else if (pos == "right")
        {
            hookAudio.clip = sounds[0];
            CreateRightHook();
        }
    }*/


    /// <summary>
    /// Shoots a Ray if projectileShooting is used
    /// </summary>
    /// <returns></returns>
    private Vector3 ShootRay()
    {
        Vector3 targetPoint = Vector3.zero;
        Ray ray = Level.instance. player.CameraMain.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ~playerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        return targetPoint;
    }

    /// <summary>
    /// Create a HookNode Instance (used for projectileShooting)
    /// </summary>
    /// <param name="index">HookNodeIndex</param>
    public void CreateHookInstance(int index)
    {
        var hooknode = Instantiate(hookData.hookNodeTemplates[index], projectileSpawn.position, Quaternion.identity);
        activeHookNodes.Add(hooknode);
        //hooknode.GetComponent<HookNode>().SetHookNodeIndex(index);

        Vector3 direction = ShootRay() - projectileSpawn.position;
        direction.Normalize();

        var rb = hooknode.GetComponent<Rigidbody>();
        rb.AddForce(direction * hookData.hookNodeSpeed, ForceMode.Impulse);

        if (index == 0)
        {
            hooknode1 = hooknode;
            GameEvents.EventOnLeftHookChanged?.Invoke(true);
        }
        else
        {
            hooknode2 = hooknode;
            GameEvents.EventOnRightHookChanged?.Invoke(true);
        }
        GameEvents.IconsUsable?.Invoke();

    }

    /// <summary>
    /// Destroys the left hooknode
    /// </summary>
    public void DestroyLeftHook()
    {
        if (hooknode1 != null)
        {
            lineAnimator.Play("line_break");
            ConnectionEstablished = false;
            Level.instance.player.CancelHookShotMovement();
            GameEvents.EventOnLeftHookChanged?.Invoke(false);
            //CancelInvoke("HookObjectsTogether");
            //CancelInvoke("PullObjectToCharakter");
            CancelPull();          
            activeHookNodes.Remove(hooknode1);
            Destroy(hooknode1,0.25f);
            hookNodeCount--;           
            hooknode1 = null;
            canPull = false;
        }
    }

    /// <summary>
    /// Creates a hookNode if PrimaryFire is pressed
    /// </summary>
    /// 
    public void CreateLeftHook()
    {
        if (!canShootLeftHook) return;
        SwapGradients(0);
        CreateHookNode(Level.instance. player.HookCheckRaycastHit, 0);
        canShootLeftHook = false;       
        //CreateHookInstance(0);
        
    }

    /// <summary>
    /// Destroys the right hooknode
    /// </summary>
    public void DestroyRightHook()
    {
        if (hooknode2 != null)
        {
            lineAnimator.Play("line_break");
            ConnectionEstablished = false;
            Level.instance.player.CancelHookShotMovement();
            //CancelInvoke("HookObjectsTogether");
            //CancelInvoke("PullObjectToCharakter");
            CancelPull();
            
            GameEvents.EventOnRightHookChanged?.Invoke(false);
            activeHookNodes.Remove(hooknode2);
            Destroy(hooknode2, 0.25f);
            hookNodeCount--;          
            hooknode2 = null;
            canPull = false;
        }
    }

    /// <summary>
    /// Creates a hookNode if SecondaryFire is pressed
    /// </summary>
    public void CreateRightHook()
    {
        if (!canShootRightHook) return;
        SwapGradients(1);
        CreateHookNode(Level.instance. player.HookCheckRaycastHit, 1);
        canShootRightHook = false;            
        //CreateHookInstance(1);
    }

    /// <summary>
    /// Create a HookNode Instance (used for raycastShooting)
    /// </summary>
    /// <param name="raycastHit">Raycast hit information</param>
    /// <param name="indx">Hooknode index</param>
    public void CreateHookNode(RaycastHit raycastHit, int indx)
    {
        Transform playerPosOnShoot = Level.instance. player.transform;
        float pivotRotationOnShoot = Level.instance.player.CameraMain.transform.parent.rotation.eulerAngles.x;

        var hookNode = Instantiate(hookData.hookNodeTemplates[indx], raycastHit.point, playerPosOnShoot.rotation);
        hookNode.transform.Rotate(Vector3.right, pivotRotationOnShoot);
        hookNode.transform.parent = raycastHit.collider.transform;
        var node = hookNode.GetComponent<HookNode>();
        node.SetHookNodeIndex(indx);

        activeHookNodes.Add(hookNode);
        hookNodeCount++;

        if (indx == 0)
        {
            hooknode1 = hookNode;           
            GameEvents.EventOnLeftHookChanged?.Invoke(true);         
            InstaniateImpactVfx(raycastHit, indx);
        }
        else
        {
            hooknode2 = hookNode;
            GameEvents.EventOnRightHookChanged?.Invoke(true);          
            InstaniateImpactVfx(raycastHit, indx);
        }

        //Level.instance.audioEvents.PlayAudioEvent("ActiveConnection", gameObject);
    }

    #endregion

    #region InteractBehaviour

    /// <summary>
    /// Pulls an non static hookable object towards the Player
    /// </summary>
    public void PullObjectToCharakter()
    {
        if (!canPull) return;

        distance = Vector3.Distance(activeHookNodes[0].transform.position, Level.instance. player.transform.position);
        //GameEvents.PlayAnimation("PullObjectStart", 0, 0, false);

        if (distance > hookData.minDistanceBeforeCut)
        {
            Vector3 direction = activeHookNodes[0].transform.position - Level.instance. player.transform.position;
            direction.Normalize();

            var rb = activeHookNodes[0].transform.GetComponentInParent<Rigidbody>();
            rb.isKinematic = false;
            rb.drag = hookData.dragOnHook;
            if(hookData.deactivateGravityOnHook) rb.useGravity = false;


            float force = 1f;
            if (hookData.ForceMultiplierByObjMass)
            {
                force = Mathf.Clamp(rb.mass * 2, 2, hookData.forceMultiplierMax);
            }
            else
            {
                force = hookData.forceMultiplier;
            }

            rb.AddForceAtPosition(-direction * force * hookData.extraforceMultiplier * Time.deltaTime, activeHookNodes[0].transform.position);
        }
        else
        {
            PlayLineDestroyAnimation();
        }
    }

    /// <summary>
    /// Pulls objects together
    /// </summary>
    public void AddForceToHookedObjects()
    {
        if (hookNodeCount == 0) return;
        
        if (canPull)
        {

            if (hookNodeCount == 1)
            {
                //GameEvents.PlayAnimation("PullStart", 0, 0, false);
                var item = activeHookNodes[0].transform.parent.GetComponent<Item>();

                if (item != null & item.Data.ItemType == ItemType.Static)
                {
                    AddForceToCharacter(activeHookNodes[0].transform);
                }
                else
                {
                    pulltoPlayer = true;
                    GameEvents.PlayAnimation("PullObjectStart", 0, 0, false);
                    //InvokeRepeating("PullObjectToCharakter", 0, 0.1f);
                }
                return;
            }

            pulltoObject = true;
            GameEvents.PlayAnimation("PullObjectStart", 0, 0, false);
            //InvokeRepeating("HookObjectsTogether", 0, 0.1f);

        }
        else
        {
            CancelPull();

        }     
    }

    /// <summary>
    /// Pulls two non static hookable objects together
    /// </summary>
    private void HookObjectsTogether()
    {
        if(!canPull) return;

        //GameEvents.PlayAnimation("PullObjectStart", 0, 0, false);
        distance = Vector3.Distance(activeHookNodes[0].transform.position, activeHookNodes[1].transform.position);

        if (distance > hookData.minDistanceBeforeCut)
        {
            for (int i = 0; i < hookNodeCount; i++)
            {
                Vector3 direction = Vector3.zero;

                if (i + 1 > hookNodeCount - 1)
                {
                    direction = activeHookNodes[0].transform.position - activeHookNodes[i].transform.position;
                }
                else
                {
                    direction = activeHookNodes[i + 1].transform.position - activeHookNodes[i].transform.position;
                }
                direction.Normalize();

                var rb = activeHookNodes[i].transform.GetComponentInParent<Rigidbody>();
                if (rb == null) continue;
                rb.isKinematic = false;
                rb.drag = hookData.dragOnHook;
                if (hookData.deactivateGravityOnHook) rb.useGravity = false;


                float force = 1f;
                if (hookData.ForceMultiplierByObjMass)
                {
                    force = Mathf.Clamp(rb.mass * 2, 2, hookData.forceMultiplierMax);
                }
                else
                {
                    force = hookData.forceMultiplier;
                }
                
                rb.AddForceAtPosition(direction * force * hookData.extraforceMultiplier * Time.deltaTime, activeHookNodes[i].transform.position);
            }
        }
        else
        {
            PlayLineDestroyAnimation();
        }
    }

    /// <summary>
    /// Set the hookmode in Player to pull him to a static hookable object
    /// </summary>
    /// <param name="hooknode"></param>
    public void AddForceToCharacter(Transform hooknode)
    {
        if (hookNodeCount == 0 | activeHookNodes[0] == null) return;

        CancelPull();
        Level.instance. player.HandleHookShotMovement(hooknode, hookData.forceMultiplier);
        //GameEvents.EventOnLeftHookChanged.Invoke(false);
        //GameEvents.EventOnRightHookChanged.Invoke(false);
    }

    #endregion

    #region Cancel & Reset

    public void CancelPull()
    {
        if (!ConnectionEstablished) return;
        //Debug.Log("PullCancled");
        pulltoPlayer = false;
        pulltoObject = false;

        GameEvents.PlayAnimation("PullObjectStop", 0, 0, false);


        for (int i = 0; i < hookNodeCount; i++)
        {
            var rb = activeHookNodes[i].transform.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.drag = 0.2f;
            if (hookData.deactivateGravityOnHook) rb.useGravity = true;
        }
    }

    public void SetPull(bool status)
    {
        canPull = status;
    }

    public void ResetShoot(int index)
    {
        if (index == 0)
        {
            canShootLeftHook = true;
        }
        else
        {
            canShootRightHook = true;
        }
    }

    /// <summary>
    /// destroys all created hooknodes
    /// </summary>
    public void DestroyHookNodes(float time)
    {
        canPull = false;
        CancelPull();
        Debug.Log("<color=magenta>HookNodesGetsDestroyed</color>");
        ConnectionEstablished = false;

        //CancelInvoke("PullObjectToCharakter");
        //CancelInvoke("HookObjectsTogether");
        pulltoPlayer = false;
        pulltoObject = false;
        hooknode1 = null;
        hooknode2 = null;
        lineRenderer1 = null;
        lineRenderer2 = null;

        hookNodeCount = 0;
                          
        foreach (var hook in activeHookNodes)
        {
            if (hook != null)
            {
                var rb = hook.transform.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    rb.drag = 0.2f;
                    if (hookData.deactivateGravityOnHook) rb.useGravity = true;
                }

                var hookNode = hook.GetComponent<HookNode>();
                hookNode.DestroyTheNode();
            }
            else
            {
                continue;
            }
        }

        activeHookNodes.Clear();
        GameEvents.EventOnLeftHookChanged?.Invoke(false);
        GameEvents.EventOnRightHookChanged?.Invoke(false);
        ResetShoot(0);
        ResetShoot(1);
    }

    public void PlayLineDestroyAnimation()
    {
        Debug.LogError("PLAYLINEDESTROY");

        if (ConnectionEstablished)
        {
            lineAnimator.Play("line_break");
        }
        else
        {
            intendedDisconnection = false;
            DestroyHookNodes(0);
        }

        /*for (int i = 0; i < hookNodeCount; i++)
        {
            var hookNode = activeHookNodes[i].GetComponent<HookNode>();
            Instantiate(hookNode.DestroyVFX, hookNode.VFXobj.transform.position, Quaternion.identity);
        }*/
    }

    public void PlayConnectionGetsDisabledSound()
    {
        Level.instance.audioEvents.PlayAudioEvent("StopConnection");
        if (intendedDisconnection)
        {
            Level.instance.audioEvents.PlayAudioEvent("ConnectionGetsDisabled", gameObject);
            intendedDisconnection = false;
        }
        else
        {
            Level.instance.audioEvents.PlayAudioEvent("ConnectionGetsInterrupted", gameObject);
        }

    }
    #endregion

    #region Visual

    /// <summary>
    /// Draws a line with LineRenderer between hookNodes
    /// </summary>
    private void DrawLine()
    {
        if (hookNodeCount == 0)
        {
            ConnectionEstablished = false;
             return;
        }

        LineRenderer lr = null;

        try
        {
            lr = activeHookNodes[0].GetComponent < LineRenderer>();

        }
        catch (System.Exception)
        {
            PlayLineDestroyAnimation();
            return;
        }

        if (lr == null && ConnectionEstablished)
        {
            lineRenderer1 = activeHookNodes[0].AddComponent<LineRenderer>();
            lineRenderer1.material = lineVFX[0].lineMaterial;
            lineRenderer1.colorGradient = lineVFX[0].lineGradient;
            lineRenderer1.startWidth = lineVFX[0].lineStartWidth;
            lineRenderer1.endWidth = lineVFX[0].lineEndWidth;
            lineRenderer1.widthMultiplier = lineVFX[0].lineCurveWidthMultiplier;
            lineRenderer1.widthCurve = lineVFX[0].lineCurve;
            lineRenderer1.textureMode = lineVFX[0].lineTextureMode;

            


            //lineRenderer2 = activeHookNodes[0].AddComponent<LineRenderer>();
            lineRenderer2 = activeHookNodes[0].transform.GetChild(0).gameObject.AddComponent<LineRenderer>();
            lineRenderer2.material = lineVFX[1].lineMaterial;
            lineRenderer2.colorGradient = lineVFX[1].lineGradient;
            lineRenderer2.startWidth = lineVFX[1].lineStartWidth;
            lineRenderer2.endWidth = lineVFX[1].lineEndWidth;
            lineRenderer2.widthMultiplier = lineVFX[1].lineCurveWidthMultiplier;
            lineRenderer2.widthCurve = lineVFX[1].lineCurve;
            lineRenderer2.textureMode = lineVFX[1].lineTextureMode;

         

            if (LineForm <= 1)
            {
                lineAnimator.Play("line_form");
            }
        }

        var node = activeHookNodes[0].GetComponent<HookNode>();
        lineRenderer1.SetPosition(0, node.RopeConnectionPoint.transform.position);
        lineRenderer2.SetPosition(0, node.RopeConnectionPoint.transform.position);

        if (hookNodeCount == 1)
        {
            node.LookAt(Level.instance.player.transform);

            lineRenderer1.SetPosition(1, playerHookpoint.position);
            lineRenderer2.SetPosition(1, playerHookpoint.position);

            var playerRayPoint = Level.instance. player.RaySpawn;
            CheckDirectVisibility(activeHookNodes[0].transform, playerRayPoint);
        }
        else
        {
            var node2 = activeHookNodes[1].GetComponent<HookNode>();

            node.LookAt(hooknode2.transform);
            node2.LookAt(hooknode1.transform);

            lineRenderer1.SetPosition(1, node2.RopeConnectionPoint.transform.position);
            lineRenderer2.SetPosition(1, node2.RopeConnectionPoint.transform.position);
            CheckDirectVisibility(activeHookNodes[0].transform, activeHookNodes[1].transform);
        }
    }


    /// <summary>
    /// Set the Vfx and Visual Animation depending on which arrow is shoot
    /// </summary>
    /// <param name="indx">index of shot (0 = left / 1= right)</param>
    public void SetArrowVFxMaterial(int indx)
    {
        if (arrowVFX == null) return;
        var mr = arrowVFX.GetComponent<MeshRenderer>();
        var mrp = arrowVFXParticles.GetComponent<ParticleSystemRenderer>();

        switch (indx)
        {
            case 0:
                mr.material = vfxMaterialsArrowLeft[0];
                mrp.material = vfxMaterialsArrowLeft[1];
                EmissionAnimator.SetBool("orange",true);
                break;

            case 1:
                mr.material = vfxMaterialsArrowRight[0];
                mrp.material = vfxMaterialsArrowRight[1];
                EmissionAnimator.SetBool("blue",true);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Checks the direct connection between hooknodes with and raycast. If connection gets interrupted hooknodes and connection gets destroyed
    /// </summary>
    /// <param name="transform1">first transform</param>
    /// <param name="transform2">second transform</param>
    private void CheckDirectVisibility(Transform transform1, Transform transform2)
    {
        float distance = 1f;
        distance = Vector3.Distance(transform1.position, transform2.position);
        //Debug.Log(distance);

        //UpdateMaterial(distance);
        if (distance > 49.5)
        {
            PlayLineDestroyAnimation();
        }
        else
        {
            Vector3 rayStart = transform2.position;
            Vector3 rayDirection = (transform1.position - transform2.position).normalized;          
            Ray ray = new Ray(rayStart, rayDirection);
            RaycastHit rayhit;

            //Debug.DrawLine(transform2.position, transform1.position, Color.magenta);
            if (Physics.Raycast(rayStart, rayDirection, out rayhit))
            {    
                //Debug.Log(rayhit.collider.name + " " + rayhit.collider.tag);

                if (!rayhit.collider.CompareTag("HookNode") && !rayhit.collider.CompareTag("Player"))
                {
                    Debug.Log("<color=magenta>HookNodes lost Direct Connection</color>");
                    /*if (hookNodeCount == 1)
                    {
                        Level.instance.audioEvents.PlayAudioEvent("PlayerConnectionGetsInterrupted", gameObject);
                    }

                    for (int i = 0; i < hookNodeCount; i++)
                    {
                        var hookNode = activeHookNodes[i].GetComponent<HookNode>();
                        hookNode.audioEvents[4].HandleEvent(hookNode.audioEvents[4].gameObject);
                    }*/
                    PlayLineDestroyAnimation();
                }
            }
            else
            {
                PlayLineDestroyAnimation();
            }
        }
    }

    /// <summary>
    /// Instancites an VFX on raycast hit position and based on what arrow was shot
    /// </summary>
    /// <param name="hit">the raycast hit</param>
    /// <param name="indx">index of shot (0 = left / 1= right)</param>
    public void InstaniateImpactVfx(RaycastHit hit, int indx)
    {
        if (hit.transform.tag == "InteractCube" || hit.transform.tag == "Companion" || hit.collider.GetComponent("DestructableItem"))
        {
            if (indx == 0)
            {
                if (effects[0] == null) return;
                var vfx = Instantiate(effects[2], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(vfx, 5.1f);
            }
            else
            {
                if (effects[1] == null) return;
                var vfx = Instantiate(effects[3], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(vfx, 5.1f);
            }
        }
        else {
            if (indx == 0)
            {
                if (effects[0] == null) return;
                var vfx = Instantiate(effects[0], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(vfx, 5.1f);
            }
            else
            {
                if (effects[1] == null) return;
                var vfx = Instantiate(effects[1], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(vfx, 5.1f);
            }
        }
    }

    /// <summary>
    /// Swaps the ColorGradient for LineRenderer based on HooknodeIndex
    /// </summary>
    /// <param name="indx"></param>
    public void SwapGradients(int indx)
    {
        if (indx == 0 || indx == 2)
        {
            lineVFX[0].lineGradient = default1;
            lineVFX[1].lineGradient = default1;
        }
        else
        {
            lineVFX[0].lineGradient = default2;
            lineVFX[1].lineGradient = default2;
        }

    }

    public void InstanciateRopeDestroyVfx()
    {
        if (ropeDestroyVfx == null)
        {
            Debug.LogError("!! VFX Reference is Missing !!");
            return;
        }

        if (hookNodeCount == 1)
        {
            var particle = Instantiate(ropeDestroyVfx, playerHookpoint.transform.position, Quaternion.identity);
            if (particle != null)
            {
                var ropevfx = particle.GetComponent<RopeVfx>();
                if (ropevfx != null)
                {
                    if (activeHookNodes[0] != null)
                    {
                        ropevfx.position = activeHookNodes[0].transform.position;
                        ropevfx.Move();
                    }                    
                }
            }
        }
        else if (hookNodeCount == 2)
        {
            if (ropeDestroyVfx == null)
            {
                var particle = Instantiate(ropeDestroyVfx, activeHookNodes[0].transform.position, Quaternion.identity);
                if (particle != null)
                {
                    var ropevfx = particle.GetComponent<RopeVfx>();
                    if (ropevfx != null)
                    {
                        ropevfx.position = activeHookNodes[1].transform.position;
                        ropevfx.Move();
                    }
                }
            }

        }
    }

    #endregion
}
