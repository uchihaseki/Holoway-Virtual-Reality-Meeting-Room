using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWaving : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsWaving", false);
    }
}
