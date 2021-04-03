using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_reloadingSys : StateMachineBehaviour
{
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");
    //OnStateExit is called 
    
    
    
   // hen a transition ends and the state machine finishes evaluating this state
     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsReloading, false);
    }

}
