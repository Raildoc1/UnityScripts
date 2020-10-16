using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBool : StateMachineBehaviour {

    [System.Serializable]
    public class TimeInterval {
        [Range(0f, 1f)]
        public float startValueNormTime = 0f;
        [Range(0f, 1f)]
        public float endValueNormTime = 1f;
    }

    public List<TimeInterval> intervals;

    public string boolName;
    public bool value;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        float time = stateInfo.normalizedTime % 1f;

        foreach (var interval in intervals) {
            if (time > interval.startValueNormTime && time < interval.endValueNormTime) { 
                animator.SetBool(boolName, value);
                return;
            }
        }

        animator.SetBool(boolName, !value);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool(boolName, !value);
    }

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
