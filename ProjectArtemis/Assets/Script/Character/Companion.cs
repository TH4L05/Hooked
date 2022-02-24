using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : Character
{
    #region Fields

    [System.Serializable]
    public enum State
    {
        Idle,
        IdleLookAtPlayer,
        OnMove,
        OnMoveGlide,
        Sleep,
    }

    [SerializeField] private SphereCollider onSleepCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBeforeSleep = 10f;
    [SerializeField] private Rigidbody rb;
    public State state;
    public State laststate;
    private bool lookAtPlayer = false;
    private Vector3 lastActivePosition = Vector3.zero;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        state = State.Idle;
        timer = GetComponent<Timer>();
        timer.ShowDebugMessages = true;
    }

    private void Start()
    {
        Timer.OnTimerFinished += CheckIdleTimer;
    }

    public void Update()
    {
        CheckState();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            rb.isKinematic = true;
            transform.position = lastActivePosition;
            Debug.Log("<color=#C99D00>COMPANION_ACTIVATE</color>");
            animator.SetBool("idle", true);
            animator.SetTrigger("sleepReset");
            onSleepCollider.enabled = false;
            state = State.IdleLookAtPlayer;
        }
    }

    #endregion

    #region Behaviour

    public void SetState(int stateIndex)
    {
        this.state = (State)stateIndex;
    }

    public void SetTheState(State state)
    {
        this.state = state;
    }

    public void CheckState()
    {
        if (lookAtPlayer)
        {
            LookAtTargetSmooth();
        }


        if (laststate == state) return;

        switch (state)
        {
            case State.Idle:
            default:
                //Debug.Log("<color=#C99D00>COMPANION_IDLE</color>");
                lookAtPlayer = false;
                animator.SetBool("idle", true);                
                //timer.StartTimer(timeBeforeSleep,false);
                break;


            case State.IdleLookAtPlayer:
                animator.SetBool("idle", true);
                lookAtPlayer = true;
                timer.StartTimer(timeBeforeSleep, false);
                break;


            case State.OnMove:
                lookAtPlayer = false;
                if (timer.IsRunning)
                {
                    timer.Stop();                   
                }
                animator.SetBool("idle", false);
                animator.SetTrigger("fly");
                break;

            case State.OnMoveGlide:
                if (timer.IsRunning)
                {
                    timer.Stop();
                }
                lookAtPlayer = false;
                animator.SetBool("idle", false);
                animator.SetTrigger("flyX");
                break;

            case State.Sleep:
                lookAtPlayer = false;
                animator.SetBool("idle", false);
                animator.SetTrigger("sleep");
                break;
        }

        laststate = state;
    }

    private void LookAtTargetSmooth()
    {
        //Debug.Log("<color=#C99D00>COMPANION_IDLE_&_LOOK_AT_PLAYER</color>");
        Vector3 playerPos = Level.instance.player.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2);
    }

    public void CheckIdleTimer()
    {
        if (state == State.IdleLookAtPlayer)
        {
            lastActivePosition = transform.position;
            lookAtPlayer = false;
            Debug.Log("<color=#C99D00>COMPANION_SLEEP</color>");
            state = State.Sleep;
            animator.SetBool("idle", false);
            animator.SetTrigger("sleep");
            onSleepCollider.enabled = true;
            rb.isKinematic = false;
        }      
    }

    #endregion
}
