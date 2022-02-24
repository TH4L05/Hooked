using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum MovementState
    {
        Default,
        HookShot,
    }

    #region Public Fields

    [Header("General")]    
    [HideInInspector] [SerializeField] private Rigidbody playerBody;
    [HideInInspector] [SerializeField] private CapsuleCollider playerBodyCollider;
    [SerializeField] private Transform rotationPivot;

    [Header("Options")]
    [HideInInspector][SerializeField] private bool useCharacterController = true;
    [SerializeField] private bool canSprint;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canSlide;

    [Header("Audio")]
    private float accumulated_Distance = 1f;
    private float step_Distance = 0f;
    [SerializeField] private float walk_step_Distance = 1f;
    [SerializeField] private float sprint_step_Distance = 0.5f;
    [SerializeField] private float crouch_step_Distance = 1.5f;

    #endregion

    #region Private Fields

    private CharacterData characterData;
    private MovementState movementState;
    private CharacterController playerController;
    private float sensitivity = 10f;

    private Vector2 axisInput;
    private Vector2 axisInputMouse;

    private float speed = 1f;
    private float animSpeed = 1f;
    private float lastspeed = 1f;

    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private bool isGrounded;
    private bool jump;
    private bool jumpLastFrame;
    private bool jumpReady;
    private bool firstAerialFrame;
    private bool landingFrame;
    private bool sprint;
    private bool hookshot;
    
    private Vector3 momentum = Vector3.zero;
    private Vector3 verticalVelocity = Vector3.zero;
    private Vector3 characterVelocity = Vector3.zero;
    private Vector3 lastmovement = Vector3.zero;
    private float xRotation = 0f; 
    private float drag;

    private bool isSliding;
    private Vector3 SlopeHitNormal = Vector3.zero;


    #endregion

    #region Unity Functions

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {        
        movementState = MovementState.Default;
        step_Distance = walk_step_Distance;
    }

    private void Update()
    {
        CheckMovementState();
    }

    #endregion

    #region Movement

    public void ResetValues()
    {
       
        momentum = Vector3.zero;
        verticalVelocity = Vector3.zero;
        characterVelocity = Vector3.zero;
        lastmovement = Vector3.zero;
    }

    public void ResetValuesAndStop()
    {
        playerController.enabled = false;
        momentum = Vector3.zero;
        verticalVelocity = Vector3.zero;
        characterVelocity = Vector3.zero;
        lastmovement = Vector3.zero;
        playerController.enabled = true;
        UpdateValues(Vector2.zero, Vector2.zero, false, false) ;
        playerController.enabled = false;
    }

    private void GetComponents()
    {
        playerController = GetComponent<CharacterController>();
        playerBody = GetComponent<Rigidbody>();
        if (playerController != null && !useCharacterController)
        {
            playerController.enabled = false;

        }

    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }

    public void SetData(CharacterData data)
    {
        characterData = data;
        speed = characterData.move_speed;
    }

    private void CheckMovementState()
    {
        switch (movementState)
        {
            case MovementState.Default:
            default:

                GameEvents.PlayAnimation?.Invoke("PullEnd", 0, 0, false);

                landingFrame = false;
                firstAerialFrame = false;
                jumpLastFrame = false;

                bool wasGrounded = isGrounded;
                GroundCheck();

                if (isGrounded && !wasGrounded)
                {
                    landingFrame = true;
                    Level.instance.audioEvents.PlayAudioEvent("PlayerLand", gameObject);
                    //Level.instance.audioEvents.PlayAudioEvent("PlayerSlide", gameObject);
                    //GameEvents.PlayAudioEvent?.Invoke("PlayerLand",gameObject);
                    //GameEvents.PlayAudioEvent?.Invoke("PlayerSlide", gameObject);
                    GameEvents.PlayAnimation?.Invoke("JumpEnd", 0, 0, true);
                    verticalVelocity.y = -1;
                    jumpReady = true;
                }
                if (wasGrounded && !isGrounded)
                {
                    firstAerialFrame = true;
                }

                SlideCheck();

                if (canJump)
                {
                    JumpCheck();
                }
                if (canSprint)
                {
                    SprintOnInput();
                }

                UpdateMovement();
                UpdateRotation();

                break;

            case MovementState.HookShot:

                GameEvents.PlayAnimation?.Invoke("PullEnd", 0, 0, true);

                HookShootMovement();
                UpdateRotation();
                break;
        }
    }

    public void UpdateValues(Vector2 axisInput, Vector2 axisInputMouse, bool jumpPressed, bool sprintPressed)
    {
        this.axisInput = axisInput;
        this.axisInputMouse = axisInputMouse;
        jump = jumpPressed;
        if (jumpPressed) jumpButtonPressedTime = Time.time;
        sprint = sprintPressed;
    }

    private void UpdateMovement()
    {      
        Vector3 horizontalVelocity = transform.right * axisInput.x + transform.forward * axisInput.y;
        if (horizontalVelocity == Vector3.zero)
        {
            animSpeed = 0f;
        }
        else
        {
            animSpeed = speed;
        }


        if (useCharacterController)
        {
            if (isGrounded)
            {
                characterVelocity = horizontalVelocity * speed;
            }
            else
            {
                if (Time.timeScale > 0)
                { 
                    momentum += horizontalVelocity * characterData.aerial_control;
                }
            }

            if (landingFrame)
            {
                momentum -= horizontalVelocity * speed;
            }

            if (jumpLastFrame || firstAerialFrame)
            {
                characterVelocity = Vector3.zero;
                momentum.x = lastmovement.x; momentum.z = lastmovement.z;
            }

            characterVelocity.y = verticalVelocity.y;

            if (canSlide && isSliding)
            {
                //slide
                characterVelocity += new Vector3(SlopeHitNormal.x, -SlopeHitNormal.y, SlopeHitNormal.z) * characterData.slide_speed;
            }
            else
            {
                //Add momentum
                characterVelocity += momentum;
            }

            //Handle Movement Wind
            Level.instance.audioEvents.SetAudioParameter("GeneralVelocity", characterVelocity.magnitude);
            if (characterVelocity.magnitude >= 8f)
            {
                Level.instance.audioEvents.PlayAudioEvent("PlayerMoveWind", gameObject);
            }
            else
            {
                Level.instance.audioEvents.PlayAudioEvent("PlayerMoveWindStop", gameObject);
            }

            lastmovement = characterVelocity;
            characterVelocity *= Time.deltaTime;         
            playerController.Move(characterVelocity);
            GameEvents.PlayAnimation?.Invoke("Move",animSpeed,0,false);

            //Damp momentum
            if (momentum.magnitude >=0f)
            {
                //prevent character from sliding on the ground for too long
                if (isGrounded)
                {
                    momentum.y = 0f;
                    momentum -= momentum * characterData.groundfriction * Time.deltaTime;

                    Level.instance.audioEvents.SetAudioParameter("HorizontalMomentum", momentum.magnitude);
                   
                    if ((horizontalVelocity * speed).magnitude > momentum.magnitude)
                    {
                        momentum = Vector3.zero;
                    }
                }
                else
                {
                    momentum -= momentum * drag * Time.deltaTime;
                    //Level.instance.audioEvents.PlayAudioEvent("PlayerSlideStop", gameObject);
                }               
            }           

            if (momentum.magnitude < 0.1f)
            {
                momentum = Vector3.zero;
            }
        }
        else
        {
            if (isGrounded)
            {
                playerBody.velocity = horizontalVelocity * speed * Time.deltaTime;
            }           
        }

        characterData.position = transform.position;
    }

    private void UpdateRotation()
    {
        var rotations = axisInputMouse * sensitivity * Time.deltaTime;
        xRotation -= rotations.y;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        transform.Rotate(Vector3.up, rotations.x);
        rotationPivot.eulerAngles = targetRotation;
        characterData.rotation = new Vector3(xRotation, rotations.x, 0);
    }

    private void JumpCheck()
    {
        if (useCharacterController)
        {
            if (jumpReady)
            {
                if (Time.time - lastGroundedTime <= characterData.coyoteTimeframe)
                {
                    if (Time.time - jumpButtonPressedTime <= characterData.jumpBufferTimeframe)
                    {
                        jumpLastFrame = true;
                        lastGroundedTime = null;
                        jumpButtonPressedTime = null;
                        jumpReady = false;
                        drag = characterData.dragdefault;
                        momentum = lastmovement;
                        verticalVelocity.y = Mathf.Sqrt(-2 * characterData.jump_force * -characterData.gravity);
                        Level.instance.audioEvents.PlayAudioEvent("PlayerJump", gameObject);                  
                        GameEvents.PlayAnimation?.Invoke("JumpStart", 0, 0, true);
                    }
                }
            }
        }
        else
        {
            JumpOnInput();
        }
    }

    private void JumpOnInput()
    {
        if (jump)
        {
            
            if (isGrounded) playerBody.velocity += characterData.jump_force * Vector3.up;         
            jump = false;
        }
    }

    private void SprintOnInput()
    {
        if (sprint)
        {
            speed = characterData.sprint_speed;
        }
        else
        {
            speed = characterData.move_speed;
        }
    }

    private void GroundCheck()
    {

        if (useCharacterController)
        {
            if (playerController.isGrounded)
            {
                isGrounded = true;
                lastGroundedTime = Time.time;
                drag = characterData.dragdefault;
                lastspeed = speed;
                PlayFootStepSound(axisInput);
            }
            else
            {
                isGrounded = false;
                verticalVelocity.y += -characterData.gravity * Time.deltaTime;
                lastspeed = speed;
            }
        }
        else
        {
            float offset = 0.01f;
            var origin = playerBodyCollider.center;
            var distance = playerBodyCollider.bounds.min.y + offset;
            RaycastHit hit;
            Color rayColor = Color.red;

            if (Physics.Raycast(origin, playerBodyCollider.transform.TransformDirection(Vector3.down), out hit ,distance))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    rayColor = Color.green;
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
            Debug.DrawRay(origin, Vector3.down * distance, rayColor);
        }
    }

    private void SlideCheck()
    {
        if (playerController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
        {
            SlopeHitNormal = slopeHit.normal;
            if (Vector3.Angle(slopeHit.normal, Vector3.up) > playerController.slopeLimit)
            {
                isSliding = true;
            }           
        }
        else
        {
            isSliding = false;
        }
    }

    private void PlayFootStepSound(Vector2 moveInput)
    {
        if (jump) return;

        if (moveInput == Vector2.zero)
        {
            accumulated_Distance = 0f;
            return;
        }

        accumulated_Distance += Time.deltaTime;

        if (accumulated_Distance > step_Distance)
        {
            accumulated_Distance = 0f;
            Level.instance.audioEvents.PlayAudioEvent("PlayerFootStep",gameObject);
        }
    }

    #endregion

    #region HookMovement

    Vector3 direction;
    Transform hookNode;
    float hookshotSpeed;
    float distance;
    float hookSpeed;

    public void HookShotMove(Transform hookNode, float forcceMultiplier)
    {   
        this.hookNode = hookNode;
        direction = hookNode.position - transform.position;
        hookshotSpeed = forcceMultiplier;
        movementState = MovementState.HookShot;
        GameEvents.PlayAnimation?.Invoke("PullStart",0,0,true);
    }

   

    private void HookShootMovement()
    {
        if (hookNode != null)
        {
            drag = characterData.draghook;
            isGrounded = false;
            distance = Vector3.Distance(transform.position, hookNode.position);

            float minMoveSpeed = 10f;
            float maxMoveSpeed = 40f;

            hookSpeed = Mathf.Clamp(distance, minMoveSpeed, maxMoveSpeed);
            float speedMultiplier = 2f;

            if (distance < 4f)
            {
                CancelHookShotMovement();
                Level.instance.HookSystem.PlayLineDestroyAnimation();
                Level.instance.HookSystem.OnHookObjectsTogether = false;
                return;
            }

            playerController.Move(direction.normalized * hookSpeed * speedMultiplier * Time.deltaTime);
            Level.instance.audioEvents.SetAudioParameter("GeneralVelocity", hookSpeed * speedMultiplier);
            Level.instance.audioEvents.PlayAudioEvent("PlayerMoveWind", gameObject);
        }
        else
        {
            CancelHookShotMovement();
        }
    }

    public void CancelHookShotMovement()
    {
        if (movementState == MovementState.Default) return;

        GameEvents.PlayAnimation?.Invoke("PullQuit", 0, 0, false);
        momentum = direction.normalized * hookSpeed * 2f;
        verticalVelocity.y = 0;
        movementState = MovementState.Default;
    }

    #endregion
}
