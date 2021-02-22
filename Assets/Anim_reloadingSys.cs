using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_reloadingSys : StateMachineBehaviour
{
    private static int IsReloading = Animator.StringToHash("isReloading");
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsReloading, false);
    }

}
