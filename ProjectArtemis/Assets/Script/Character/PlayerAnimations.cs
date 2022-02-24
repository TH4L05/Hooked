using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animMain;
    [SerializeField] private Animator animArrow;
    [SerializeField] private Animator animCamera;
    [SerializeField] private Animator animEmission;

    private void Awake()
    {
        GameEvents.PlayAnimation += PlayAnimation;
    }

    private void OnDestroy()
    {
        GameEvents.PlayAnimation -= PlayAnimation;
    }

    /// <summary>
    /// Plays an Animation based on String 
    /// </summary>
    /// <param name="animName"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    public void PlayAnimation(string animName, float value1, int value2, bool value3)
    {
        if (animName == string.Empty) return;

        if (animMain == null)
        {
            Debug.LogError("!! Animator Reference is Missing !!");
        }

        switch (animName)
        {
            case "Idle":
                Idle();
                break;

            case "ShootStart":
                ShootStart();
                break;

            case "ShootEnd":
                ShootEnd();
                break;

            case "ShootEnding":
                ShootEnding();
                break;

            case "Move":
                Move(value1);
                break;

            case "JumpStart":
                JumpStart();
                break;

            case "JumpEnd":
                JumpEnd();
                break;

            case "PullStart":
                PullStart();
                break;

            case "PullQuit":
                CancelPull();
                break;

            case "PullObjectStart":
                PullObjectStart();
                break;

            case "PullObjectStop":
                PullObjectStop();
                break;

            case "BowEmissionResetBlue":
                BowEmissionResetBlue();
                break;

            case "BowEmissionResetOrange":
                BowEmissionResetOrange();
                break;

            default:
                break;
        }
    }


    #region AnimatorSettings

    private void Idle()
    {
        animMain.SetBool("onhold", false);
        animMain.SetBool("shoot", false);
        animMain.SetBool("shotStart", false);
        animMain.SetBool("onPull", false);
        animMain.SetBool("onPullObject", false); 
        animMain.SetBool("onCut", false);
        animMain.SetFloat("speed", 0);
        animMain.Play("Idle");
    }

    private void ShootStart()
    {
        animMain.SetBool("onhold", true);
        animMain.SetBool("shoot", false);

        if (animArrow == null) return;
        animArrow.SetBool("onhold", true);
        animArrow.SetBool("shoot", false);
    }

    private void ShootEnding()
    {
        animMain.SetBool("shotStarted", false);
    }

    private void ShootEnd()
    {
        animMain.SetBool("shoot", true);

        if (animArrow == null) return;
        animArrow.SetBool("shoot", true);
        animArrow.SetBool("onhold", false);
    }

    private void Move(float speed)
    {
        
        animMain.SetFloat("speed", speed);
    }

    private void JumpStart()
    {
        animMain.SetTrigger("jumpStart");
    }

    private void JumpEnd()
    {
        animMain.SetTrigger("jumpLand");

        if (animCamera == null) return;
        animCamera.Play("player_jump_landing");
    }

    private void PullStart()
    {
        animMain.SetTrigger("pull");
    }

    private void PullObjectStart()
    {
        animMain.SetTrigger("pullObject");
    }

    private void PullObjectStop()
    {
        animMain.SetBool("onPullObject", false);
    }

    private void CancelPull()
    {
        animMain.SetTrigger("cut");
        animMain.SetBool("onCut", true);
        animMain.SetBool("onPullObject", false);
    }

    private void BowEmissionResetBlue()
    {
        animEmission.SetBool("blue", false);
    }

    private void BowEmissionResetOrange()
    {
        animEmission.SetBool("orange", false);
    }


    #endregion
}
