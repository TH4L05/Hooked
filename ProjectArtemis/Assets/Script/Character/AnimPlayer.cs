using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
        if (stateInfo.IsName("Hook_Pull_Start"))
        {
            animator.SetBool("onPull", true);
            return;
        }
        else if (stateInfo.IsName("Hook_Objects_Activate"))
        {
            animator.SetBool("onPullObject", true);
            return;
        }
        else if (stateInfo.IsName("Hook_Cut"))
        {
            animator.SetBool("onCut", false);
            animator.SetBool("onPull", false);
            animator.SetBool("onPullObject", false);
            return;
        }
        else if (stateInfo.IsName("Shoot_Start"))
        {
            animator.SetBool("onPull", false);
            animator.SetBool("shotStart", true);
            return;
        }

        else if (stateInfo.IsName("Shoot_End"))
        {
            animator.SetBool("onhold", false);
            animator.SetBool("shoot", false);
            animator.SetBool("shotStart", false);
            return;
        }
        else
        {
            return;
        }
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
